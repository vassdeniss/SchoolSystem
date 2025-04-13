using System;
using System.Collections.Generic;

namespace SchoolSystem.Infrastructure.Models;

public partial class School
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int AddressId { get; set; }

    public virtual Address Address { get; set; } = null!;

    public virtual Principal? Principal { get; set; }

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();

    public virtual Teacher? Teacher { get; set; }
}
