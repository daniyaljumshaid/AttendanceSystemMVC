using AttendanceSystemMVC.Models;

namespace AttendanceSystemMVC.ViewModels
{
    public class TeacherDashboardVm
    {
        public TeacherProfile? TeacherProfile { get; set; }
        public List<CourseSchedule> TodaySchedules { get; set; } = new();
        public List<CourseSchedule> WeekSchedules { get; set; } = new();
        public CourseSchedule? CurrentClass { get; set; }
        public List<Course> AssignedCourses { get; set; } = new();
        public int TotalStudentsToday { get; set; }
        public int PendingAttendance { get; set; }
        public bool IsApproved { get; set; }
        public string WelcomeMessage { get; set; } = string.Empty;
        public Session? CurrentSession { get; set; }
    }
}