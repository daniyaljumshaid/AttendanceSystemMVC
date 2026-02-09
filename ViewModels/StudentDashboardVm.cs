using AttendanceSystemMVC.Models;

namespace AttendanceSystemMVC.ViewModels
{
    public class StudentDashboardVm
    {
        public StudentProfile? Student { get; set; }
        public Session? CurrentSession { get; set; }
        public List<Course> EnrolledCourses { get; set; } = new();
        public List<CourseSchedule> TodaySchedules { get; set; } = new();
        public StudentAttendanceStatsVm AttendanceStats { get; set; } = new();
        public string WelcomeMessage => $"Welcome, {Student?.FullName ?? "Student"}!";
    }
}