using System;
using System.Collections.Generic;

namespace AttendanceSystemMVC.Models.Scaffolded;

public partial class TeacherProfile
{
    public int Id { get; set; }

    public string? EmployeeId { get; set; }

    public string? ApplicationUserId { get; set; }

    public virtual AspNetUser? ApplicationUser { get; set; }

    public virtual ICollection<CourseAssignment> CourseAssignments { get; set; } = new List<CourseAssignment>();

    public virtual ICollection<CourseSchedule> CourseSchedules { get; set; } = new List<CourseSchedule>();
}
