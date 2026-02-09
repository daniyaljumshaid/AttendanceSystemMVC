using AttendanceSystemMVC.Data;
using AttendanceSystemMVC.Models;
using AttendanceSystemMVC.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace AttendanceSystemMVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminManagementController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminManagementController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // ===================== DASHBOARD =====================
        public IActionResult Index()
        {
            return View();
        }

        // ===================== COURSES =====================
        public async Task<IActionResult> Courses()
        {
            var courses = await _context.Courses
                .AsNoTracking()
                .OrderBy(c => c.Name)
                .ToListAsync();
            return View("Courses/Index", courses);
        }

        public IActionResult CreateCourse()
        {
            return View("Courses/Create");
        }

        [HttpPost]
        public async Task<IActionResult> CreateCourse(Course model)
        {
            if (!ModelState.IsValid)
            {
                return View("Courses/Create", model);
            }

            _context.Courses.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Courses));
        }

        public async Task<IActionResult> EditCourse(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null) return NotFound();
            return View("Courses/Edit", course);
        }

        [HttpPost]
        public async Task<IActionResult> EditCourse(Course model)
        {
            if (!ModelState.IsValid)
            {
                return View("Courses/Edit", model);
            }

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
                // Delete all related records first to avoid foreign key constraint violations

                // 1. Delete AttendanceRecords
                var attendanceRecords = await _context.AttendanceRecords
                    .Where(a => a.CourseId == id)
                    .ToListAsync();
                _context.AttendanceRecords.RemoveRange(attendanceRecords);

                // 2. Delete CourseAssignments
                var courseAssignments = await _context.CourseAssignments
                    .Where(ca => ca.CourseId == id)
                    .ToListAsync();
                _context.CourseAssignments.RemoveRange(courseAssignments);

                // 3. Delete CourseSchedules
                var courseSchedules = await _context.CourseSchedules
                    .Where(cs => cs.CourseId == id)
                    .ToListAsync();
                _context.CourseSchedules.RemoveRange(courseSchedules);

                // 4. Delete Enrollments
                var enrollments = await _context.Enrollments
                    .Where(e => e.CourseId == id)
                    .ToListAsync();
                _context.Enrollments.RemoveRange(enrollments);

                // 5. Delete StudentCourseAssignments
                var studentCourseAssignments = await _context.StudentCourseAssignments
                    .Where(sca => sca.CourseId == id)
                    .ToListAsync();
                _context.StudentCourseAssignments.RemoveRange(studentCourseAssignments);

                // Finally, delete the course itself
                _context.Courses.Remove(course);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Courses));
        }

        // ===================== SECTIONS =====================
        public async Task<IActionResult> Sections()
        {
            var sections = await _context.Sections
                .AsNoTracking()
                .OrderBy(s => s.Name)
                .ToListAsync();
            return View("Sections/Index", sections);
        }

        public IActionResult CreateSection()
        {
            return View("Sections/Create");
        }

        [HttpPost]
        public async Task<IActionResult> CreateSection(Section model)
        {
            if (!ModelState.IsValid)
            {
                return View("Sections/Create", model);
            }

            _context.Sections.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Sections));
        }

        public async Task<IActionResult> EditSection(int id)
        {
            var section = await _context.Sections.FindAsync(id);
            if (section == null) return NotFound();
            return View("Sections/Edit", section);
        }

        [HttpPost]
        public async Task<IActionResult> EditSection(Section model)
        {
            if (!ModelState.IsValid)
            {
                return View("Sections/Edit", model);
            }

            var section = await _context.Sections.FindAsync(model.Id);
            if (section == null) return NotFound();

            section.Name = model.Name;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Sections));
        }

        public async Task<IActionResult> DeleteSection(int id)
        {
            var section = await _context.Sections.FindAsync(id);
            if (section != null)
            {
                _context.Sections.Remove(section);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Sections));
        }

        // ===================== SESSIONS =====================
        public async Task<IActionResult> Sessions()
        {
            var sessions = await _context.Sessions
                .AsNoTracking()
                .OrderByDescending(s => s.StartDate)
                .ToListAsync();
            return View("Sessions/Index", sessions);
        }

        public IActionResult CreateSession()
        {
            return View("Sessions/Create");
        }

        [HttpPost]
        public async Task<IActionResult> CreateSession(Session model)
        {
            if (!ModelState.IsValid)
            {
                return View("Sessions/Create", model);
            }

            _context.Sessions.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Sessions));
        }

        public async Task<IActionResult> EditSession(int id)
        {
            var session = await _context.Sessions.FindAsync(id);
            if (session == null) return NotFound();
            return View("Sessions/Edit", session);
        }

        [HttpPost]
        public async Task<IActionResult> EditSession(Session model)
        {
            if (!ModelState.IsValid)
            {
                return View("Sessions/Edit", model);
            }

            var session = await _context.Sessions.FindAsync(model.Id);
            if (session == null) return NotFound();

            session.Name = model.Name;
            session.StartDate = model.StartDate;
            session.EndDate = model.EndDate;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Sessions));
        }

        public async Task<IActionResult> DeleteSession(int id)
        {
            var session = await _context.Sessions.FindAsync(id);
            if (session != null)
            {
                _context.Sessions.Remove(session);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Sessions));
        }

        // ===================== TEACHERS =====================
        public async Task<IActionResult> Teachers()
        {
            var teachers = await _context.TeacherProfiles
                .AsNoTracking()
                .Include(t => t.ApplicationUser)
                .OrderBy(t => t.EmployeeId)
                .ToListAsync();

            return View("Teachers/Index", teachers);
        }

        public IActionResult CreateTeacher()
        {
            return View("Teachers/Create");
        }

        [HttpPost]
        public async Task<IActionResult> CreateTeacher(string email, string fullName, string employeeId, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.Error = "Email and password are required.";
                return View("Teachers/Create");
            }

            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                FullName = fullName,
                IsApproved = true,
                MustChangePassword = true
            };

            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                ViewBag.Error = string.Join("; ", result.Errors.Select(e => e.Description));
                return View("Teachers/Create");
            }

            await _userManager.AddToRoleAsync(user, "Teacher");

            _context.TeacherProfiles.Add(new TeacherProfile
            {
                ApplicationUserId = user.Id,
                EmployeeId = employeeId
            });

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Teachers));
        }

        public async Task<IActionResult> EditTeacher(int id)
        {
            var teacher = await _context.TeacherProfiles
                .Include(t => t.ApplicationUser)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (teacher == null) return NotFound();
            return View("Teachers/Edit", teacher);
        }

        [HttpPost]
        public async Task<IActionResult> EditTeacher(int id, string fullName, string employeeId)
        {
            var teacher = await _context.TeacherProfiles
                .Include(t => t.ApplicationUser)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (teacher == null) return NotFound();

            if (teacher.ApplicationUser != null)
            {
                teacher.ApplicationUser.FullName = fullName;
                await _userManager.UpdateAsync(teacher.ApplicationUser);
            }

            teacher.EmployeeId = employeeId;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Teachers));
        }

        public async Task<IActionResult> DeleteTeacher(int id)
        {
            var teacher = await _context.TeacherProfiles.FindAsync(id);
            if (teacher != null)
            {
                _context.TeacherProfiles.Remove(teacher);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Teachers));
        }

        // ===================== STUDENTS =====================
        public async Task<IActionResult> Students()
        {
            var students = await _context.StudentProfiles
                .AsNoTracking()
                .Include(s => s.ApplicationUser)
                .Include(s => s.Section)
                .OrderBy(s => s.RollNumber)
                .ToListAsync();

            return View("Students/Index", students);
        }

        public async Task<IActionResult> CreateStudent()
        {
            ViewBag.Sections = await _context.Sections
                .OrderBy(s => s.Name)
                .ToListAsync();
            return View("Students/Create");
        }

        [HttpPost]
        public async Task<IActionResult> CreateStudent(CreateStudentVm vm)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Sections = await _context.Sections
                    .OrderBy(s => s.Name)
                    .ToListAsync();
                return View("Students/Create", vm);
            }

            var user = new ApplicationUser
            {
                UserName = vm.Email,
                Email = vm.Email,
                FullName = vm.FullName,
                MustChangePassword = true,
                IsApproved = true
            };

            var result = await _userManager.CreateAsync(user, "Student@123");
            if (!result.Succeeded)
            {
                ViewBag.Error = string.Join("; ", result.Errors.Select(e => e.Description));
                ViewBag.Sections = await _context.Sections
                    .OrderBy(s => s.Name)
                    .ToListAsync();
                return View("Students/Create", vm);
            }

            await _userManager.AddToRoleAsync(user, "Student");

            _context.StudentProfiles.Add(new StudentProfile
            {
                ApplicationUserId = user.Id,
                FullName = vm.FullName,
                RollNumber = vm.RollNumber,
                SectionId = vm.SectionId ?? 0
            });

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Students));
        }

        public async Task<IActionResult> EditStudent(int id)
        {
            var student = await _context.StudentProfiles
                .Include(s => s.ApplicationUser)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (student == null) return NotFound();

            ViewBag.Sections = await _context.Sections
                .OrderBy(s => s.Name)
                .ToListAsync();
            return View("Students/Edit", student);
        }

        [HttpPost]
        public async Task<IActionResult> EditStudent(int id, string fullName, int rollNumber, int sectionId)
        {
            var student = await _context.StudentProfiles
                .Include(s => s.ApplicationUser)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (student == null) return NotFound();

            if (student.ApplicationUser != null)
            {
                student.ApplicationUser.FullName = fullName;
                await _userManager.UpdateAsync(student.ApplicationUser);
            }

            student.FullName = fullName;
            student.RollNumber = rollNumber;
            student.SectionId = sectionId;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Students));
        }

        public async Task<IActionResult> DeleteStudent(int id)
        {
            var student = await _context.StudentProfiles
                .Include(s => s.ApplicationUser)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (student != null)
            {
                // Delete all related records first to avoid foreign key constraint violations

                // 1. Delete AttendanceRecords
                var attendanceRecords = await _context.AttendanceRecords
                    .Where(a => a.StudentProfileId == id)
                    .ToListAsync();
                _context.AttendanceRecords.RemoveRange(attendanceRecords);

                // 2. Delete Enrollments
                var enrollments = await _context.Enrollments
                    .Where(e => e.StudentProfileId == id)
                    .ToListAsync();
                _context.Enrollments.RemoveRange(enrollments);

                // 3. Delete StudentCourseAssignments
                var studentCourseAssignments = await _context.StudentCourseAssignments
                    .Where(sca => sca.StudentProfileId == id)
                    .ToListAsync();
                _context.StudentCourseAssignments.RemoveRange(studentCourseAssignments);

                // 4. Delete the StudentProfile
                _context.StudentProfiles.Remove(student);

                // 5. Delete the associated ApplicationUser if exists
                if (student.ApplicationUser != null)
                {
                    await _userManager.DeleteAsync(student.ApplicationUser);
                }

                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Students));
        }

        // ===================== USERS =====================
        public async Task<IActionResult> Users()
        {
            var users = await _userManager.Users
                .OrderBy(u => u.Email)
                .ToListAsync();

            // Create a list with users and their roles
            var usersWithRoles = new List<(ApplicationUser User, IList<string> Roles)>();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                usersWithRoles.Add((user, roles));
            }

            return View("Users/Index", usersWithRoles);
        }

        public async Task<IActionResult> CreateUser()
        {
            ViewBag.Roles = new[] { "Admin", "Teacher", "Student" };
            ViewBag.Sections = await _context.Sections.OrderBy(s => s.Name).ToListAsync();
            return View("Users/Create");
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(string email, string fullName, string role, string password,
            int? rollNumber, int? sectionId, string? employeeId)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.Error = "Email and password are required.";
                ViewBag.Roles = new[] { "Admin", "Teacher", "Student" };
                ViewBag.Sections = await _context.Sections.OrderBy(s => s.Name).ToListAsync();
                return View("Users/Create");
            }

            // Validate role-specific fields
            if (role == "Student")
            {
                if (!rollNumber.HasValue || rollNumber.Value <= 0)
                {
                    ViewBag.Error = "Roll Number is required for students.";
                    ViewBag.Roles = new[] { "Admin", "Teacher", "Student" };
                    ViewBag.Sections = await _context.Sections.OrderBy(s => s.Name).ToListAsync();
                    return View("Users/Create");
                }
                if (!sectionId.HasValue || sectionId.Value <= 0)
                {
                    ViewBag.Error = "Section is required for students.";
                    ViewBag.Roles = new[] { "Admin", "Teacher", "Student" };
                    ViewBag.Sections = await _context.Sections.OrderBy(s => s.Name).ToListAsync();
                    return View("Users/Create");
                }
            }
            else if (role == "Teacher")
            {
                if (string.IsNullOrWhiteSpace(employeeId))
                {
                    ViewBag.Error = "Employee ID is required for teachers.";
                    ViewBag.Roles = new[] { "Admin", "Teacher", "Student" };
                    ViewBag.Sections = await _context.Sections.OrderBy(s => s.Name).ToListAsync();
                    return View("Users/Create");
                }
            }

            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                FullName = fullName,
                IsApproved = true,
                MustChangePassword = role != "Admin"
            };

            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                ViewBag.Error = string.Join("; ", result.Errors.Select(e => e.Description));
                ViewBag.Roles = new[] { "Admin", "Teacher", "Student" };
                ViewBag.Sections = await _context.Sections.OrderBy(s => s.Name).ToListAsync();
                return View("Users/Create");
            }

            if (!string.IsNullOrWhiteSpace(role))
            {
                await _userManager.AddToRoleAsync(user, role);

                // Auto-create Student or Teacher profile based on role
                if (role == "Student")
                {
                    _context.StudentProfiles.Add(new StudentProfile
                    {
                        ApplicationUserId = user.Id,
                        FullName = fullName,
                        RollNumber = rollNumber!.Value,
                        SectionId = sectionId!.Value
                    });
                }
                else if (role == "Teacher")
                {
                    _context.TeacherProfiles.Add(new TeacherProfile
                    {
                        ApplicationUserId = user.Id,
                        EmployeeId = employeeId
                    });
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Users));
        }

        public async Task<IActionResult> EditUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var userRoles = await _userManager.GetRolesAsync(user);
            ViewBag.UserRoles = userRoles;
            ViewBag.Roles = new[] { "Admin", "Teacher", "Student" };

            return View("Users/Edit", user);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(string id, string fullName, string role, IFormCollection form)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            // Handle checkbox values properly
            bool isApproved = form["isApproved"].ToString().Contains("true");
            bool mustChangePassword = form["mustChangePassword"].ToString().Contains("true");

            user.FullName = fullName;
            user.IsApproved = isApproved;
            user.MustChangePassword = mustChangePassword;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                ViewBag.Error = string.Join("; ", result.Errors.Select(e => e.Description));
                var userRoles = await _userManager.GetRolesAsync(user);
                ViewBag.UserRoles = userRoles;
                ViewBag.Roles = new[] { "Admin", "Teacher", "Student" };
                return View("Users/Edit", user);
            }

            // Update role if changed
            var currentRoles = await _userManager.GetRolesAsync(user);
            if (!string.IsNullOrWhiteSpace(role) && !currentRoles.Contains(role))
            {
                await _userManager.RemoveFromRolesAsync(user, currentRoles);
                await _userManager.AddToRoleAsync(user, role);
            }

            return RedirectToAction(nameof(Users));
        }

        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            try
            {
                // Check if user is a student
                var studentProfile = await _context.StudentProfiles
                    .FirstOrDefaultAsync(s => s.ApplicationUserId == id);

                if (studentProfile != null)
                {
                    // Delete student's enrollments
                    var enrollments = await _context.Enrollments
                        .Where(e => e.StudentProfileId == studentProfile.Id)
                        .ToListAsync();
                    _context.Enrollments.RemoveRange(enrollments);

                    // Delete student's attendance records
                    var attendanceRecords = await _context.AttendanceRecords
                        .Where(ar => ar.StudentProfileId == studentProfile.Id)
                        .ToListAsync();
                    _context.AttendanceRecords.RemoveRange(attendanceRecords);

                    // Delete student's course assignments
                    var studentCourseAssignments = await _context.StudentCourseAssignments
                        .Where(sca => sca.StudentProfileId == studentProfile.Id)
                        .ToListAsync();
                    _context.StudentCourseAssignments.RemoveRange(studentCourseAssignments);

                    // Delete student profile
                    _context.StudentProfiles.Remove(studentProfile);
                }

                // Check if user is a teacher
                var teacherProfile = await _context.TeacherProfiles
                    .FirstOrDefaultAsync(t => t.ApplicationUserId == id);

                if (teacherProfile != null)
                {
                    // Delete teacher's course assignments
                    var courseAssignments = await _context.CourseAssignments
                        .Where(ca => ca.TeacherProfileId == teacherProfile.Id)
                        .ToListAsync();
                    _context.CourseAssignments.RemoveRange(courseAssignments);

                    // Delete course schedules assigned to this teacher
                    var courseSchedules = await _context.CourseSchedules
                        .Where(cs => cs.TeacherProfileId == teacherProfile.Id)
                        .ToListAsync();
                    _context.CourseSchedules.RemoveRange(courseSchedules);

                    // Delete teacher profile
                    _context.TeacherProfiles.Remove(teacherProfile);
                }

                // Save changes to delete related data
                await _context.SaveChangesAsync();

                // Remove user from all roles
                var userRoles = await _userManager.GetRolesAsync(user);
                if (userRoles.Any())
                {
                    await _userManager.RemoveFromRolesAsync(user, userRoles);
                }

                // Finally delete the user
                var result = await _userManager.DeleteAsync(user);

                if (result.Succeeded)
                {
                    TempData["Success"] = "User deleted successfully.";
                }
                else
                {
                    TempData["Error"] = "Failed to delete user: " + string.Join(", ", result.Errors.Select(e => e.Description));
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error deleting user: {ex.Message}";
            }

            return RedirectToAction(nameof(Users));
        }

        // ===================== COURSE ASSIGNMENTS =====================
        public async Task<IActionResult> CourseAssignments()
        {
            var assignments = await _context.CourseAssignments
                .AsNoTracking()
                .Include(ca => ca.Teacher).ThenInclude(t => t!.ApplicationUser)
                .Include(ca => ca.Course)
                .Include(ca => ca.Section)
                .Include(ca => ca.Session)
                .OrderBy(ca => ca.Course!.Name)
                .ToListAsync();

            return View("CourseAssignments/Index", assignments);
        }

        public async Task<IActionResult> AssignTeacher()
        {
            ViewBag.Teachers = await _context.TeacherProfiles
                .Include(t => t.ApplicationUser)
                .OrderBy(t => t.EmployeeId)
                .ToListAsync();
            ViewBag.Courses = await _context.Courses
                .OrderBy(c => c.Name)
                .ToListAsync();
            ViewBag.Sections = await _context.Sections
                .OrderBy(s => s.Name)
                .ToListAsync();
            ViewBag.Sessions = await _context.Sessions
                .OrderByDescending(s => s.StartDate)
                .ToListAsync();
            return View("Assign/AssignTeacher");
        }

        [HttpPost]
        public async Task<IActionResult> AssignTeacher(AssignTeacherVm vm)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Teachers = await _context.TeacherProfiles
                    .Include(t => t.ApplicationUser)
                    .OrderBy(t => t.EmployeeId)
                    .ToListAsync();
                ViewBag.Courses = await _context.Courses
                    .OrderBy(c => c.Name)
                    .ToListAsync();
                ViewBag.Sections = await _context.Sections
                    .OrderBy(s => s.Name)
                    .ToListAsync();
                ViewBag.Sessions = await _context.Sessions
                    .OrderByDescending(s => s.StartDate)
                    .ToListAsync();
                return View("Assign/AssignTeacher", vm);
            }

            _context.CourseAssignments.Add(new CourseAssignment
            {
                TeacherProfileId = vm.TeacherProfileId,
                CourseId = vm.CourseId,
                SectionId = vm.SectionId,
                SessionId = vm.SessionId
            });

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(CourseAssignments));
        }

        public async Task<IActionResult> DeleteCourseAssignment(int id)
        {
            var assignment = await _context.CourseAssignments.FindAsync(id);
            if (assignment != null)
            {
                _context.CourseAssignments.Remove(assignment);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(CourseAssignments));
        }

        // ===================== ENROLLMENTS =====================
        public async Task<IActionResult> Enrollments()
        {
            var assignments = await _context.StudentCourseAssignments
                .AsNoTracking()
                .Include(sca => sca.Student).ThenInclude(s => s!.ApplicationUser)
                .Include(sca => sca.Course)
                .Include(sca => sca.Section)
                .Include(sca => sca.Session)
                .OrderBy(sca => sca.Student!.RollNumber)
                .ToListAsync();

            return View("Enrollments/Index", assignments);
        }

        public IActionResult EnrollStudent()
        {
            ViewBag.Students = _context.StudentProfiles
                .Include(s => s.ApplicationUser)
                .Where(s => s.ApplicationUser != null)
                .OrderBy(s => s.RollNumber)
                .ToList();
            ViewBag.Courses = _context.Courses
                .OrderBy(c => c.Name)
                .ToList();
            ViewBag.Sections = _context.Sections
                .OrderBy(s => s.Name)
                .ToList();
            ViewBag.Sessions = _context.Sessions
                .OrderByDescending(s => s.StartDate)
                .ToList();
            return View("Assign/EnrollStudent");
        }

        [HttpPost]
        public async Task<IActionResult> EnrollStudent(EnrollmentViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Students = await _context.StudentProfiles
                    .Include(s => s.ApplicationUser)
                    .Where(s => s.ApplicationUser != null)
                    .OrderBy(s => s.RollNumber)
                    .ToListAsync();
                ViewBag.Courses = await _context.Courses
                    .OrderBy(c => c.Name)
                    .ToListAsync();
                ViewBag.Sections = await _context.Sections
                    .OrderBy(s => s.Name)
                    .ToListAsync();
                ViewBag.Sessions = await _context.Sessions
                    .OrderByDescending(s => s.StartDate)
                    .ToListAsync();
                return View("Assign/EnrollStudent", vm);
            }

            // Check if student already has this course assigned or enrolled
            var existingAssignment = await _context.StudentCourseAssignments
                .FirstOrDefaultAsync(sca => sca.StudentProfileId == vm.StudentProfileId &&
                                           sca.CourseId == vm.CourseId &&
                                           sca.SessionId == vm.SessionId);

            var existingEnrollment = await _context.Enrollments
                .FirstOrDefaultAsync(e => e.StudentProfileId == vm.StudentProfileId &&
                                         e.CourseId == vm.CourseId &&
                                         e.SessionId == vm.SessionId);

            if (existingAssignment != null || existingEnrollment != null)
            {
                ViewBag.Error = "This course has already been assigned to this student for the selected session.";
                ViewBag.Students = await _context.StudentProfiles
                    .Include(s => s.ApplicationUser)
                    .Where(s => s.ApplicationUser != null)
                    .OrderBy(s => s.RollNumber)
                    .ToListAsync();
                ViewBag.Courses = await _context.Courses
                    .OrderBy(c => c.Name)
                    .ToListAsync();
                ViewBag.Sections = await _context.Sections
                    .OrderBy(s => s.Name)
                    .ToListAsync();
                ViewBag.Sessions = await _context.Sessions
                    .OrderByDescending(s => s.StartDate)
                    .ToListAsync();
                return View("Assign/EnrollStudent", vm);
            }

            // Create course assignment
            var assignment = new StudentCourseAssignment
            {
                StudentProfileId = vm.StudentProfileId,
                CourseId = vm.CourseId,
                SectionId = vm.SectionId,
                SessionId = vm.SessionId,
                AssignedDate = DateTime.UtcNow
            };

            _context.StudentCourseAssignments.Add(assignment);

            // Also create enrollment record immediately (admin enrollment auto-enrolls the student)
            var enrollment = new Enrollment
            {
                StudentProfileId = vm.StudentProfileId,
                CourseId = vm.CourseId,
                SectionId = vm.SectionId,
                SessionId = vm.SessionId
            };

            _context.Enrollments.Add(enrollment);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Course successfully assigned and student enrolled. Student will appear in attendance and their dashboard.";
            return RedirectToAction(nameof(Enrollments));
        }

        public async Task<IActionResult> DeleteEnrollment(int id)
        {
            var assignment = await _context.StudentCourseAssignments.FindAsync(id);
            if (assignment != null)
            {
                // Also delete any enrollment the student made for this course
                var enrollment = await _context.Enrollments
                    .FirstOrDefaultAsync(e => e.StudentProfileId == assignment.StudentProfileId &&
                                              e.CourseId == assignment.CourseId &&
                                              e.SessionId == assignment.SessionId);
                if (enrollment != null)
                {
                    _context.Enrollments.Remove(enrollment);
                }

                _context.StudentCourseAssignments.Remove(assignment);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Enrollments));
        }

        [HttpGet]
        public async Task<IActionResult> GetAvailableCourses(int studentId, int sessionId)
        {
            // Get courses already enrolled by this student in this session
            var enrolledCourseIds = await _context.Enrollments
                .Where(e => e.StudentProfileId == studentId && e.SessionId == sessionId)
                .Select(e => e.CourseId)
                .ToListAsync();

            // Get courses already assigned by admin to this student in this session
            var assignedCourseIds = await _context.StudentCourseAssignments
                .Where(sca => sca.StudentProfileId == studentId && sca.SessionId == sessionId)
                .Select(sca => sca.CourseId)
                .ToListAsync();

            // Combine both lists
            var unavailableCourseIds = enrolledCourseIds.Concat(assignedCourseIds).Distinct().ToList();

            // Get all courses not already enrolled or assigned
            var availableCourses = await _context.Courses
                .Where(c => !unavailableCourseIds.Contains(c.Id))
                .OrderBy(c => c.Name)
                .Select(c => new { c.Id, c.Name, c.Code })
                .ToListAsync();

            return Json(availableCourses);
        }

        // ===================== SECTION SESSIONS =====================
        public async Task<IActionResult> SectionSessions()
        {
            var sectionSessions = await _context.SectionSessions
                .AsNoTracking()
                .Include(ss => ss.Section)
                .Include(ss => ss.Session)
                .OrderBy(ss => ss.Section!.Name)
                .ToListAsync();

            return View("SectionSessions/Index", sectionSessions);
        }

        public IActionResult CreateSectionSession()
        {
            ViewBag.Sections = _context.Sections.ToList();
            ViewBag.Sessions = _context.Sessions.ToList();
            return View("SectionSessions/Create");
        }

        [HttpPost]
        public async Task<IActionResult> CreateSectionSession(int sectionId, int sessionId)
        {
            _context.SectionSessions.Add(new SectionSession
            {
                SectionId = sectionId,
                SessionId = sessionId
            });

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(SectionSessions));
        }

        public async Task<IActionResult> DeleteSectionSession(int id)
        {
            var sectionSession = await _context.SectionSessions.FindAsync(id);
            if (sectionSession != null)
            {
                _context.SectionSessions.Remove(sectionSession);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(SectionSessions));
        }

        // ===================== COURSE SCHEDULES =====================
        public async Task<IActionResult> CourseSchedules()
        {
            var schedules = await _context.CourseSchedules
                .AsNoTracking()
                .Include(cs => cs.Course)
                .Include(cs => cs.Section)
                .Include(cs => cs.Teacher).ThenInclude(t => t!.ApplicationUser)
                .OrderBy(cs => cs.DayOfWeek)
                .ThenBy(cs => cs.StartTime)
                .ToListAsync();

            return View("CourseSchedules/Index", schedules);
        }

        public IActionResult CreateCourseSchedule()
        {
            ViewBag.Courses = _context.Courses.ToList();
            ViewBag.Sections = _context.Sections.ToList();
            ViewBag.Sessions = _context.Sessions.ToList();
            ViewBag.Teachers = _context.TeacherProfiles.Include(t => t.ApplicationUser).ToList();
            return View("CourseSchedules/Create");
        }

        [HttpPost]
        public async Task<IActionResult> CreateCourseSchedule(CourseSchedule model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Courses = _context.Courses.ToList();
                ViewBag.Sections = _context.Sections.ToList();
                ViewBag.Sessions = _context.Sessions.ToList();
                ViewBag.Teachers = _context.TeacherProfiles.Include(t => t.ApplicationUser).ToList();
                return View("CourseSchedules/Create", model);
            }

            _context.CourseSchedules.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(CourseSchedules));
        }

        public async Task<IActionResult> EditCourseSchedule(int id)
        {
            var schedule = await _context.CourseSchedules.FindAsync(id);
            if (schedule == null)
                return NotFound();

            ViewBag.Courses = await _context.Courses.ToListAsync();
            ViewBag.Teachers = await _context.TeacherProfiles.Include(t => t.ApplicationUser).ToListAsync();
            ViewBag.Sections = await _context.Sections.ToListAsync();
            ViewBag.Sessions = await _context.Sessions.ToListAsync();
            return View("CourseSchedules/Edit", schedule);
        }

        [HttpPost]
        public async Task<IActionResult> EditCourseSchedule(CourseSchedule model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Courses = await _context.Courses.ToListAsync();
                ViewBag.Teachers = await _context.TeacherProfiles.Include(t => t.ApplicationUser).ToListAsync();
                ViewBag.Sections = await _context.Sections.ToListAsync();
                ViewBag.Sessions = await _context.Sessions.ToListAsync();
                return View("CourseSchedules/Edit", model);
            }

            _context.CourseSchedules.Update(model);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(CourseSchedules));
        }

        public async Task<IActionResult> DeleteCourseSchedule(int id)
        {
            var schedule = await _context.CourseSchedules.FindAsync(id);
            if (schedule != null)
            {
                _context.CourseSchedules.Remove(schedule);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(CourseSchedules));
        }

        // Alias for older links or views that expect 'StudentsManagement'
        public IActionResult StudentsManagement()
        {
            return RedirectToAction(nameof(Students));
        }

        // ===================== ASSIGN STUDENTS TO SECTIONS =====================
        public async Task<IActionResult> AssignStudentsToSection()
        {
            ViewBag.Students = await _context.StudentProfiles
                .Include(s => s.ApplicationUser)
                .Include(s => s.Section)
                .OrderBy(s => s.RollNumber)
                .ToListAsync();
            ViewBag.Sections = await _context.Sections
                .OrderBy(s => s.Name)
                .ToListAsync();
            return View("Assign/AssignStudentsToSection");
        }

        [HttpPost]
        public async Task<IActionResult> AssignStudentsToSection(int[] studentIds, int sectionId)
        {
            if (studentIds == null || studentIds.Length == 0)
            {
                ViewBag.Error = "Please select at least one student.";
                ViewBag.Students = await _context.StudentProfiles
                    .Include(s => s.ApplicationUser)
                    .Include(s => s.Section)
                    .ToListAsync();
                ViewBag.Sections = await _context.Sections.ToListAsync();
                return View("Assign/AssignStudentsToSection");
            }

            foreach (var studentId in studentIds)
            {
                var student = await _context.StudentProfiles.FindAsync(studentId);
                if (student != null)
                {
                    student.SectionId = sectionId;
                }
            }

            await _context.SaveChangesAsync();
            TempData["Success"] = $"Successfully assigned {studentIds.Length} student(s) to the section.";
            return RedirectToAction(nameof(Students));
        }

        // ===================== DIAGNOSTIC & FIX TOOLS =====================
        [HttpGet]
        public async Task<IActionResult> DiagnoseEnrollments()
        {
            var output = new System.Text.StringBuilder();
            output.AppendLine("=== ENROLLMENT DIAGNOSIS ===\n");

            // Check assignments without enrollments
            var assignmentsWithoutEnrollments = await _context.StudentCourseAssignments
                .Include(sca => sca.Student).ThenInclude(s => s!.ApplicationUser)
                .Include(sca => sca.Course)
                .Include(sca => sca.Section)
                .Include(sca => sca.Session)
                .Where(sca => !_context.Enrollments.Any(e =>
                    e.StudentProfileId == sca.StudentProfileId &&
                    e.CourseId == sca.CourseId &&
                    e.SessionId == sca.SessionId))
                .ToListAsync();

            output.AppendLine($"Assignments WITHOUT Enrollments: {assignmentsWithoutEnrollments.Count}");
            foreach (var a in assignmentsWithoutEnrollments)
            {
                output.AppendLine($"  - Student: {a.Student?.ApplicationUser?.FullName} (Roll: {a.Student?.RollNumber})");
                output.AppendLine($"    Course: {a.Course?.Name}, Section: {a.Section?.Name}, Session: {a.Session?.Name}");
                output.AppendLine($"    IDs: StudentId={a.StudentProfileId}, CourseId={a.CourseId}, SectionId={a.SectionId}, SessionId={a.SessionId}");
                output.AppendLine();
            }

            // Check enrollments
            var allEnrollments = await _context.Enrollments
                .Include(e => e.Student).ThenInclude(s => s!.ApplicationUser)
                .Include(e => e.Course)
                .Include(e => e.Section)
                .Include(e => e.Session)
                .ToListAsync();

            output.AppendLine($"\nTotal Enrollments: {allEnrollments.Count}");
            foreach (var e in allEnrollments.Take(10))
            {
                output.AppendLine($"  - Student: {e.Student?.ApplicationUser?.FullName} (Roll: {e.Student?.RollNumber})");
                output.AppendLine($"    Course: {e.Course?.Name}, Section: {e.Section?.Name}, Session: {e.Session?.Name}");
                output.AppendLine();
            }

            return Content(output.ToString(), "text/plain");
        }

        [HttpPost]
        public async Task<IActionResult> FixMissingEnrollments()
        {
            var output = new System.Text.StringBuilder();
            output.AppendLine("=== FIXING MISSING ENROLLMENTS ===\n");

            // Get assignments without enrollments
            var assignmentsWithoutEnrollments = await _context.StudentCourseAssignments
                .Where(sca => !_context.Enrollments.Any(e =>
                    e.StudentProfileId == sca.StudentProfileId &&
                    e.CourseId == sca.CourseId &&
                    e.SessionId == sca.SessionId))
                .ToListAsync();

            output.AppendLine($"Found {assignmentsWithoutEnrollments.Count} assignments without enrollments");

            int created = 0;
            foreach (var assignment in assignmentsWithoutEnrollments)
            {
                var enrollment = new Enrollment
                {
                    StudentProfileId = assignment.StudentProfileId,
                    CourseId = assignment.CourseId,
                    SectionId = assignment.SectionId,
                    SessionId = assignment.SessionId
                };

                _context.Enrollments.Add(enrollment);
                created++;
                output.AppendLine($"Created enrollment for StudentId={assignment.StudentProfileId}, CourseId={assignment.CourseId}");
            }

            await _context.SaveChangesAsync();
            output.AppendLine($"\n✓ Successfully created {created} enrollment records!");
            output.AppendLine("\nNow try:");
            output.AppendLine("1. Go to teacher dashboard and load attendance - new students should appear");
            output.AppendLine("2. Login as student - current class should show on their dashboard");

            TempData["Success"] = $"Fixed {created} missing enrollments!";
            return Content(output.ToString(), "text/plain");
        }
    }
}
