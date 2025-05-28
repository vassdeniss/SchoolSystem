using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SchoolSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class StudentData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Students",
                columns: new[] { "Id", "ClassId", "UserId" },
                values: new object[,]
                {
                    { new Guid("10101010-1010-1010-1010-101010101010"), new Guid("33333333-3333-3333-3333-333333333333"), new Guid("97c18abd-5743-4173-8f66-cd43363e55d5") },
                    { new Guid("20202020-2020-2020-2020-202020202020"), new Guid("33333333-3333-3333-3333-333333333333"), new Guid("bdcc8dcc-4d8e-4c97-a576-3aee878059c0") }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "Id",
                keyValue: new Guid("10101010-1010-1010-1010-101010101010"));

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "Id",
                keyValue: new Guid("20202020-2020-2020-2020-202020202020"));
        }
    }
}
