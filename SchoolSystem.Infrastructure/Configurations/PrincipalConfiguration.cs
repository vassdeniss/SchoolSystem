using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolSystem.Infrastructure.Models;

namespace SchoolSystem.Infrastructure.Configurations;

public class PrincipalConfiguration : IEntityTypeConfiguration<Principal>
{
    public void Configure(EntityTypeBuilder<Principal> builder)
    {
        builder.HasData(new List<Principal>
        {
            new()
            {
                Id = Guid.Parse("aae52599-edf2-4b1d-8f89-51a76b689d25"),
                Specialization = "School Management",
                PhoneNumber = "0888234567",
                // Principal User
                UserId = Guid.Parse("c7d81a30-6455-4a1f-8f47-923c1234abcd")
            }
        });
    }
}
