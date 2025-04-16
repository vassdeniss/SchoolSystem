using System.ComponentModel.DataAnnotations;

namespace SchoolSystem.Infrastructure.Models;

public class School
{
    [Key]
    public Guid Id { get; init; } = Guid.NewGuid();

    [Required] [MaxLength(100)] public string Name { get; init; } = null!;

    [Required]
    public Guid AddressId { get; init; }

    public Address Address { get; init; } = null!;
}
