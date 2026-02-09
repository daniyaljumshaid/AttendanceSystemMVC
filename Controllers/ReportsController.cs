using AttendanceSystemMVC.Data;
using AttendanceSystemMVC.ReportService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AttendanceSystemMVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ReportsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ReportsService _reportsService;

        public ReportsController(ApplicationDbContext context, ReportsService reportsService)
        {
            _context = context;
            _reportsService = reportsService;
        }

        // ===================== REPORTS DASHBOARD =====================
        public async Task<IActionResult> Index()
        {
            var stats = new
            {
                TotalStudents = await _context.StudentProfiles
                    .Where(s => s.ApplicationUser != null)
                    .CountAsync(),
                TotalTeachers = await _context.TeacherProfiles
                    .Where(t => t.ApplicationUser != null)
                    .CountAsync(),
                TotalCourses = await _context.Courses.CountAsync(),
                TotalSections = await _context.Sections.CountAsync(),
                TotalEnrollments = await _context.Enrollments.CountAsync(),
                TotalCourseAssignments = await _context.CourseAssignments.CountAsync()
            };

            return View(stats);
        }

        // ===================== ANALYTICS =====================
        public async Task<IActionResult> Analytics()
        {
            var enrollmentsBySection = await _context.Enrollments
                .Include(e => e.Section)
                .Where(e => e.Section != null && e.Section.Name != null)
                .GroupBy(e => e.Section!.Name)
                .Select(g => new { Section = g.Key, Count = g.Count() })
                .OrderByDescending(g => g.Count)
                .ToListAsync();

            var enrollmentsByCourse = await _context.Enrollments
                .Include(e => e.Course)
                .Where(e => e.Course != null && e.Course.Name != null)
                .GroupBy(e => e.Course!.Name)
                .Select(g => new { Course = g.Key, Count = g.Count() })
                .OrderByDescending(g => g.Count)
                .ToListAsync();

            ViewBag.EnrollmentsBySection = enrollmentsBySection;
            ViewBag.EnrollmentsByCourse = enrollmentsByCourse;

            return View();
        }

        // ===================== STUDENT ENROLLMENT REPORT =====================
        public async Task<IActionResult> StudentEnrollments()
        {
            var enrollments = await _context.Enrollments
                .AsNoTracking()
                .Include(e => e.Student).ThenInclude(s => s!.ApplicationUser)
                .Include(e => e.Course)
                .Include(e => e.Section)
                .Include(e => e.Session)
                .OrderBy(e => e.Student!.RollNumber)
                .ToListAsync();

            return View(enrollments);
        }

        // ===================== TEACHER ASSIGNMENTS REPORT =====================
        public async Task<IActionResult> TeacherAssignments()
        {
            var assignments = await _context.CourseAssignments
                .AsNoTracking()
                .Include(ca => ca.Teacher).ThenInclude(t => t!.ApplicationUser)
                .Include(ca => ca.Course)
                .Include(ca => ca.Section)
                .Include(ca => ca.Session)
                .OrderBy(ca => ca.Teacher!.EmployeeId)
                .ToListAsync();

            return View(assignments);
        }

        // ===================== ATTENDANCE REPORTS WITH FILTERS =====================
        public async Task<IActionResult> AttendanceReport(int? courseId, int? studentId, DateTime? startDate, DateTime? endDate, int? sectionId, int? sessionId)
        {
            // Populate dropdowns
            ViewBag.Courses = await _context.Courses.OrderBy(c => c.Name).ToListAsync();
            ViewBag.Students = await _context.StudentProfiles
                .Include(s => s.ApplicationUser)
                .Where(s => s.ApplicationUser != null)
                .OrderBy(s => s.RollNumber)
                .ToListAsync();
            ViewBag.Sections = await _context.Sections.OrderBy(s => s.Name).ToListAsync();
            ViewBag.Sessions = await _context.Sessions.OrderByDescending(s => s.StartDate).ToListAsync();

            // Set default date range if not provided
            if (!startDate.HasValue)
                startDate = DateTime.Today.AddMonths(-1);
            if (!endDate.HasValue)
                endDate = DateTime.Today;

            // Build query
            var query = _context.AttendanceRecords
                .Include(ar => ar.Student)
                    .ThenInclude(s => s!.ApplicationUser)
                .Include(ar => ar.Course)
                .Include(ar => ar.Section)
                .Include(ar => ar.MarkedByTeacher)
                    .ThenInclude(t => t!.ApplicationUser)
                .AsQueryable();

            // Apply filters
            if (courseId.HasValue)
                query = query.Where(ar => ar.CourseId == courseId.Value);

            if (studentId.HasValue)
                query = query.Where(ar => ar.StudentProfileId == studentId.Value);

            if (sectionId.HasValue)
                query = query.Where(ar => ar.SectionId == sectionId.Value);

            if (startDate.HasValue)
                query = query.Where(ar => ar.Date >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(ar => ar.Date <= endDate.Value);

            var records = await query
                .OrderByDescending(ar => ar.Date)
                .ThenBy(ar => ar.Student!.RollNumber)
                .ToListAsync();

            // Calculate statistics
            var stats = new
            {
                TotalRecords = records.Count,
                TotalPresent = records.Count(r => r.Status == AttendanceSystemMVC.Models.AttendanceStatus.Present),
                TotalAbsent = records.Count(r => r.Status == AttendanceSystemMVC.Models.AttendanceStatus.Absent),
                TotalLate = records.Count(r => r.Status == AttendanceSystemMVC.Models.AttendanceStatus.Late),
                AttendancePercentage = records.Count > 0 ?
                    Math.Round((double)records.Count(r => r.Status == AttendanceSystemMVC.Models.AttendanceStatus.Present) / records.Count * 100, 1) : 0
            };

            ViewBag.Stats = stats;
            ViewBag.SelectedCourseId = courseId;
            ViewBag.SelectedStudentId = studentId;
            ViewBag.SelectedSectionId = sectionId;
            ViewBag.SelectedSessionId = sessionId;
            ViewBag.StartDate = startDate?.ToString("yyyy-MM-dd");
            ViewBag.EndDate = endDate?.ToString("yyyy-MM-dd");

            return View(records);
        }
    }
}