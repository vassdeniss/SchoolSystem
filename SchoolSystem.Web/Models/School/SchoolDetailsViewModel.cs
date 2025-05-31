using SchoolSystem.Web.Models.Class;
using SchoolSystem.Web.Models.Subject;
using SchoolSystem.Web.Models.Teacher;

namespace SchoolSystem.Web.Models.School;

public class SchoolDetailsViewModel
{
    public Guid Id { get; init; }
    
    public string Name { get; init; } = null!;

    public string PrincipalName { get; init; } = null!;
    
    public List<TeacherViewModel> Teachers { get; init; } = [];  
    
    public List<ClassViewModel> Classes { get; init; } = [];  
    
    public List<SubjectViewModel> Subjects { get; init; } = [];    
}
