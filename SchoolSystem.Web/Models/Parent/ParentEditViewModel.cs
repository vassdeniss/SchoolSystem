using System.ComponentModel.DataAnnotations;
using SchoolSystem.Web.Models.Student;

namespace SchoolSystem.Web.Models.Parent;

public class ParentEditViewModel
{
    public Guid Id { get; init; }
    
    [Required(ErrorMessage = "Phone number is required")]
    [Phone(ErrorMessage = "Invalid phone number")]
    [Display(Name = "Phone Number")]
    public string PhoneNumber { get; init; } = null!;
    
    public IEnumerable<StudentViewModel> Students { get; init; } = new List<StudentViewModel>();
}
