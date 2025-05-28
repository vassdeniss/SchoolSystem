using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SchoolSystem.Web.Models.Student;

public class StudentCreateViewModel
{
    public Guid ClassId { get; set; }
    
    public Guid SchoolId { get; set; }
    
    [Required(ErrorMessage = "Please select a student")]
    [Display(Name = "Student")]
    public Guid UserId { get; init; }
    
    public SelectList? AvailableStudents { get; set; }
}
