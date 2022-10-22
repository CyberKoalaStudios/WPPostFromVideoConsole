using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WPPostFromVideoConsole.Migrations
{
    /// <inheritdoc />
    public partial class CommentsUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsPublished",
                table: "Videos",
                type: "INTEGER",
                nullable: false,
                comment: "Wordpress Publication Status; Whether future or now = true. If video post exist in WP",
                oldClrType: typeof(bool),
                oldType: "INTEGER",
                oldComment: "Wordpress Publication Status; Whether future or now = true. If videopost exist in WP");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsPublished",
                table: "Videos",
                type: "INTEGER",
                nullable: false,
                comment: "Wordpress Publication Status; Whether future or now = true. If videopost exist in WP",
                oldClrType: typeof(bool),
                oldType: "INTEGER",
                oldComment: "Wordpress Publication Status; Whether future or now = true. If video post exist in WP");
        }
    }
}
