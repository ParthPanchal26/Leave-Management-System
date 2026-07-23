using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Leave_Management_System.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    DepartmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DepartmentName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.DepartmentId);
                });

            migrationBuilder.CreateTable(
                name: "Holidays",
                columns: table => new
                {
                    HolidayId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HolidayName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HolidayDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsOptional = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Holidays", x => x.HolidayId);
                });

            migrationBuilder.CreateTable(
                name: "LeaveTypes",
                columns: table => new
                {
                    LeaveTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LeaveTypeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaxDaysPerYear = table.Column<int>(type: "int", nullable: false),
                    IsPaid = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaveTypes", x => x.LeaveTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOfBirth = table.Column<DateOnly>(type: "date", nullable: false),
                    HireDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Salary = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ManagerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.EmployeeId);
                    table.ForeignKey(
                        name: "FK_Employees_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "DepartmentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Employees_Employees_ManagerId",
                        column: x => x.ManagerId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId");
                    table.ForeignKey(
                        name: "FK_Employees_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LeaveBalances",
                columns: table => new
                {
                    LeaveBalanceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LeaveTyprId = table.Column<int>(type: "int", nullable: false),
                    LeaveTypeId = table.Column<int>(type: "int", nullable: false),
                    LeaveYear = table.Column<int>(type: "int", nullable: false),
                    AllocatedDays = table.Column<int>(type: "int", nullable: false),
                    UsedDays = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaveBalances", x => x.LeaveBalanceId);
                    table.ForeignKey(
                        name: "FK_LeaveBalances_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LeaveBalances_LeaveTypes_LeaveTypeId",
                        column: x => x.LeaveTypeId,
                        principalTable: "LeaveTypes",
                        principalColumn: "LeaveTypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LeaveRequests",
                columns: table => new
                {
                    LeaveId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LeaveTypeId = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LeaveReason = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LeaveStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApprovedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ApproveDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RejectReason = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaveRequests", x => x.LeaveId);
                    table.ForeignKey(
                        name: "FK_LeaveRequests_Employees_ApprovedBy",
                        column: x => x.ApprovedBy,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LeaveRequests_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LeaveRequests_LeaveTypes_LeaveTypeId",
                        column: x => x.LeaveTypeId,
                        principalTable: "LeaveTypes",
                        principalColumn: "LeaveTypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "DepartmentId", "DepartmentName", "Description" },
                values: new object[,]
                {
                    { 1, ".NET", "Development team working on .NET applications and services." },
                    { 2, "Python", "Development team responsible for Python-based applications and APIs." },
                    { 3, "React", "Frontend development team using React.js." },
                    { 4, "Angular", "Frontend development team using Angular framework." },
                    { 5, "React Native", "Mobile application development team using React Native." },
                    { 6, "QA", "Quality Assurance team responsible for testing and quality control." },
                    { 7, "DevOps", "Infrastructure, CI/CD, cloud deployment, and automation team." },
                    { 8, "PHP", "Development team responsible for PHP-based web applications." }
                });

            migrationBuilder.InsertData(
                table: "Holidays",
                columns: new[] { "HolidayId", "Description", "HolidayDate", "HolidayName", "IsOptional" },
                values: new object[,]
                {
                    { 1, "Harvest festival celebrated across India.", new DateTime(2026, 1, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "Makar Sankranti", false },
                    { 2, "Festival dedicated to Lord Shiva.", new DateTime(2026, 2, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Maha Shivratri", false },
                    { 3, "Bonfire marking the victory of good over evil.", new DateTime(2026, 3, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "Holika Dahan", true },
                    { 4, "Festival of Colors.", new DateTime(2026, 3, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "Holi (Dhulandi)", false },
                    { 5, "Birth anniversary of Lord Rama.", new DateTime(2026, 3, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ram Navami", false },
                    { 6, "Birth anniversary of Lord Hanuman.", new DateTime(2026, 4, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Hanuman Jayanti", true },
                    { 7, "Auspicious day for prosperity and new beginnings.", new DateTime(2026, 4, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Akshaya Tritiya", true },
                    { 8, "Festival honoring spiritual and academic teachers.", new DateTime(2026, 7, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "Guru Purnima", true },
                    { 9, "Celebration of the bond between brothers and sisters.", new DateTime(2026, 8, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "Raksha Bandhan", true },
                    { 10, "Birth anniversary of Lord Krishna.", new DateTime(2026, 9, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "Janmashtami", false },
                    { 11, "Birth of Lord Ganesha.", new DateTime(2026, 9, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ganesh Chaturthi", false },
                    { 12, "Beginning of the nine-day festival dedicated to Goddess Durga.", new DateTime(2026, 10, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sharad Navratri Begins", true },
                    { 13, "Important day during Navratri.", new DateTime(2026, 10, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Durga Ashtami", true },
                    { 14, "Victory of Lord Rama over Ravana.", new DateTime(2026, 10, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Dussehra (Vijayadashami)", false },
                    { 15, "Festival observed by married Hindu women.", new DateTime(2026, 10, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "Karva Chauth", true },
                    { 16, "Beginning of the Diwali festivities.", new DateTime(2026, 11, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "Dhanteras", true },
                    { 17, "Also known as Choti Diwali.", new DateTime(2026, 11, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "Naraka Chaturdashi", true },
                    { 18, "Festival of Lights.", new DateTime(2026, 11, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Diwali (Lakshmi Puja)", false },
                    { 19, "Celebration of Lord Krishna lifting Govardhan Hill.", new DateTime(2026, 11, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "Govardhan Puja", true },
                    { 20, "Celebration of the bond between brothers and sisters.", new DateTime(2026, 11, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bhai Dooj", true }
                });

            migrationBuilder.InsertData(
                table: "LeaveTypes",
                columns: new[] { "LeaveTypeId", "IsPaid", "LeaveTypeName", "MaxDaysPerYear" },
                values: new object[,]
                {
                    { 1, true, "Sick Leave", 6 },
                    { 2, true, "Employee Leave (EL)", 12 },
                    { 3, true, "Flexible Festival Leave (FFL)", 1 },
                    { 4, true, "Management Leave (ML)", 3 }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "RoleId", "RoleName" },
                values: new object[,]
                {
                    { 1, "Admin" },
                    { 2, "HR" },
                    { 3, "Manager" },
                    { 4, "Employee" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_DepartmentId",
                table: "Employees",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_ManagerId",
                table: "Employees",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_RoleId",
                table: "Employees",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveBalances_EmployeeId",
                table: "LeaveBalances",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveBalances_LeaveTypeId",
                table: "LeaveBalances",
                column: "LeaveTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveRequests_ApprovedBy",
                table: "LeaveRequests",
                column: "ApprovedBy");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveRequests_EmployeeId",
                table: "LeaveRequests",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveRequests_LeaveTypeId",
                table: "LeaveRequests",
                column: "LeaveTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Holidays");

            migrationBuilder.DropTable(
                name: "LeaveBalances");

            migrationBuilder.DropTable(
                name: "LeaveRequests");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "LeaveTypes");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
