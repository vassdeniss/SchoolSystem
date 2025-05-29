using System.ComponentModel.DataAnnotations;

namespace SchoolSystem.Web.Models.Subject;

public class SubjectEditViewModel
{
    public Guid Id { get; init; }
    
    [Required(ErrorMessage = "Subject name is required")]
    [MaxLength(100)]
    public string Name { get; init; } = null!;

    [Required]
    public Guid SchoolId { get; init; }
}
