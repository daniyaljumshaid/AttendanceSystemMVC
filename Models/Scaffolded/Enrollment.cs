using System;
using System.Collections.Generic;

namespace AttendanceSystemMVC.Models.Scaffolded;

public partial class Enrollment
{
    public int Id { get; set; }

    public int StudentProfileId { get; set; }

    public int CourseId { get; set; }

    public int SectionId { get; set; }

    public int SessionId { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual StudentProfile StudentProfile { get; set; } = null!;
}
