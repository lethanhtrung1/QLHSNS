using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QLHSNS.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEmployeeFamily : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeFamilies_Employees_EmployeeId",
                table: "EmployeeFamilies");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeFamilies_Employees_EmployeeId",
                table: "EmployeeFamilies",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeFamilies_Employees_EmployeeId",
                table: "EmployeeFamilies");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeFamilies_Employees_EmployeeId",
                table: "EmployeeFamilies",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id");
        }
    }
}
