using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WPPostFromVideoConsole.Migrations
{
    /// <inheritdoc />
    public partial class posts10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "url",
                table: "Posts",
                newName: "Url");

            migrationBuilder.RenameColumn(
                name: "timestamp",
                table: "Posts",
                newName: "Timestamp");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "Posts",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "postName",
                table: "Posts",
                newName: "PostName");

            migrationBuilder.RenameColumn(
                name: "imageUrl",
                table: "Posts",
                newName: "ImageUrl");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "Posts",
                newName: "Description");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Url",
                table: "Posts",
                newName: "url");

            migrationBuilder.RenameColumn(
                name: "Timestamp",
                table: "Posts",
                newName: "timestamp");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Posts",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "PostName",
                table: "Posts",
                newName: "postName");

            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "Posts",
                newName: "imageUrl");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Posts",
                newName: "description");
        }
    }
}
