using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolSystem.Infrastructure.Models;
using System;
using System.Collections.Generic;

namespace SchoolSystem.Infrastructure.Configurations
{
    public class GradeConfiguration : IEntityTypeConfiguration<Grade>
    {
        public void Configure(EntityTypeBuilder<Grade> builder)
        {
            builder.HasData(new List<Grade>
            {
                new Grade
                {
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    StudentId = Guid.Parse("10101010-1010-1010-1010-101010101010"),
                    SubjectId = Guid.Parse("20202020-2020-2020-2020-202020202020"),
                    GradeValue = 5,
                    GradeDate = new DateTime(2025, 1, 15, 0, 0, 0, DateTimeKind.Utc)
                },
                new Grade
                {
                    Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    StudentId = Guid.Parse("30303030-3030-3030-3030-303030303030"),
                    SubjectId = Guid.Parse("40404040-4040-4040-4040-404040404040"),
                    GradeValue = 4,
                    GradeDate = new DateTime(2025, 2, 20, 0, 0, 0, DateTimeKind.Utc)
                },
                new Grade
                {
                    Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    StudentId = Guid.Parse("50505050-5050-5050-5050-505050505050"),
                    SubjectId = Guid.Parse("60606060-6060-6060-6060-606060606060"),
                    GradeValue = 6,
                    GradeDate = new DateTime(2025, 3, 25, 0, 0, 0, DateTimeKind.Utc)
                }
            });
        }
    }
}
