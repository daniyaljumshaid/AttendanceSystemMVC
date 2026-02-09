using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AttendanceSystemMVC.Models;

namespace AttendanceSystemMVC.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        // Initialize DbSet properties with null-forgiving operator to satisfy nullable reference analysis
        public DbSet<StudentProfile> StudentProfiles { get; set; } = null!;
        public DbSet<TeacherProfile> TeacherProfiles { get; set; } = null!;
        public DbSet<Course> Courses { get; set; } = null!;
        public DbSet<Section> Sections { get; set; } = null!;
        public DbSet<Session> Sessions { get; set; } = null!;
        public DbSet<SectionSession> SectionSessions { get; set; } = null!;
        public DbSet<Enrollment> Enrollments { get; set; } = null!;
        public DbSet<CourseSchedule> CourseSchedules { get; set; } = null!;
        public DbSet<AttendanceRecord> AttendanceRecords { get; set; } = null!;
        public DbSet<CourseAssignment> CourseAssignments { get; set; } = null!;
        public DbSet<StudentCourseAssignment> StudentCourseAssignments { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Prevent multiple cascade paths for AttendanceRecords -> StudentProfile -> Section
            builder.Entity<AttendanceRecord>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasOne(a => a.Student)
                    .WithMany()
                    .HasForeignKey(a => a.StudentProfileId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Attendance_Student");

                entity.HasOne(a => a.Course)
                    .WithMany()
                    .HasForeignKey(a => a.CourseId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Attendance_Course");

                entity.HasOne(a => a.Section)
                    .WithMany()
                    .HasForeignKey(a => a.SectionId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Attendance_Section");
            });

            // StudentProfile mapping
            builder.Entity<StudentProfile>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ApplicationUserId).HasMaxLength(450);
                entity.Property(e => e.FullName).HasMaxLength(100);

                entity.HasOne(s => s.ApplicationUser)
                    .WithOne(u => u.StudentProfile)
                    .HasForeignKey<StudentProfile>(s => s.ApplicationUserId);

                entity.HasOne(s => s.Section)
                    .WithMany()
                    .HasForeignKey(s => s.SectionId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // TeacherProfile mapping
            builder.Entity<TeacherProfile>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ApplicationUserId).HasMaxLength(450);
                entity.Property(e => e.EmployeeId).HasMaxLength(50);

                entity.HasOne(t => t.ApplicationUser)
                    .WithOne(u => u.TeacherProfile)
                    .HasForeignKey<TeacherProfile>(t => t.ApplicationUserId)
                    .HasConstraintName("FK_TeacherProfile_AspNetUsers");
            });

            // Course mapping
            builder.Entity<Course>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Code).HasMaxLength(50);
                entity.Property(e => e.CreditHours).HasDefaultValue(3);
                entity.Property(e => e.Name).HasMaxLength(50);
                entity.Property(e => e.Title).HasMaxLength(100);
            });

            // Section mapping
            builder.Entity<Section>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).HasMaxLength(50);
            });

            // Session mapping
            builder.Entity<Session>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).HasMaxLength(100);
                entity.Property(e => e.StartDate).HasColumnType("datetime");
                entity.Property(e => e.EndDate).HasColumnType("datetime");
            });

            // SectionSession mapping
            builder.Entity<SectionSession>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Section).WithMany().HasForeignKey(e => e.SectionId).HasConstraintName("FK_SectionSessions_Sections");
                entity.HasOne(e => e.Session).WithMany().HasForeignKey(e => e.SessionId).HasConstraintName("FK_SectionSessions_Sessions");
            });

            // CourseAssignment mapping
            builder.Entity<CourseAssignment>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasOne(e => e.Course)
                    .WithMany()
                    .HasForeignKey(e => e.CourseId)
                    .HasConstraintName("FK_CourseAssignments_Courses");

                entity.HasOne(e => e.Teacher)
                    .WithMany(t => t.CourseAssignments)
                    .HasForeignKey(e => e.TeacherProfileId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_CourseAssignments_TeacherProfiles");

                entity.HasOne(e => e.Session)
                    .WithMany()
                    .HasForeignKey(e => e.SessionId)
                    .HasConstraintName("FK_CourseAssignments_Sessions");

                entity.HasOne(e => e.Section)
                    .WithMany()
                    .HasForeignKey(e => e.SectionId)
                    .HasConstraintName("FK_CourseAssignments_Sections");
            });

            // CourseSchedule mapping
            builder.Entity<CourseSchedule>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasOne(e => e.Course)
                    .WithMany()
                    .HasForeignKey(e => e.CourseId)
                    .HasConstraintName("FK_CourseSchedules_Courses");

                entity.HasOne(e => e.Teacher)
                    .WithMany(t => t.CourseSchedules)
                    .HasForeignKey(e => e.TeacherProfileId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_CourseSchedules_TeacherProfiles");

                entity.HasOne(e => e.Section)
                    .WithMany()
                    .HasForeignKey(e => e.SectionId)
                    .HasConstraintName("FK_CourseSchedules_Sections");

                entity.HasOne(e => e.Session)
                    .WithMany()
                    .HasForeignKey(e => e.SessionId)
                    .HasConstraintName("FK_CourseSchedules_Sessions");

                entity.Property(e => e.StartTime).HasColumnType("time");
                entity.Property(e => e.EndTime).HasColumnType("time");
            });

            // Enrollment mapping
            builder.Entity<Enrollment>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasOne(e => e.Course)
                    .WithMany()
                    .HasForeignKey(e => e.CourseId)
                    .HasConstraintName("FK_Enrollments_Courses");

                entity.HasOne(e => e.Student)
                    .WithMany()
                    .HasForeignKey(e => e.StudentProfileId)
                    .HasConstraintName("FK_Enrollments_StudentProfiles");

                entity.HasOne(e => e.Session)
                    .WithMany()
                    .HasForeignKey(e => e.SessionId)
                    .HasConstraintName("FK_Enrollments_Sessions");

                entity.HasOne(e => e.Section)
                    .WithMany()
                    .HasForeignKey(e => e.SectionId)
                    .HasConstraintName("FK_Enrollments_Sections");
            });

            // StudentCourseAssignment mapping
            builder.Entity<StudentCourseAssignment>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasOne(e => e.Course)
                    .WithMany()
                    .HasForeignKey(e => e.CourseId)
                    .HasConstraintName("FK_StudentCourseAssignments_Courses");

                entity.HasOne(e => e.Student)
                    .WithMany()
                    .HasForeignKey(e => e.StudentProfileId)
                    .HasConstraintName("FK_StudentCourseAssignments_StudentProfiles");

                entity.HasOne(e => e.Session)
                    .WithMany()
                    .HasForeignKey(e => e.SessionId)
                    .HasConstraintName("FK_StudentCourseAssignments_Sessions");

                entity.HasOne(e => e.Section)
                    .WithMany()
                    .HasForeignKey(e => e.SectionId)
                    .OnDelete(DeleteBehavior.NoAction)
                    .HasConstraintName("FK_StudentCourseAssignments_Sections");
            });
        }
    }
}

