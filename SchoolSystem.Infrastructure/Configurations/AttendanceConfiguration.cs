using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolSystem.Infrastructure.Models;

namespace SchoolSystem.Infrastructure.Configurations;

public class AttendanceConfiguration : IEntityTypeConfiguration<Attendance>
{
    public void Configure(EntityTypeBuilder<Attendance> builder)
    {
        builder.HasData(new List<Attendance>
        {
            new()
            {
                // Примерен запис: формуляр с група данни за отсъствие
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                StudentId = Guid.Parse("10101010-1010-1010-1010-101010101010"),
                SubjectId = Guid.Parse("20202020-2020-2020-2020-202020202020"),
                AbsenceType = "Sick Leave",
                Status = "Approved"
            },
            new()
            {
                Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                StudentId = Guid.Parse("30303030-3030-3030-3030-303030303030"),
                SubjectId = Guid.Parse("40404040-4040-4040-4040-404040404040"),
                AbsenceType = "Unexcused Absence",
                Status = "Absent"
            },
            new()
            {
                Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                StudentId = Guid.Parse("50505050-5050-5050-5050-505050505050"),
                SubjectId = Guid.Parse("60606060-6060-6060-6060-606060606060"),
                AbsenceType = "Late Arrival",
                Status = "Approved"
            }
        });
    }
}