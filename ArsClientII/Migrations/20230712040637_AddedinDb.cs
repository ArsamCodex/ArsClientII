using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArsClientII.Migrations
{
    /// <inheritdoc />
    public partial class AddedinDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TranslationWords",
                columns: table => new
                {
                    TranslationWordsID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EnglishWord = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Farsi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Dutch = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    French = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TranslationWords", x => x.TranslationWordsID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TranslationWords");
        }
    }
}
