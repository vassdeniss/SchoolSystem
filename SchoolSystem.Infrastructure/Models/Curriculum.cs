using System;
using System.Collections.Generic;

namespace SchoolSystem.Infrastructure.Models;

public partial class Curriculum
{
    public int Id { get; set; }

    public string DayOfWeek { get; set; } = null!;

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    public int TeacherId { get; set; }

    public int ClassId { get; set; }

    public virtual Class Class { get; set; } = null!;

    public virtual Teacher Teacher { get; set; } = null!;

    public virtual ICollection<Subject> Subjects { get; set; } = new List<Subject>();
}
