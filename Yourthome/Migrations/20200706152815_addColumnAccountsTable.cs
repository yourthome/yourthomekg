using Microsoft.EntityFrameworkCore.Migrations;

namespace Yourthome.Migrations
{
    public partial class addColumnAccountsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AccountId",
                table: "Rental",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Rental_AccountId",
                table: "Rental",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rental_Accounts_AccountId",
                table: "Rental",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rental_Accounts_AccountId",
                table: "Rental");

            migrationBuilder.DropIndex(
                name: "IX_Rental_AccountId",
                table: "Rental");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "Rental");
        }
    }
}
