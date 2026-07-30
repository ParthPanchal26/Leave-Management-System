using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Leave_Management_System.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateLeaveRequestManagerColumnName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LeaveRequests_Employees_ApprovedBy",
                table: "LeaveRequests");

            migrationBuilder.RenameColumn(
                name: "ApprovedBy",
                table: "LeaveRequests",
                newName: "ReviewedBy");

            migrationBuilder.RenameIndex(
                name: "IX_LeaveRequests_ApprovedBy",
                table: "LeaveRequests",
                newName: "IX_LeaveRequests_ReviewedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_LeaveRequests_Employees_ReviewedBy",
                table: "LeaveRequests",
                column: "ReviewedBy",
                principalTable: "Employees",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LeaveRequests_Employees_ReviewedBy",
                table: "LeaveRequests");

            migrationBuilder.RenameColumn(
                name: "ReviewedBy",
                table: "LeaveRequests",
                newName: "ApprovedBy");

            migrationBuilder.RenameIndex(
                name: "IX_LeaveRequests_ReviewedBy",
                table: "LeaveRequests",
                newName: "IX_LeaveRequests_ApprovedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_LeaveRequests_Employees_ApprovedBy",
                table: "LeaveRequests",
                column: "ApprovedBy",
                principalTable: "Employees",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
