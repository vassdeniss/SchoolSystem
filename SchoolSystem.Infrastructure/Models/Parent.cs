using System.ComponentModel.DataAnnotations;

namespace SchoolSystem.Infrastructure.Models;

public class Parent
{
    [Key]
    public Guid Id { get; init; } = Guid.NewGuid();

    [Required] [MaxLength(15)] public string PhoneNumber { get; init; } = null!;

    public Guid UserId { get; init; }
    public User User { get; init; } = null!;
}
