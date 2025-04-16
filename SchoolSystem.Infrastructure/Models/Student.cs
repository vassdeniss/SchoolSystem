using System.ComponentModel.DataAnnotations;

namespace SchoolSystem.Infrastructure.Models;

public class Student
{
    [Key]
    public Guid Id { get; init; } = Guid.NewGuid();

    public Guid UserId { get; init; }
    public User User { get; init; } = null!;

    public Guid ClassId { get; init; }
    public Class Class { get; init; } = null!;

    public Guid SchoolId { get; init; }
    public School School { get; init; } = null!;
}
