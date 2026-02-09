using System.ComponentModel.DataAnnotations;

namespace AttendanceSystemMVC.ViewModels
{
    public class EnrollmentViewModel
    {
        [Required(ErrorMessage = "Student is required")]
        [Display(Name = "Student")]
        public int StudentProfileId { get; set; }

        [Required(ErrorMessage = "Course is required")]
        [Display(Name = "Course")]
        public int CourseId { get; set; }

        [Required(ErrorMessage = "Section is required")]
        [Display(Name = "Section")]
        public int SectionId { get; set; }

        [Required(ErrorMessage = "Session is required")]
        [Display(Name = "Session")]
        public int SessionId { get; set; }
    }
}
