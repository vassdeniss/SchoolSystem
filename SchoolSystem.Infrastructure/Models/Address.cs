using System.ComponentModel.DataAnnotations;

namespace SchoolSystem.Infrastructure.Models;

public class Address
{
    [Key]
    public Guid Id { get; init; } = Guid.NewGuid();

    [Required] [MaxLength(100)] public string City { get; init; } = null!;

    [Required] [MaxLength(100)] public string Street { get; init; } = null!;

    [Required] [MaxLength(10)] public string StreetNumber { get; init; } = null!;

    [MaxLength(10)]
    public string? PostalCode { get; init; }
}

