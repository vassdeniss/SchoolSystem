using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SchoolSystem.Web.Models.School;

public class SchoolEditViewModel
{
    public Guid Id { get; set; }
    
    [Required(ErrorMessage = "School name is required")]
    [MaxLength(100)]
    public string Name { get; init; } = null!;

    [Required(ErrorMessage = "Address is required")]
    [MaxLength(100)]
    public string Address { get; init; } = null!;
    
    [Required(ErrorMessage = "Please select a principal")]
    [Display(Name = "Principal")]
    public Guid PrincipalId { get; init; }
    
    public SelectList? AvailablePrincipals { get; set; }
}
