namespace AttendanceSystemMVC.Models
{
    public class CourseSchedule
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public Course? Course { get; set; }
        public int TeacherProfileId { get; set; }
        public TeacherProfile? Teacher { get; set; }
        public int SectionId { get; set; }
        public Section? Section { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int SessionId { get; set; }
        public Session? Session { get; set; }
    }
}