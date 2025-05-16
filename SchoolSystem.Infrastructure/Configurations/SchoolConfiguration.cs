using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolSystem.Infrastructure.Models;
using System;
using System.Collections.Generic;

namespace SchoolSystem.Infrastructure.Configurations;

public class SchoolConfiguration : IEntityTypeConfiguration<School>
{
    public void Configure(EntityTypeBuilder<School> builder)
    {
        builder.HasOne(s => s.Principal)
            .WithOne(p => p.School)
            .HasForeignKey<School>(s => s.PrincipalId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasData(new List<School>
        {
            new()
            {
                Id = Guid.Parse("b0cf0f90-50a5-4e86-9a29-fdf3928af26b"),
                Name = "Westside Academy",
                Address = "456 West St, Townsville",
                PrincipalId = Guid.Parse("aae52599-edf2-4b1d-8f89-51a76b689d25")
            }
        });
    }
}
