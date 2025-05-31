using SchoolSystem.Infrastructure.Models;

namespace SchoolSystem.Services.Dtos;

public class TeacherDto
{
    public Guid Id { get; init; }
    
    public string Specialization { get; init; } = null!;

    public Guid SchoolId { get; init; }
    
    public School School { get; init; } = null!;
    
    public Guid UserId { get; set; }
    
    public User User { get; init; } = null!;
}
