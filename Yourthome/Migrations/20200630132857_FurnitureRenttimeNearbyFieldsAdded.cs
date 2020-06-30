using Microsoft.EntityFrameworkCore.Migrations;

namespace Yourthome.Migrations
{
    public partial class FurnitureRenttimeNearbyFieldsAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Furniture",
                table: "Rental",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Nearby",
                table: "Rental",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RentTime",
                table: "Rental",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Furniture",
                table: "Rental");

            migrationBuilder.DropColumn(
                name: "Nearby",
                table: "Rental");

            migrationBuilder.DropColumn(
                name: "RentTime",
                table: "Rental");
        }
    }
}
