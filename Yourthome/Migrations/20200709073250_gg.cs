using Microsoft.EntityFrameworkCore.Migrations;

namespace Yourthome.Migrations
{
    public partial class gg : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RentalID",
                table: "Bookings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_RentalID",
                table: "Bookings",
                column: "RentalID");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Rental_RentalID",
                table: "Bookings",
                column: "RentalID",
                principalTable: "Rental",
                principalColumn: "RentalID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Rental_RentalID",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_RentalID",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "RentalID",
                table: "Bookings");
        }
    }
}
