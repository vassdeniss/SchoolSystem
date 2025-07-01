namespace SchoolSystem.Web.Models.Attendance;

public class AttendanceListViewModel
{
    public Guid StudentId { get; init; }
    
    public string StudentName { get; init; } = null!;
    
    public Guid SchoolId { get; init; }
    
    public IEnumerable<AttendanceViewModel> Attendances { get; init; } = [];
}
