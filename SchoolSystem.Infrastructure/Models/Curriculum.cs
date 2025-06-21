using System.ComponentModel.DataAnnotations;

namespace SchoolSystem.Infrastructure.Models;

public class Curriculum
{
    [Key]
    public Guid Id { get; init; } = Guid.NewGuid();

    [Required]
    [MaxLength(20)]
    public string DayOfWeek { get; set; } = null!;

    [Required]
    public TimeSpan StartTime { get; set; }

    [Required]
    public TimeSpan EndTime { get; set; }

    public Guid TeacherId { get; set; }
    public Teacher Teacher { get; init; } = null!;

    public Guid ClassId { get; set; }
    public Class Class { get; init; } = null!;
    
    public Guid SubjectId { get; set; }
    public Subject Subject { get; init; } = null!;
}
