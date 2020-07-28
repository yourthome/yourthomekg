using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Yourthome.Migrations
{
    public partial class deleteavatar : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Photos_AvatarImageModelID",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_AvatarImageModelID",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "AvatarImageModelID",
                table: "Users");

            migrationBuilder.AddColumn<byte[]>(
                name: "AvatarStored",
                table: "Users",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvatarStored",
                table: "Users");

            migrationBuilder.AddColumn<int>(
                name: "AvatarImageModelID",
                table: "Users",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_AvatarImageModelID",
                table: "Users",
                column: "AvatarImageModelID");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Photos_AvatarImageModelID",
                table: "Users",
                column: "AvatarImageModelID",
                principalTable: "Photos",
                principalColumn: "ImageModelID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
