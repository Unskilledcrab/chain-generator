using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChainGenerator.Migrations
{
    /// <inheritdoc />
    public partial class AddingInputParameters : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "ChainGeneratorPageModel",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "ChainGeneratorPageModel",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "HtmlOutputTemplate",
                table: "ChainGeneratorPageModel",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "ChainGeneratorInputParameter",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DefaultValue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    ChainGeneratorPageModelId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChainGeneratorInputParameter", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChainGeneratorInputParameter_ChainGeneratorPageModel_ChainGeneratorPageModelId",
                        column: x => x.ChainGeneratorPageModelId,
                        principalTable: "ChainGeneratorPageModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChainGeneratorInputParameter_ChainGeneratorPageModelId",
                table: "ChainGeneratorInputParameter",
                column: "ChainGeneratorPageModelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChainGeneratorInputParameter");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "ChainGeneratorPageModel");

            migrationBuilder.DropColumn(
                name: "HtmlOutputTemplate",
                table: "ChainGeneratorPageModel");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "ChainGeneratorPageModel",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
