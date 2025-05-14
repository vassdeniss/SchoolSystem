using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace SchoolSystem.Infrastructure.Models;

public class User : IdentityUser<Guid>
{
    [Required]
    [MaxLength(50)]
    public string FirstName { get; init; } = null!;

    [MaxLength(50)]
    public string? MiddleName { get; init; }

    [Required] [MaxLength(50)] public string LastName { get; init; } = null!;

    [Required]
    public DateTime DateOfBirth { get; init; }
}
