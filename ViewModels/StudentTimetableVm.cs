

namespace AttendanceSystemMVC.ViewModels
{
    public class StudentTimetableVm
    {
        public string ?CourseTitle { get; set; }
        public string ?SectionName { get; set; }
        public DayOfWeek Day { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string? TeacherName { get; set; }
    }
}
