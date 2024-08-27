using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PublicSpaceMaintenanceRequestMS.Migrations
{
    /// <inheritdoc />
    public partial class FK_Requests_Departments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Departments_AssignedDepartmentId",
                table: "Requests");

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Departments",
                table: "Requests",
                column: "AssignedDepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Departments",
                table: "Requests");

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Departments_AssignedDepartmentId",
                table: "Requests",
                column: "AssignedDepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
