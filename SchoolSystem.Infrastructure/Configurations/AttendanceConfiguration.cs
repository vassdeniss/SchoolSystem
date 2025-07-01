using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolSystem.Infrastructure.Models;

namespace SchoolSystem.Infrastructure.Configurations;

public class AttendanceConfiguration : IEntityTypeConfiguration<Attendance>
{
    public void Configure(EntityTypeBuilder<Attendance> builder)
    {
        builder.HasOne(a => a.Student)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction);
        
        builder.HasData(new List<Attendance>
        {
            new()
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                StudentId = Guid.Parse("10101010-1010-1010-1010-101010101010"),
                SubjectId = Guid.Parse("a1b2c3d4-e5f6-4789-8abc-111111111111"),
                AbsenceType = "Sick Leave"
            },
            new()
            {
                Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                StudentId = Guid.Parse("10101010-1010-1010-1010-101010101010"),
                SubjectId = Guid.Parse("a1b2c3d4-e5f6-4789-8abc-111111111111"),
                AbsenceType = "Unexcused Absence"
            },
            new()
            {
                Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                StudentId = Guid.Parse("10101010-1010-1010-1010-101010101010"),
                SubjectId = Guid.Parse("a1b2c3d4-e5f6-4789-8abc-111111111111"),
                AbsenceType = "Late Arrival"
            }
        });
    }
}
