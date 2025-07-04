﻿using Microsoft.AspNetCore.Identity;
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
    public User User7 { get; set; }
    public User User8 { get; set; }
    public User User9 { get; set; }
    public User User10 { get; set; }
    public User User11 { get; set; }
    public User User12 { get; set; }
    public User User13 { get; set; }
    public User User14 { get; set; }



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

    public Parent Parent1 { get; set; }
    public Parent Parent2 { get; set; }
    public Parent Parent3 { get; set; }
    public Parent Parent4 { get; set; }
    public Parent Parent5 { get; set; }
    public Parent Parent6 { get; set; }

    public Subject Subject1 { get; set; }
    public Subject Subject2 { get; set; }
    public Subject Subject3 { get; set; }

    public Teacher Teacher1 { get; set; }
    public Teacher Teacher2 { get; set; }

    public Curriculum Curriculum1 { get; set; }
    public Curriculum Curriculum2 { get; set; }

    public Grade Grade1 { get; set; }
    public Grade Grade2 { get; set; }

    public Attendance Attendance1 { get; set; }
    public Attendance Attendance2 { get; set; }

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
        this.User7 = CreateUser(userManager, "user7");
        this.User8 = CreateUser(userManager, "user8");
        this.User9 = CreateUser(userManager, "user9");
        this.User10 = CreateUser(userManager, "user10");
        this.User11 = CreateUser(userManager, "user11");
        this.User12 = CreateUser(userManager, "user12");
        this.User13 = CreateUser(userManager, "user13");
        this.User14 = CreateUser(userManager, "user14");
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

    private void SeedParents()
    {
        this.Parent1 = new Parent
        {
            PhoneNumber = "0897111111",
            UserId = this.User7.Id,
            Students = new HashSet<Student> { this.Student1 }
        };
        this.Parent2 = new Parent
        {
            PhoneNumber = "0897222222",
            UserId = this.User8.Id,
            Students = new HashSet<Student> { this.Student1 }
        };
        this.Parent3 = new Parent
        {
            PhoneNumber = "0897333333",
            UserId = this.User9.Id,
            Students = new HashSet<Student> { this.Student2 }
        };
        this.Parent4 = new Parent
        {
            PhoneNumber = "0897444444",
            UserId = this.User10.Id,
            Students = new HashSet<Student> { this.Student2 }
        };
        this.Parent5 = new Parent
        {
            PhoneNumber = "0897555555",
            UserId = this.User11.Id,
            Students = new HashSet<Student> { this.Student3 }
        };
        this.Parent6 = new Parent
        {
            PhoneNumber = "0897666666",
            UserId = this.User12.Id,
            Students = new HashSet<Student> { this.Student3 }
        };

        dbContext.AddRange(this.Parent1, this.Parent2, this.Parent3, this.Parent4, this.Parent5, this.Parent6);
    }

    private void SeedSubjects()
    {
        this.Subject1 = new Subject
        {
            Id = Guid.NewGuid(),
            Name = "Mathematics",
            SchoolId = this.School1.Id
        };

        this.Subject2 = new Subject
        {
            Id = Guid.NewGuid(),
            Name = "Chemistry",
            SchoolId = this.School1.Id
        };

        this.Subject3 = new Subject
        {
            Id = Guid.NewGuid(),
            Name = "History",
            SchoolId = this.School2.Id
        };

        dbContext.AddRange(this.Subject1, this.Subject2, this.Subject3);
    }

    private void SeedTeachers()
    {
        this.Teacher1 = new Teacher
        {
            Id = Guid.NewGuid(),
            Specialization = "Mathematics",
            UserId = this.User13.Id,
            User = this.User13,
            Schools = new HashSet<School> { this.School1 }
        };

        this.Teacher2 = new Teacher
        {
            Id = Guid.NewGuid(),
            Specialization = "Chemistry",
            UserId = this.User14.Id,
            User = this.User14,
            Schools = new HashSet<School> { this.School2 }
        };

        dbContext.AddRange(this.Teacher1, this.Teacher2);
    }

    public void SeedCurriculums()
    {
        this.Curriculum1 = new Curriculum
        {
            Id = Guid.NewGuid(),
            DayOfWeek = "Monday",
            StartTime = new TimeSpan(8, 30, 0),
            EndTime = new TimeSpan(9, 15, 0),
            TeacherId = this.Teacher1.Id,
            Teacher = this.Teacher1,
            ClassId = this.Class1.Id,
            Class = this.Class1,
            SubjectId = this.Subject1.Id,
            Subject = this.Subject1
        };

        this.Curriculum2 = new Curriculum
        {
            Id = Guid.NewGuid(),
            DayOfWeek = "Wednesday",
            StartTime = new TimeSpan(11, 0, 0),
            EndTime = new TimeSpan(11, 45, 0),
            TeacherId = this.Teacher2.Id,
            Teacher = this.Teacher2,
            ClassId = this.Class2.Id,
            Class = this.Class2,
            SubjectId = this.Subject2.Id,
            Subject = this.Subject2
        };

        dbContext.Curriculums.AddRange(Curriculum1, Curriculum2);
        dbContext.SaveChanges();
    }

    public void SeedGrades()
    {
        this.Grade1 = new Grade
        {
            Id = Guid.NewGuid(),
            GradeValue = 6,
            GradeDate = DateTime.Today.AddDays(-2),
            StudentId = this.Student1.Id,
            Student = this.Student1,
            SubjectId = this.Subject1.Id,
            Subject = this.Subject1
        };

        this.Grade2 = new Grade
        {
            Id = Guid.NewGuid(),
            GradeValue = 4,
            GradeDate = DateTime.Today.AddDays(-1),
            StudentId = this.Student1.Id,
            Student = this.Student1,
            SubjectId = this.Subject2.Id,
            Subject = this.Subject2
        };

        dbContext.Grades.AddRange(Grade1, Grade2);
        dbContext.SaveChanges();
    }

    public void SeedAttendances()
    {
        this.Attendance1 = new Attendance
        {
            Id = Guid.NewGuid(),
            StudentId = this.Student1.Id,
            Student = this.Student1,
            SubjectId = this.Subject1.Id,
            Subject = this.Subject1,
            AbsenceType = "Excused"
        };

        this.Attendance2 = new Attendance
        {
            Id = Guid.NewGuid(),
            StudentId = this.Student1.Id,
            Student = this.Student1,
            SubjectId = this.Subject2.Id,
            Subject = this.Subject2,
            AbsenceType = "Unexcused"
        };

        dbContext.Attendances.AddRange(Attendance1, Attendance2);
        dbContext.SaveChanges();
    }

    private void SeedDatabase()
    {
        var userManager = CreateUserManager();

        SeedUsers(userManager);
        SeedPrincipals();
        SeedSchools();
        SeedClasses();
        SeedStudents();
        SeedParents();
        SeedSubjects();
        SeedTeachers();
        SeedCurriculums();
        SeedGrades();
        SeedAttendances();

        dbContext.SaveChanges();
    }

    public void ClearDatabase()
    {
        dbContext.Subjects.RemoveRange(dbContext.Subjects);
        dbContext.Parents.RemoveRange(dbContext.Parents);
        dbContext.Students.RemoveRange(dbContext.Students);
        dbContext.Classes.RemoveRange(dbContext.Classes);
        dbContext.Schools.RemoveRange(dbContext.Schools);
        dbContext.Principals.RemoveRange(dbContext.Principals);
        dbContext.Users.RemoveRange(dbContext.Users);
        dbContext.Users.RemoveRange(dbContext.Users);

        dbContext.SaveChanges();
    }
}
