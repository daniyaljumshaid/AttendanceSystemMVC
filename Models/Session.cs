namespace AttendanceSystemMVC.Models { 
public class Session
{
    public int Id { get; set; }
    public string? Name { get; set; } // e.g. "2024-25"
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}
}