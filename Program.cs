using AttendanceSystemMVC.Data;
using AttendanceSystemMVC.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AttendanceSystemMVC.ReportService;

var builder = WebApplication.CreateBuilder(args);

// Connection string
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Add services
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Configure Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.SignIn.RequireConfirmedEmail = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// Reports service
builder.Services.AddScoped<ReportsService>();

var app = builder.Build();

// Seed roles, users, and initial data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await SeedDatabaseAsync(services);
}

// Middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

// Default route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();


// =====================
// Seed DB
// =====================
async Task SeedDatabaseAsync(IServiceProvider services)
{
    var context = services.GetRequiredService<ApplicationDbContext>();
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

    try
    {
        // Use Migrate for proper migration support
        context.Database.Migrate();
    }
    catch (Exception ex) when (ex.Message.Contains("There is already an object named"))
    {
        // Database exists but migrations weren't tracked properly
        // This can happen when switching from EnsureCreated to Migrate
        Console.WriteLine("Database exists but migrations not tracked. Trying to sync migration history...");

        // Try to just ensure database exists and continue with seeding
        context.Database.EnsureCreated();

        // Manually ensure SectionSessions table exists
        try
        {
            context.Database.ExecuteSqlRaw(@"
                IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='SectionSessions' AND xtype='U')
                CREATE TABLE [SectionSessions] (
                    [Id] int IDENTITY(1,1) NOT NULL,
                    [SectionId] int NOT NULL,
                    [SessionId] int NOT NULL,
                    CONSTRAINT [PK_SectionSessions] PRIMARY KEY ([Id]),
                    CONSTRAINT [FK_SectionSessions_Sections] FOREIGN KEY ([SectionId]) REFERENCES [Sections] ([Id]) ON DELETE CASCADE,
                    CONSTRAINT [FK_SectionSessions_Sessions] FOREIGN KEY ([SessionId]) REFERENCES [Sessions] ([Id]) ON DELETE CASCADE
                )");
        }
        catch (Exception sqlEx)
        {
            Console.WriteLine($"Note: SectionSessions table creation skipped: {sqlEx.Message}");
        }
    }

    // 1️⃣ Seed Roles
    string[] roles = { "Admin", "Teacher", "Student" };
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole(role));
    }

    // 2️⃣ Seed Admin
    var adminEmail = "admin@school.local";
    if (await userManager.FindByEmailAsync(adminEmail) == null)
    {
        var admin = new ApplicationUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            FullName = "System Admin",
            IsApproved = true,
            MustChangePassword = false
        };
        await userManager.CreateAsync(admin, "Admin@123");
        await userManager.AddToRoleAsync(admin, "Admin");
    }

    // 3️⃣ Seed Sections
    if (!context.Sections.Any())
    {
        context.Sections.AddRange(
            new Section { Name = "A" },
            new Section { Name = "B" }
        );
        await context.SaveChangesAsync();
    }

    // 4️⃣ Seed Sessions
    if (!context.Sessions.Any())
    {
        context.Sessions.AddRange(
            new Session { Name = "Fall 2024", StartDate = new DateTime(2024, 8, 1), EndDate = new DateTime(2024, 12, 31) },
            new Session { Name = "Spring 2025", StartDate = new DateTime(2025, 1, 15), EndDate = new DateTime(2025, 5, 31) },
            new Session { Name = "Summer 2025", StartDate = new DateTime(2025, 6, 1), EndDate = new DateTime(2025, 8, 31) }
        );
        await context.SaveChangesAsync();
    }

    // 5️⃣ Seed Courses
    if (!context.Courses.Any())
    {
        context.Courses.AddRange(
            new Course { Name = "Object Oriented Programming", Title = "OOP", Code = "201", CreditHours = 3 },
            new Course { Name = "English", Title = "English", Code = "101", CreditHours = 3 },
            new Course { Name = "Mathematics", Title = "Math", Code = "301", CreditHours = 4 }
        );
        await context.SaveChangesAsync();
    }

    // 6️⃣ Seed Teacher
    if (!context.TeacherProfiles.Any())
    {
        var teacherEmail = "teacher@school.local";
        var teacherUser = await userManager.FindByEmailAsync(teacherEmail);
        if (teacherUser == null)
        {
            teacherUser = new ApplicationUser
            {
                UserName = teacherEmail,
                Email = teacherEmail,
                FullName = "Teacher One",
                IsApproved = true,
                MustChangePassword = false
            };
            await userManager.CreateAsync(teacherUser, "Teacher@123");
            await userManager.AddToRoleAsync(teacherUser, "Teacher");
        }

        context.TeacherProfiles.Add(new TeacherProfile
        {
            ApplicationUserId = teacherUser.Id,
            EmployeeId = "T001"
        });
        await context.SaveChangesAsync();
    }

    // 7️⃣ Seed Students
    var studentEmail = "student@school.local";
    ApplicationUser? studentUser = await userManager.FindByEmailAsync(studentEmail);

    if (!context.StudentProfiles.Any())
    {
        if (studentUser == null)
        {
            studentUser = new ApplicationUser
            {
                UserName = studentEmail,
                Email = studentEmail,
                FullName = "Ali Khan",
                IsApproved = true,
                MustChangePassword = false
            };
            await userManager.CreateAsync(studentUser, "Student@123");
            await userManager.AddToRoleAsync(studentUser, "Student");
        }

        context.StudentProfiles.Add(new StudentProfile
        {
            FullName = "Ali Khan",
            RollNumber = 1,
            SectionId = context.Sections.First().Id,
            ApplicationUserId = studentUser.Id
        });
        await context.SaveChangesAsync();
        Console.WriteLine($"✓ Created student profile for {studentUser.Email}");
    }
    else
    {
        // Student profiles exist, but make sure we have the user
        if (studentUser == null)
        {
            // Try to find student user by looking up from an existing profile
            var existingProfile = await context.StudentProfiles.Include(s => s.ApplicationUser).FirstOrDefaultAsync();
            if (existingProfile?.ApplicationUser != null)
            {
                studentUser = existingProfile.ApplicationUser;
                Console.WriteLine($"✓ Found existing student user: {studentUser.Email}");
            }
            else
            {
                Console.WriteLine("⚠ Student profiles exist but no associated user found");
            }
        }
        else
        {
            Console.WriteLine($"✓ Found student user: {studentUser.Email}");
        }
    }

    // 8️⃣ Seed Enrollments
    Console.WriteLine("Checking Enrollments...");
    var enrollmentExists = await context.Enrollments.AnyAsync();
    Console.WriteLine($"Enrollments exist: {enrollmentExists}");

    if (!enrollmentExists && studentUser != null)
    {
        Console.WriteLine("Seeding Enrollments...");
        var student = await context.StudentProfiles.FirstOrDefaultAsync(s => s.ApplicationUserId == studentUser.Id);
        var session = await context.Sessions.OrderBy(s => s.Id).FirstOrDefaultAsync();
        var section = await context.Sections.OrderBy(s => s.Id).FirstOrDefaultAsync(); // Get Section A
        var courses = await context.Courses.OrderBy(c => c.Id).Take(3).ToListAsync();

        Console.WriteLine($"Student: {student?.FullName}, Session: {session?.Name} (ID: {session?.Id}), Section: {section?.Name} (ID: {section?.Id}), Courses: {courses.Count}");

        if (student != null && session != null && section != null && courses.Any())
        {
            foreach (var course in courses)
            {
                context.Enrollments.Add(new Enrollment
                {
                    StudentProfileId = student.Id,
                    CourseId = course.Id,
                    SessionId = session.Id,
                    SectionId = section.Id
                });
                Console.WriteLine($"Added enrollment: Student {student.FullName} -> Course {course.Name}");
            }
            await context.SaveChangesAsync();
            Console.WriteLine($"✓ Created {courses.Count} enrollments");
        }
        else
        {
            Console.WriteLine("⚠ Could not create enrollments - missing required data");
            Console.WriteLine($"   Student: {student != null}, Session: {session != null}, Section: {section != null}, Courses: {courses.Count}");
        }
    }
    else
    {
        var enrollmentCount = await context.Enrollments.CountAsync();
        Console.WriteLine($"Skipping enrollment seeding. Exists: {enrollmentExists}, StudentUser: {studentUser != null}, Count: {enrollmentCount}");
    }

    // 9️⃣ Seed Course Schedules
    if (!context.CourseSchedules.Any())
    {
        var teacher = await context.TeacherProfiles.FirstOrDefaultAsync();
        var courses = await context.Courses.OrderBy(c => c.Id).Take(2).ToListAsync();
        var section = await context.Sections.OrderBy(s => s.Id).FirstOrDefaultAsync(); // Get Section A
        var session = await context.Sessions.OrderBy(s => s.Id).FirstOrDefaultAsync();

        Console.WriteLine($"Creating schedules - Teacher: {teacher?.EmployeeId}, Section: {section?.Name} (ID: {section?.Id}), Session: {session?.Name} (ID: {session?.Id}), Courses: {courses.Count}");

        if (teacher != null && courses.Any() && section != null && session != null)
        {
            try
            {
                foreach (var course in courses)
                {
                    context.CourseSchedules.Add(new CourseSchedule
                    {
                        CourseId = course.Id,
                        TeacherProfileId = teacher.Id,
                        SectionId = section.Id,
                        SessionId = session.Id,
                        DayOfWeek = DayOfWeek.Monday,
                        StartTime = new TimeSpan(9, 0, 0),
                        EndTime = new TimeSpan(10, 30, 0)
                    });

                    context.CourseSchedules.Add(new CourseSchedule
                    {
                        CourseId = course.Id,
                        TeacherProfileId = teacher.Id,
                        SectionId = section.Id,
                        SessionId = session.Id,
                        DayOfWeek = DayOfWeek.Wednesday,
                        StartTime = new TimeSpan(10, 30, 0),
                        EndTime = new TimeSpan(12, 0, 0)
                    });

                    context.CourseSchedules.Add(new CourseSchedule
                    {
                        CourseId = course.Id,
                        TeacherProfileId = teacher.Id,
                        SectionId = section.Id,
                        SessionId = session.Id,
                        DayOfWeek = DayOfWeek.Friday,
                        StartTime = new TimeSpan(14, 0, 0),
                        EndTime = new TimeSpan(15, 30, 0)
                    });
                }
                await context.SaveChangesAsync();
                Console.WriteLine($"✓ Created course schedules");
            }
            catch (DbUpdateException ex)
            {
                // Log the inner exception for debugging
                var innerMessage = ex.InnerException?.Message ?? ex.Message;
                Console.WriteLine($"Error seeding CourseSchedules: {innerMessage}");
                throw new Exception($"Failed to seed CourseSchedules: {innerMessage}", ex);
            }
        }
    }

    // 🔟 Fix data consistency - ensure enrollments and schedules use the same section
    Console.WriteLine("Checking data consistency...");
    var firstSection = await context.Sections.OrderBy(s => s.Id).FirstOrDefaultAsync();
    if (firstSection != null)
    {
        // Update any enrollments using different sections
        var mismatchedEnrollments = await context.Enrollments
            .Where(e => e.SectionId != firstSection.Id)
            .ToListAsync();

        if (mismatchedEnrollments.Any())
        {
            Console.WriteLine($"Fixing {mismatchedEnrollments.Count} enrollments with wrong section...");
            foreach (var enrollment in mismatchedEnrollments)
            {
                enrollment.SectionId = firstSection.Id;
            }
            await context.SaveChangesAsync();
            Console.WriteLine($"✓ Fixed enrollments to use Section {firstSection.Name}");
        }

        // Update any schedules using different sections
        var mismatchedSchedules = await context.CourseSchedules
            .Where(cs => cs.SectionId != firstSection.Id)
            .ToListAsync();

        if (mismatchedSchedules.Any())
        {
            Console.WriteLine($"Fixing {mismatchedSchedules.Count} course schedules with wrong section...");
            foreach (var schedule in mismatchedSchedules)
            {
                schedule.SectionId = firstSection.Id;
            }
            await context.SaveChangesAsync();
            Console.WriteLine($"✓ Fixed course schedules to use Section {firstSection.Name}");
        }
    }
}
