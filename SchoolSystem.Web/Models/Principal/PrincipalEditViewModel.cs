using System.ComponentModel.DataAnnotations;

namespace SchoolSystem.Web.Models.Principal;

public class PrincipalEditViewModel
{
    public Guid Id { get; init; }
    
    [Required(ErrorMessage = "Specialization is required")]
    [Display(Name = "Specialization")]
    [StringLength(50, ErrorMessage = "Maximum 50 characters")]
    public string Specialization { get; init; } = null!;

    [Required(ErrorMessage = "Phone number is required")]
    [Phone(ErrorMessage = "Invalid phone number")]
    [Display(Name = "Phone Number")]
    public string PhoneNumber { get; init; } = null!;
}
