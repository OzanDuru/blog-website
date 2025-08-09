using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebSitem.Migrations
{
    /// <inheritdoc />
    public partial class ExternalLinkNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "StoredFileName",
                table: "FileResources",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "ExternalLink",
                table: "FileResources",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "FileResources",
                keyColumn: "StoredFileName",
                keyValue: null,
                column: "StoredFileName",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "StoredFileName",
                table: "FileResources",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "FileResources",
                keyColumn: "ExternalLink",
                keyValue: null,
                column: "ExternalLink",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "ExternalLink",
                table: "FileResources",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
