namespace SchoolSystem.Web.Models.Class;

public class ClassViewModel
{
    public Guid Id { get; init; }
    
    public string Name { get; init; } = null!;

    public int Year { get; init; }

    public string Term { get; init; } = null!;
    
    public Guid SchoolId { get; init; }
}
