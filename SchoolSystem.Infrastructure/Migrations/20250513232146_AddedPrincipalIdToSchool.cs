using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SchoolSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedPrincipalIdToSchool : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Principals_Schools_SchoolId",
                table: "Principals");

            migrationBuilder.DropIndex(
                name: "IX_Principals_SchoolId",
                table: "Principals");

            migrationBuilder.AddColumn<Guid>(
                name: "PrincipalId",
                table: "Schools",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Principals",
                columns: new[] { "Id", "PhoneNumber", "SchoolId", "Specialization", "UserId" },
                values: new object[] { new Guid("aae52599-edf2-4b1d-8f89-51a76b689d25"), "0888234567", new Guid("b0cf0f90-50a5-4e86-9a29-fdf3928af26b"), "School Management", new Guid("c7d81a30-6455-4a1f-8f47-923c1234abcd") });

            migrationBuilder.InsertData(
                table: "Schools",
                columns: new[] { "Id", "Address", "Name", "PrincipalId" },
                values: new object[,]
                {
                    { new Guid("b0cf0f90-50a5-4e86-9a29-fdf3928af26b"), "456 West St, Townsville", "Westside Academy", null },
                    { new Guid("d41f3aa8-3a0a-46c0-95d3-06e3b92de6a4"), "789 East St, Villagetown", "Eastside Institute", null },
                    { new Guid("e84a9f88-379d-4b86-9b02-db6e6434f2a0"), "123 Main St, Cityville", "Central High School", null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Schools_PrincipalId",
                table: "Schools",
                column: "PrincipalId",
                unique: true,
                filter: "[PrincipalId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Schools_Principals_PrincipalId",
                table: "Schools",
                column: "PrincipalId",
                principalTable: "Principals",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Schools_Principals_PrincipalId",
                table: "Schools");

            migrationBuilder.DropIndex(
                name: "IX_Schools_PrincipalId",
                table: "Schools");

            migrationBuilder.DeleteData(
                table: "Principals",
                keyColumn: "Id",
                keyValue: new Guid("aae52599-edf2-4b1d-8f89-51a76b689d25"));

            migrationBuilder.DeleteData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: new Guid("b0cf0f90-50a5-4e86-9a29-fdf3928af26b"));

            migrationBuilder.DeleteData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: new Guid("d41f3aa8-3a0a-46c0-95d3-06e3b92de6a4"));

            migrationBuilder.DeleteData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: new Guid("e84a9f88-379d-4b86-9b02-db6e6434f2a0"));

            migrationBuilder.DropColumn(
                name: "PrincipalId",
                table: "Schools");

            migrationBuilder.CreateIndex(
                name: "IX_Principals_SchoolId",
                table: "Principals",
                column: "SchoolId");

            migrationBuilder.AddForeignKey(
                name: "FK_Principals_Schools_SchoolId",
                table: "Principals",
                column: "SchoolId",
                principalTable: "Schools",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
