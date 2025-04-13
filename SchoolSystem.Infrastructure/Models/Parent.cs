using System;
using System.Collections.Generic;

namespace SchoolSystem.Infrastructure.Models;

public partial class Parent
{
    public int Id { get; set; }

    public string PhoneNumber { get; set; } = null!;

    public int UserId { get; set; }

    public virtual User User { get; set; } = null!;

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();
}
