using Microsoft.AspNetCore.Identity;

namespace AttendanceSystemMVC.Models
{
    public class StudentProfile
    {
        public int Id { get; set; }

        // Link to Identity User
        public string? ApplicationUserId { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }

        public int RollNumber { get; set; }
        public string ?FullName { get; set; }
        public int SectionId { get; set; }
        public Section? Section { get; set; }

    }
}
