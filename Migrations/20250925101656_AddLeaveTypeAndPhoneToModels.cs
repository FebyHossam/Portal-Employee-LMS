using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Leave_Mangement_System.Migrations
{
    /// <inheritdoc />
    public partial class AddLeaveTypeAndPhoneToModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "HrApprovalDate",
                table: "LeaveRequests",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LeaveType",
                table: "LeaveRequests",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "ManagerApprovalDate",
                table: "LeaveRequests",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Reason",
                table: "LeaveRequests",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "LeaveRequests",
                keyColumn: "RequestId",
                keyValue: 1,
                columns: new[] { "HrApprovalDate", "LeaveType", "ManagerApprovalDate", "Reason" },
                values: new object[] { null, "Normal", null, null });

            migrationBuilder.UpdateData(
                table: "LeaveRequests",
                keyColumn: "RequestId",
                keyValue: 2,
                columns: new[] { "HrApprovalDate", "LeaveType", "ManagerApprovalDate", "Reason" },
                values: new object[] { null, "Normal", null, null });

            migrationBuilder.UpdateData(
                table: "LeaveRequests",
                keyColumn: "RequestId",
                keyValue: 3,
                columns: new[] { "HrApprovalDate", "LeaveType", "ManagerApprovalDate", "Reason" },
                values: new object[] { null, "Normal", null, null });

            migrationBuilder.UpdateData(
                table: "LeaveRequests",
                keyColumn: "RequestId",
                keyValue: 4,
                columns: new[] { "HrApprovalDate", "LeaveType", "ManagerApprovalDate", "Reason" },
                values: new object[] { null, "Normal", null, null });

            migrationBuilder.UpdateData(
                table: "LeaveRequests",
                keyColumn: "RequestId",
                keyValue: 5,
                columns: new[] { "HrApprovalDate", "LeaveType", "ManagerApprovalDate", "Reason" },
                values: new object[] { null, "Normal", null, null });

            migrationBuilder.UpdateData(
                table: "LeaveRequests",
                keyColumn: "RequestId",
                keyValue: 6,
                columns: new[] { "HrApprovalDate", "LeaveType", "ManagerApprovalDate", "Reason" },
                values: new object[] { null, "Normal", null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HrApprovalDate",
                table: "LeaveRequests");

            migrationBuilder.DropColumn(
                name: "LeaveType",
                table: "LeaveRequests");

            migrationBuilder.DropColumn(
                name: "ManagerApprovalDate",
                table: "LeaveRequests");

            migrationBuilder.DropColumn(
                name: "Reason",
                table: "LeaveRequests");
        }
    }
}
