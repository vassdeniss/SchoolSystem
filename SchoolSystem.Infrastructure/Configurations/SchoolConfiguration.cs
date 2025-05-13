using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolSystem.Infrastructure.Models;
using System;
using System.Collections.Generic;

namespace SchoolSystem.Infrastructure.Configurations
{
    public class SchoolConfiguration : IEntityTypeConfiguration<School>
    {
        public void Configure(EntityTypeBuilder<School> builder)
        {
            builder.HasData(new List<School>
            {
                new School
                {
                    Id = Guid.Parse("e84a9f88-379d-4b86-9b02-db6e6434f2a0"),
                    Name = "Central High School",
                    Address = "123 Main St, Cityville"
                },
                new School
                {
                    Id = Guid.Parse("b0cf0f90-50a5-4e86-9a29-fdf3928af26b"),
                    Name = "Westside Academy",
                    Address = "456 West St, Townsville"
                },
                new School
                {
                    Id = Guid.Parse("d41f3aa8-3a0a-46c0-95d3-06e3b92de6a4"),
                    Name = "Eastside Institute",
                    Address = "789 East St, Villagetown"
                }
            });
        }
    }
}
