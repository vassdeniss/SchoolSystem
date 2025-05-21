using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolSystem.Infrastructure.Models;

namespace SchoolSystem.Infrastructure.Configurations;

public class StudentConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.HasData(new List<Student>
        {
            new()
            {
                Id = Guid.Parse("10101010-1010-1010-1010-101010101010"),
                UserId = Guid.Parse("66666666-6666-6666-6666-666666666666"),
                ClassId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa")
            },
            new()
            {
                Id = Guid.Parse("20202020-2020-2020-2020-202020202020"),
                UserId = Guid.Parse("77777777-7777-7777-7777-777777777777"),
                ClassId = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc")
            },
            new()
            {
                Id = Guid.Parse("30303030-3030-3030-3030-303030303030"),
                UserId = Guid.Parse("88888888-8888-8888-8888-888888888888"),
                ClassId = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd")
            }
        });
    }
}
