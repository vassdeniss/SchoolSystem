using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolSystem.Infrastructure.Models;
using System;
using System.Collections.Generic;

namespace SchoolSystem.Infrastructure.Configurations
{
    public class PrincipalConfiguration : IEntityTypeConfiguration<Principal>
    {
        public void Configure(EntityTypeBuilder<Principal> builder)
        {
            builder.HasData(new List<Principal>
            {
                new Principal
                {
                    Id = Guid.Parse("3b9f9d48-2a86-4a3c-88b1-09e9c1f9c1e8"),
                    Specialization = "Educational Administration",
                    PhoneNumber = "0888123456",
                    // School: Central High School
                    SchoolId = Guid.Parse("e84a9f88-379d-4b86-9b02-db6e6434f2a0"),
                    // Principal User
                    UserId = Guid.Parse("e2f4d483-a7fe-4b3c-9d6f-0c24e1234567")
                },
                new Principal
                {
                    Id = Guid.Parse("aae52599-edf2-4b1d-8f89-51a76b689d25"),
                    Specialization = "School Management",
                    PhoneNumber = "0888234567",
                    // School: Westside Academy
                    SchoolId = Guid.Parse("b0cf0f90-50a5-4e86-9a29-fdf3928af26b"),
                    // Principal User
                    UserId = Guid.Parse("c7d81a30-6455-4a1f-8f47-923c1234abcd")
                }
            });
        }
    }
}
