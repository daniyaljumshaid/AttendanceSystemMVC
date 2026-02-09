namespace AttendanceSystemMVC.ViewModels
{
    public class StudentAttendanceStatsVm
    {
        public int TotalClasses { get; set; }
        public int PresentClasses { get; set; }
        public int AbsentClasses { get; set; }
        public int LateClasses { get; set; }
        public double AttendancePercentage { get; set; }
    }
}