using System.ComponentModel.DataAnnotations;

namespace SchoolSystem.Infrastructure.Models;

public class Class
{
    [Key]
    public Guid Id { get; init; } = Guid.NewGuid();

    [Required]
    [MaxLength(10)]
    public string Name { get; set; } = null!;

    [Required]
    public int Year { get; set; }

    [Required] [MaxLength(20)] public string Term { get; set; } = null!;
    
    public Guid SchoolId { get; set; }
    
    public School School { get; init; } = null!;
}
