using Microsoft.EntityFrameworkCore.Migrations;

namespace Yourthome.Migrations
{
    public partial class ewfwef : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Photos_Rental_RentalID",
                table: "Photos");

            migrationBuilder.AlterColumn<int>(
                name: "RentalID",
                table: "Photos",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Photos_Rental_RentalID",
                table: "Photos",
                column: "RentalID",
                principalTable: "Rental",
                principalColumn: "RentalID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Photos_Rental_RentalID",
                table: "Photos");

            migrationBuilder.AlterColumn<int>(
                name: "RentalID",
                table: "Photos",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Photos_Rental_RentalID",
                table: "Photos",
                column: "RentalID",
                principalTable: "Rental",
                principalColumn: "RentalID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
