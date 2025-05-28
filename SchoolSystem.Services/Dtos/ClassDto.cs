using SchoolSystem.Infrastructure.Models;

namespace SchoolSystem.Services.Dtos;

public class ClassDto
{
    public Guid Id { get; init; }
    
    public string Name { get; init; } = null!;

    public int Year { get; init; }

    public string Term { get; init; } = null!;
    
    public Guid SchoolId { get; init; }
    
    public School School { get; init; } = null!;
}
