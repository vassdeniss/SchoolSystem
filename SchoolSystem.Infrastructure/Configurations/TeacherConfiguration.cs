using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolSystem.Infrastructure.Models;
using System;
using System.Collections.Generic;

namespace SchoolSystem.Infrastructure.Configurations
{
    public class TeacherConfiguration : IEntityTypeConfiguration<Teacher>
    {
        public void Configure(EntityTypeBuilder<Teacher> builder)
        {
            builder.HasData(new List<Teacher>
            {
                new Teacher
                {
                    Id = Guid.Parse("8f374d37-5a0c-4637-ba8e-2b4d2ceef15f"),
                    Specialization = "Applied Mathematics",
                    SchoolId = Guid.Parse("d3f1c3e1-4f7e-4c92-8d6b-1234567890ab"),
                    UserId = Guid.Parse("11111111-1111-1111-1111-111111111111")
                },
                new Teacher
                {
                    Id = Guid.Parse("2a3d47b0-28d1-48f9-bd9a-504a9f2a1cbd"),
                    Specialization = "Theoretical Physics",
                    SchoolId = Guid.Parse("e7c1a0d2-6e2b-4bbc-8bbc-abcdef123456"),
                    UserId = Guid.Parse("22222222-2222-2222-2222-222222222222")
                },
                new Teacher
                {
                    Id = Guid.Parse("f1e3d2c1-9b8a-4d3f-867a-112233445566"),
                    Specialization = "Modern History",
                    SchoolId = Guid.Parse("a1b2c3d4-e5f6-4789-8abc-def012345678"),
                    UserId = Guid.Parse("33333333-3333-3333-3333-333333333333")
                }
            });
        }
    }
}
