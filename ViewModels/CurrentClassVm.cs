using AttendanceSystemMVC.Models;

namespace AttendanceSystemMVC.ViewModels
{
    public class CurrentClassVm
    {
        public CourseSchedule? Schedule { get; set; }
        public List<AttendanceRecord> Attendance { get; set; } = new();
        public string CourseName => Schedule?.Course?.Name ?? "Unknown Course";
        public string SectionName => Schedule?.Section?.Name ?? "Unknown Section";
        public string TimeSlot => Schedule != null ? $"{Schedule.StartTime:hh\\:mm} - {Schedule.EndTime:hh\\:mm}" : "";
        public DateTime CurrentDate => DateTime.Today;
    }
}