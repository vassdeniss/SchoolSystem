using SchoolSystem.Infrastructure.Models;

namespace SchoolSystem.Common;

public class SchoolDto
{
    public Guid Id { get; init; }
    
    public string Name { get; init; } = null!;
    
    public string Address { get; init; } = null!;
    
    public Guid PrincipalId { get; init; }

    public Principal Principal { get; init; } = null!;
}
