using System.ComponentModel.DataAnnotations;

namespace SchoolSystem.Infrastructure.Models;

public class Class
{
    [Key]
    public Guid Id { get; init; } = Guid.NewGuid();

    [Required]
    [MaxLength(10)]
    public string Name { get; init; } = null!;

    [Required]
    public int Year { get; init; }

    [Required] [MaxLength(20)] public string Term { get; init; } = null!;
}
