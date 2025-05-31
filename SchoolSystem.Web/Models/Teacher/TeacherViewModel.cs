namespace SchoolSystem.Web.Models.Teacher;

public class TeacherViewModel
{
    public Guid Id { get; init; }

    public string FullName { get; init; } = null!;
    
    public string Specialization { get; init; } = null!;
}
