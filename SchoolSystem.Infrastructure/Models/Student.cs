using System.ComponentModel.DataAnnotations;

namespace SchoolSystem.Infrastructure.Models;

public class Student
{
    [Key]
    public Guid Id { get; init; } = Guid.NewGuid();

    public Guid UserId { get; init; }
    public User User { get; init; } = null!;

    public Guid ClassId { get; set; }
    public Class Class { get; init; } = null!;
}
