using System;
using System.Collections.Generic;

namespace SchoolSystem.Infrastructure.Models;

public partial class Attendance
{
    public int Id { get; set; }

    public int StudentId { get; set; }

    public int SubjectId { get; set; }

    public string AbsenceType { get; set; } = null!;

    public string Status { get; set; } = null!;

    public virtual Student Student { get; set; } = null!;

    public virtual Subject Subject { get; set; } = null!;
}
