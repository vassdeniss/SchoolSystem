using SchoolSystem.Infrastructure.Models;

namespace SchoolSystem.Services.Dtos;

public class GradeDto
{
    public Guid Id { get; init; }
    
    public int GradeValue { get; init; }

    public DateTime GradeDate { get; init; }

    public Guid StudentId { get; init; }
    
    public Student Student { get; init; } = null!;
    
    public Guid SubjectId { get; init; }
    
    public Subject Subject { get; init; } = null!;
}
