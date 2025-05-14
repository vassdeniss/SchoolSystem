namespace SchoolSystem.Common;

public class UserDto
{
    public Guid Id { get; init; }
    
    public string FullName { get; init; } = null!;
    
    public string Email { get; init; } = null!;
    
    public DateTime DateOfBirth { get; init; }
    
    public IEnumerable<string> Roles { get; set; } = new HashSet<string>();
}
