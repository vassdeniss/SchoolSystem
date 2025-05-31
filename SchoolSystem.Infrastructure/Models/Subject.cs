using System.ComponentModel.DataAnnotations;

namespace SchoolSystem.Infrastructure.Models;

public class Subject
{
    [Key]
    public Guid Id { get; init; } = Guid.NewGuid();

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = null!;
    
    [Required]
    public Guid SchoolId { get; set; }
    
    public School School { get; set; } = null!;
}
