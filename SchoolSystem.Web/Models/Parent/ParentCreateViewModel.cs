using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SchoolSystem.Web.Models.Parent;

public class ParentCreateViewModel
{
    [Required(ErrorMessage = "Phone number is required")]
    [Phone(ErrorMessage = "Invalid phone number")]
    [Display(Name = "Phone Number")]
    public string PhoneNumber { get; init; } = null!;

    [Required(ErrorMessage = "Please select a principal")]
    [Display(Name = "Principal")]
    public Guid UserId { get; init; }
    
    public SelectList? AvailableParents { get; set; }
}
