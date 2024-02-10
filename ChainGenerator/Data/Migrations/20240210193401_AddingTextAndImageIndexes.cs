using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChainGenerator.Migrations
{
    /// <inheritdoc />
    public partial class AddingTextAndImageIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GeneratedImageUrl",
                table: "WidgetGeneratorModel");

            migrationBuilder.AddColumn<string>(
                name: "GeneratedTextReferences",
                table: "WidgetGeneratorModel",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "SelectedGeneratedImageIndex",
                table: "WidgetGeneratorModel",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SelectedGeneratedTextIndex",
                table: "WidgetGeneratorModel",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GeneratedTextReferences",
                table: "WidgetGeneratorModel");

            migrationBuilder.DropColumn(
                name: "SelectedGeneratedImageIndex",
                table: "WidgetGeneratorModel");

            migrationBuilder.DropColumn(
                name: "SelectedGeneratedTextIndex",
                table: "WidgetGeneratorModel");

            migrationBuilder.AddColumn<string>(
                name: "GeneratedImageUrl",
                table: "WidgetGeneratorModel",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
