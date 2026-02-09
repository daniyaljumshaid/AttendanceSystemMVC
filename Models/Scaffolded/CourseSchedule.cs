using System;
using System.Collections.Generic;

namespace AttendanceSystemMVC.Models.Scaffolded;

public partial class CourseSchedule
{
    public int Id { get; set; }

    public int CourseId { get; set; }

    public int TeacherProfileId { get; set; }

    public int SectionId { get; set; }

    public int DayOfWeek { get; set; }

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    public int SessionId { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual TeacherProfile TeacherProfile { get; set; } = null!;
}
