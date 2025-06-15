using SchoolSystem.Infrastructure.Models;

namespace SchoolSystem.Services.Dtos;

public class ParentDto
{
    public Guid Id { get; init; }
    
    public Guid UserId { get; init; }

    public User User { get; init; } = null!;
    
    public string PhoneNumber { get; init; } = null!;

    public ICollection<StudentDto> Students { get; init; } = [];
}
