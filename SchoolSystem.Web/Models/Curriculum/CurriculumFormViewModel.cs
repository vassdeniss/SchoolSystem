using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SchoolSystem.Web.Models.Curriculum;

public class CurriculumFormViewModel
{
    public Guid Id { get; init; }
    
    [Required]
    public string DayOfWeek { get; init; } = null!;

    [Required]
    public TimeSpan StartTime { get; init; }

    [Required]
    public TimeSpan EndTime { get; init; }

    [Required]
    public Guid TeacherId { get; init; }
    
    public SelectList? AvailableTeachers { get; set; }

    [Required]
    public Guid ClassId { get; init; }
    
    [Required]
    public Guid SubjectId { get; init; }
    
    [Required]
    public Guid SchoolId { get; init; }
    
    public SelectList? AvailableSubjects { get; set; }
}
