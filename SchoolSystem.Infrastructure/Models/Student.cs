using System;
using System.Collections.Generic;

namespace SchoolSystem.Infrastructure.Models;

public partial class Student
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int ClassId { get; set; }

    public int SchoolId { get; set; }

    public virtual ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();

    public virtual Class Class { get; set; } = null!;

    public virtual ICollection<Grade> Grades { get; set; } = new List<Grade>();

    public virtual School School { get; set; } = null!;

    public virtual User User { get; set; } = null!;

    public virtual ICollection<Parent> Parents { get; set; } = new List<Parent>();
}
