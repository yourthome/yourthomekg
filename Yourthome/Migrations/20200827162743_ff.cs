using Microsoft.EntityFrameworkCore.Migrations;

namespace Yourthome.Migrations
{
    public partial class ff : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Rental");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Phone",
                table: "Rental",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
