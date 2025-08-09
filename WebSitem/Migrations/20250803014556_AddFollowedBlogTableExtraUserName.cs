using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebSitem.Migrations
{
    /// <inheritdoc />
    public partial class AddFollowedBlogTableExtraUserName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AddedByUserName",
                table: "FollowedBlogs",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddedByUserName",
                table: "FollowedBlogs");
        }
    }
}
