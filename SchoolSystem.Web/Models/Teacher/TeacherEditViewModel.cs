using System.ComponentModel.DataAnnotations;

namespace SchoolSystem.Web.Models.Teacher;

public class TeacherEditViewModel
{
    public Guid Id { get; init; }
    
    [Required(ErrorMessage = "Specialization is required")]
    [MaxLength(100)]
    public string Specialization { get; init; } = null!;

    [Required]
    public Guid SchoolId { get; set; }
}
