using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SchoolSystem.Web.Models.Grade;

public class GradeFormViewModel
{
    public Guid Id { get; init; }
    
    [Required]
    public int GradeValue { get; init; }

    [Required]
    public DateTime GradeDate { get; init; }

    [Required]
    public Guid StudentId { get; init; }

    [Required]
    public Guid ClassId { get; init; }
    
    [Required]
    public Guid SubjectId { get; init; }
    
    [Required]
    public Guid SchoolId { get; init; }
    
    public SelectList? AvailableSubjects { get; set; }
}
