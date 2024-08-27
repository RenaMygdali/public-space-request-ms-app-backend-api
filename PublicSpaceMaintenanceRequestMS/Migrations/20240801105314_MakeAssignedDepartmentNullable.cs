using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PublicSpaceMaintenanceRequestMS.Migrations
{
    /// <inheritdoc />
    public partial class MakeAssignedDepartmentNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Αλλαγή του πεδίου AssignedDepartmentId για να δέχεται NULL τιμές
            migrationBuilder.AlterColumn<int>(
                name: "AssignedDepartmentId",
                table: "Requests",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Επαναφορά του πεδίου AssignedDepartmentId στην προηγούμενη κατάσταση
            migrationBuilder.AlterColumn<int>(
                name: "AssignedDepartmentId",
                table: "Requests",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
