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
        builder.HasData(new List<Parent>
        {
            new()
            {
                Id = Guid.Parse("8b5f7c12-0a97-4e45-9b34-123456789abc"),
                PhoneNumber = "0888123456",
                UserId = Guid.Parse("11111111-1111-1111-1111-111111111111")
            },
            new()
            {
                Id = Guid.Parse("9c6f8d23-1b07-4f56-ab45-23456789abcd"),
                PhoneNumber = "0888234567",
                UserId = Guid.Parse("22222222-2222-2222-2222-222222222222")
            },
            new()
            {
                Id = Guid.Parse("ac7f9e34-2c18-4f67-bc56-3456789abcde"),
                PhoneNumber = "0888345678",
                UserId = Guid.Parse("33333333-3333-3333-3333-333333333333")
            }
        });
    }
}