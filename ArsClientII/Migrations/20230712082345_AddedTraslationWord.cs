using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArsClientII.Migrations
{
    /// <inheritdoc />
    public partial class AddedTraslationWord : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Spanish",
                table: "TranslationWords",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Spanish",
                table: "TranslationWords");
        }
    }
}
