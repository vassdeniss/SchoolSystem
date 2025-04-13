using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SchoolSystem.Infrastructure.Models;

public partial class SchoolLogContext : DbContext
{
    public SchoolLogContext()
    {
    }

    public SchoolLogContext(DbContextOptions<SchoolLogContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Address> Addresses { get; set; }

    public virtual DbSet<Attendance> Attendances { get; set; }

    public virtual DbSet<Class> Classes { get; set; }

    public virtual DbSet<Curriculum> Curriculums { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Grade> Grades { get; set; }

    public virtual DbSet<Parent> Parents { get; set; }

    public virtual DbSet<Principal> Principals { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<School> Schools { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<Subject> Subjects { get; set; }

    public virtual DbSet<Teacher> Teachers { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-NUTNJDT;Database=SchoolLog;Integrated Security=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Addresse__3214EC0766F21489");

            entity.Property(e => e.City).HasMaxLength(100);
            entity.Property(e => e.PostalCode).HasMaxLength(10);
            entity.Property(e => e.Street).HasMaxLength(100);
            entity.Property(e => e.StreetNumber).HasMaxLength(10);
        });

        modelBuilder.Entity<Attendance>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Attendan__3214EC07BADA6825");

            entity.Property(e => e.AbsenceType).HasMaxLength(50);
            entity.Property(e => e.Status).HasMaxLength(50);

            entity.HasOne(d => d.Student).WithMany(p => p.Attendances)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Attendanc__Stude__5EBF139D");

            entity.HasOne(d => d.Subject).WithMany(p => p.Attendances)
                .HasForeignKey(d => d.SubjectId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Attendanc__Subje__5FB337D6");
        });

        modelBuilder.Entity<Class>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Classes__3214EC075942E587");

            entity.Property(e => e.Name).HasMaxLength(10);
            entity.Property(e => e.Term).HasMaxLength(20);
        });

        modelBuilder.Entity<Curriculum>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Curricul__3214EC07DEC3A958");

            entity.Property(e => e.DayOfWeek).HasMaxLength(20);

            entity.HasOne(d => d.Class).WithMany(p => p.Curricula)
                .HasForeignKey(d => d.ClassId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Curriculu__Class__6477ECF3");

            entity.HasOne(d => d.Teacher).WithMany(p => p.Curricula)
                .HasForeignKey(d => d.TeacherId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Curriculu__Teach__6383C8BA");
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Departme__3214EC0738508C1B");

            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Grade>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Grades__3214EC07AF4BC44E");

            entity.HasOne(d => d.Student).WithMany(p => p.Grades)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Grades__StudentI__59063A47");

            entity.HasOne(d => d.Subject).WithMany(p => p.Grades)
                .HasForeignKey(d => d.SubjectId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Grades__SubjectI__59FA5E80");
        });

        modelBuilder.Entity<Parent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Parents__3214EC0764D4B203");

            entity.HasIndex(e => e.UserId, "UQ__Parents__1788CC4D9EAF62D2").IsUnique();

            entity.HasIndex(e => e.PhoneNumber, "UQ__Parents__85FB4E385C967599").IsUnique();

            entity.Property(e => e.PhoneNumber).HasMaxLength(15);

            entity.HasOne(d => d.User).WithOne(p => p.Parent)
                .HasForeignKey<Parent>(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Parents__UserId__49C3F6B7");

            entity.HasMany(d => d.Students).WithMany(p => p.Parents)
                .UsingEntity<Dictionary<string, object>>(
                    "ParentStudent",
                    r => r.HasOne<Student>().WithMany()
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__ParentStu__Stude__5535A963"),
                    l => l.HasOne<Parent>().WithMany()
                        .HasForeignKey("ParentId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__ParentStu__Paren__5441852A"),
                    j =>
                    {
                        j.HasKey("ParentId", "StudentId").HasName("PK__ParentSt__501503D6C7891B13");
                        j.ToTable("ParentStudent");
                    });
        });

        modelBuilder.Entity<Principal>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Principa__3214EC07C4A94510");

            entity.HasIndex(e => e.UserId, "UQ__Principa__1788CC4D11D40F22").IsUnique();

            entity.HasIndex(e => e.SchoolId, "UQ__Principa__3DA4675AA095C167").IsUnique();

            entity.HasIndex(e => e.PhoneNumber, "UQ__Principa__85FB4E3823946930").IsUnique();

            entity.Property(e => e.PhoneNumber).HasMaxLength(15);
            entity.Property(e => e.Specialization).HasMaxLength(50);

            entity.HasOne(d => d.School).WithOne(p => p.Principal)
                .HasForeignKey<Principal>(d => d.SchoolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Principal__Schoo__35BCFE0A");

            entity.HasOne(d => d.User).WithOne(p => p.Principal)
                .HasForeignKey<Principal>(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Principal__UserI__36B12243");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Roles__3214EC07DA5E19FC");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.RoleName).HasMaxLength(50);
        });

        modelBuilder.Entity<School>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Schools__3214EC0746BC9259");

            entity.Property(e => e.Name).HasMaxLength(100);

            entity.HasOne(d => d.Address).WithMany(p => p.Schools)
                .HasForeignKey(d => d.AddressId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Schools__Address__300424B4");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Students__3214EC072F43C1B3");

            entity.HasIndex(e => e.UserId, "UQ__Students__1788CC4DAB22BCFF").IsUnique();

            entity.HasOne(d => d.Class).WithMany(p => p.Students)
                .HasForeignKey(d => d.ClassId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Students__ClassI__5070F446");

            entity.HasOne(d => d.School).WithMany(p => p.Students)
                .HasForeignKey(d => d.SchoolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Students__School__5165187F");

            entity.HasOne(d => d.User).WithOne(p => p.Student)
                .HasForeignKey<Student>(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Students__UserId__4F7CD00D");
        });

        modelBuilder.Entity<Subject>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Subjects__3214EC0776E4AE13");

            entity.Property(e => e.Name).HasMaxLength(100);

            entity.HasOne(d => d.Department).WithMany(p => p.Subjects)
                .HasForeignKey(d => d.DepartmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Subjects__Depart__3B75D760");

            entity.HasMany(d => d.Curricula).WithMany(p => p.Subjects)
                .UsingEntity<Dictionary<string, object>>(
                    "SubjectCurriculum",
                    r => r.HasOne<Curriculum>().WithMany()
                        .HasForeignKey("CurriculumId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__SubjectCu__Curri__68487DD7"),
                    l => l.HasOne<Subject>().WithMany()
                        .HasForeignKey("SubjectId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__SubjectCu__Subje__6754599E"),
                    j =>
                    {
                        j.HasKey("SubjectId", "CurriculumId").HasName("PK__SubjectC__7C773C090B0EBCDF");
                        j.ToTable("SubjectCurriculum");
                    });
        });

        modelBuilder.Entity<Teacher>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Teachers__3214EC07C636E6C9");

            entity.HasIndex(e => e.UserId, "UQ__Teachers__1788CC4DC02ACA03").IsUnique();

            entity.HasIndex(e => e.SchoolId, "UQ__Teachers__3DA4675A28484680").IsUnique();

            entity.Property(e => e.Specialization).HasMaxLength(50);

            entity.HasOne(d => d.School).WithOne(p => p.Teacher)
                .HasForeignKey<Teacher>(d => d.SchoolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Teachers__School__403A8C7D");

            entity.HasOne(d => d.User).WithOne(p => p.Teacher)
                .HasForeignKey<Teacher>(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Teachers__UserId__412EB0B6");

            entity.HasMany(d => d.Subjects).WithMany(p => p.Teachers)
                .UsingEntity<Dictionary<string, object>>(
                    "TeacherSubject",
                    r => r.HasOne<Subject>().WithMany()
                        .HasForeignKey("SubjectId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__TeacherSu__Subje__44FF419A"),
                    l => l.HasOne<Teacher>().WithMany()
                        .HasForeignKey("TeacherId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__TeacherSu__Teach__440B1D61"),
                    j =>
                    {
                        j.HasKey("TeacherId", "SubjectId").HasName("PK__TeacherS__7733E35E2C9C0753");
                        j.ToTable("TeacherSubjects");
                    });
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC0786F963B9");

            entity.HasIndex(e => e.Email, "UQ__Users__A9D105343B26F91A").IsUnique();

            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.MiddleName).HasMaxLength(50);
            entity.Property(e => e.Password).HasMaxLength(256);

            entity.HasMany(d => d.Roles).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "UserRole",
                    r => r.HasOne<Role>().WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__UserRoles__RoleI__2B3F6F97"),
                    l => l.HasOne<User>().WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__UserRoles__UserI__2A4B4B5E"),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId").HasName("PK__UserRole__AF2760ADC3AEE6E0");
                        j.ToTable("UserRoles");
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
