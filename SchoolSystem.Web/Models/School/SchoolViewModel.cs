namespace SchoolSystem.Web.Models.School;

public class SchoolViewModel
{
    public Guid Id { get; init; }
    
    public string Name { get; init; } = null!;
    
    public string Address { get; init; } = null!;
    
    public string? PrincipalName  { get; init; }
}
