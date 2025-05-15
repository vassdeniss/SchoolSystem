using SchoolSystem.Infrastructure.Models;

namespace SchoolSystem.Common;

public class PrincipalDto
{
    public Guid Id { get; init; }
    
    public Guid? UserId { get; init; }
    
    public User? User { get; init; }
    
    public Guid? SchoolId { get; init; }
    
    public School? School { get; init; }

    public string Specialization { get; init; } = null!;
    
    public string PhoneNumber { get; init; } = null!;
}
