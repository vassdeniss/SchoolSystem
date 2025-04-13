using System;
using System.Collections.Generic;

namespace SchoolSystem.Infrastructure.Models;

public partial class Department
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Subject> Subjects { get; set; } = new List<Subject>();
}
