using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolSystem.Infrastructure.Models;

public class UserRole
{
    [Key, Column(Order = 0)]
    public Guid UserId { get; init; }

    [Key, Column(Order = 1)]
    public Guid RoleId { get; init; }

    public User User { get; init; } = null!;
    public Role Role { get; init; } = null!;
}

