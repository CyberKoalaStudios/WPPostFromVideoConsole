using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WPPostFromVideoConsole.Migrations
{
    /// <inheritdoc />
    public partial class posts9 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsPublished",
                table: "Videos",
                type: "INTEGER",
                nullable: false,
                comment: "Wordpress Publication Status; Whether future or now = true. If videopost exist in WP",
                oldClrType: typeof(bool),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<byte>(
                name: "status",
                table: "Posts",
                type: "INTEGER",
                nullable: false,
                comment: "Publish= 0, Future=1, Private=2 .Draft=3, Pending=4,Trash=5",
                oldClrType: typeof(byte),
                oldType: "INTEGER");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsPublished",
                table: "Videos",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "INTEGER",
                oldComment: "Wordpress Publication Status; Whether future or now = true. If videopost exist in WP");

            migrationBuilder.AlterColumn<byte>(
                name: "status",
                table: "Posts",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "INTEGER",
                oldComment: "Publish= 0, Future=1, Private=2 .Draft=3, Pending=4,Trash=5");
        }
    }
}
