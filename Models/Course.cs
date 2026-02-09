namespace AttendanceSystemMVC.Models
{
    public class Course
    {
        public int Id { get; set; }

        // DB has both Name and Title
        public string? Name { get; set; }

        // Code is string in the database (not int)
        public string? Code { get; set; }

        public string? Title { get; set; }
        public int CreditHours { get; set; }
    }
}
