using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolSystem.Infrastructure.Models;
using System;
using System.Collections.Generic;

namespace SchoolSystem.Infrastructure.Configurations
{
    public class SubjectConfiguration : IEntityTypeConfiguration<Subject>
    {
        public void Configure(EntityTypeBuilder<Subject> builder)
        {
            builder.HasData(new List<Subject>
            {
                new Subject
                {
                    Id = Guid.Parse("a1b2c3d4-e5f6-4789-8abc-111111111111"),
                    Name = "Mathematics",
                    DepartmentId = Guid.Parse("d3f1c3e1-4f7e-4c92-8d6b-1234567890ab")
                },
                new Subject
                {
                    Id = Guid.Parse("b2c3d4e5-f6a7-589a-8bcd-222222222222"),
                    Name = "Physics",
                    DepartmentId = Guid.Parse("e7c1a0d2-6e2b-4bbc-8bbc-abcdef123456")
                },
                new Subject
                {
                    Id = Guid.Parse("c3d4e5f6-a7b8-69ac-9bde-333333333333"),
                    Name = "Chemistry",
                    DepartmentId = Guid.Parse("f8d2b1c3-7a8b-4e9d-9cde-123456789abc")
                },
                new Subject
                {
                    Id = Guid.Parse("d4e5f6a7-b8c9-7abd-aced-444444444444"),
                    Name = "History",
                    DepartmentId = Guid.Parse("a9b8c7d6-e5f4-3210-abcd-9876543210fe")
                },
                new Subject
                {
                    Id = Guid.Parse("e5f6a7b8-c9d0-8bde-afed-555555555555"),
                    Name = "Biology",
                    DepartmentId = Guid.Parse("b0a1c2d3-e4f5-6789-cdef-0987654321ab")
                }
            });
        }
    }
}
