using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QLHSNS.Migrations
{
    /// <inheritdoc />
    public partial class EditEmployeeTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "DateOfJoining",
                table: "Employees",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<DateOnly>(
                name: "DateOfLeaving",
                table: "Employees",
                type: "date",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateOfJoining",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "DateOfLeaving",
                table: "Employees");
        }
    }
}
