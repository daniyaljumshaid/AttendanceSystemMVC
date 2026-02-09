using System.ComponentModel.DataAnnotations;
using AttendanceSystemMVC.Models;

namespace AttendanceSystemMVC.ViewModels
{
    public class AttendanceEditVm
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Attendance status is required")]
        [Display(Name = "Status")]
        public AttendanceStatus? Status { get; set; }

        [Display(Name = "Student Name")]
        public string StudentName { get; set; } = string.Empty;

        [Display(Name = "Roll Number")]
        public int RollNumber { get; set; }
    }
}