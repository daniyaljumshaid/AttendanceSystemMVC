namespace AttendanceSystemMVC.Models
{
    public class TeacherProfile
    {
        public int Id { get; set; }
        public string? EmployeeId { get; set; }
        public string? ApplicationUserId { get; set; }

        // Navigation property to ApplicationUser
        public ApplicationUser? ApplicationUser { get; set; }

        // Navigation collections used by scaffolded mappings and views
        public virtual ICollection<CourseAssignment> CourseAssignments { get; set; } = new List<CourseAssignment>();
        public virtual ICollection<CourseSchedule> CourseSchedules { get; set; } = new List<CourseSchedule>();
    }
}