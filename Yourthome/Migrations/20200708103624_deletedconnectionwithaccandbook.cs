using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Yourthome.Migrations
{
    public partial class deletedconnectionwithaccandbook : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rental_Accounts_AccountID",
                table: "Rental");

            migrationBuilder.DropTable(
                name: "Booking");

            migrationBuilder.DropIndex(
                name: "IX_Rental_AccountID",
                table: "Rental");

            migrationBuilder.DropColumn(
                name: "AccountID",
                table: "Rental");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AccountID",
                table: "Rental",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Booking",
                columns: table => new
                {
                    BookingID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CheckInDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    EvictionDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    GuestName = table.Column<string>(type: "text", nullable: true),
                    RentalID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Booking", x => x.BookingID);
                    table.ForeignKey(
                        name: "FK_Booking_Rental_RentalID",
                        column: x => x.RentalID,
                        principalTable: "Rental",
                        principalColumn: "RentalID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rental_AccountID",
                table: "Rental",
                column: "AccountID");

            migrationBuilder.CreateIndex(
                name: "IX_Booking_RentalID",
                table: "Booking",
                column: "RentalID");

            migrationBuilder.AddForeignKey(
                name: "FK_Rental_Accounts_AccountID",
                table: "Rental",
                column: "AccountID",
                principalTable: "Accounts",
                principalColumn: "AccountID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
