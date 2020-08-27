using Microsoft.EntityFrameworkCore.Migrations;

namespace Yourthome.Migrations
{
    public partial class phonefield : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Phone",
                table: "Rental",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Rental");
        }
    }
}
