using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolSystem.Infrastructure.Models;
using System;
using System.Collections.Generic;

namespace SchoolSystem.Infrastructure.Configurations;

public class CurriculumConfiguration : IEntityTypeConfiguration<Curriculum>
{
    public void Configure(EntityTypeBuilder<Curriculum> builder)
    {
        builder.HasData(new List<Curriculum>
        {
            new()
            {
                Id = Guid.Parse("1a2b3c4d-1111-2222-3333-444455556666"),
                DayOfWeek = "Monday",
                StartTime = new TimeSpan(8, 0, 0),
                EndTime = new TimeSpan(9, 30, 0),
                TeacherId = Guid.Parse("8f374d37-5a0c-4637-ba8e-2b4d2ceef15f"),
                ClassId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                SubjectId = Guid.Parse("a1b2c3d4-e5f6-4789-8abc-111111111111")
            },
            new()
            {
                Id = Guid.Parse("2b3c4d5e-2222-3333-4444-555566667777"),
                DayOfWeek = "Wednesday",
                StartTime = new TimeSpan(10, 0, 0),
                EndTime = new TimeSpan(11, 30, 0),
                TeacherId = Guid.Parse("8f374d37-5a0c-4637-ba8e-2b4d2ceef15f"),
                ClassId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                SubjectId = Guid.Parse("a1b2c3d4-e5f6-4789-8abc-111111111111")
            },
            new()
            {
                Id = Guid.Parse("3c4d5e6f-3333-4444-5555-666677778888"),
                DayOfWeek = "Friday",
                StartTime = new TimeSpan(13, 0, 0),
                EndTime = new TimeSpan(14, 30, 0),
                TeacherId = Guid.Parse("2a3d47b0-28d1-48f9-bd9a-504a9f2a1cbd"),
                ClassId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                SubjectId = Guid.Parse("b2c3d4e5-f6a7-589a-8bcd-222222222222")
            }
        });
    }
}