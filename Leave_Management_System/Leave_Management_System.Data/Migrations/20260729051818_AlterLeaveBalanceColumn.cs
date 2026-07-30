using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Leave_Management_System.Data.Migrations
{
    /// <inheritdoc />
    public partial class AlterLeaveBalanceColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LeaveTyprId",
                table: "LeaveBalances");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LeaveTyprId",
                table: "LeaveBalances",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
