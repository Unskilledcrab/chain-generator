using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChainGenerator.Migrations
{
    /// <inheritdoc />
    public partial class AddingTextReferenceModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GeneratedOutput",
                table: "WidgetGeneratorModel");

            migrationBuilder.DropColumn(
                name: "GeneratedTextReferences",
                table: "WidgetGeneratorModel");

            migrationBuilder.CreateTable(
                name: "WidgetTextReferenceModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WidgetId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WidgetTextReferenceModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WidgetTextReferenceModel_WidgetGeneratorModel_WidgetId",
                        column: x => x.WidgetId,
                        principalTable: "WidgetGeneratorModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WidgetTextReferenceModel_WidgetId",
                table: "WidgetTextReferenceModel",
                column: "WidgetId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WidgetTextReferenceModel");

            migrationBuilder.AddColumn<string>(
                name: "GeneratedOutput",
                table: "WidgetGeneratorModel",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GeneratedTextReferences",
                table: "WidgetGeneratorModel",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
