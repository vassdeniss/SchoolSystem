using System;
using System.Collections.Generic;

namespace SchoolSystem.Infrastructure.Models;

public partial class Class
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int Year { get; set; }

    public string Term { get; set; } = null!;

    public virtual ICollection<Curriculum> Curricula { get; set; } = new List<Curriculum>();

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();
}
