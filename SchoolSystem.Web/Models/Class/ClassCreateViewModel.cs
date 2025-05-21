using System.ComponentModel.DataAnnotations;

namespace SchoolSystem.Web.Models.Class;

public class ClassCreateViewModel
{
    [Required(ErrorMessage = "Class name is required")]
    [MaxLength(100)]
    public string Name { get; init; } = null!;

    [Required(ErrorMessage = "Year is required")]
    public int Year { get; init; }
    
    [Required(ErrorMessage = "Term is required")]
    [Display(Name = "Term")]
    public string Term { get; init; } = null!;
    
    [Required]
    public Guid SchoolId { get; init; }
}
