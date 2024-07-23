using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QLHSNS.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEmployeeTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeFamilies_Employees_EmployeeId",
                table: "EmployeeFamilies");

            migrationBuilder.AddColumn<int>(
                name: "IsWorking",
                table: "Employees",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeFamilies_Employees_EmployeeId",
                table: "EmployeeFamilies",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeFamilies_Employees_EmployeeId",
                table: "EmployeeFamilies");

            migrationBuilder.DropColumn(
                name: "IsWorking",
                table: "Employees");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeFamilies_Employees_EmployeeId",
                table: "EmployeeFamilies",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
