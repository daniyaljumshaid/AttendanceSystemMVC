using AttendanceSystemMVC.Data;
using AttendanceSystemMVC.Models;
using AttendanceSystemMVC.ViewModels;
using AttendanceSystemMVC.ReportService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AttendanceSystemMVC.Controllers
{
    public class EnrollmentRequest
    {
        public int CourseId { get; set; }
    }
    [Authorize(Roles = "Student")]
    public class StudentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ReportsService _reports;

        public StudentController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, ReportsService reports)
        {
            _context = context;
            _userManager = userManager;
            _reports = reports;
        }

        private async Task<StudentProfile?> GetStudentProfile()
        {
            var userId = _userManager.GetUserId(User);
            return await _context.StudentProfiles
                .Include(s => s.ApplicationUser)
                .Include(s => s.Section)
                .FirstOrDefaultAsync(s => s.ApplicationUserId == userId);
        }

        // ===================== STUDENT DASHBOARD =====================
        public async Task<IActionResult> Index()
        {
            var student = await GetStudentProfile();
            if (student == null)
                return View("NoProfile");

            // Get the active session for the student's section
            var sectionSession = await _context.SectionSessions
                .Include(ss => ss.Session)
                .Where(ss => ss.SectionId == student.SectionId &&
                            ss.Session!.StartDate <= DateTime.Today &&
                            ss.Session!.EndDate >= DateTime.Today)
                .FirstOrDefaultAsync();

            Session? currentSession = null;

            if (sectionSession != null)
            {
                currentSession = sectionSession.Session;
            }
            else
            {
                // Fallback: Get the latest session assigned to this section
                var latestSectionSession = await _context.SectionSessions
                    .Include(ss => ss.Session)
                    .Where(ss => ss.SectionId == student.SectionId)
                    .OrderByDescending(ss => ss.Session!.StartDate)
                    .FirstOrDefaultAsync();

                if (latestSectionSession != null)
                {
                    currentSession = latestSectionSession.Session;
                }
            }

            // Get enrolled courses for current session
            var enrolledCourses = new List<Course>();
            if (currentSession != null)
            {
                enrolledCourses = await _context.Enrollments
                    .Include(e => e.Course)
                    .Where(e => e.StudentProfileId == student.Id && e.SessionId == currentSession.Id)
                    .Select(e => e.Course!)
                    .ToListAsync();
            }

            // Get today's schedule
            var today = DateTime.Today;
            var todaySchedules = await _context.CourseSchedules
                .Include(cs => cs.Course)
                .Include(cs => cs.Section)
                .Where(cs => cs.DayOfWeek == today.DayOfWeek &&
                           _context.Enrollments.Any(e => e.StudentProfileId == student.Id &&
                                                        e.CourseId == cs.CourseId &&
                                                        e.SectionId == cs.SectionId))
                .OrderBy(cs => cs.StartTime)
                .ToListAsync();

            // Calculate attendance statistics for current month
            var attendanceStats = await GetMonthlyAttendanceStats(student.Id, DateTime.Now.Year, DateTime.Now.Month);

            var viewModel = new StudentDashboardVm
            {
                Student = student,
                CurrentSession = currentSession,
                EnrolledCourses = enrolledCourses,
                TodaySchedules = todaySchedules,
                AttendanceStats = attendanceStats
            };

            return View(viewModel);
        }

        // ===================== STUDENT COURSES =====================
        public async Task<IActionResult> Courses()
        {
            var student = await GetStudentProfile();
            if (student == null) return View("NoProfile");

            // Get the active session for the student's section
            var sectionSession = await _context.SectionSessions
                .Include(ss => ss.Session)
                .Where(ss => ss.SectionId == student.SectionId)
                .OrderByDescending(ss => ss.Session!.StartDate)
                .FirstOrDefaultAsync();

            if (sectionSession == null || sectionSession.Session == null)
                return View(new List<Course>());

            // Get enrolled courses for this student in their section's session
            var courses = await _context.Enrollments
                .Include(e => e.Course)
                .Where(e => e.StudentProfileId == student.Id && e.SessionId == sectionSession.SessionId)
                .Select(e => e.Course)
                .Distinct()
                .ToListAsync();

            return View(courses);
        }

        // ===================== STUDENT SESSIONS =====================
        public async Task<IActionResult> Sessions()
        {
            var sessions = await _context.Sessions.OrderByDescending(s => s.StartDate).ToListAsync();
            return View(sessions);
        }

        // ===================== INDIVIDUAL MONTHLY ATTENDANCE REPORT =====================
        public async Task<IActionResult> MonthlyReport(int? year, int? month)
        {
            var student = await GetStudentProfile();
            if (student == null) return View("NoProfile");

            int y = year ?? DateTime.Now.Year;
            int m = month ?? DateTime.Now.Month;

            // Get student's individual monthly report
            var attendanceRecords = await _context.AttendanceRecords
                .Include(ar => ar.Course)
                .Include(ar => ar.Section)
                .Where(ar => ar.StudentProfileId == student.Id &&
                           ar.Date.Year == y &&
                           ar.Date.Month == m)
                .OrderBy(ar => ar.Date)
                .ThenBy(ar => ar.Course!.Name)
                .ToListAsync();

            // Group by course
            var courseReports = attendanceRecords
                .GroupBy(ar => new { ar.CourseId, ar.Course!.Name })
                .Select(g => new StudentCourseAttendanceVm
                {
                    CourseName = g.Key.Name,
                    CourseId = g.Key.CourseId,
                    TotalClasses = g.Count(),
                    PresentClasses = g.Count(ar => ar.Status == AttendanceStatus.Present),
                    AbsentClasses = g.Count(ar => ar.Status == AttendanceStatus.Absent),
                    LateClasses = g.Count(ar => ar.Status == AttendanceStatus.Late),
                    AttendancePercentage = g.Count() > 0 ?
                        Math.Round((double)g.Count(ar => ar.Status == AttendanceStatus.Present) / g.Count() * 100, 1) : 0,
                    Records = g.ToList()
                })
                .ToList();

            var vm = new StudentMonthlyReportVm
            {
                Student = student,
                Year = y,
                Month = m,
                CourseReports = courseReports,
                TotalClasses = courseReports.Sum(cr => cr.TotalClasses),
                TotalPresent = courseReports.Sum(cr => cr.PresentClasses),
                TotalAbsent = courseReports.Sum(cr => cr.AbsentClasses),
                TotalLate = courseReports.Sum(cr => cr.LateClasses),
                OverallAttendancePercentage = courseReports.Sum(cr => cr.TotalClasses) > 0 ?
                    Math.Round((double)courseReports.Sum(cr => cr.PresentClasses) / courseReports.Sum(cr => cr.TotalClasses) * 100, 1) : 0
            };

            ViewBag.Year = y;
            ViewBag.Month = m;
            return View(vm);
        }

        // ===================== STUDENT TIMETABLE =====================
        public async Task<IActionResult> Timetable()
        {
            var student = await GetStudentProfile();
            if (student == null) return View("NoProfile");

            // Find enrollments for latest session
            var latestSession = await _context.Sessions.OrderByDescending(s => s.StartDate).FirstOrDefaultAsync();
            if (latestSession == null) return View(new List<CourseSchedule>());

            var enrollments = await _context.Enrollments
                .Include(e => e.Course)
                .Include(e => e.Section)
                .Where(e => e.StudentProfileId == student.Id && e.SessionId == latestSession.Id)
                .ToListAsync();

            var schedule = new List<CourseSchedule>();
            foreach (var e in enrollments)
            {
                var schedules = await _context.CourseSchedules
                    .Include(cs => cs.Course)
                    .Include(cs => cs.Section)
                    .Where(cs => cs.CourseId == e.CourseId && cs.SectionId == e.SectionId && cs.SessionId == e.SessionId)
                    .ToListAsync();
                schedule.AddRange(schedules);
            }

            return View(schedule.OrderBy(s => s.DayOfWeek).ThenBy(s => s.StartTime).ToList());
        }

        // Helper method to calculate monthly attendance stats
        private async Task<StudentAttendanceStatsVm> GetMonthlyAttendanceStats(int studentId, int year, int month)
        {
            var records = await _context.AttendanceRecords
                .Where(ar => ar.StudentProfileId == studentId &&
                           ar.Date.Year == year &&
                           ar.Date.Month == month)
                .ToListAsync();

            return new StudentAttendanceStatsVm
            {
                TotalClasses = records.Count,
                PresentClasses = records.Count(r => r.Status == AttendanceStatus.Present),
                AbsentClasses = records.Count(r => r.Status == AttendanceStatus.Absent),
                LateClasses = records.Count(r => r.Status == AttendanceStatus.Late),
                AttendancePercentage = records.Count > 0 ?
                    Math.Round((double)records.Count(r => r.Status == AttendanceStatus.Present) / records.Count * 100, 1) : 0
            };
        }

        // ===================== COURSE REGISTRATION =====================
        public async Task<IActionResult> RegisterCourses()
        {
            var student = await GetStudentProfile();
            if (student == null) return View("NoProfile");

            // Get the active session for the student's section
            var sectionSession = await _context.SectionSessions
                .Include(ss => ss.Session)
                .Where(ss => ss.SectionId == student.SectionId &&
                            ss.Session!.StartDate <= DateTime.Today &&
                            ss.Session!.EndDate >= DateTime.Today)
                .FirstOrDefaultAsync();

            Session? currentSession = null;

            if (sectionSession != null)
            {
                currentSession = sectionSession.Session;
            }
            else
            {
                // Fallback: Get the latest session assigned to this section
                var latestSectionSession = await _context.SectionSessions
                    .Include(ss => ss.Session)
                    .Where(ss => ss.SectionId == student.SectionId)
                    .OrderByDescending(ss => ss.Session!.StartDate)
                    .FirstOrDefaultAsync();

                if (latestSectionSession != null)
                {
                    currentSession = latestSectionSession.Session;
                }
            }

            // Get ALL courses assigned to this student by admin (StudentCourseAssignments)
            // Don't filter by session initially - show all assignments
            var assignedCourses = await _context.StudentCourseAssignments
                .Include(sca => sca.Course)
                .Include(sca => sca.Section)
                .Include(sca => sca.Session)
                .Where(sca => sca.StudentProfileId == student.Id)
                .ToListAsync();

            // If we have a current session, prioritize those assignments
            if (currentSession != null)
            {
                var sessionAssignments = assignedCourses.Where(ac => ac.SessionId == currentSession.Id).ToList();
                if (sessionAssignments.Any())
                {
                    assignedCourses = sessionAssignments;
                }
            }
            else if (assignedCourses.Any())
            {
                // Use the session from the first assignment if no section session found
                currentSession = assignedCourses.First().Session;
            }

            if (currentSession == null)
            {
                TempData["Error"] = "No session found. Contact administrator.";
                return RedirectToAction("Index");
            }

            // Get course schedules for assigned courses to show teacher and timing info
            var assignedCourseIds = assignedCourses.Select(ac => ac.CourseId).ToList();

            var courseSchedules = await _context.CourseSchedules
                .Include(cs => cs.Course)
                .Include(cs => cs.Teacher)
                    .ThenInclude(t => t!.ApplicationUser)
                .Include(cs => cs.Section)
                .Include(cs => cs.Session)
                .Where(cs => assignedCourseIds.Contains(cs.CourseId))
                .OrderBy(cs => cs.Course!.Name)
                .ToListAsync();

            // Get already enrolled course IDs for the current session
            var enrolledCourseIds = await _context.Enrollments
                .Where(e => e.StudentProfileId == student.Id && e.SessionId == currentSession.Id)
                .Select(e => e.CourseId)
                .ToListAsync();

            ViewBag.CurrentSession = currentSession;
            ViewBag.EnrolledCourseIds = enrolledCourseIds;
            ViewBag.AssignedCourses = assignedCourses;
            ViewBag.StudentSection = student.Section;

            return View(courseSchedules);
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> EnrollInCourse([FromBody] EnrollmentRequest request)
        {
            try
            {
                var student = await GetStudentProfile();
                if (student == null) return Json(new { success = false, message = "Student profile not found" });

                // Verify the course exists
                var courseExists = await _context.Courses.AnyAsync(c => c.Id == request.CourseId);
                if (!courseExists)
                    return Json(new { success = false, message = "Course not found. Please refresh the page and try again." });

                // Get the active session for the student's section
                var sectionSession = await _context.SectionSessions
                    .Include(ss => ss.Session)
                    .Where(ss => ss.SectionId == student.SectionId &&
                                ss.Session!.StartDate <= DateTime.Today &&
                                ss.Session!.EndDate >= DateTime.Today)
                    .FirstOrDefaultAsync();

                Session? currentSession = null;

                if (sectionSession != null)
                {
                    currentSession = sectionSession.Session;
                }
                else
                {
                    // Fallback: Get the latest session assigned to this section
                    var latestSectionSession = await _context.SectionSessions
                        .Include(ss => ss.Session)
                        .Where(ss => ss.SectionId == student.SectionId)
                        .OrderByDescending(ss => ss.Session!.StartDate)
                        .FirstOrDefaultAsync();

                    if (latestSectionSession != null)
                    {
                        currentSession = latestSectionSession.Session;
                    }
                }

                if (currentSession == null)
                    return Json(new { success = false, message = "No session assigned to your section" });

                // Check if already enrolled
                var exists = await _context.Enrollments
                    .AnyAsync(e => e.StudentProfileId == student.Id &&
                                 e.CourseId == request.CourseId &&
                                 e.SessionId == currentSession.Id);

                if (exists)
                    return Json(new { success = false, message = "Already enrolled in this course" });

                // Create enrollment
                var enrollment = new Enrollment
                {
                    StudentProfileId = student.Id,
                    CourseId = request.CourseId,
                    SectionId = student.SectionId, // Use student's section
                    SessionId = currentSession.Id
                };

                _context.Enrollments.Add(enrollment);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Successfully enrolled in course" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"An error occurred: {ex.Message}" });
            }
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> UnenrollFromCourse([FromBody] EnrollmentRequest request)
        {
            try
            {
                var student = await GetStudentProfile();
                if (student == null) return Json(new { success = false, message = "Student profile not found" });

                // Get the active session for the student's section
                var sectionSession = await _context.SectionSessions
                    .Include(ss => ss.Session)
                    .Where(ss => ss.SectionId == student.SectionId &&
                                ss.Session!.StartDate <= DateTime.Today &&
                                ss.Session!.EndDate >= DateTime.Today)
                    .FirstOrDefaultAsync();

                Session? currentSession = null;

                if (sectionSession != null)
                {
                    currentSession = sectionSession.Session;
                }
                else
                {
                    // Fallback: Get the latest session assigned to this section
                    var latestSectionSession = await _context.SectionSessions
                        .Include(ss => ss.Session)
                        .Where(ss => ss.SectionId == student.SectionId)
                        .OrderByDescending(ss => ss.Session!.StartDate)
                        .FirstOrDefaultAsync();

                    if (latestSectionSession != null)
                    {
                        currentSession = latestSectionSession.Session;
                    }
                }

                if (currentSession == null)
                    return Json(new { success = false, message = "No session assigned to your section" });

                var enrollment = await _context.Enrollments
                    .FirstOrDefaultAsync(e => e.StudentProfileId == student.Id &&
                                            e.CourseId == request.CourseId &&
                                            e.SessionId == currentSession.Id);

                if (enrollment == null)
                    return Json(new { success = false, message = "Enrollment not found" });

                _context.Enrollments.Remove(enrollment);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Successfully unenrolled from course" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"An error occurred: {ex.Message}" });
            }
        }

        // ===================== CHANGE PASSWORD =====================
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordVm model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "Account");

            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

            if (result.Succeeded)
            {
                // Clear MustChangePassword flag if it was set
                if (user.MustChangePassword)
                {
                    user.MustChangePassword = false;
                    await _userManager.UpdateAsync(user);
                }

                TempData["Success"] = "Password changed successfully!";
                return RedirectToAction("Index");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }
    }
}
