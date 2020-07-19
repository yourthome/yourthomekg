using Microsoft.EntityFrameworkCore.Migrations;

namespace Yourthome.Migrations
{
    public partial class deletedconnection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rental_Users_UserID",
                table: "Rental");

            migrationBuilder.DropIndex(
                name: "IX_Rental_UserID",
                table: "Rental");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Rental_UserID",
                table: "Rental",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Rental_Users_UserID",
                table: "Rental",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
