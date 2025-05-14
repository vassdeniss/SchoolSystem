using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolSystem.Infrastructure.Models;
using System;
using System.Collections.Generic;

namespace SchoolSystem.Infrastructure.Configurations;

public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        builder.HasData(new List<Department>
        {
            new()
            {
                Id = Guid.Parse("11111111-aaaa-1111-aaaa-111111111111"),
                Name = "Science"
            },
            new()
            {
                Id = Guid.Parse("22222222-bbbb-2222-bbbb-222222222222"),
                Name = "Mathematics"
            },
            new()
            {
                Id = Guid.Parse("33333333-cccc-3333-cccc-333333333333"),
                Name = "Humanities"
            }
        });
    }
}