using AttendanceSystemMVC.Models;

namespace AttendanceSystemMVC.ViewModels
{
    public class StudentMonthlyReportVm
    {
        public StudentProfile? Student { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public List<StudentCourseAttendanceVm> CourseReports { get; set; } = new();
        public int TotalClasses { get; set; }
        public int TotalPresent { get; set; }
        public int TotalAbsent { get; set; }
        public int TotalLate { get; set; }
        public double OverallAttendancePercentage { get; set; }
        public string MonthName => new DateTime(Year, Month, 1).ToString("MMMM yyyy");
        
        // Backward compatibility properties
        public int TotalDays => TotalClasses;
        public int Present => TotalPresent;
        public int Absent => TotalAbsent;
        public double Percentage => OverallAttendancePercentage;
    }
}
