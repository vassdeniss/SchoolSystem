using System.ComponentModel.DataAnnotations;

namespace SchoolSystem.Infrastructure.Models;

public class School
{
    [Key]
    public Guid Id { get; init; } = Guid.NewGuid();

    [Required] [MaxLength(100)] public string Name { get; init; } = null!;

    [Required]
    [MaxLength(100)]
    public string Address { get; init; } = null!;
    
    public Guid? PrincipalId { get; init; }
    public Principal Principal { get; init; } = null!;
}
