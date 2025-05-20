namespace SchoolSystem.Services.Dtos;

public class UserDto
{
    public Guid Id { get; init; }
    
    public string? FirstName { get; init; }
    
    public string? MiddleName { get; init; }
    
    public string? LastName { get; init; }    
    
    public string? Email { get; init; }
    
    public DateTime? DateOfBirth { get; init; }
    
    public IEnumerable<string>? Roles { get; set; } = new HashSet<string>();
}
