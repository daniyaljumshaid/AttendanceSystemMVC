using System;
using System.Collections.Generic;

namespace AttendanceSystemMVC.Models.Scaffolded;

public partial class AttendanceRecord
{
    public int Id { get; set; }

    public DateTime Date { get; set; }

    public int CourseId { get; set; }

    public int StudentProfileId { get; set; }

    public int SectionId { get; set; }

    public int Status { get; set; }

    public int? MarkedByTeacherId { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual Section Section { get; set; } = null!;

    public virtual StudentProfile StudentProfile { get; set; } = null!;
}
