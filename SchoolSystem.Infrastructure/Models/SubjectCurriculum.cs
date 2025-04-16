using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolSystem.Infrastructure.Models;

public class SubjectCurriculum
{
    [Key, Column(Order = 0)]
    public Guid SubjectId { get; init; }

    [Key, Column(Order = 1)]
    public Guid CurriculumId { get; init; }

    public Subject Subject { get; init; } = null!;
    public Curriculum Curriculum { get; init; } = null!;
}
