using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace AttendanceSystemMVC.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string ?FullName { get; set; }
        public bool IsApproved { get; set; } = false; // students need admin approval
        public bool MustChangePassword { get; set; } = true; // force change on first login


        public virtual StudentProfile? StudentProfile { get; set; }
        public virtual TeacherProfile? TeacherProfile { get; set; }
    }
}