using SchoolSystem.Infrastructure.Models;

namespace SchoolSystem.Services.Dtos;

public class AttendanceDto
{
    public Guid Id { get; init; }

    public Guid StudentId { get; init; }

    public Student Student { get; init; } = null!;

    public Guid SubjectId { get; init; }

    public Subject Subject { get; init; } = null!;

    public string AbsenceType { get; init; } = null!;
}
