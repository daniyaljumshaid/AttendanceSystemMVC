using System;
using AttendanceSystemMVC.Models;

namespace AttendanceSystemMVC.Models
{
    public class AttendanceRecord
    {
        public int Id { get; set; }

        public int StudentProfileId { get; set; }
        public StudentProfile? Student { get; set; }   // ✅ renamed

        public int CourseId { get; set; }
        public Course? Course { get; set; }

        public int SectionId { get; set; }
        public Section? Section { get; set; }

        public AttendanceStatus Status { get; set; } = AttendanceStatus.Unmarked;
        public int? MarkedByTeacherId { get; set; }
        public TeacherProfile? MarkedByTeacher { get; set; }
        public DateTime Date { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}

