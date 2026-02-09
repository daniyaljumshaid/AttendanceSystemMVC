using System.ComponentModel.DataAnnotations;

namespace AttendanceSystemMVC.ViewModels
{
    public class StudentViewModel
    {
        public string? Id { get; set; } // ApplicationUser Id

        [Required(ErrorMessage = "Full name is required")]
        [StringLength(100, ErrorMessage = "Full name cannot exceed 100 characters")]
        [Display(Name = "Full Name")]
        public string? FullName { get; set; }

        [Required(ErrorMessage = "Roll number is required")]
        [StringLength(20, ErrorMessage = "Roll number cannot exceed 20 characters")]
        [Display(Name = "Roll Number")]
        public string? RollNumber { get; set; }

        [Display(Name = "Approved")]
        public bool IsApproved { get; set; }
    }
}
