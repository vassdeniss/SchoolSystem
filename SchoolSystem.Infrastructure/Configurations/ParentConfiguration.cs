using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolSystem.Infrastructure.Models;
using System;
using System.Collections.Generic;

namespace SchoolSystem.Infrastructure.Configurations;

public class ParentConfiguration : IEntityTypeConfiguration<Parent>
{
    public void Configure(EntityTypeBuilder<Parent> builder)
    {
        builder.HasMany(p => p.Students)
            .WithMany(s => s.Parents);
        
        builder.HasData(new List<Parent>
        {
            new()
            {
                Id = Guid.Parse("8b5f7c12-0a97-4e45-9b34-123456789abc"),
                PhoneNumber = "0888123456",
                UserId = Guid.Parse("3905cc60-cff6-4b59-b365-03d1749d9c7b")
            },
            new()
            {
                Id = Guid.Parse("9c6f8d23-1b07-4f56-ab45-23456789abcd"),
                PhoneNumber = "0888234567",
                UserId = Guid.Parse("35ea475e-72e3-4786-8c66-c2586503171b")
            }
        });
    }
}
