using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolSystem.Infrastructure.Models;
using System;
using System.Collections.Generic;

namespace SchoolSystem.Infrastructure.Configurations;

public class GradeConfiguration : IEntityTypeConfiguration<Grade>
{
    public void Configure(EntityTypeBuilder<Grade> builder)
    {
        builder.HasData(new List<Grade>
        {
            new()
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                StudentId = Guid.Parse("10101010-1010-1010-1010-101010101010"),
                SubjectId = Guid.Parse("a1b2c3d4-e5f6-4789-8abc-111111111111"),
                GradeValue = 5,
                GradeDate = new DateTime(2025, 1, 15, 0, 0, 0, DateTimeKind.Utc)
            },
            new()
            {
                Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                StudentId = Guid.Parse("10101010-1010-1010-1010-101010101010"),
                SubjectId = Guid.Parse("a1b2c3d4-e5f6-4789-8abc-111111111111"),
                GradeValue = 4,
                GradeDate = new DateTime(2025, 2, 20, 0, 0, 0, DateTimeKind.Utc)
            },
            new()
            {
                Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                StudentId = Guid.Parse("10101010-1010-1010-1010-101010101010"),
                SubjectId = Guid.Parse("a1b2c3d4-e5f6-4789-8abc-111111111111"),
                GradeValue = 6,
                GradeDate = new DateTime(2025, 3, 25, 0, 0, 0, DateTimeKind.Utc)
            }
        });
    }
}