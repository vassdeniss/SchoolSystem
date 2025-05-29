namespace SchoolSystem.Web.Models.Subject;

public class SubjectViewModel
{
    public Guid Id { get; init; }
    
    public string Name { get; init; } = null!;
    
    public Guid SchoolId { get; init; }
}
