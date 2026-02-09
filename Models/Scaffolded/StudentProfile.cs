using System;
using System.Collections.Generic;

namespace AttendanceSystemMVC.Models.Scaffolded;

public partial class StudentProfile
{
    public int Id { get; set; }

    public string FullName { get; set; } = null!;

    public string? ApplicationUserId { get; set; }

    public int RollNumber { get; set; }

    public int SectionId { get; set; }

    public virtual ICollection<AttendanceRecord> AttendanceRecords { get; set; } = new List<AttendanceRecord>();

    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}
