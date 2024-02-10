using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChainGenerator.Migrations
{
    /// <inheritdoc />
    public partial class AddingImageReference : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ImageReferenceModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WidgetGeneratorModelId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImageReferenceModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImageReferenceModel_WidgetGeneratorModel_WidgetGeneratorModelId",
                        column: x => x.WidgetGeneratorModelId,
                        principalTable: "WidgetGeneratorModel",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ImageReferenceModel_WidgetGeneratorModelId",
                table: "ImageReferenceModel",
                column: "WidgetGeneratorModelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ImageReferenceModel");
        }
    }
}
