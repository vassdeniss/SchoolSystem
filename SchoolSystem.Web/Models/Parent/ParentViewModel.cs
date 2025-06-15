using SchoolSystem.Services.Dtos;
using SchoolSystem.Web.Models.Student;

namespace SchoolSystem.Web.Models.Parent;

public class ParentViewModel
{
    public Guid Id { get; init; }

    public string FullName { get; init; } = null!;
    
    public string PhoneNumber { get; init; } = null!;

    public ICollection<StudentViewModel> Students { get; init; } = [];
}
