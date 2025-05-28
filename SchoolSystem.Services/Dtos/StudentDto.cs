using SchoolSystem.Infrastructure.Models;

namespace SchoolSystem.Services.Dtos;

public class StudentDto
{
    public Guid Id { get; init; }
    
    public Guid UserId { get; init; }

    public User User { get; init; } = null!;
    
    public Guid ClassId { get; init; }
    
    public Class Class { get; init; } = null!;
}
