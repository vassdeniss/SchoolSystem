using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolSystem.Infrastructure.Models;
using System;
using System.Collections.Generic;

namespace SchoolSystem.Infrastructure.Configurations;

public class TeacherConfiguration : IEntityTypeConfiguration<Teacher>
{
    public void Configure(EntityTypeBuilder<Teacher> builder)
    {
        builder.HasData(new List<Teacher>
        {
            new()
            {
                Id = Guid.Parse("8f374d37-5a0c-4637-ba8e-2b4d2ceef15f"),
                Specialization = "Applied Mathematics",
                UserId = Guid.Parse("1396ac5f-745d-47b4-8cef-21a0e7e32bd9")
            },
            new()
            {
                Id = Guid.Parse("2a3d47b0-28d1-48f9-bd9a-504a9f2a1cbd"),
                Specialization = "Theoretical Physics",
                UserId = Guid.Parse("7f74d5cd-5061-4e53-a10b-221cfb9488a0")
            }
        });
    }
}
