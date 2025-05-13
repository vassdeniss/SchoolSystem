using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SchoolSystem.Infrastructure.Models;

namespace SchoolSystem.Infrastructure;

public class SchoolLogContext(DbContextOptions<SchoolLogContext> options)
    : IdentityDbContext<User, IdentityRole<Guid>, Guid>(options)
{
    public DbSet<School> Schools { get; set; }
    public DbSet<Principal> Principals { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Subject> Subjects { get; set; }
    public DbSet<Teacher> Teachers { get; set; }
    public DbSet<TeacherSubject> TeacherSubjects { get; set; }
    public DbSet<Parent> Parents { get; set; }
    public DbSet<Class> Classes { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<ParentStudent> ParentStudents { get; set; }
    public DbSet<Grade> Grades { get; set; }
    public DbSet<Attendance> Attendances { get; set; }
    public DbSet<Curriculum> Curriculums { get; set; }
    public DbSet<SubjectCurriculum> SubjectCurriculums { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
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
