using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolSystem.Infrastructure.Models;

namespace SchoolSystem.Infrastructure.Configurations;

public class ClassConfiguration : IEntityTypeConfiguration<Class>
{
    public void Configure(EntityTypeBuilder<Class> builder)
    {
        builder.HasData(new List<Class>
        {
            new()
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Name = "A1",
                Year = 2025,
                Term = "Fall",
                SchoolId = Guid.Parse("b0cf0f90-50a5-4e86-9a29-fdf3928af26b"),
            },
            new()
            {
                Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                Name = "B2",
                Year = 2025,
                Term = "Spring",
                SchoolId = Guid.Parse("b0cf0f90-50a5-4e86-9a29-fdf3928af26b"),
            },
            new()
            {
                Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                Name = "C3",
                Year = 2024,
                Term = "Fall",
                SchoolId = Guid.Parse("b0cf0f90-50a5-4e86-9a29-fdf3928af26b"),
            }
        });
    }
}