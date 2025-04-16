using System.ComponentModel.DataAnnotations;

namespace SchoolSystem.Infrastructure.Models;

public class Grade
{
    [Key]
    public Guid Id { get; init; } = Guid.NewGuid();

    public Guid StudentId { get; init; }
    public Student Student { get; init; } = null!;

    public Guid SubjectId { get; init; }
    public Subject Subject { get; init; } = null!;

    [Range(2, 6)]
    public int GradeValue { get; init; }

    public DateTime GradeDate { get; init; }
}
