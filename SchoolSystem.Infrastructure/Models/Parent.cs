using System.ComponentModel.DataAnnotations;

namespace SchoolSystem.Infrastructure.Models;

public class Parent
{
    [Key]
    public Guid Id { get; init; } = Guid.NewGuid();

    [Required] [MaxLength(15)] public string PhoneNumber { get; set; } = null!;

    public Guid UserId { get; init; }
    public User User { get; init; } = null!;
    
    public virtual ICollection<Student> Students { get; init; } = new HashSet<Student>();
}
