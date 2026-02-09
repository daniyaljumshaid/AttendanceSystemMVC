using AttendanceSystemMVC.Data;
using AttendanceSystemMVC.Models;
using AttendanceSystemMVC.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AttendanceSystemMVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        // ====== Courses ======
        public async Task<IActionResult> Courses()
        {
            var courses = await _context.Courses.ToListAsync();
            return View("Courses/Index", courses);
        }

        public IActionResult CreateCourse() => View();

        [HttpPost]
        public async Task<IActionResult> CreateCourse(CourseViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var course = new Course
            {
                Name = model.Name,
                Code = model.Code,
                Title = model.Title,
                CreditHours = model.CreditHours
            };

            _context.Courses.Add(course);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Courses));
        }

        public async Task<IActionResult> EditCourse(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null) return NotFound();

            var vm = new CourseViewModel
            {
                Id = course.Id,
                Name = course.Name,
                Code = course.Code,
                Title = course.Title,
                CreditHours = course.CreditHours
            };
            return View("Courses/Edit", vm);
        }

        [HttpPost]
        public async Task<IActionResult> EditCourse(CourseViewModel model)
        {
            if (!ModelState.IsValid) return View("Courses/Edit", model);

            var course = await _context.Courses.FindAsync(model.Id);
            if (course == null) return NotFound();

            course.Name = model.Name;
            course.Code = model.Code;
            course.Title = model.Title;
            course.CreditHours = model.CreditHours;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Courses));
        }

        public async Task<IActionResult> DeleteCourse(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course != null)
            {
                _context.Courses.Remove(course);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Courses));
        }

        // ====== Students ======
        public async Task<IActionResult> Students()
        {
            var students = await _userManager.GetUsersInRoleAsync("Student");
            var vm = students.Select(s => new StudentViewModel
            {
                Id = s.Id,
                FullName = s.FullName,
                RollNumber = s.StudentProfile != null ? s.StudentProfile.RollNumber.ToString() : null,
                IsApproved = s.IsApproved
            }).ToList();

            return View("Students/Index", vm);
        }

        public async Task<IActionResult> ApproveStudent(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            user.IsApproved = true;
            await _userManager.UpdateAsync(user);
            return RedirectToAction(nameof(Students));
        }

        public async Task<IActionResult> RejectStudent(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            user.IsApproved = false;
            await _userManager.UpdateAsync(user);
            return RedirectToAction(nameof(Students));
        }

        // ====== Teachers ======
        public async Task<IActionResult> Teachers()
        {
            var teachers = await _userManager.GetUsersInRoleAsync("Teacher");
            var vm = teachers.Select(t => new TeacherViewModel
            {
                Id = t.Id,
                FullName = t.FullName,
                EmployeeId = t.TeacherProfile?.EmployeeId
            }).ToList();

            return View(vm);
        }
    }
}
