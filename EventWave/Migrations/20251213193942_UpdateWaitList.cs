using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventWave.Migrations
{
    /// <inheritdoc />
    public partial class UpdateWaitList : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WaitLists_AspNetUsers_UserId",
                table: "WaitLists");

            migrationBuilder.DropForeignKey(
                name: "FK_WaitLists_Events_EventId",
                table: "WaitLists");

            migrationBuilder.DropIndex(
                name: "IX_WaitLists_EventId",
                table: "WaitLists");

            migrationBuilder.DropIndex(
                name: "IX_WaitLists_UserId",
                table: "WaitLists");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "WaitLists",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "TicketCount",
                table: "WaitLists",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TicketType",
                table: "WaitLists",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EventId",
                table: "Tickets",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TicketCount",
                table: "WaitLists");

            migrationBuilder.DropColumn(
                name: "TicketType",
                table: "WaitLists");

            migrationBuilder.DropColumn(
                name: "EventId",
                table: "Tickets");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "WaitLists",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_WaitLists_EventId",
                table: "WaitLists",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_WaitLists_UserId",
                table: "WaitLists",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_WaitLists_AspNetUsers_UserId",
                table: "WaitLists",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WaitLists_Events_EventId",
                table: "WaitLists",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
