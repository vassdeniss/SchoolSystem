using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolSystem.Infrastructure.Models;

public class ParentStudent
{
    [Key, Column(Order = 0)]
    public Guid ParentId { get; init; }

    [Key, Column(Order = 1)]
    public Guid StudentId { get; init; }

    public Parent Parent { get; init; } = null!;
    public Student Student { get; init; } = null!;
}
