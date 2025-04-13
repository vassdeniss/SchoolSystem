using System;
using System.Collections.Generic;

namespace SchoolSystem.Infrastructure.Models;

public partial class User
{
    public int Id { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string? MiddleName { get; set; }

    public string LastName { get; set; } = null!;

    public DateOnly DateOfBirth { get; set; }

    public virtual Parent? Parent { get; set; }

    public virtual Principal? Principal { get; set; }

    public virtual Student? Student { get; set; }

    public virtual Teacher? Teacher { get; set; }

    public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
}
