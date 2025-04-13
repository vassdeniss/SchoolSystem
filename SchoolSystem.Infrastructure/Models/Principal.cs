using System;
using System.Collections.Generic;

namespace SchoolSystem.Infrastructure.Models;

public partial class Principal
{
    public int Id { get; set; }

    public string Specialization { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public int SchoolId { get; set; }

    public int UserId { get; set; }

    public virtual School School { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
