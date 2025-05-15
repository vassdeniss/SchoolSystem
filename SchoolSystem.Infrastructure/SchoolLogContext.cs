using Microsoft.AspNetCore.Identity;
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
    public DbSet<Department> Departments { get; init; }
    public DbSet<Subject> Subjects { get; init; }
    public DbSet<Teacher> Teachers { get; init; }
    public DbSet<TeacherSubject> TeacherSubjects { get; init; }
    public DbSet<Parent> Parents { get; init; }
    public DbSet<Class> Classes { get; init; }
    public DbSet<Student> Students { get; init; }
    public DbSet<ParentStudent> ParentStudents { get; init; }
    public DbSet<Grade> Grades { get; init; }
    public DbSet<Attendance> Attendances { get; init; }
    public DbSet<Curriculum> Curriculums { get; init; }
    public DbSet<SubjectCurriculum> SubjectCurriculums { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new SchoolConfiguration());
        modelBuilder.ApplyConfiguration(new PrincipalConfiguration());
        
        modelBuilder.Entity<Principal>()
            .HasOne(p => p.School)
            .WithOne(s => s.Principal)
            .HasForeignKey<School>(s => s.PrincipalId);
        
        modelBuilder.Entity<TeacherSubject>()
            .HasKey(ts => new { ts.TeacherId, ts.SubjectId });

        modelBuilder.Entity<ParentStudent>()
            .HasKey(ps => new { ps.ParentId, ps.StudentId });
        
        modelBuilder.Entity<ParentStudent>()
            .HasOne(ps => ps.Parent)
            .WithMany()
            .HasForeignKey(ps => ps.ParentId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ParentStudent>()
            .HasOne(ps => ps.Student)
            .WithMany()
            .HasForeignKey(ps => ps.StudentId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<SubjectCurriculum>()
            .HasKey(sc => new { sc.SubjectId, sc.CurriculumId });

        base.OnModelCreating(modelBuilder);
    }
}
