using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using SchoolSystem.Infrastructure;
using SchoolSystem.Infrastructure.Models;

namespace SchoolSystem.Tests.Common;

public class SchoolLogTestDb
{
    internal readonly SchoolLogContext dbContext;

    public SchoolLogTestDb(SchoolLogContext dbContext)
    {
        this.dbContext = dbContext;
        this.SeedDatabase();
    }

    public User User1 { get; set; }
    public User User2 { get; set; }
    public User User3 { get; set; }
    public User User4 { get; set; }
    public User User5 { get; set; }
    public User User6 { get; set; }

    public Principal Principal1 { get; set; }
    public Principal Principal2 { get; set; }
    public Principal Principal3 { get; set; }

    public School School1 { get; set; }
    public School School2 { get; set; }

    public Class Class1 { get; set; }
    public Class Class2 { get; set; }

    public Student Student1 { get; set; }
    public Student Student2 { get; set; }
    public Student Student3 { get; set; }

    private UserManager<User> CreateUserManager()
    {
        var userStore = new UserOnlyStore<User, SchoolLogContext, Guid>(dbContext);
        return new UserManager<User>(
            userStore,
            null!,
            new PasswordHasher<User>(),
            null!, null!,
            new UpperInvariantLookupNormalizer(),
            null!, null!, null!);
    }

    private User CreateUser(UserManager<User> userManager, string prefix)
    {
        string ticks = DateTime.Now.Ticks.ToString().Substring(10);
        string userName = $"{prefix}{ticks}";
        string normalizedUserName = userName.ToUpper();
        string email = $"{prefix}@mail.com";
        string normalizedEmail = email.ToUpper();

        User user = new()
        {
            Id = Guid.NewGuid(),
            UserName = userName,
            NormalizedUserName = normalizedUserName,
            Email = email,
            NormalizedEmail = normalizedEmail,
            FirstName = "User",
            MiddleName = "G.",
            LastName = prefix.Replace("user", ""),
            DateOfBirth = DateTime.Today.AddYears(-10),
            EmailConfirmed = true
        };

        userManager.CreateAsync(user, $"{prefix}Pass").Wait();
        return user;
    }

    private void SeedUsers(UserManager<User> userManager)
    {
        this.User1 = CreateUser(userManager, "user1");
        this.User2 = CreateUser(userManager, "user2");
        this.User3 = CreateUser(userManager, "user3");
        this.User4 = CreateUser(userManager, "user4");
        this.User5 = CreateUser(userManager, "user5");
        this.User6 = CreateUser(userManager, "user6");
    }

    private void SeedPrincipals()
    {
        this.Principal1 = new Principal
        {
            Id = Guid.NewGuid(),
            Specialization = "Mathematics",
            PhoneNumber = "123456789",
            UserId = this.User1.Id
        };
        this.Principal2 = new Principal
        {
            Id = Guid.NewGuid(),
            Specialization = "Chemistry",
            PhoneNumber = "132456789",
            UserId = this.User2.Id
        };
        this.Principal3 = new Principal
        {
            Id = Guid.NewGuid(),
            Specialization = "History",
            PhoneNumber = "132456782",
            UserId = this.User3.Id
        };

        dbContext.AddRange(this.Principal1, this.Principal2, this.Principal3);
    }

    private void SeedSchools()
    {
        this.School1 = new School
        {
            Id = Guid.NewGuid(),
            Address = "address1",
            Name = "school1",
            PrincipalId = this.Principal1.Id
        };
        this.School2 = new School
        {
            Id = Guid.NewGuid(),
            Address = "address2",
            Name = "school2",
            PrincipalId = this.Principal2.Id
        };

        dbContext.AddRange(this.School1, this.School2);
    }

    private void SeedClasses()
    {
        this.Class1 = new Class
        {
            Id = Guid.NewGuid(),
            Name = "class1",
            Year = DateTime.Now.Year,
            Term = "Fall",
            SchoolId = this.School1.Id
        };
        this.Class2 = new Class
        {
            Id = Guid.NewGuid(),
            Name = "class2",
            Year = DateTime.Now.Year,
            Term = "Spring",
            SchoolId = this.School1.Id
        };

        dbContext.AddRange(this.Class1, this.Class2);
    }

    private void SeedStudents()
    {
        this.Student3 = new Student
        {
            Id = Guid.NewGuid(),
            UserId = this.User4.Id,
            ClassId = this.Class2.Id
        };
        this.Student1 = new Student
        {
            Id = Guid.NewGuid(),
            UserId = this.User5.Id,
            ClassId = this.Class1.Id
        };
        this.Student2 = new Student
        {
            Id = Guid.NewGuid(),
            UserId = this.User6.Id,
            ClassId = this.Class1.Id
        };

        dbContext.AddRange(this.Student1, this.Student2);
    }

    private void SeedDatabase()
    {
        var userManager = CreateUserManager();

        SeedUsers(userManager);
        SeedPrincipals();
        SeedSchools();
        SeedClasses();
        SeedStudents();

        dbContext.SaveChanges();
    }

    public void ClearDatabase()
    {
        dbContext.Students.RemoveRange(dbContext.Students);
        dbContext.Classes.RemoveRange(dbContext.Classes);
        dbContext.Schools.RemoveRange(dbContext.Schools);
        dbContext.Principals.RemoveRange(dbContext.Principals);
        dbContext.Users.RemoveRange(dbContext.Users);                                   

        dbContext.SaveChanges();
    }

}
