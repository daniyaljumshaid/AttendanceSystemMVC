using System.ComponentModel.DataAnnotations;

namespace AttendanceSystemMVC.ViewModels
{
    public class LoginVm
    {
        [Required(ErrorMessage = "Email/Username is required")]
        [Display(Name = "Email / Username")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }
    }
}
