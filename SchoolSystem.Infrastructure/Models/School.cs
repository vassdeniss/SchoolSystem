using System.ComponentModel.DataAnnotations;

namespace SchoolSystem.Infrastructure.Models;

public class School
{
    [Key]
    public Guid Id { get; init; } = Guid.NewGuid();

    [Required] [MaxLength(100)] public string Name { get; set; } = null!;

    [Required]
    [MaxLength(100)]
    public string Address { get; set; } = null!;
    
    public Guid PrincipalId { get; set; }
    public Principal Principal { get; init; } = null!;
    
    public virtual ICollection<Teacher> Teachers { get; set; } = new HashSet<Teacher>();
    public virtual ICollection<Class> Classes { get; set; } = new HashSet<Class>();
    public virtual ICollection<Subject> Subjects { get; set; } = new HashSet<Subject>();
}
