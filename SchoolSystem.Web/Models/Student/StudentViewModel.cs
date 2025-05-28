namespace SchoolSystem.Web.Models.Student;

public class StudentViewModel
{
    public Guid Id { get; init; }
    public string FullName { get; init; } = null!;
    public DateTime Dob { get; init; }
    public Guid ClassId { get; init; }
}
