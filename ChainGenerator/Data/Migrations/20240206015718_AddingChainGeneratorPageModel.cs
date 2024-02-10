using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChainGenerator.Migrations
{
    /// <inheritdoc />
    public partial class AddingChainGeneratorPageModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChainGeneratorPageModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OwnerId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChainGeneratorPageModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChainGeneratorPageModel_AspNetUsers_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WidgetGeneratorModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Prompt = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PromptIntent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GeneratedPrompt = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsTextGenerator = table.Column<bool>(type: "bit", nullable: false),
                    GeneratedOutput = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GeneratedImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChainGeneratorPageModelId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WidgetGeneratorModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WidgetGeneratorModel_ChainGeneratorPageModel_ChainGeneratorPageModelId",
                        column: x => x.ChainGeneratorPageModelId,
                        principalTable: "ChainGeneratorPageModel",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChainGeneratorPageModel_OwnerId",
                table: "ChainGeneratorPageModel",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_WidgetGeneratorModel_ChainGeneratorPageModelId",
                table: "WidgetGeneratorModel",
                column: "ChainGeneratorPageModelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WidgetGeneratorModel");

            migrationBuilder.DropTable(
                name: "ChainGeneratorPageModel");
        }
    }
}
