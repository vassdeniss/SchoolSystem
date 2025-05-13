using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolSystem.Infrastructure.Models;
using System;
using System.Collections.Generic;

namespace SchoolSystem.Infrastructure.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasData(new List<User>
            {
                new User
                {
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    FirstName = "John",
                    MiddleName = "A.",
                    LastName = "Doe",
                    DateOfBirth = new DateTime(1990, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    UserName = "johndoe",
                    NormalizedUserName = "JOHNDOE",
                    Email = "johndoe@example.com",
                    NormalizedEmail = "JOHNDOE@EXAMPLE.COM",
                    EmailConfirmed = true
                },
                new User
                {
                    Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    FirstName = "Jane",
                    MiddleName = "B.",
                    LastName = "Smith",
                    DateOfBirth = new DateTime(1985, 5, 15, 0, 0, 0, DateTimeKind.Utc),
                    UserName = "janesmith",
                    NormalizedUserName = "JANESMITH",
                    Email = "janesmith@example.com",
                    NormalizedEmail = "JANESMITH@EXAMPLE.COM",
                    EmailConfirmed = true
                },
                new User
                {
                    Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    FirstName = "Alice",
                    MiddleName = "C.",
                    LastName = "Brown",
                    DateOfBirth = new DateTime(1992, 11, 23, 0, 0, 0, DateTimeKind.Utc),
                    UserName = "alicebrown",
                    NormalizedUserName = "ALICEBROWN",
                    Email = "alicebrown@example.com",
                    NormalizedEmail = "ALICEBROWN@EXAMPLE.COM",
                    EmailConfirmed = true
                },
                new User
                {
                    Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                    FirstName = "Bob",
                    MiddleName = "D.",
                    LastName = "Wilson",
                    DateOfBirth = new DateTime(1988, 7, 8, 0, 0, 0, DateTimeKind.Utc),
                    UserName = "bobwilson",
                    NormalizedUserName = "BOBWILSON",
                    Email = "bobwilson@example.com",
                    NormalizedEmail = "BOBWILSON@EXAMPLE.COM",
                    EmailConfirmed = true
                },
                new User
                {
                    Id = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                    FirstName = "Charlie",
                    MiddleName = "E.",
                    LastName = "Evans",
                    DateOfBirth = new DateTime(1995, 3, 30, 0, 0, 0, DateTimeKind.Utc),
                    UserName = "charlieevans",
                    NormalizedUserName = "CHARLIEEVANS",
                    Email = "charlieevans@example.com",
                    NormalizedEmail = "CHARLIEEVANS@EXAMPLE.COM",
                    EmailConfirmed = true
                }
            });
        }
    }
}
