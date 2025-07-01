namespace SchoolSystem.Web.Models.Attendance;

public class AttendanceViewModel
{
    public Guid Id { get; init; }
    
    public string SubjectName { get; init; } = null!;
    
    public string AbsenceType { get; init; } = null!;
}
