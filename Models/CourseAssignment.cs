namespace AttendanceSystemMVC.Models
{
    public class CourseAssignment
    {
        public int Id { get; set; }
        public int TeacherProfileId { get; set; }
        public TeacherProfile? Teacher { get; set; }
        public int CourseId { get; set; }
        public Course? Course { get; set; }
        public int SessionId { get; set; }
        public Session? Session { get; set; }
        public int SectionId { get; set; }
        public Section? Section { get; set; }
    }
}