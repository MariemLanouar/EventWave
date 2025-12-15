using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventWave.Migrations
{
    /// <inheritdoc />
    public partial class PaymentMethodAsString : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Capacity",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "TicketsRemaining",
                table: "Events");

            migrationBuilder.AddColumn<int>(
                name: "PaymentMethod",
                table: "WaitLists",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "PaymentMethod",
                table: "Registrations",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentMethod",
                table: "WaitLists");

            migrationBuilder.AlterColumn<int>(
                name: "PaymentMethod",
                table: "Registrations",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "Capacity",
                table: "Events",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TicketsRemaining",
                table: "Events",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
