using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChainGenerator.Migrations
{
    /// <inheritdoc />
    public partial class ImageToWidget : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImageReferenceModel_WidgetGeneratorModel_WidgetGeneratorModelId",
                table: "ImageReferenceModel");

            migrationBuilder.DropIndex(
                name: "IX_ImageReferenceModel_WidgetGeneratorModelId",
                table: "ImageReferenceModel");

            migrationBuilder.DropColumn(
                name: "WidgetGeneratorModelId",
                table: "ImageReferenceModel");

            migrationBuilder.AddColumn<int>(
                name: "WidgetId",
                table: "ImageReferenceModel",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ImageReferenceModel_WidgetId",
                table: "ImageReferenceModel",
                column: "WidgetId");

            migrationBuilder.AddForeignKey(
                name: "FK_ImageReferenceModel_WidgetGeneratorModel_WidgetId",
                table: "ImageReferenceModel",
                column: "WidgetId",
                principalTable: "WidgetGeneratorModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImageReferenceModel_WidgetGeneratorModel_WidgetId",
                table: "ImageReferenceModel");

            migrationBuilder.DropIndex(
                name: "IX_ImageReferenceModel_WidgetId",
                table: "ImageReferenceModel");

            migrationBuilder.DropColumn(
                name: "WidgetId",
                table: "ImageReferenceModel");

            migrationBuilder.AddColumn<int>(
                name: "WidgetGeneratorModelId",
                table: "ImageReferenceModel",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ImageReferenceModel_WidgetGeneratorModelId",
                table: "ImageReferenceModel",
                column: "WidgetGeneratorModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_ImageReferenceModel_WidgetGeneratorModel_WidgetGeneratorModelId",
                table: "ImageReferenceModel",
                column: "WidgetGeneratorModelId",
                principalTable: "WidgetGeneratorModel",
                principalColumn: "Id");
        }
    }
}
