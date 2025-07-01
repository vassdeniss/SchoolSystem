using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SchoolSystem.Web.Models.Attendance;

public class AttendanceFormViewModel
{
    public Guid Id { get; init; }
    
    [Required]
    public Guid StudentId { get; init; }
    
    [Required]
    public Guid SubjectId { get; init; }
    
    public Guid SchoolId { get; init; }
    
    public SelectList? AvailableSubjects { get; set; }
    
    public string AbsenceType { get; init; } = null!;
}
