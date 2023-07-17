using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArsClientII.Migrations
{
    /// <inheritdoc />
    public partial class AddedMovingAverages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "MovingAverage100",
                table: "CoinAnalysis",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "MovingAverage200",
                table: "CoinAnalysis",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "MovingAverage21",
                table: "CoinAnalysis",
                type: "float",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MovingAverage100",
                table: "CoinAnalysis");

            migrationBuilder.DropColumn(
                name: "MovingAverage200",
                table: "CoinAnalysis");

            migrationBuilder.DropColumn(
                name: "MovingAverage21",
                table: "CoinAnalysis");
        }
    }
}
