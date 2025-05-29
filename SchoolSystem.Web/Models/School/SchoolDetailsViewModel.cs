using SchoolSystem.Web.Models.Class;
using SchoolSystem.Web.Models.Subject;

namespace SchoolSystem.Web.Models.School;

public class SchoolDetailsViewModel
{
    public Guid Id { get; init; }
    
    public string SchoolName { get; init; } = null!;

    public string PrincipalName { get; init; } = null!;
    
    public List<ClassViewModel> Classes { get; init; } = [];  
    
    public List<SubjectViewModel> Subjects { get; init; } = [];    
}
