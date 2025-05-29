using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolSystem.Infrastructure.Models;
using System;
using System.Collections.Generic;

namespace SchoolSystem.Infrastructure.Configurations;

public class SubjectConfiguration : IEntityTypeConfiguration<Subject>
{
    public void Configure(EntityTypeBuilder<Subject> builder)
    {
        builder.HasOne(s => s.School)
            .WithMany()
            .HasForeignKey(s => s.SchoolId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasData(new List<Subject>
        {
            new()
            {
                Id = Guid.Parse("a1b2c3d4-e5f6-4789-8abc-111111111111"),
                Name = "Mathematics",
                SchoolId = Guid.Parse("b0cf0f90-50a5-4e86-9a29-fdf3928af26b")
            },
            new()
            {
                Id = Guid.Parse("b2c3d4e5-f6a7-589a-8bcd-222222222222"),
                Name = "Physics",
                SchoolId = Guid.Parse("b0cf0f90-50a5-4e86-9a29-fdf3928af26b")
            },
            new()
            {
                Id = Guid.Parse("c3d4e5f6-a7b8-69ac-9bde-333333333333"),
                Name = "Chemistry",
                SchoolId = Guid.Parse("b0cf0f90-50a5-4e86-9a29-fdf3928af26b")
            },
            new()
            {
                Id = Guid.Parse("d4e5f6a7-b8c9-7abd-aced-444444444444"),
                Name = "History",
                SchoolId = Guid.Parse("b0cf0f90-50a5-4e86-9a29-fdf3928af26b")
            },
            new()
            {
                Id = Guid.Parse("e5f6a7b8-c9d0-8bde-afed-555555555555"),
                Name = "Biology",
                SchoolId = Guid.Parse("b0cf0f90-50a5-4e86-9a29-fdf3928af26b")
            }
        });
    }
}
