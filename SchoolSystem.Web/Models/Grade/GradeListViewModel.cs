namespace SchoolSystem.Web.Models.Grade;

public class GradeListViewModel
{
    public Guid StudentId { get; init; }
    
    public string StudentName { get; init; } = null!;
    
    public Guid SchoolId { get; init; }
    
    public IEnumerable<GradeViewModel> Grades { get; init; } = [];
}
