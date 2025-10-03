using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Leave_Mangement_System.Migrations
{
    /// <inheritdoc />
    public partial class AddLeaveTypeAndPhoneToModels3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Employees",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "JobTitle",
                table: "Employees",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "EmpId",
                keyValue: 1,
                columns: new[] { "Address", "JobTitle" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "EmpId",
                keyValue: 2,
                columns: new[] { "Address", "JobTitle" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "EmpId",
                keyValue: 3,
                columns: new[] { "Address", "JobTitle" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "EmpId",
                keyValue: 4,
                columns: new[] { "Address", "JobTitle" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "EmpId",
                keyValue: 5,
                columns: new[] { "Address", "JobTitle" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "EmpId",
                keyValue: 6,
                columns: new[] { "Address", "JobTitle" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "EmpId",
                keyValue: 7,
                columns: new[] { "Address", "JobTitle" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "EmpId",
                keyValue: 8,
                columns: new[] { "Address", "JobTitle" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "EmpId",
                keyValue: 9,
                columns: new[] { "Address", "JobTitle" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "EmpId",
                keyValue: 10,
                columns: new[] { "Address", "JobTitle" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "EmpId",
                keyValue: 11,
                columns: new[] { "Address", "JobTitle" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "EmpId",
                keyValue: 12,
                columns: new[] { "Address", "JobTitle" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "EmpId",
                keyValue: 13,
                columns: new[] { "Address", "JobTitle" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "EmpId",
                keyValue: 14,
                columns: new[] { "Address", "JobTitle" },
                values: new object[] { null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "JobTitle",
                table: "Employees");
        }
    }
}
