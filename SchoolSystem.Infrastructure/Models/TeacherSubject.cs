using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolSystem.Infrastructure.Models;

public class TeacherSubject
{
    [Key, Column(Order = 0)]
    public Guid TeacherId { get; init; }

    [Key, Column(Order = 1)]
    public Guid SubjectId { get; init; }

    public Teacher Teacher { get; init; } = null!;
    public Subject Subject { get; init; } = null!;
}
