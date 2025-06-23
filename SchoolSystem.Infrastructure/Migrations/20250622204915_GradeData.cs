using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SchoolSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class GradeData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Grades",
                columns: new[] { "Id", "GradeDate", "GradeValue", "StudentId", "SubjectId" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc), 5, new Guid("10101010-1010-1010-1010-101010101010"), new Guid("a1b2c3d4-e5f6-4789-8abc-111111111111") },
                    { new Guid("22222222-2222-2222-2222-222222222222"), new DateTime(2025, 2, 20, 0, 0, 0, 0, DateTimeKind.Utc), 4, new Guid("10101010-1010-1010-1010-101010101010"), new Guid("a1b2c3d4-e5f6-4789-8abc-111111111111") },
                    { new Guid("33333333-3333-3333-3333-333333333333"), new DateTime(2025, 3, 25, 0, 0, 0, 0, DateTimeKind.Utc), 6, new Guid("10101010-1010-1010-1010-101010101010"), new Guid("a1b2c3d4-e5f6-4789-8abc-111111111111") }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Grades",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "Grades",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"));

            migrationBuilder.DeleteData(
                table: "Grades",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"));
        }
    }
}
