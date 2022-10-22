using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WPPostFromVideoConsole.Migrations
{
    /// <inheritdoc />
    public partial class postidfromwp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WordpressId",
                table: "Posts",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WordpressId",
                table: "Posts");
        }
    }
}
