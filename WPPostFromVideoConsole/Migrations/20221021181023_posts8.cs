using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WPPostFromVideoConsole.Migrations
{
    /// <inheritdoc />
    public partial class posts8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Videos_Id",
                table: "Videos",
                column: "Id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Videos_Id",
                table: "Videos");
        }
    }
}
