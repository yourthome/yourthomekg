using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Yourthome.Migrations
{
    public partial class useravatarad : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvatarStored",
                table: "Users");

            migrationBuilder.AddColumn<int>(
                name: "AvatarStoredImageModelID",
                table: "Users",
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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<byte[]>(
                name: "AvatarStored",
                table: "Users",
                type: "bytea",
                nullable: true);
        }
    }
}
