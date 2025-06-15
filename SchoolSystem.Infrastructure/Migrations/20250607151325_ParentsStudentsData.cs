using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SchoolSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ParentsStudentsData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ParentStudent",
                columns: table => new
                {
                    ParentsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParentStudent", x => new { x.ParentsId, x.StudentsId });
                    table.ForeignKey(
                        name: "FK_ParentStudent_Parents_ParentsId",
                        column: x => x.ParentsId,
                        principalTable: "Parents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ParentStudent_Students_StudentsId",
                        column: x => x.StudentsId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Parents",
                columns: new[] { "Id", "PhoneNumber", "UserId" },
                values: new object[,]
                {
                    { new Guid("8b5f7c12-0a97-4e45-9b34-123456789abc"), "0888123456", new Guid("3905cc60-cff6-4b59-b365-03d1749d9c7b") },
                    { new Guid("9c6f8d23-1b07-4f56-ab45-23456789abcd"), "0888234567", new Guid("35ea475e-72e3-4786-8c66-c2586503171b") }
                });

            migrationBuilder.InsertData(
                table: "ParentStudent",
                columns: new[] { "ParentsId", "StudentsId" },
                values: new object[,]
                {
                    { new Guid("8b5f7c12-0a97-4e45-9b34-123456789abc"), new Guid("10101010-1010-1010-1010-101010101010") },
                    { new Guid("8b5f7c12-0a97-4e45-9b34-123456789abc"), new Guid("20202020-2020-2020-2020-202020202020") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ParentStudent_StudentsId",
                table: "ParentStudent",
                column: "StudentsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ParentStudent");

            migrationBuilder.DeleteData(
                table: "Parents",
                keyColumn: "Id",
                keyValue: new Guid("8b5f7c12-0a97-4e45-9b34-123456789abc"));

            migrationBuilder.DeleteData(
                table: "Parents",
                keyColumn: "Id",
                keyValue: new Guid("9c6f8d23-1b07-4f56-ab45-23456789abcd"));
        }
    }
}
