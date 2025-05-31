using SchoolSystem.Infrastructure.Models;
using SchoolSystem.Web.Models.Class;
using SchoolSystem.Web.Models.Subject;

namespace SchoolSystem.Web.Models.School;

public class SchoolViewModel
{
    public Guid Id { get; init; }
    
    public string Name { get; init; } = null!;
    
    public string Address { get; init; } = null!;
    
    public string? PrincipalName  { get; init; }
        
    //public ICollection<TeacherViewModel> Teachers { get; set; } = new HashSet<Teacher>();
    
    public ICollection<ClassViewModel> Classes { get; set; } = new HashSet<ClassViewModel>();
    
    public ICollection<SubjectViewModel> Subjects { get; set; } = new HashSet<SubjectViewModel>();
}
