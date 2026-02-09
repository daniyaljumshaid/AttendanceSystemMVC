namespace AttendanceSystemMVC.Models
{
    public class StudentCourseAssignment
    {
        public int Id { get; set; }
        public int StudentProfileId { get; set; }
        public StudentProfile? Student { get; set; }
        public int CourseId { get; set; }
        public Course? Course { get; set; }
        public int SessionId { get; set; }
        public Session? Session { get; set; }
        public int SectionId { get; set; }
        public Section? Section { get; set; }
        public DateTime AssignedDate { get; set; }
    }
}
