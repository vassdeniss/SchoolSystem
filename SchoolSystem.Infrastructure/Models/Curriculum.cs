using System.ComponentModel.DataAnnotations;

namespace SchoolSystem.Infrastructure.Models;

public class Curriculum
{
    [Key]
    public Guid Id { get; init; } = Guid.NewGuid();

    [Required]
    [MaxLength(20)]
    public string DayOfWeek { get; init; } = null!;

    [Required]
    public TimeSpan StartTime { get; init; }

    [Required]
    public TimeSpan EndTime { get; init; }

    public Guid TeacherId { get; init; }
    public Teacher Teacher { get; init; } = null!;

    public Guid ClassId { get; init; }
    public Class Class { get; init; } = null!;
}

