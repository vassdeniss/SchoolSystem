using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SchoolSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CurriculumData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SubjectCurriculums");

            migrationBuilder.AddColumn<Guid>(
                name: "SubjectId",
                table: "Curriculums",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.InsertData(
                table: "Curriculums",
                columns: new[] { "Id", "ClassId", "DayOfWeek", "EndTime", "StartTime", "SubjectId", "TeacherId" },
                values: new object[,]
                {
                    { new Guid("1a2b3c4d-1111-2222-3333-444455556666"), new Guid("11111111-1111-1111-1111-111111111111"), "Monday", new TimeSpan(0, 9, 30, 0, 0), new TimeSpan(0, 8, 0, 0, 0), new Guid("a1b2c3d4-e5f6-4789-8abc-111111111111"), new Guid("8f374d37-5a0c-4637-ba8e-2b4d2ceef15f") },
                    { new Guid("2b3c4d5e-2222-3333-4444-555566667777"), new Guid("11111111-1111-1111-1111-111111111111"), "Wednesday", new TimeSpan(0, 11, 30, 0, 0), new TimeSpan(0, 10, 0, 0, 0), new Guid("a1b2c3d4-e5f6-4789-8abc-111111111111"), new Guid("8f374d37-5a0c-4637-ba8e-2b4d2ceef15f") },
                    { new Guid("3c4d5e6f-3333-4444-5555-666677778888"), new Guid("11111111-1111-1111-1111-111111111111"), "Friday", new TimeSpan(0, 14, 30, 0, 0), new TimeSpan(0, 13, 0, 0, 0), new Guid("b2c3d4e5-f6a7-589a-8bcd-222222222222"), new Guid("2a3d47b0-28d1-48f9-bd9a-504a9f2a1cbd") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Curriculums_SubjectId",
                table: "Curriculums",
                column: "SubjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Curriculums_Subjects_SubjectId",
                table: "Curriculums",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Curriculums_Subjects_SubjectId",
                table: "Curriculums");

            migrationBuilder.DropIndex(
                name: "IX_Curriculums_SubjectId",
                table: "Curriculums");

            migrationBuilder.DeleteData(
                table: "Curriculums",
                keyColumn: "Id",
                keyValue: new Guid("1a2b3c4d-1111-2222-3333-444455556666"));

            migrationBuilder.DeleteData(
                table: "Curriculums",
                keyColumn: "Id",
                keyValue: new Guid("2b3c4d5e-2222-3333-4444-555566667777"));

            migrationBuilder.DeleteData(
                table: "Curriculums",
                keyColumn: "Id",
                keyValue: new Guid("3c4d5e6f-3333-4444-5555-666677778888"));

            migrationBuilder.DropColumn(
                name: "SubjectId",
                table: "Curriculums");

            migrationBuilder.CreateTable(
                name: "SubjectCurriculums",
                columns: table => new
                {
                    SubjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CurriculumId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubjectCurriculums", x => new { x.SubjectId, x.CurriculumId });
                    table.ForeignKey(
                        name: "FK_SubjectCurriculums_Curriculums_CurriculumId",
                        column: x => x.CurriculumId,
                        principalTable: "Curriculums",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubjectCurriculums_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SubjectCurriculums_CurriculumId",
                table: "SubjectCurriculums",
                column: "CurriculumId");
        }
    }
}
