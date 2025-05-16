using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SchoolSystem.Web.Models.Principal;

public class PrincipalCreateViewModel
{
    [Required(ErrorMessage = "Specialization is required")]
    [Display(Name = "Specialization")]
    [StringLength(50, ErrorMessage = "Maximum 50 characters")]
    public string Specialization { get; init; } = null!;

    [Required(ErrorMessage = "Phone number is required")]
    [Phone(ErrorMessage = "Invalid phone number")]
    [Display(Name = "Phone Number")]
    public string PhoneNumber { get; init; } = null!;

    [Required(ErrorMessage = "Please select a principal")]
    [Display(Name = "Principal")]
    public Guid UserId { get; init; }
    
    public SelectList? AvailablePrincipals { get; set; }
}
