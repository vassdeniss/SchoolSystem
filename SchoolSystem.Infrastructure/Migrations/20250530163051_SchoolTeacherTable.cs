using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SchoolSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SchoolTeacherTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Teachers_Schools_SchoolId",
                table: "Teachers");

            migrationBuilder.DropIndex(
                name: "IX_Teachers_SchoolId",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "Teachers");

            migrationBuilder.CreateTable(
                name: "SchoolTeacher",
                columns: table => new
                {
                    SchoolsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TeachersId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchoolTeacher", x => new { x.SchoolsId, x.TeachersId });
                    table.ForeignKey(
                        name: "FK_SchoolTeacher_Schools_SchoolsId",
                        column: x => x.SchoolsId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SchoolTeacher_Teachers_TeachersId",
                        column: x => x.TeachersId,
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Teachers",
                columns: new[] { "Id", "Specialization", "UserId" },
                values: new object[,]
                {
                    { new Guid("2a3d47b0-28d1-48f9-bd9a-504a9f2a1cbd"), "Theoretical Physics", new Guid("7f74d5cd-5061-4e53-a10b-221cfb9488a0") },
                    { new Guid("8f374d37-5a0c-4637-ba8e-2b4d2ceef15f"), "Applied Mathematics", new Guid("1396ac5f-745d-47b4-8cef-21a0e7e32bd9") }
                });

            migrationBuilder.InsertData(
                table: "SchoolTeacher",
                columns: new[] { "SchoolsId", "TeachersId" },
                values: new object[,]
                {
                    { new Guid("b0cf0f90-50a5-4e86-9a29-fdf3928af26b"), new Guid("2a3d47b0-28d1-48f9-bd9a-504a9f2a1cbd") },
                    { new Guid("b0cf0f90-50a5-4e86-9a29-fdf3928af26b"), new Guid("8f374d37-5a0c-4637-ba8e-2b4d2ceef15f") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_SchoolTeacher_TeachersId",
                table: "SchoolTeacher",
                column: "TeachersId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SchoolTeacher");

            migrationBuilder.DeleteData(
                table: "Teachers",
                keyColumn: "Id",
                keyValue: new Guid("2a3d47b0-28d1-48f9-bd9a-504a9f2a1cbd"));

            migrationBuilder.DeleteData(
                table: "Teachers",
                keyColumn: "Id",
                keyValue: new Guid("8f374d37-5a0c-4637-ba8e-2b4d2ceef15f"));

            migrationBuilder.AddColumn<Guid>(
                name: "SchoolId",
                table: "Teachers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_SchoolId",
                table: "Teachers",
                column: "SchoolId");

            migrationBuilder.AddForeignKey(
                name: "FK_Teachers_Schools_SchoolId",
                table: "Teachers",
                column: "SchoolId",
                principalTable: "Schools",
                principalColumn: "Id");
        }
    }
}
