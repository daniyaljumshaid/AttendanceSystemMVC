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
    [Authorize(Roles = "Teacher")]
    public class TeacherController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ReportsService _reports;

        public TeacherController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, ReportsService reports)
        {
            _context = context;
            _userManager = userManager;
            _reports = reports;
        }

        private async Task<TeacherProfile?> GetTeacherProfile()
        {
            var userId = _userManager.GetUserId(User);
            return await _context.TeacherProfiles
                .Include(t => t.ApplicationUser)
                .FirstOrDefaultAsync(t => t.ApplicationUserId == userId);
        }

        // ===================== TEACHER DASHBOARD =====================
        public async Task<IActionResult> Index()
        {
            var teacher = await GetTeacherProfile();
            if (teacher == null)
                return View("NoProfile");

            // Check if teacher is approved
            if (teacher.ApplicationUser != null && !teacher.ApplicationUser.IsApproved)
            {
                ViewBag.Message = "Your account is pending approval by the administrator. You will be notified once approved.";
                return View("PendingApproval");
            }

            var now = DateTime.Now;
            var today = DateTime.Today;

            // Get current session - first try to find by date
            var currentSession = await _context.Sessions
                .Where(s => s.StartDate <= today && s.EndDate >= today)
                .FirstOrDefaultAsync();

            // If no session found by date, get the session from teacher's course schedules
            if (currentSession == null)
            {
                var teacherSchedule = await _context.CourseSchedules
                    .Include(cs => cs.Session)
                    .Where(cs => cs.TeacherProfileId == teacher.Id)
                    .OrderByDescending(cs => cs.Session!.StartDate)
                    .FirstOrDefaultAsync();

                if (teacherSchedule != null)
                {
                    currentSession = teacherSchedule.Session;
                }
            }

            // Get ALL assigned courses for this teacher (no session filter for course count)
            var allAssignedCourses = await _context.CourseSchedules
                .Include(cs => cs.Course)
                .Where(cs => cs.TeacherProfileId == teacher.Id)
                .Select(cs => cs.Course!)
                .Distinct()
                .ToListAsync();

            // Get today's schedules (if we have a current session, filter by it; otherwise show all for today)
            var todaySchedules = await _context.CourseSchedules
                .Include(cs => cs.Course)
                .Include(cs => cs.Section)
                .Include(cs => cs.Session)
                .Where(cs => cs.TeacherProfileId == teacher.Id &&
                           cs.DayOfWeek == now.DayOfWeek)
                .OrderBy(cs => cs.StartTime)
                .ToListAsync();

            // Get current class
            var currentClass = todaySchedules
                .FirstOrDefault(cs => cs.StartTime <= now.TimeOfDay &&
                                    cs.EndTime >= now.TimeOfDay);

            // Get week schedules
            var weekSchedules = await _context.CourseSchedules
                .Include(cs => cs.Course)
                .Include(cs => cs.Section)
                .Include(cs => cs.Session)
                .Where(cs => cs.TeacherProfileId == teacher.Id)
                .OrderBy(cs => cs.DayOfWeek)
                .ThenBy(cs => cs.StartTime)
                .ToListAsync();

            // Calculate statistics
            var totalStudentsToday = 0;
            var pendingAttendance = 0;

            foreach (var schedule in todaySchedules)
            {
                var enrollmentCount = await _context.Enrollments
                    .CountAsync(e => e.CourseId == schedule.CourseId &&
                               e.SectionId == schedule.SectionId &&
                               e.SessionId == schedule.SessionId);
                totalStudentsToday += enrollmentCount;

                var hasAttendance = await _context.AttendanceRecords
                    .AnyAsync(ar => ar.CourseId == schedule.CourseId &&
                                  ar.SectionId == schedule.SectionId &&
                                  ar.Date == today);

                if (!hasAttendance && schedule.StartTime < now.TimeOfDay)
                    pendingAttendance++;
            }

            var vm = new TeacherDashboardVm
            {
                TeacherProfile = teacher,
                TodaySchedules = todaySchedules,
                WeekSchedules = weekSchedules,
                CurrentClass = currentClass,
                AssignedCourses = allAssignedCourses,
                TotalStudentsToday = totalStudentsToday,
                PendingAttendance = pendingAttendance,
                IsApproved = teacher.ApplicationUser?.IsApproved ?? false,
                WelcomeMessage = $"Welcome back, {teacher.ApplicationUser?.FullName ?? "Teacher"}!",
                CurrentSession = currentSession
            };

            return View(vm);
        }

        // ===================== TEACHER PROFILE =====================
        public async Task<IActionResult> Profile()
        {
            var teacher = await GetTeacherProfile();
            if (teacher == null)
                return View("NoProfile");

            return View(teacher);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(TeacherProfile model)
        {
            var teacher = await GetTeacherProfile();
            if (teacher == null)
                return View("NoProfile");

            if (ModelState.IsValid)
            {
                teacher.EmployeeId = model.EmployeeId;

                // Update user info if provided
                if (teacher.ApplicationUser != null)
                {
                    teacher.ApplicationUser.FullName = model.ApplicationUser?.FullName ?? teacher.ApplicationUser.FullName;
                    teacher.ApplicationUser.PhoneNumber = model.ApplicationUser?.PhoneNumber ?? teacher.ApplicationUser.PhoneNumber;
                }

                await _context.SaveChangesAsync();
                TempData["Success"] = "Profile updated successfully!";
                return RedirectToAction(nameof(Profile));
            }

            return View("Profile", teacher);
        }

        // ===================== MY STUDENTS =====================
        public async Task<IActionResult> MyStudents()
        {
            var teacher = await GetTeacherProfile();
            if (teacher == null)
                return View("NoProfile");

            var students = await _context.Enrollments
                .Include(e => e.Student!)
                    .ThenInclude(s => s.ApplicationUser)
                .Include(e => e.Course)
                .Include(e => e.Section)
                .Include(e => e.Session)
                .Where(e => _context.CourseSchedules
                    .Any(cs => cs.TeacherProfileId == teacher.Id &&
                      cs.CourseId == e.CourseId &&
                      cs.SectionId == e.SectionId))
        .ToListAsync();

            // Group by course and section
            var groupedStudents = students
                .GroupBy(e => new
                {
                    CourseId = e.CourseId,
                    CourseName = e.Course?.Name,
                    SectionId = e.SectionId,
                    SectionName = e.Section?.Name,
                    SessionName = e.Session?.Name
                })
                .ToDictionary(g => g.Key, g => g.ToList());

            ViewBag.GroupedStudents = groupedStudents;
            return View(students);
        }

        // ===================== CALENDAR VIEW =====================
        public async Task<IActionResult> Calendar()
        {
            var teacher = await GetTeacherProfile();
            if (teacher == null)
                return View("NoProfile");

            var schedules = await _context.CourseSchedules
                .Include(cs => cs.Course)
                .Include(cs => cs.Section)
                .Include(cs => cs.Session)
                .Where(cs => cs.TeacherProfileId == teacher.Id)
                .ToListAsync();

            return View(schedules);
        }

        // ===================== ATTENDANCE STATISTICS =====================
        public async Task<IActionResult> AttendanceStats()
        {
            var teacher = await GetTeacherProfile();
            if (teacher == null)
                return View("NoProfile");

            var courses = await _context.CourseSchedules
                .Include(cs => cs.Course)
                .Include(cs => cs.Section)
                .Where(cs => cs.TeacherProfileId == teacher.Id)
                .Select(cs => new { cs.CourseId, cs.Course, cs.SectionId, cs.Section })
                .Distinct()
                .ToListAsync();

            var stats = new List<dynamic>();

            foreach (var course in courses)
            {
                var totalStudents = await _context.Enrollments
                    .CountAsync(e => e.CourseId == course.CourseId && e.SectionId == course.SectionId);

                var totalClasses = await _context.AttendanceRecords
                    .Where(ar => ar.CourseId == course.CourseId && ar.SectionId == course.SectionId)
                    .Select(ar => ar.Date)
                    .Distinct()
                    .CountAsync();

                var presentCount = await _context.AttendanceRecords
                    .CountAsync(ar => ar.CourseId == course.CourseId &&
                                    ar.SectionId == course.SectionId &&
                                    ar.Status == AttendanceStatus.Present);

                var absentCount = await _context.AttendanceRecords
                    .CountAsync(ar => ar.CourseId == course.CourseId &&
                                    ar.SectionId == course.SectionId &&
                                    ar.Status == AttendanceStatus.Absent);

                stats.Add(new
                {
                    CourseName = course.Course?.Name,
                    SectionName = course.Section?.Name,
                    TotalStudents = totalStudents,
                    TotalClasses = totalClasses,
                    PresentCount = presentCount,
                    AbsentCount = absentCount,
                    AttendanceRate = totalStudents > 0 && totalClasses > 0 ?
                        Math.Round((double)presentCount / (totalStudents * totalClasses) * 100, 1) : 0
                });
            }

            ViewBag.Stats = stats;
            return View();
        }

        // ===================== CURRENT CLASS =====================
        public async Task<IActionResult> CurrentClass()
        {
            var teacher = await GetTeacherProfile();
            if (teacher == null)
                return View("NoProfile");

            var now = DateTime.Now;
            var today = DateTime.Today;

            // Find the current active session
            var currentSession = await _context.Sessions
                .Where(s => s.StartDate <= today && s.EndDate >= today)
                .FirstOrDefaultAsync();

            if (currentSession == null)
            {
                ViewBag.Message = "No active session found for today's date.";
                return View("NoCurrentClass");
            }

            var schedule = await _context.CourseSchedules
                .Include(s => s.Course)
                .Include(s => s.Section)
                .Include(s => s.Session)
                .Where(s =>
                    s.TeacherProfileId == teacher.Id &&
                    s.SessionId == currentSession.Id &&
                    s.DayOfWeek == now.DayOfWeek &&
                    s.StartTime <= now.TimeOfDay &&
                    s.EndTime >= now.TimeOfDay)
                .FirstOrDefaultAsync();

            if (schedule == null)
            {
                // Check if there are any schedules today for debugging
                var todaySchedules = await _context.CourseSchedules
                    .Include(s => s.Course)
                    .Include(s => s.Section)
                    .Where(s => s.TeacherProfileId == teacher.Id &&
                               s.SessionId == currentSession.Id &&
                               s.DayOfWeek == now.DayOfWeek)
                    .ToListAsync();

                if (todaySchedules.Any())
                {
                    var upcomingClass = todaySchedules
                        .Where(s => s.StartTime > now.TimeOfDay)
                        .OrderBy(s => s.StartTime)
                        .FirstOrDefault();

                    if (upcomingClass != null)
                    {
                        ViewBag.Message = $"Your next class is {upcomingClass.Course?.Name} at {upcomingClass.StartTime:hh\\:mm}";
                    }
                    else
                    {
                        ViewBag.Message = "All your classes for today have ended.";
                    }
                }
                else
                {
                    ViewBag.Message = $"You have no scheduled classes on {now.DayOfWeek} in the current session.";
                }

                return View("NoCurrentClass");
            }

            await EnsureAttendanceCreatedForClass(
                schedule.CourseId,
                schedule.SectionId,
                schedule.SessionId,
                today,
                teacher.Id
            );

            var attendance = await _context.AttendanceRecords
                .Include(a => a.Student!).ThenInclude(s => s.ApplicationUser)
                .Where(a =>
                    a.CourseId == schedule.CourseId &&
                    a.SectionId == schedule.SectionId &&
                    a.Date == today)
                .OrderBy(a => a.Student!.RollNumber)
                .ToListAsync();

            // Debug logging
            Console.WriteLine($"CurrentClass - Course: {schedule.CourseId}, Section: {schedule.SectionId}, Session: {schedule.SessionId}");
            Console.WriteLine($"Attendance records found: {attendance.Count}");

            if (!attendance.Any())
            {
                // Check if there are enrollments
                var enrollmentCount = await _context.Enrollments
                    .CountAsync(e => e.CourseId == schedule.CourseId &&
                                   e.SectionId == schedule.SectionId &&
                                   e.SessionId == schedule.SessionId);
                Console.WriteLine($"Enrollments found: {enrollmentCount}");

                if (enrollmentCount == 0)
                {
                    ViewBag.NoStudentsMessage = $"No students are enrolled in this course (Course ID: {schedule.CourseId}, Section ID: {schedule.SectionId}, Session ID: {schedule.SessionId}). Please ensure students are enrolled before marking attendance.";
                }
            }

            var vm = new CurrentClassVm
            {
                Schedule = schedule,
                Attendance = attendance
            };

            return View(vm);
        }

        // ===================== ATTENDANCE MANAGEMENT =====================
        public async Task<IActionResult> AttendanceManagement(int? courseId, int? sectionId, DateTime? date)
        {
            var user = await _userManager.GetUserAsync(User);
            Console.WriteLine($"=== AttendanceManagement ===");
            Console.WriteLine($"Current User: {user?.Email}, ID: {user?.Id}, IsApproved: {user?.IsApproved}");

            var teacher = await GetTeacherProfile();
            if (teacher == null)
            {
                Console.WriteLine("ERROR: Teacher profile not found!");
                return View("NoProfile");
            }

            Console.WriteLine($"Teacher Profile - ID: {teacher.Id}, EmployeeId: {teacher.EmployeeId}, UserId: {teacher.ApplicationUserId}");

            var selectedDate = date ?? DateTime.Today;

            // Get teacher's courses with session information
            var teacherCourses = await _context.CourseSchedules
                .Include(cs => cs.Course)
                .Include(cs => cs.Section)
                .Include(cs => cs.Session)
                .Where(cs => cs.TeacherProfileId == teacher.Id)
                .ToListAsync();

            Console.WriteLine($"Found {teacherCourses.Count} course schedules for teacher {teacher.Id}");

            if (teacherCourses.Count == 0)
            {
                Console.WriteLine("WARNING: No course schedules found - checking database...");
                var allSchedules = await _context.CourseSchedules.CountAsync();
                Console.WriteLine($"Total schedules in database: {allSchedules}");
            }
            else
            {
                foreach (var schedule in teacherCourses.Take(5))
                {
                    Console.WriteLine($"  - Course: {schedule.Course?.Name} (ID: {schedule.CourseId}), Section: {schedule.Section?.Name} (ID: {schedule.SectionId})");
                }
            }

            var coursesForView = teacherCourses
                .Select(cs => new { cs.Course, cs.Section, cs.CourseId, cs.SectionId, cs.SessionId, cs.Session })
                .Distinct()
                .ToList();

            Console.WriteLine($"Distinct courses for dropdown: {coursesForView.Count}");

            ViewBag.Courses = coursesForView;
            ViewBag.SelectedDate = selectedDate;

            if (courseId.HasValue && sectionId.HasValue)
            {
                // Get the session for this course schedule
                var schedule = await _context.CourseSchedules
                    .FirstOrDefaultAsync(cs => cs.TeacherProfileId == teacher.Id &&
                                              cs.CourseId == courseId.Value &&
                                              cs.SectionId == sectionId.Value);

                if (schedule != null)
                {
                    await EnsureAttendanceCreatedForClass(courseId.Value, sectionId.Value, schedule.SessionId, selectedDate, teacher.Id);

                    var attendance = await _context.AttendanceRecords
                        .Include(a => a.Student!).ThenInclude(s => s.ApplicationUser)
                        .Include(a => a.Course)
                        .Include(a => a.Section)
                        .Where(a => a.CourseId == courseId.Value &&
                                  a.SectionId == sectionId.Value &&
                                  a.Date == selectedDate)
                        .OrderBy(a => a.Student!.RollNumber)
                        .ToListAsync();

                    ViewBag.Attendance = attendance;
                    ViewBag.SelectedCourseId = courseId;
                    ViewBag.SelectedSectionId = sectionId;
                }
            }

            return View();
        }

        // ===================== SAVE ATTENDANCE =====================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveAttendance(List<AttendanceEditVm> edits)
        {
            // Debug logging
            Console.WriteLine($"SaveAttendance called with {edits?.Count ?? 0} edits");

            var teacher = await GetTeacherProfile();
            if (teacher == null)
            {
                Console.WriteLine("Teacher profile not found");
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    return Json(new { success = false, message = "Teacher profile not found" });
                TempData["Error"] = "Teacher profile not found";
                return RedirectToAction(nameof(Index));
            }

            if (edits == null || !edits.Any())
            {
                Console.WriteLine("No edits received");
                // Log form data for debugging
                foreach (var key in Request.Form.Keys)
                {
                    Console.WriteLine($"Form key: {key}, Value: {Request.Form[key]}");
                }

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    return Json(new { success = false, message = "No attendance records to save. Please select attendance status for students." });
                TempData["Error"] = "No attendance records to save";
                return RedirectToAction(nameof(CurrentClass));
            }

            try
            {
                int updatedCount = 0;
                foreach (var e in edits)
                {
                    Console.WriteLine($"Processing edit - Id: {e.Id}, Status: {e.Status}");
                    var record = await _context.AttendanceRecords.FindAsync(e.Id);
                    if (record == null)
                    {
                        Console.WriteLine($"Record not found for Id: {e.Id}");
                        continue;
                    }

                    record.Status = e.Status ?? AttendanceStatus.Unmarked;
                    record.MarkedByTeacherId = teacher.Id;
                    updatedCount++;
                }

                await _context.SaveChangesAsync();
                Console.WriteLine($"Successfully saved {updatedCount} attendance records");

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest" || Request.Headers["Accept"].ToString().Contains("application/json"))
                    return Json(new { success = true, message = $"Attendance saved successfully for {updatedCount} students" });

                TempData["Success"] = $"Attendance saved successfully for {updatedCount} students";
                return RedirectToAction(nameof(CurrentClass));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving attendance: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest" || Request.Headers["Accept"].ToString().Contains("application/json"))
                    return Json(new { success = false, message = $"Error saving attendance: {ex.Message}" });

                TempData["Error"] = $"Error saving attendance: {ex.Message}";
                return RedirectToAction(nameof(CurrentClass));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> QuickSaveAttendance(int courseId, int sectionId, DateTime date, List<AttendanceEditVm> edits)
        {
            Console.WriteLine($"QuickSaveAttendance - CourseId: {courseId}, SectionId: {sectionId}, Date: {date:yyyy-MM-dd}");
            Console.WriteLine($"Edits received: {edits?.Count ?? 0}");

            var teacher = await GetTeacherProfile();
            if (teacher == null)
            {
                TempData["Error"] = "Teacher profile not found";
                return RedirectToAction(nameof(AttendanceManagement), new { courseId, sectionId, date });
            }

            if (edits == null || !edits.Any())
            {
                TempData["Error"] = "No attendance records to save";
                return RedirectToAction(nameof(AttendanceManagement), new { courseId, sectionId, date });
            }

            try
            {
                int updatedCount = 0;
                foreach (var e in edits)
                {
                    var record = await _context.AttendanceRecords.FindAsync(e.Id);
                    if (record == null)
                    {
                        Console.WriteLine($"Record not found: {e.Id}");
                        continue;
                    }

                    record.Status = e.Status ?? AttendanceStatus.Unmarked;
                    record.MarkedByTeacherId = teacher.Id;
                    updatedCount++;
                }

                await _context.SaveChangesAsync();
                TempData["Success"] = $"Attendance saved successfully for {updatedCount} students";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving attendance: {ex.Message}");
                TempData["Error"] = $"Error saving attendance: {ex.Message}";
            }

            return RedirectToAction(nameof(AttendanceManagement), new { courseId, sectionId, date });
        }

        // ===================== REPORTS =====================
        public async Task<IActionResult> ClassMonthlyReport(int courseId, int sectionId, int? year, int? month)
        {
            var teacher = await GetTeacherProfile();
            if (teacher == null)
                return View("NoProfile");

            // Verify teacher has access to this course/section
            var hasAccess = await _context.CourseSchedules
                .AnyAsync(cs => cs.TeacherProfileId == teacher.Id &&
                              cs.CourseId == courseId &&
                              cs.SectionId == sectionId);

            if (!hasAccess)
                return Forbid();

            int y = year ?? DateTime.Now.Year;
            int m = month ?? DateTime.Now.Month;
            var vm = await _reports.GetClassMonthlyReport(courseId, sectionId, y, m);
            return View(vm);
        }

        public async Task<IActionResult> ClassSemesterReport(int courseId, int sectionId, int sessionId)
        {
            var teacher = await GetTeacherProfile();
            if (teacher == null)
                return View("NoProfile");

            // Verify teacher has access to this course/section
            var hasAccess = await _context.CourseSchedules
                .AnyAsync(cs => cs.TeacherProfileId == teacher.Id &&
                              cs.CourseId == courseId &&
                              cs.SectionId == sectionId);

            if (!hasAccess)
                return Forbid();

            var vm = await _reports.GetClassSemesterReport(courseId, sectionId, sessionId);
            return View(vm);
        }

        public async Task<IActionResult> MyReports()
        {
            var teacher = await GetTeacherProfile();
            if (teacher == null)
                return View("NoProfile");

            var assignedCourses = await _context.CourseSchedules
                .Include(cs => cs.Course)
                .Include(cs => cs.Section)
                .Include(cs => cs.Session)
                .Where(cs => cs.TeacherProfileId == teacher.Id)
                .GroupBy(cs => new { cs.CourseId, cs.SectionId, cs.SessionId })
                .Select(g => g.First())
                .ToListAsync();

            return View(assignedCourses);
        }

        // ===================== HELPER =====================
        // Temporary diagnostic action - remove after debugging
        public async Task<IActionResult> DiagnosticInfo()
        {
            var teacher = await GetTeacherProfile();
            if (teacher == null)
                return Content("Teacher profile not found");

            var output = new System.Text.StringBuilder();
            output.AppendLine("=== DIAGNOSTIC INFORMATION ===\n");

            var now = DateTime.Now;
            var today = DateTime.Today;

            // Check current session
            var currentSession = await _context.Sessions
                .Where(s => s.StartDate <= today && s.EndDate >= today)
                .FirstOrDefaultAsync();

            output.AppendLine($"Current Date: {today:yyyy-MM-dd}");
            output.AppendLine($"Current Session: {currentSession?.Name ?? "NONE"} (ID: {currentSession?.Id})");
            output.AppendLine();

            // Check sessions
            var sessions = await _context.Sessions.ToListAsync();
            output.AppendLine($"Total Sessions: {sessions.Count}");
            foreach (var session in sessions)
            {
                var isCurrent = session.StartDate <= today && session.EndDate >= today;
                output.AppendLine($"  - {session.Name}: {session.StartDate:yyyy-MM-dd} to {session.EndDate:yyyy-MM-dd} (ID: {session.Id}) {(isCurrent ? "*** CURRENT ***" : "")}");
            }
            output.AppendLine();

            // Check teacher's schedules for today
            var todaySchedules = await _context.CourseSchedules
                .Include(cs => cs.Course)
                .Include(cs => cs.Section)
                .Include(cs => cs.Session)
                .Where(cs => cs.TeacherProfileId == teacher.Id &&
                           cs.DayOfWeek == now.DayOfWeek &&
                           (currentSession == null || cs.SessionId == currentSession.Id))
                .ToListAsync();

            output.AppendLine($"Today's Schedules ({now.DayOfWeek}): {todaySchedules.Count}");
            foreach (var schedule in todaySchedules)
            {
                output.AppendLine($"  - {schedule.Course?.Name} / {schedule.Section?.Name} / {schedule.Session?.Name}");
                output.AppendLine($"    Time: {schedule.StartTime:hh\\:mm} - {schedule.EndTime:hh\\:mm}");
                output.AppendLine($"    IDs: Course={schedule.CourseId}, Section={schedule.SectionId}, Session={schedule.SessionId}");

                // Check enrollments for this specific combination
                var enrolledCount = await _context.Enrollments
                    .CountAsync(e => e.CourseId == schedule.CourseId &&
                                   e.SectionId == schedule.SectionId &&
                                   e.SessionId == schedule.SessionId);
                output.AppendLine($"    Enrolled Students: {enrolledCount}");

                if (enrolledCount > 0)
                {
                    var students = await _context.Enrollments
                        .Include(e => e.Student!)
                            .ThenInclude(s => s.ApplicationUser)
                        .Where(e => e.CourseId == schedule.CourseId &&
                                  e.SectionId == schedule.SectionId &&
                                  e.SessionId == schedule.SessionId)
                        .ToListAsync();

                    foreach (var enrollment in students.Take(5))
                    {
                        output.AppendLine($"      * {enrollment.Student?.ApplicationUser?.FullName} (Roll: {enrollment.Student?.RollNumber})");
                    }
                }
            }
            output.AppendLine();

            // Check all teacher's schedules
            var allSchedules = await _context.CourseSchedules
                .Include(cs => cs.Course)
                .Include(cs => cs.Section)
                .Include(cs => cs.Session)
                .Where(cs => cs.TeacherProfileId == teacher.Id)
                .ToListAsync();
            output.AppendLine($"All Teacher's Schedules: {allSchedules.Count}");
            foreach (var schedule in allSchedules)
            {
                output.AppendLine($"  - {schedule.Course?.Name} / {schedule.Section?.Name} / {schedule.Session?.Name}");
                output.AppendLine($"    Day: {schedule.DayOfWeek}, IDs: C={schedule.CourseId}, Sec={schedule.SectionId}, Ses={schedule.SessionId}");
            }
            output.AppendLine();

            // Check ALL enrollments
            var enrollments = await _context.Enrollments
                .Include(e => e.Course)
                .Include(e => e.Section)
                .Include(e => e.Session)
                .Include(e => e.Student!)
                    .ThenInclude(s => s.ApplicationUser)
                .ToListAsync();
            output.AppendLine($"Total Enrollments in System: {enrollments.Count}");

            var groupedEnrollments = enrollments
                .GroupBy(e => new
                {
                    e.CourseId,
                    e.SectionId,
                    e.SessionId,
                    CourseName = e.Course?.Name,
                    SectionName = e.Section?.Name,
                    SessionName = e.Session?.Name
                })
                .ToList();

            foreach (var group in groupedEnrollments)
            {
                output.AppendLine($"  - {group.Key.CourseName} / {group.Key.SectionName} / {group.Key.SessionName}");
                output.AppendLine($"    IDs: Course={group.Key.CourseId}, Section={group.Key.SectionId}, Session={group.Key.SessionId}");
                output.AppendLine($"    Students: {group.Count()}");
                foreach (var enrollment in group.Take(3))
                {
                    output.AppendLine($"      * {enrollment.Student?.ApplicationUser?.FullName} (Roll: {enrollment.Student?.RollNumber})");
                }
            }

            return Content(output.ToString(), "text/plain");
        }

        // Force create attendance for debugging
        public async Task<IActionResult> ForceCreateAttendance(int courseId, int sectionId, int sessionId)
        {
            var teacher = await GetTeacherProfile();
            if (teacher == null)
                return Content("Teacher profile not found");

            var output = new System.Text.StringBuilder();
            output.AppendLine("=== FORCE CREATE ATTENDANCE ===\n");
            output.AppendLine($"CourseId: {courseId}, SectionId: {sectionId}, SessionId: {sessionId}");
            output.AppendLine($"Date: {DateTime.Today:yyyy-MM-dd}\n");

            // Check if already exists
            var existing = await _context.AttendanceRecords
                .Where(a => a.CourseId == courseId &&
                           a.SectionId == sectionId &&
                           a.Date == DateTime.Today)
                .ToListAsync();

            output.AppendLine($"Existing attendance records: {existing.Count}");
            if (existing.Any())
            {
                output.AppendLine("Deleting existing records...");
                _context.AttendanceRecords.RemoveRange(existing);
                await _context.SaveChangesAsync();
            }

            // Get students
            var students = await _context.Enrollments
                .Include(e => e.Student!)
                    .ThenInclude(s => s.ApplicationUser)
                .Where(e => e.CourseId == courseId &&
                           e.SectionId == sectionId &&
                           e.SessionId == sessionId)
                .ToListAsync();

            output.AppendLine($"\nStudents found: {students.Count}");

            if (!students.Any())
            {
                output.AppendLine("\n❌ ERROR: No students enrolled!");
                output.AppendLine("\nChecking what enrollments exist:");

                var allEnrollments = await _context.Enrollments
                    .Include(e => e.Course)
                    .Include(e => e.Section)
                    .Include(e => e.Session)
                    .ToListAsync();

                foreach (var e in allEnrollments.Take(10))
                {
                    output.AppendLine($"  - Course: {e.Course?.Name} (ID: {e.CourseId}), " +
                                    $"Section: {e.Section?.Name} (ID: {e.SectionId}), " +
                                    $"Session: {e.Session?.Name} (ID: {e.SessionId})");
                }
            }
            else
            {
                output.AppendLine("\nCreating attendance records:");
                foreach (var enrollment in students)
                {
                    var record = new AttendanceRecord
                    {
                        CourseId = courseId,
                        SectionId = sectionId,
                        StudentProfileId = enrollment.StudentProfileId,
                        Date = DateTime.Today,
                        Status = AttendanceStatus.Present,
                        MarkedByTeacherId = teacher.Id,
                        CreatedAt = DateTime.UtcNow
                    };
                    _context.AttendanceRecords.Add(record);
                    output.AppendLine($"  ✓ {enrollment.Student?.ApplicationUser?.FullName} (Roll: {enrollment.Student?.RollNumber})");
                }

                await _context.SaveChangesAsync();
                output.AppendLine($"\n✅ SUCCESS: Created {students.Count} attendance records!");
                output.AppendLine($"\nNow visit: /Teacher/CurrentClass");
            }

            return Content(output.ToString(), "text/plain");
        }

        private async Task EnsureAttendanceCreatedForClass(
            int courseId,
            int sectionId,
            int sessionId,
            DateTime date,
            int teacherId)
        {
            Console.WriteLine($"=== EnsureAttendanceCreatedForClass ===");
            Console.WriteLine($"CourseId: {courseId}, SectionId: {sectionId}, SessionId: {sessionId}, Date: {date:yyyy-MM-dd}");

            // Get all students enrolled in this specific course, section, and session
            var enrolledStudents = await _context.Enrollments
                .Include(e => e.Student)
                .Where(e =>
                    e.CourseId == courseId &&
                    e.SectionId == sectionId &&
                    e.SessionId == sessionId)
                .Select(e => e.Student!)
                .ToListAsync();

            Console.WriteLine($"Found {enrolledStudents.Count} enrolled students");

            if (!enrolledStudents.Any())
            {
                // Additional debugging - check what enrollments exist
                var allEnrollments = await _context.Enrollments
                    .Include(e => e.Course)
                    .Include(e => e.Section)
                    .Include(e => e.Session)
                    .Where(e => e.CourseId == courseId || e.SectionId == sectionId || e.SessionId == sessionId)
                    .ToListAsync();

                Console.WriteLine($"Total enrollments with matching IDs: {allEnrollments.Count}");
                foreach (var enrollment in allEnrollments.Take(5))
                {
                    Console.WriteLine($"  - Course: {enrollment.Course?.Name} (ID: {enrollment.CourseId}), " +
                                    $"Section: {enrollment.Section?.Name} (ID: {enrollment.SectionId}), " +
                                    $"Session: {enrollment.Session?.Name} (ID: {enrollment.SessionId})");
                }

                Console.WriteLine($"ERROR: No students found for Course {courseId}, Section {sectionId}, Session {sessionId}");
                return;
            }

            // Get student IDs who already have attendance records for today
            var existingAttendanceStudentIds = await _context.AttendanceRecords
                .Where(a =>
                    a.CourseId == courseId &&
                    a.SectionId == sectionId &&
                    a.Date == date)
                .Select(a => a.StudentProfileId)
                .ToListAsync();

            Console.WriteLine($"Students with existing attendance: {existingAttendanceStudentIds.Count}");

            // Find students who don't have attendance records yet (newly enrolled students)
            var studentsWithoutAttendance = enrolledStudents
                .Where(s => !existingAttendanceStudentIds.Contains(s.Id))
                .ToList();

            Console.WriteLine($"Students needing attendance records: {studentsWithoutAttendance.Count}");

            if (!studentsWithoutAttendance.Any())
            {
                Console.WriteLine("All enrolled students already have attendance records");
                return;
            }

            // Create attendance records for students who don't have them
            foreach (var s in studentsWithoutAttendance)
            {
                Console.WriteLine($"Creating attendance record for student: {s.ApplicationUser?.FullName} (Roll: {s.RollNumber})");
                _context.AttendanceRecords.Add(new AttendanceRecord
                {
                    CourseId = courseId,
                    SectionId = sectionId,
                    StudentProfileId = s.Id,
                    Date = date,
                    Status = AttendanceStatus.Present, // All marked present by default
                    MarkedByTeacherId = teacherId,
                    CreatedAt = DateTime.UtcNow
                });
            }

            await _context.SaveChangesAsync();
            Console.WriteLine($"Successfully created {studentsWithoutAttendance.Count} attendance records");
        }
    }
}
