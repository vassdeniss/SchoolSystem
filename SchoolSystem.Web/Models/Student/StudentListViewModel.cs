namespace SchoolSystem.Web.Models.Student;

public class StudentListViewModel
{
    public Guid Id { get; set; }
    public Guid SchoolId { get; init; }
    public string ClassName { get; init; } = null!;
    public int Year { get; init; }
    public string Term { get; init; } = null!;
    public IEnumerable<StudentViewModel> Students { get; init; } = [];
}
