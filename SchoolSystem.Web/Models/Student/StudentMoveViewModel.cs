using Microsoft.AspNetCore.Mvc.Rendering;

namespace SchoolSystem.Web.Models.Student;

public class StudentMoveViewModel
{
    public Guid Id { get; init; }
    public string? StudentName { get; init; }
    public Guid CurrentClassId { get; init; }
    public Guid SchoolId { get; init; }
    public List<SelectListItem>? AvailableClasses { get; set; }
    public Guid ClassId { get; init; }
}
