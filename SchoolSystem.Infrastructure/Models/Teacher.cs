using System.ComponentModel.DataAnnotations;

namespace SchoolSystem.Infrastructure.Models;

public class Teacher
{
    [Key]
    public Guid Id { get; init; } = Guid.NewGuid();

    [Required]
    [MaxLength(50)]
    public string Specialization { get; set; } = null!;

    public Guid UserId { get; init; }
    public User User { get; init; } = null!;
    
    public virtual ICollection<School> Schools { get; set; } = new HashSet<School>();
}
