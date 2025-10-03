using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Leave_Mangement_System.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMyModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "EmpId",
                keyValue: 1,
                columns: new[] { "Address", "JobTitle", "PhoneNumber" },
                values: new object[] { "Cairo, Egypt", "HR Manager", "01234567890" });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "EmpId",
                keyValue: 2,
                columns: new[] { "Address", "JobTitle", "PhoneNumber" },
                values: new object[] { "Giza, Egypt", "HR Specialist", "01234567891" });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "EmpId",
                keyValue: 3,
                columns: new[] { "Address", "JobTitle", "PhoneNumber" },
                values: new object[] { "Alexandria, Egypt", "IT Manager", "01234567892" });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "EmpId",
                keyValue: 4,
                columns: new[] { "Address", "JobTitle", "PhoneNumber" },
                values: new object[] { "Cairo, Egypt", "Software Developer", "01234567893" });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "EmpId",
                keyValue: 5,
                columns: new[] { "Address", "JobTitle", "PhoneNumber" },
                values: new object[] { "Mansoura, Egypt", "Senior Developer", "01234567894" });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "EmpId",
                keyValue: 6,
                columns: new[] { "Address", "JobTitle", "PhoneNumber" },
                values: new object[] { "Cairo, Egypt", "Finance Manager", "01234567895" });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "EmpId",
                keyValue: 7,
                columns: new[] { "Address", "JobTitle", "PhoneNumber" },
                values: new object[] { "Giza, Egypt", "Accountant", "01234567896" });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "EmpId",
                keyValue: 8,
                columns: new[] { "Address", "JobTitle", "PhoneNumber" },
                values: new object[] { "Alexandria, Egypt", "Marketing Manager", "01234567897" });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "EmpId",
                keyValue: 9,
                columns: new[] { "Address", "JobTitle", "PhoneNumber" },
                values: new object[] { "Cairo, Egypt", "Marketing Specialist", "01234567898" });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "EmpId",
                keyValue: 10,
                columns: new[] { "Address", "JobTitle", "PhoneNumber" },
                values: new object[] { "Tanta, Egypt", "Operations Manager", "01234567899" });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "EmpId",
                keyValue: 11,
                columns: new[] { "Address", "JobTitle", "PhoneNumber" },
                values: new object[] { "Cairo, Egypt", "Operations Specialist", "01234567800" });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "EmpId",
                keyValue: 12,
                columns: new[] { "Address", "JobTitle", "PhoneNumber" },
                values: new object[] { "Alexandria, Egypt", "Sales Manager", "01234567801" });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "EmpId",
                keyValue: 13,
                columns: new[] { "Address", "JobTitle", "PhoneNumber" },
                values: new object[] { "Cairo, Egypt", "Sales Representative", "01234567802" });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "EmpId",
                keyValue: 14,
                columns: new[] { "Address", "JobTitle", "PhoneNumber" },
                values: new object[] { "Giza, Egypt", "Senior Sales Representative", "01234567803" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "EmpId", "Address", "DeptId", "Email", "EmployeeName", "Hr", "InitialPassword", "JobTitle", "LeaveBalance", "Manager", "ManagerEmail", "ManagerName", "PhoneNumber", "PolicyId", "UserId" },
                values: new object[] { 15, "edgetgase", 5, "mohamed@gmail.com", "Mohamed", false, null, "Software Developer", 3, false, "heba.mostafa@company.com", "Heba Mostafa", "01028742819", 1, null });

            migrationBuilder.UpdateData(
                table: "LeaveRequests",
                keyColumn: "RequestId",
                keyValue: 1,
                column: "LeaveType",
                value: "Annual");

            migrationBuilder.UpdateData(
                table: "LeaveRequests",
                keyColumn: "RequestId",
                keyValue: 2,
                column: "LeaveType",
                value: "Sick");

            migrationBuilder.UpdateData(
                table: "LeaveRequests",
                keyColumn: "RequestId",
                keyValue: 3,
                column: "LeaveType",
                value: "Annual");

            migrationBuilder.UpdateData(
                table: "LeaveRequests",
                keyColumn: "RequestId",
                keyValue: 4,
                column: "LeaveType",
                value: "Casual");

            migrationBuilder.UpdateData(
                table: "LeaveRequests",
                keyColumn: "RequestId",
                keyValue: 5,
                column: "LeaveType",
                value: "Annual");

            migrationBuilder.UpdateData(
                table: "LeaveRequests",
                keyColumn: "RequestId",
                keyValue: 6,
                column: "LeaveType",
                value: "Emergency");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmpId",
                keyValue: 15);

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "EmpId",
                keyValue: 1,
                columns: new[] { "Address", "JobTitle", "PhoneNumber" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "EmpId",
                keyValue: 2,
                columns: new[] { "Address", "JobTitle", "PhoneNumber" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "EmpId",
                keyValue: 3,
                columns: new[] { "Address", "JobTitle", "PhoneNumber" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "EmpId",
                keyValue: 4,
                columns: new[] { "Address", "JobTitle", "PhoneNumber" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "EmpId",
                keyValue: 5,
                columns: new[] { "Address", "JobTitle", "PhoneNumber" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "EmpId",
                keyValue: 6,
                columns: new[] { "Address", "JobTitle", "PhoneNumber" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "EmpId",
                keyValue: 7,
                columns: new[] { "Address", "JobTitle", "PhoneNumber" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "EmpId",
                keyValue: 8,
                columns: new[] { "Address", "JobTitle", "PhoneNumber" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "EmpId",
                keyValue: 9,
                columns: new[] { "Address", "JobTitle", "PhoneNumber" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "EmpId",
                keyValue: 10,
                columns: new[] { "Address", "JobTitle", "PhoneNumber" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "EmpId",
                keyValue: 11,
                columns: new[] { "Address", "JobTitle", "PhoneNumber" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "EmpId",
                keyValue: 12,
                columns: new[] { "Address", "JobTitle", "PhoneNumber" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "EmpId",
                keyValue: 13,
                columns: new[] { "Address", "JobTitle", "PhoneNumber" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "EmpId",
                keyValue: 14,
                columns: new[] { "Address", "JobTitle", "PhoneNumber" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "LeaveRequests",
                keyColumn: "RequestId",
                keyValue: 1,
                column: "LeaveType",
                value: "Normal");

            migrationBuilder.UpdateData(
                table: "LeaveRequests",
                keyColumn: "RequestId",
                keyValue: 2,
                column: "LeaveType",
                value: "Normal");

            migrationBuilder.UpdateData(
                table: "LeaveRequests",
                keyColumn: "RequestId",
                keyValue: 3,
                column: "LeaveType",
                value: "Normal");

            migrationBuilder.UpdateData(
                table: "LeaveRequests",
                keyColumn: "RequestId",
                keyValue: 4,
                column: "LeaveType",
                value: "Normal");

            migrationBuilder.UpdateData(
                table: "LeaveRequests",
                keyColumn: "RequestId",
                keyValue: 5,
                column: "LeaveType",
                value: "Normal");

            migrationBuilder.UpdateData(
                table: "LeaveRequests",
                keyColumn: "RequestId",
                keyValue: 6,
                column: "LeaveType",
                value: "Normal");
        }
    }
}
