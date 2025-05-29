using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SchoolSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SubjectInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subjects_Departments_DepartmentId",
                table: "Subjects");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.RenameColumn(
                name: "DepartmentId",
                table: "Subjects",
                newName: "SchoolId");

            migrationBuilder.RenameIndex(
                name: "IX_Subjects_DepartmentId",
                table: "Subjects",
                newName: "IX_Subjects_SchoolId");

            migrationBuilder.InsertData(
                table: "Subjects",
                columns: new[] { "Id", "Name", "SchoolId" },
                values: new object[,]
                {
                    { new Guid("a1b2c3d4-e5f6-4789-8abc-111111111111"), "Mathematics", new Guid("b0cf0f90-50a5-4e86-9a29-fdf3928af26b") },
                    { new Guid("b2c3d4e5-f6a7-589a-8bcd-222222222222"), "Physics", new Guid("b0cf0f90-50a5-4e86-9a29-fdf3928af26b") },
                    { new Guid("c3d4e5f6-a7b8-69ac-9bde-333333333333"), "Chemistry", new Guid("b0cf0f90-50a5-4e86-9a29-fdf3928af26b") },
                    { new Guid("d4e5f6a7-b8c9-7abd-aced-444444444444"), "History", new Guid("b0cf0f90-50a5-4e86-9a29-fdf3928af26b") },
                    { new Guid("e5f6a7b8-c9d0-8bde-afed-555555555555"), "Biology", new Guid("b0cf0f90-50a5-4e86-9a29-fdf3928af26b") }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Subjects_Schools_SchoolId",
                table: "Subjects",
                column: "SchoolId",
                principalTable: "Schools",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subjects_Schools_SchoolId",
                table: "Subjects");

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-e5f6-4789-8abc-111111111111"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("b2c3d4e5-f6a7-589a-8bcd-222222222222"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("c3d4e5f6-a7b8-69ac-9bde-333333333333"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("d4e5f6a7-b8c9-7abd-aced-444444444444"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("e5f6a7b8-c9d0-8bde-afed-555555555555"));

            migrationBuilder.RenameColumn(
                name: "SchoolId",
                table: "Subjects",
                newName: "DepartmentId");

            migrationBuilder.RenameIndex(
                name: "IX_Subjects_SchoolId",
                table: "Subjects",
                newName: "IX_Subjects_DepartmentId");

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Subjects_Departments_DepartmentId",
                table: "Subjects",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
