using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OutOfOffice.DAL.Migrations
{
    /// <inheritdoc />
    public partial class LeaveToApprovalOneToOne : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ApprovalRequests_LeaveRequestId",
                table: "ApprovalRequests");

            migrationBuilder.AddColumn<int>(
                name: "ApprovalRequestId",
                table: "LeaveRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalRequests_LeaveRequestId",
                table: "ApprovalRequests",
                column: "LeaveRequestId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ApprovalRequests_LeaveRequestId",
                table: "ApprovalRequests");

            migrationBuilder.DropColumn(
                name: "ApprovalRequestId",
                table: "LeaveRequests");

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalRequests_LeaveRequestId",
                table: "ApprovalRequests",
                column: "LeaveRequestId");
        }
    }
}
