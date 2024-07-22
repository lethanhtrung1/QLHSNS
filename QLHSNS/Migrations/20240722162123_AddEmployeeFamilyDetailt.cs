using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QLHSNS.Migrations
{
    /// <inheritdoc />
    public partial class AddEmployeeFamilyDetailt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_EmployeeFamilies_EmployeeId",
                table: "EmployeeFamilies");

            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "EmployeeFamilies");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "EmployeeFamilies");

            migrationBuilder.DropColumn(
                name: "Occupation",
                table: "EmployeeFamilies");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "EmployeeFamilies");

            migrationBuilder.DropColumn(
                name: "Relationship",
                table: "EmployeeFamilies");

            migrationBuilder.AddColumn<int>(
                name: "Deduction",
                table: "EmployeeFamilies",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "EffectiveDate",
                table: "EmployeeFamilies",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "EmployeeFamilyDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeFamilyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Relationship = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Occupation = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeFamilyDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeFamilyDetails_EmployeeFamilies_EmployeeFamilyId",
                        column: x => x.EmployeeFamilyId,
                        principalTable: "EmployeeFamilies",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeFamilies_EmployeeId",
                table: "EmployeeFamilies",
                column: "EmployeeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeFamilyDetails_EmployeeFamilyId",
                table: "EmployeeFamilyDetails",
                column: "EmployeeFamilyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeFamilyDetails");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeFamilies_EmployeeId",
                table: "EmployeeFamilies");

            migrationBuilder.DropColumn(
                name: "Deduction",
                table: "EmployeeFamilies");

            migrationBuilder.DropColumn(
                name: "EffectiveDate",
                table: "EmployeeFamilies");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfBirth",
                table: "EmployeeFamilies",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "EmployeeFamilies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Occupation",
                table: "EmployeeFamilies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "EmployeeFamilies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Relationship",
                table: "EmployeeFamilies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeFamilies_EmployeeId",
                table: "EmployeeFamilies",
                column: "EmployeeId");
        }
    }
}
