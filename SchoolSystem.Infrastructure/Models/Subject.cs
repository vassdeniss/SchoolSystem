using System.ComponentModel.DataAnnotations;

namespace SchoolSystem.Infrastructure.Models;

public class Subject
{
    [Key]
    public Guid Id { get; init; } = Guid.NewGuid();

    [Required]
    [MaxLength(100)]
    public string Name { get; init; } = null!;

    public Guid DepartmentId { get; init; }
    public Department Department { get; init; } = null!;
}

