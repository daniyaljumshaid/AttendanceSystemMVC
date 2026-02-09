using AttendanceSystemMVC.Models;

namespace AttendanceSystemMVC.ViewModels
{
    public class StudentCourseAttendanceVm
    {
        public string CourseName { get; set; } = string.Empty;
        public int CourseId { get; set; }
        public int TotalClasses { get; set; }
        public int PresentClasses { get; set; }
        public int AbsentClasses { get; set; }
        public int LateClasses { get; set; }
        public double AttendancePercentage { get; set; }
        public List<AttendanceRecord> Records { get; set; } = new();
    }
}