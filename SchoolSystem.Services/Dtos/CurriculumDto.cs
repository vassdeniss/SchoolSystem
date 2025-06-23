using SchoolSystem.Infrastructure.Models;

namespace SchoolSystem.Services.Dtos;

public class CurriculumDto
{
    public Guid Id { get; init; }
    
    public string DayOfWeek { get; init; } = null!;

    public TimeSpan StartTime { get; init; }

    public TimeSpan EndTime { get; init; }

    public Guid TeacherId { get; init; }
    
    public Teacher Teacher { get; init; } = null!;
    
    public Class Class { get; init; } = null!;
    
    public Guid SubjectId { get; init; }
    
    public Subject Subject { get; init; } = null!;
}
