using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Leave_Mangement_System.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    DeptId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.DeptId);
                });

            migrationBuilder.CreateTable(
                name: "LeavePolicies",
                columns: table => new
                {
                    PolicyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PolicyName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RegularDays = table.Column<int>(type: "int", nullable: false),
                    ExceptionDays = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeavePolicies", x => x.PolicyId);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    EmpId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameEmployee = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Manager = table.Column<bool>(type: "bit", nullable: false),
                    Hr = table.Column<bool>(type: "bit", nullable: false),
                    LeaveBalance = table.Column<int>(type: "int", nullable: false),
                    ManagerName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    EmailManager = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DeptId = table.Column<int>(type: "int", nullable: false),
                    PolicyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.EmpId);
                    table.ForeignKey(
                        name: "FK_Employees_Departments_DeptId",
                        column: x => x.DeptId,
                        principalTable: "Departments",
                        principalColumn: "DeptId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Employees_LeavePolicies_PolicyId",
                        column: x => x.PolicyId,
                        principalTable: "LeavePolicies",
                        principalColumn: "PolicyId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LeaveRequests",
                columns: table => new
                {
                    RequestId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmpId = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NumberOfDays = table.Column<int>(type: "int", nullable: false),
                    HrApproved = table.Column<bool>(type: "bit", nullable: false),
                    ManagerApproved = table.Column<bool>(type: "bit", nullable: false),
                    Pending = table.Column<bool>(type: "bit", nullable: false),
                    Rejected = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaveRequests", x => x.RequestId);
                    table.ForeignKey(
                        name: "FK_LeaveRequests_Employees_EmpId",
                        column: x => x.EmpId,
                        principalTable: "Employees",
                        principalColumn: "EmpId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "DeptId", "Name" },
                values: new object[,]
                {
                    { 1, "Human Resources" },
                    { 2, "Information Technology" },
                    { 3, "Finance" },
                    { 4, "Marketing" },
                    { 5, "Operations" },
                    { 6, "Sales" }
                });

            migrationBuilder.InsertData(
                table: "LeavePolicies",
                columns: new[] { "PolicyId", "ExceptionDays", "PolicyName", "RegularDays" },
                values: new object[,]
                {
                    { 1, 5, "Standard Policy", 21 },
                    { 2, 10, "Senior Policy", 30 },
                    { 3, 15, "Management Policy", 35 },
                    { 4, 20, "Executive Policy", 40 }
                });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "EmpId", "DeptId", "Email", "EmailManager", "Hr", "LeaveBalance", "Manager", "ManagerName", "NameEmployee", "PolicyId" },
                values: new object[,]
                {
                    { 1, 1, "ahmed.hassan@company.com", null, true, 35, true, null, "Ahmed Hassan", 3 },
                    { 2, 1, "fatma.mohamed@company.com", "ahmed.hassan@company.com", true, 28, false, "Ahmed Hassan", "Fatma Mohamed", 2 },
                    { 3, 2, "omar.ali@company.com", null, false, 30, true, null, "Omar Ali", 3 },
                    { 4, 2, "mona.ibrahim@company.com", "omar.ali@company.com", false, 21, false, "Omar Ali", "Mona Ibrahim", 1 },
                    { 5, 2, "khaled.mahmoud@company.com", "omar.ali@company.com", false, 25, false, "Omar Ali", "Khaled Mahmoud", 2 },
                    { 6, 3, "sara.ahmed@company.com", null, false, 32, true, null, "Sara Ahmed", 3 },
                    { 7, 3, "mohamed.youssef@company.com", "sara.ahmed@company.com", false, 18, false, "Sara Ahmed", "Mohamed Youssef", 1 },
                    { 8, 4, "nour.kamal@company.com", null, false, 28, true, null, "Nour Kamal", 3 },
                    { 9, 4, "amr.salah@company.com", "nour.kamal@company.com", false, 22, false, "Nour Kamal", "Amr Salah", 1 },
                    { 10, 5, "heba.mostafa@company.com", null, false, 35, true, null, "Heba Mostafa", 3 },
                    { 11, 5, "tamer.rashad@company.com", "heba.mostafa@company.com", false, 19, false, "Heba Mostafa", "Tamer Rashad", 1 },
                    { 12, 6, "yasmin.fouad@company.com", null, false, 30, true, null, "Yasmin Fouad", 2 },
                    { 13, 6, "hassan.gamal@company.com", "yasmin.fouad@company.com", false, 15, false, "Yasmin Fouad", "Hassan Gamal", 1 },
                    { 14, 6, "rana.tarek@company.com", "yasmin.fouad@company.com", false, 26, false, "Yasmin Fouad", "Rana Tarek", 2 }
                });

            migrationBuilder.InsertData(
                table: "LeaveRequests",
                columns: new[] { "RequestId", "EmpId", "EndDate", "HrApproved", "ManagerApproved", "NumberOfDays", "Pending", "Rejected", "StartDate" },
                values: new object[,]
                {
                    { 1, 4, new DateTime(2024, 12, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), true, true, 5, false, false, new DateTime(2024, 12, 15, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, 7, new DateTime(2025, 1, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), false, true, 3, true, false, new DateTime(2025, 1, 5, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, 9, new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), false, false, 10, false, true, new DateTime(2025, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, 11, new DateTime(2025, 3, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), false, false, 6, true, false, new DateTime(2025, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, 13, new DateTime(2025, 1, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), true, true, 3, false, false, new DateTime(2025, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 6, 5, new DateTime(2025, 4, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), false, true, 5, true, false, new DateTime(2025, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_DeptId",
                table: "Employees",
                column: "DeptId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_PolicyId",
                table: "Employees",
                column: "PolicyId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveRequests_EmpId",
                table: "LeaveRequests",
                column: "EmpId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LeaveRequests");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropTable(
                name: "LeavePolicies");
        }
    }
}
