using System.ComponentModel.DataAnnotations;

namespace AttendanceSystemMVC.ViewModels
{
    public class CourseViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Course name is required")]
        [StringLength(100, ErrorMessage = "Course name cannot exceed 100 characters")]
        [Display(Name = "Course Name")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Course code is required")]
        [StringLength(20, ErrorMessage = "Course code cannot exceed 20 characters")]
        [RegularExpression(@"^[A-Z]{2,4}[0-9]{3,4}$", ErrorMessage = "Course code must be in format like CS101 or MATH1001")]
        [Display(Name = "Course Code")]
        public string? Code { get; set; }

        [Required(ErrorMessage = "Course title is required")]
        [StringLength(200, ErrorMessage = "Course title cannot exceed 200 characters")]
        [Display(Name = "Course Title")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "Credit hours is required")]
        [Range(1, 6, ErrorMessage = "Credit hours must be between 1 and 6")]
        [Display(Name = "Credit Hours")]
        public int CreditHours { get; set; }
    }
}
