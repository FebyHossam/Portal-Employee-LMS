using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Leave_Mangement_System.Migrations
{
    /// <inheritdoc />
    public partial class SecondCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Pending",
                table: "LeaveRequests");

            migrationBuilder.RenameColumn(
                name: "Rejected",
                table: "LeaveRequests",
                newName: "IsRejected");

            migrationBuilder.RenameColumn(
                name: "NameEmployee",
                table: "Employees",
                newName: "EmployeeName");

            migrationBuilder.RenameColumn(
                name: "EmailManager",
                table: "Employees",
                newName: "ManagerEmail");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsRejected",
                table: "LeaveRequests",
                newName: "Rejected");

            migrationBuilder.RenameColumn(
                name: "ManagerEmail",
                table: "Employees",
                newName: "EmailManager");

            migrationBuilder.RenameColumn(
                name: "EmployeeName",
                table: "Employees",
                newName: "NameEmployee");

            migrationBuilder.AddColumn<bool>(
                name: "Pending",
                table: "LeaveRequests",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "LeaveRequests",
                keyColumn: "RequestId",
                keyValue: 1,
                column: "Pending",
                value: false);

            migrationBuilder.UpdateData(
                table: "LeaveRequests",
                keyColumn: "RequestId",
                keyValue: 2,
                column: "Pending",
                value: true);

            migrationBuilder.UpdateData(
                table: "LeaveRequests",
                keyColumn: "RequestId",
                keyValue: 3,
                column: "Pending",
                value: false);

            migrationBuilder.UpdateData(
                table: "LeaveRequests",
                keyColumn: "RequestId",
                keyValue: 4,
                column: "Pending",
                value: true);

            migrationBuilder.UpdateData(
                table: "LeaveRequests",
                keyColumn: "RequestId",
                keyValue: 5,
                column: "Pending",
                value: false);

            migrationBuilder.UpdateData(
                table: "LeaveRequests",
                keyColumn: "RequestId",
                keyValue: 6,
                column: "Pending",
                value: true);
        }
    }
}
