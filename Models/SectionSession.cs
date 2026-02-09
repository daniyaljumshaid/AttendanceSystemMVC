namespace AttendanceSystemMVC.Models
{
    public class SectionSession
    {
        public int Id { get; set; }
        public int SectionId { get; set; }
        public Section? Section { get; set; }
        public int SessionId { get; set; }
        public Session? Session { get; set; }
    }
}
