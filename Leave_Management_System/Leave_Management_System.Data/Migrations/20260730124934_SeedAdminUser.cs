using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Leave_Management_System.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedAdminUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "EmployeeId", "CreatedAt", "DateOfBirth", "DepartmentId", "Email", "FirstName", "HireDate", "IsActive", "LastName", "ManagerId", "PasswordHash", "PhoneNumber", "RoleId", "Salary", "UpdatedAt" },
                values: new object[] { new Guid("e0e5fff4-ecaa-4d8b-b369-0d879988344d"), new DateTime(2026, 7, 30, 0, 0, 0, 0, DateTimeKind.Utc), new DateOnly(2005, 2, 26), null, "admin@lms.com", "Parth", new DateOnly(2026, 7, 30), true, "Panchal", null, "AQAAAAIAAYagAAAAENb7C8bXILY0mqaTLTYbbbZhB3CFNMROTUHHzU4TM8ce3axFXU7IfsbCZEBqlpbAXQ==", "0123456789", 1, 1200000m, new DateTime(2026, 7, 30, 0, 0, 0, 0, DateTimeKind.Utc) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: new Guid("e0e5fff4-ecaa-4d8b-b369-0d879988344d"));
        }
    }
}
