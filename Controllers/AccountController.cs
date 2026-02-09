using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using AttendanceSystemMVC.Models;
using AttendanceSystemMVC.ViewModels;
using System.Threading.Tasks;

namespace AttendanceSystemMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        // GET: /Account/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginVm());
        }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVm model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Username);
            if (user != null && !user.IsApproved && !await _userManager.IsInRoleAsync(user, "Admin"))
            {
                ViewBag.Error = "Your account is pending approval. Please contact the administrator.";
                return View(model);
            }

            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user, model.Password, isPersistent: model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    // If user must change password on first login, redirect to change password
                    if (user.MustChangePassword)
                    {
                        return RedirectToAction("ChangePassword", new { firstTime = true });
                    }

                    // Redirect based on role
                    if (await _userManager.IsInRoleAsync(user, "Admin"))
                        return RedirectToAction("Index", "Admin");
                    else if (await _userManager.IsInRoleAsync(user, "Teacher"))
                        return RedirectToAction("Index", "Teacher");
                    else if (await _userManager.IsInRoleAsync(user, "Student"))
                        return RedirectToAction("Index", "Student");

                    return RedirectToAction("Index", "Home");
                }
            }

            ViewBag.Error = "Invalid username or password";
            return View(model);
        }

        // GET: /Account/Register (teacher only - students cannot register)
        [HttpGet]
        public IActionResult Register()
        {
            ViewBag.Role = "Teacher";
            ViewBag.Message = "Only teachers can register through this form. Students are registered by administrators.";
            return View(new TeacherRegistrationVm());
        }

        // POST: /Account/Register (teacher only)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(TeacherRegistrationVm model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Role = "Teacher";
                return View(model);
            }

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FullName = model.FullName,
                IsApproved = false, // Teachers need admin approval
                MustChangePassword = true
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                ViewBag.Error = string.Join("; ", result.Errors.Select(e => e.Description));
                ViewBag.Role = "Teacher";
                return View(model);
            }

            // Assign teacher role
            await _userManager.AddToRoleAsync(user, "Teacher");

            // Create teacher profile
            var context = HttpContext.RequestServices.GetService(typeof(AttendanceSystemMVC.Data.ApplicationDbContext)) as AttendanceSystemMVC.Data.ApplicationDbContext;
            if (context != null)
            {
                context.TeacherProfiles.Add(new TeacherProfile { ApplicationUserId = user.Id, EmployeeId = model.EmployeeId });
                await context.SaveChangesAsync();
            }

            ViewBag.Message = "Teacher registration successful. Your account is pending approval by the administrator. You will be notified once approved.";
            return View("RegisterConfirmation");
        }

        // GET: /Account/Logout
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }

        // GET: /Account/ChangePassword
        [HttpGet]
        public IActionResult ChangePassword(bool firstTime = false)
        {
            ViewBag.FirstTime = firstTime;
            return View();
        }

        // POST: /Account/ChangePassword
        [HttpPost]
        public async Task<IActionResult> ChangePassword(string oldPassword, string newPassword)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login");
            }

            var result = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
            if (!result.Succeeded)
            {
                ViewBag.Error = string.Join("; ", result.Errors.Select(e => e.Description));
                return View();
            }

            user.MustChangePassword = false;
            await _userManager.UpdateAsync(user);

            // After changing password, redirect based on role
            if (await _userManager.IsInRoleAsync(user, "Admin"))
                return RedirectToAction("Index", "Admin");
            else if (await _userManager.IsInRoleAsync(user, "Teacher"))
                return RedirectToAction("Index", "Teacher");
            else if (await _userManager.IsInRoleAsync(user, "Student"))
                return RedirectToAction("Index", "Student");

            return RedirectToAction("Index", "Home");
        }
    }
}
