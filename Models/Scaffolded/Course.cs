using System;
using System.Collections.Generic;

namespace AttendanceSystemMVC.Models.Scaffolded;

public partial class Course
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string Code { get; set; } = null!;

    public string Title { get; set; } = null!;

    public int CreditHours { get; set; }

    public virtual ICollection<AttendanceRecord> AttendanceRecords { get; set; } = new List<AttendanceRecord>();

    public virtual ICollection<CourseAssignment> CourseAssignments { get; set; } = new List<CourseAssignment>();

    public virtual ICollection<CourseSchedule> CourseSchedules { get; set; } = new List<CourseSchedule>();

    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}
