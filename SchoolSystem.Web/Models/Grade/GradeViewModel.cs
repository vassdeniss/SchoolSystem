namespace SchoolSystem.Web.Models.Grade;

public class GradeViewModel
{
    public Guid Id { get; init; }
    
    public int GradeValue { get; init; }

    public DateTime GradeDate { get; init; }
    
    public string SubjectName { get; init; } = null!;
}
