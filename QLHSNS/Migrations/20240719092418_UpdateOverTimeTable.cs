using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QLHSNS.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOverTimeTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Month",
                table: "OverTimes");

            migrationBuilder.DropColumn(
                name: "Year",
                table: "OverTimes");

            migrationBuilder.AddColumn<DateOnly>(
                name: "OverTimeDate",
                table: "OverTimes",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OverTimeDate",
                table: "OverTimes");

            migrationBuilder.AddColumn<int>(
                name: "Month",
                table: "OverTimes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Year",
                table: "OverTimes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
