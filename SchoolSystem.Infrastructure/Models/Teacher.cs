using System;
using System.Collections.Generic;

namespace SchoolSystem.Infrastructure.Models;

public partial class Teacher
{
    public int Id { get; set; }

    public string Specialization { get; set; } = null!;

    public int SchoolId { get; set; }

    public int UserId { get; set; }

    public virtual ICollection<Curriculum> Curricula { get; set; } = new List<Curriculum>();

    public virtual School School { get; set; } = null!;

    public virtual User User { get; set; } = null!;

    public virtual ICollection<Subject> Subjects { get; set; } = new List<Subject>();
}
