using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using SchoolSystem.Infrastructure;
using SchoolSystem.Infrastructure.Models;

namespace SchoolSystem.Tests.Common;

public class SchoolLogTestDb
{
    public SchoolLogTestDb(SchoolLogContext dbContext)
    {
        this.SeedDatabase(dbContext);
    }

    public User User1 { get; set; }
    public User User2 { get; set; }
    public User User3 { get; set; }

    public Principal Principal1 { get; set; }
    public Principal Principal2 { get; set; }
    
    private void SeedDatabase(SchoolLogContext dbContext)
    {
        UserOnlyStore<User, SchoolLogContext, Guid> userStore = new(dbContext);
        PasswordHasher<User> hasher = new();
        UpperInvariantLookupNormalizer normalizer = new();
        UserManager<User> userManager = new(
            userStore, 
            null, 
            hasher, 
            null, null, 
            normalizer, 
            null, null, null);

        this.User1 = new User
        {
            Id = Guid.NewGuid(),
            UserName = $"user1{DateTime.Now.Ticks.ToString().Substring(10)}",
            NormalizedUserName = $"USER1{DateTime.Now.Ticks.ToString().Substring(10)}",
            Email = "user1@mail.com",
            NormalizedEmail = "USER1@MAIL.COM", 
            FirstName = "User",
            MiddleName = "G.",
            LastName = "1",
            DateOfBirth = DateTime.Today.AddYears(-20),
            EmailConfirmed = true,
        };

        userManager.CreateAsync(this.User1, "user1Pass")
            .Wait();

        this.User2 = new User
        {
            Id = Guid.NewGuid(),
            UserName = $"user2{DateTime.Now.Ticks.ToString().Substring(10)}",
            NormalizedUserName = $"USER2{DateTime.Now.Ticks.ToString().Substring(10)}",
            Email = "user2@mail.com",
            NormalizedEmail = "USER2@MAIL.COM", 
            FirstName = "User",
            MiddleName = "G.",
            LastName = "2",
            DateOfBirth = DateTime.Today.AddYears(-30),
            EmailConfirmed = true,
        };

        userManager.CreateAsync(this.User2, "user2Pass")
            .Wait();
        
        this.User3 = new User
        {
            Id = Guid.NewGuid(),
            UserName = $"user3{DateTime.Now.Ticks.ToString().Substring(10)}",
            NormalizedUserName = $"USER3{DateTime.Now.Ticks.ToString().Substring(10)}",
            Email = "user3@mail.com",
            NormalizedEmail = "USER3@MAIL.COM", 
            FirstName = "User",
            MiddleName = "G.",
            LastName = "3",
            DateOfBirth = DateTime.Today.AddYears(-10),
            EmailConfirmed = true,
        };

        userManager.CreateAsync(this.User3, "user3Pass")
            .Wait();

        this.Principal1 = new Principal
        {
            Id = Guid.NewGuid(),
            Specialization = "Specialization1",
            PhoneNumber = "123456789",
            UserId = this.User1.Id
        };

        dbContext.Add(this.Principal1);

        this.Principal2 = new Principal
        {
            Id = Guid.NewGuid(),
            Specialization = "Specialization2",
            PhoneNumber = "132456789",
            UserId = this.User2.Id
        };
        
        dbContext.Add(this.Principal2);
        
        dbContext.SaveChanges();
    }
}