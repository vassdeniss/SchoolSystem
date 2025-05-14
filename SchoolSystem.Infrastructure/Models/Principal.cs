using System.ComponentModel.DataAnnotations;

namespace SchoolSystem.Infrastructure.Models;

public class Principal
{
    [Key]
    public Guid Id { get; init; } = Guid.NewGuid();

    [Required] [MaxLength(50)] public string Specialization { get; set; } = null!;

    [Required]
    [MaxLength(15)]
    public string PhoneNumber { get; set; } = null!;

    public Guid SchoolId { get; init; }
    public School School { get; init; } = null!;

    public Guid UserId { get; init; }
    public User User { get; init; } = null!;
}
