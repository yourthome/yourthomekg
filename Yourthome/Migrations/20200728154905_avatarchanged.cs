using Microsoft.EntityFrameworkCore.Migrations;

namespace Yourthome.Migrations
{
    public partial class avatarchanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Photos_AvatarStoredImageModelID",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_AvatarStoredImageModelID",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "AvatarStoredImageModelID",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "AvatarName",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AvatarPath",
                table: "Users",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvatarName",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "AvatarPath",
                table: "Users");

            migrationBuilder.AddColumn<int>(
                name: "AvatarStoredImageModelID",
                table: "Users",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_AvatarStoredImageModelID",
                table: "Users",
                column: "AvatarStoredImageModelID");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Photos_AvatarStoredImageModelID",
                table: "Users",
                column: "AvatarStoredImageModelID",
                principalTable: "Photos",
                principalColumn: "ImageModelID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
