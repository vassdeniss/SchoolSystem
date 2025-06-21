namespace SchoolSystem.Web.Models.Curriculum;

public class CurriculumListViewModel
{
    public Guid ClassId { get; init; }
    public string ClassName { get; init; } = null!;
    public Guid SchoolId { get; init; }
    public IEnumerable<CurriculumViewModel> Curriculum { get; init; } = [];
}
