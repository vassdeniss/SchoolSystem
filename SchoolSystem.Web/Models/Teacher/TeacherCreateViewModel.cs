using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolSystem.Services.Dtos;
using SchoolSystem.Web.Models.Subject;

namespace SchoolSystem.Web.Models.Teacher;

public class TeacherCreateViewModel
{
    [Required(ErrorMessage = "Specialization is required")]
    [MaxLength(100)]
    public string Specialization { get; init; } = null!;
    
    [Required]
    public Guid SchoolId { get; set; }
    
    [Required(ErrorMessage = "Please select a teacher")]
    [Display(Name = "Teacher")]
    public Guid UserId { get; set; }
    
    public SelectList? AvailableTeachers { get; set; }
}
