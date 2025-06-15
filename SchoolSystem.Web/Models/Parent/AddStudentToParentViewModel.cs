using Microsoft.AspNetCore.Mvc.Rendering;

namespace SchoolSystem.Web.Models.Parent;

public class AddStudentToParentViewModel
{
    public Guid ParentId { get; set; }
    public Guid SelectedStudentId { get; set; }
    public SelectList? AvailableStudents { get; set; }
}
