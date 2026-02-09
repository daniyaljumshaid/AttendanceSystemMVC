using System.ComponentModel.DataAnnotations;

namespace AttendanceSystemMVC.ViewModels
{
    public class TeacherViewModel
    {
        public string? Id { get; set; } // ApplicationUser Id

        [Required(ErrorMessage = "Full name is required")]
        [StringLength(100, ErrorMessage = "Full name cannot exceed 100 characters")]
        [Display(Name = "Full Name")]
        public string? FullName { get; set; }

        [Required(ErrorMessage = "Employee ID is required")]
        [StringLength(50, ErrorMessage = "Employee ID cannot exceed 50 characters")]
        [Display(Name = "Employee ID")]
        public string? EmployeeId { get; set; }
    }
}
