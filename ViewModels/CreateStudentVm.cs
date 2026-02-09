using System.ComponentModel.DataAnnotations;

namespace AttendanceSystemMVC.ViewModels
{
    public class CreateStudentVm
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [Display(Name = "Email Address")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Full name is required")]
        [StringLength(100, ErrorMessage = "Full name cannot exceed 100 characters")]
        [Display(Name = "Full Name")]
        public string? FullName { get; set; }

        [Required(ErrorMessage = "Roll number is required")]
        [Range(1, 999999, ErrorMessage = "Roll number must be between 1 and 999999")]
        [Display(Name = "Roll Number")]
        public int RollNumber { get; set; }

        [Required(ErrorMessage = "Section is required")]
        [Display(Name = "Section")]
        public int? SectionId { get; set; }
    }
}
