using System;
using System.Collections.Generic;

namespace SchoolSystem.Infrastructure.Models;

public partial class Subject
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int DepartmentId { get; set; }

    public virtual ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();

    public virtual Department Department { get; set; } = null!;

    public virtual ICollection<Grade> Grades { get; set; } = new List<Grade>();

    public virtual ICollection<Curriculum> Curricula { get; set; } = new List<Curriculum>();

    public virtual ICollection<Teacher> Teachers { get; set; } = new List<Teacher>();
}
