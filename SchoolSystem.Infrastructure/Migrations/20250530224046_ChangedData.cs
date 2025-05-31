using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TeacherId",
                table: "Subjects",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-e5f6-4789-8abc-111111111111"),
                column: "TeacherId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("b2c3d4e5-f6a7-589a-8bcd-222222222222"),
                column: "TeacherId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("c3d4e5f6-a7b8-69ac-9bde-333333333333"),
                column: "TeacherId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("d4e5f6a7-b8c9-7abd-aced-444444444444"),
                column: "TeacherId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("e5f6a7b8-c9d0-8bde-afed-555555555555"),
                column: "TeacherId",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_TeacherId",
                table: "Subjects",
                column: "TeacherId");

            migrationBuilder.AddForeignKey(
                name: "FK_Subjects_Teachers_TeacherId",
                table: "Subjects",
                column: "TeacherId",
                principalTable: "Teachers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subjects_Teachers_TeacherId",
                table: "Subjects");

            migrationBuilder.DropIndex(
                name: "IX_Subjects_TeacherId",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "TeacherId",
                table: "Subjects");
        }
    }
}
