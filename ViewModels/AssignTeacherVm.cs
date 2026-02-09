using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AttendanceSystemMVC.ViewModels
{
    public class AssignTeacherVm
    {
        [Required(ErrorMessage = "Please select a teacher")]
        [Display(Name = "Teacher")]
        public int TeacherProfileId { get; set; }

        [Required(ErrorMessage = "Please select a course")]
        [Display(Name = "Course")]
        public int CourseId { get; set; }

        [Required(ErrorMessage = "Please select a session")]
        [Display(Name = "Session")]
        public int SessionId { get; set; }

        [Required(ErrorMessage = "Please select a section")]
        [Display(Name = "Section")]
        public int SectionId { get; set; }

        public List<SelectListItem>? Teachers { get; set; }
        public List<SelectListItem>? Courses { get; set; }
        public List<SelectListItem>? Sessions { get; set; }
        public List<SelectListItem>? Sections { get; set; }
    }
}
