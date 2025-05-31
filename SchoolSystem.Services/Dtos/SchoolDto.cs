using SchoolSystem.Infrastructure.Models;

namespace SchoolSystem.Services.Dtos;

public class SchoolDto
{
    public Guid Id { get; init; }
    
    public string Name { get; init; } = null!;
    
    public string Address { get; init; } = null!;
    
    public Guid PrincipalId { get; init; }

    public Principal Principal { get; init; } = null!;
    
    public ICollection<TeacherDto> Teachers { get; set; } = new HashSet<TeacherDto>();
    
    public ICollection<ClassDto> Classes { get; set; } = new HashSet<ClassDto>();
    
    public ICollection<SubjectDto> Subjects { get; set; } = new HashSet<SubjectDto>();
}
