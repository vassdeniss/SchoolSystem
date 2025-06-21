namespace SchoolSystem.Web.Models.Curriculum;

public class CurriculumViewModel
{
    public Guid Id { get; init; }
    
    public string DayOfWeek { get; init; } = null!;

    public TimeSpan StartTime { get; init; }

    public TimeSpan EndTime { get; init; }

    public string TeacherName { get; init; } = null!;
    
    public string SubjectName { get; init; } = null!;
    
    public string ClassName { get; init; } = null!;
}
