using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArsClientII.Migrations
{
    /// <inheritdoc />
    public partial class AddedTraderClass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Trader",
                columns: table => new
                {
                    TraderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Pair = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BuyPrice = table.Column<double>(type: "float", nullable: true),
                    SelPrice = table.Column<double>(type: "float", nullable: true),
                    BougtDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SoldDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsTradeCompleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trader", x => x.TraderId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Trader");
        }
    }
}
