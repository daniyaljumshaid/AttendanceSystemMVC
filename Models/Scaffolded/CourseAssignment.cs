using System;
using System.Collections.Generic;

namespace AttendanceSystemMVC.Models.Scaffolded;

public partial class CourseAssignment
{
    public int Id { get; set; }

    public int TeacherProfileId { get; set; }

    public int CourseId { get; set; }

    public int SessionId { get; set; }

    public int SectionId { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual TeacherProfile TeacherProfile { get; set; } = null!;
}
