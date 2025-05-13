using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolSystem.Infrastructure.Models;
using System;
using System.Collections.Generic;

namespace SchoolSystem.Infrastructure.Configurations
{
    public class CurriculumConfiguration : IEntityTypeConfiguration<Curriculum>
    {
        public void Configure(EntityTypeBuilder<Curriculum> builder)
        {
            builder.HasData(new List<Curriculum>
            {
                new Curriculum
                {
                    Id = Guid.Parse("1a2b3c4d-1111-2222-3333-444455556666"),
                    DayOfWeek = "Monday",
                    StartTime = new TimeSpan(8, 0, 0),
                    EndTime = new TimeSpan(9, 30, 0),
                    TeacherId = Guid.Parse("7f8e9d0c-aaaa-bbbb-cccc-ddddeeeeffff"),
                    ClassId = Guid.Parse("12345678-aaaa-bbbb-cccc-1234567890ab")
                },
                new Curriculum
                {
                    Id = Guid.Parse("2b3c4d5e-2222-3333-4444-555566667777"),
                    DayOfWeek = "Wednesday",
                    StartTime = new TimeSpan(10, 0, 0),
                    EndTime = new TimeSpan(11, 30, 0),
                    TeacherId = Guid.Parse("8f9e0c1d-bbbb-cccc-dddd-eeeeffff0000"),
                    ClassId = Guid.Parse("23456789-bbbb-cccc-dddd-234567890abc")
                },
                new Curriculum
                {
                    Id = Guid.Parse("3c4d5e6f-3333-4444-5555-666677778888"),
                    DayOfWeek = "Friday",
                    StartTime = new TimeSpan(13, 0, 0),
                    EndTime = new TimeSpan(14, 30, 0),
                    TeacherId = Guid.Parse("9fa0b1c2-cccc-dddd-eeee-ffff00001111"),
                    ClassId = Guid.Parse("34567890-cccc-dddd-eeee-34567890abcd")
                }
            });
        }
    }
}
