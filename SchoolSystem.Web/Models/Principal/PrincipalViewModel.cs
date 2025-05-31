namespace SchoolSystem.Web.Models.Principal;

public class PrincipalViewModel
{
    public Guid Id { get; init; }
    
    public string? FullName { get; init; }
    
    public string? SchoolName { get; init; }
    
    public string? Specialization { get; init; }
    
    public string? PhoneNumber { get; init; }
}
