﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SchoolSystem.Infrastructure.Configurations;
using SchoolSystem.Infrastructure.Models;

namespace SchoolSystem.Infrastructure;

public class SchoolLogContext(DbContextOptions<SchoolLogContext> options)
    : IdentityDbContext<User, IdentityRole<Guid>, Guid>(options)
{
    public DbSet<School> Schools { get; init; }
    public DbSet<Principal> Principals { get; init; }
    public DbSet<Subject> Subjects { get; init; }
    public DbSet<Teacher> Teachers { get; init; }
    public DbSet<TeacherSubject> TeacherSubjects { get; init; }
    public DbSet<Parent> Parents { get; init; }
    public DbSet<Class> Classes { get; init; }
    public DbSet<Student> Students { get; init; }
    public DbSet<Grade> Grades { get; init; }
    public DbSet<Attendance> Attendances { get; init; }
    public DbSet<Curriculum> Curriculums { get; init; }
    public DbSet<SubjectCurriculum> SubjectCurriculums { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new PrincipalConfiguration());
        modelBuilder.ApplyConfiguration(new SchoolConfiguration());
        modelBuilder.ApplyConfiguration(new ClassConfiguration());
        modelBuilder.ApplyConfiguration(new StudentConfiguration());
        modelBuilder.ApplyConfiguration(new SubjectConfiguration());
        modelBuilder.ApplyConfiguration(new TeacherConfiguration());
        modelBuilder.ApplyConfiguration(new ParentConfiguration());
        
        modelBuilder.Entity<Principal>()
            .HasOne(p => p.School)
            .WithOne(s => s.Principal)
            .HasForeignKey<School>(s => s.PrincipalId);
        
        modelBuilder.Entity<TeacherSubject>()
            .HasKey(ts => new { ts.TeacherId, ts.SubjectId });

        modelBuilder.Entity<SubjectCurriculum>()
            .HasKey(sc => new { sc.SubjectId, sc.CurriculumId });

        modelBuilder.Entity("SchoolTeacher").HasData(
            new { SchoolsId = Guid.Parse("b0cf0f90-50a5-4e86-9a29-fdf3928af26b"), TeachersId = Guid.Parse("8f374d37-5a0c-4637-ba8e-2b4d2ceef15f") },
            new { SchoolsId = Guid.Parse("b0cf0f90-50a5-4e86-9a29-fdf3928af26b"), TeachersId = Guid.Parse("2a3d47b0-28d1-48f9-bd9a-504a9f2a1cbd") }
        );
        
        modelBuilder.Entity("ParentStudent").HasData(
            new { ParentsId = Guid.Parse("8b5f7c12-0a97-4e45-9b34-123456789abc"), StudentsId = Guid.Parse("10101010-1010-1010-1010-101010101010") },
            new { ParentsId = Guid.Parse("8b5f7c12-0a97-4e45-9b34-123456789abc"), StudentsId = Guid.Parse("20202020-2020-2020-2020-202020202020") }
        );

        modelBuilder.Entity<Grade>()
            .HasOne(g => g.Student)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction);
        
        modelBuilder.Entity<Attendance>()
            .HasOne(a => a.Student)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction);
        
        base.OnModelCreating(modelBuilder);
    }
}
