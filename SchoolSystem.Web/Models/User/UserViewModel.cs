namespace SchoolSystem.Web.Models.User;

public class UserViewModel
{
    public Guid Id { get; init; }
    
    public string? FullName { get; init; }
    
    public string? Email { get; init; }
    
    public DateTime? DateOfBirth { get; init; }
    
    public IEnumerable<string>? Roles { get; init; } = new HashSet<string>();    
}
