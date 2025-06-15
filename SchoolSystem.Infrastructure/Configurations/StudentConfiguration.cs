using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolSystem.Infrastructure.Models;

namespace SchoolSystem.Infrastructure.Configurations;

public class StudentConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.HasOne(s => s.Class)
            .WithMany()
            .OnDelete(DeleteBehavior.SetNull);
        
        builder.HasData(new List<Student>
        {
            new()
            {
                Id = Guid.Parse("10101010-1010-1010-1010-101010101010"),
                UserId = Guid.Parse("97c18abd-5743-4173-8f66-cd43363e55d5"),
                ClassId = Guid.Parse("33333333-3333-3333-3333-333333333333")
            },
            new()
            {
                Id = Guid.Parse("20202020-2020-2020-2020-202020202020"),
                UserId = Guid.Parse("bdcc8dcc-4d8e-4c97-a576-3aee878059c0"),
                ClassId = Guid.Parse("33333333-3333-3333-3333-333333333333")
            }
        });
    }
}
