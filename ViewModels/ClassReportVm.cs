namespace AttendanceSystemMVC.ViewModels
{
    public class ClassReportVm
    {
        public int TotalDays { get; set; }
        public int Present { get; set; }
        public int Absent { get; set; }
        public double Percentage { get; set; }

        // context
        public int CourseId { get; set; }
        public int SectionId { get; set; }
        public int? SessionId { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }

        // For multi-month reports
        public List<MonthlyBreakdown>? MonthlyBreakdowns { get; set; }
    }

    public class MonthlyBreakdown
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public string MonthName { get; set; } = string.Empty;
        public int TotalDays { get; set; }
        public int Present { get; set; }
        public int Absent { get; set; }
        public double Percentage { get; set; }
    }
}
