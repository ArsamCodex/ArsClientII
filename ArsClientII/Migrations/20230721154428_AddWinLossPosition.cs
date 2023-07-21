using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArsClientII.Migrations
{
    /// <inheritdoc />
    public partial class AddWinLossPosition : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "LossPosition",
                table: "Trader",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "WinPosition",
                table: "Trader",
                type: "float",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LossPosition",
                table: "Trader");

            migrationBuilder.DropColumn(
                name: "WinPosition",
                table: "Trader");
        }
    }
}
