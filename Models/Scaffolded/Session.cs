using System;
using System.Collections.Generic;

namespace AttendanceSystemMVC.Models.Scaffolded;

public partial class Session
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }
}
