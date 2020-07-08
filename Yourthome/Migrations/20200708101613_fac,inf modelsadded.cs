using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Yourthome.Migrations
{
    public partial class facinfmodelsadded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Booking_Rental_RentalId",
                table: "Booking");

            migrationBuilder.DropForeignKey(
                name: "FK_Rental_Accounts_AccountId",
                table: "Rental");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Booking",
                table: "Booking");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Accounts",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "Furniture",
                table: "Rental");

            migrationBuilder.DropColumn(
                name: "Nearby",
                table: "Rental");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Accounts");

            migrationBuilder.RenameColumn(
                name: "AccountId",
                table: "Rental",
                newName: "AccountID");

            migrationBuilder.RenameIndex(
                name: "IX_Rental_AccountId",
                table: "Rental",
                newName: "IX_Rental_AccountID");

            migrationBuilder.RenameColumn(
                name: "RentalId",
                table: "Booking",
                newName: "RentalID");

            migrationBuilder.RenameIndex(
                name: "IX_Booking_RentalId",
                table: "Booking",
                newName: "IX_Booking_RentalID");

            migrationBuilder.AddColumn<int>(
                name: "BookingID",
                table: "Booking",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "AccountID",
                table: "Accounts",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Booking",
                table: "Booking",
                column: "BookingID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Accounts",
                table: "Accounts",
                column: "AccountID");

            migrationBuilder.CreateTable(
                name: "Facilities",
                columns: table => new
                {
                    FacilitiesID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RentalID = table.Column<int>(nullable: false),
                    Internet = table.Column<bool>(nullable: false),
                    Phone = table.Column<bool>(nullable: false),
                    Refrigerator = table.Column<bool>(nullable: false),
                    Kitchen = table.Column<bool>(nullable: false),
                    TV = table.Column<bool>(nullable: false),
                    Balcony = table.Column<bool>(nullable: false),
                    Washer = table.Column<bool>(nullable: false),
                    AirConditioning = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Facilities", x => x.FacilitiesID);
                    table.ForeignKey(
                        name: "FK_Facilities_Rental_RentalID",
                        column: x => x.RentalID,
                        principalTable: "Rental",
                        principalColumn: "RentalID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Infrastructure",
                columns: table => new
                {
                    InfrastructureID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RentalID = table.Column<int>(nullable: false),
                    Cafe = table.Column<bool>(nullable: false),
                    Kindergarten = table.Column<bool>(nullable: false),
                    Parking = table.Column<bool>(nullable: false),
                    BusStop = table.Column<bool>(nullable: false),
                    Supermarket = table.Column<bool>(nullable: false),
                    Park = table.Column<bool>(nullable: false),
                    Hospital = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Infrastructure", x => x.InfrastructureID);
                    table.ForeignKey(
                        name: "FK_Infrastructure_Rental_RentalID",
                        column: x => x.RentalID,
                        principalTable: "Rental",
                        principalColumn: "RentalID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Facilities_RentalID",
                table: "Facilities",
                column: "RentalID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Infrastructure_RentalID",
                table: "Infrastructure",
                column: "RentalID",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_Rental_RentalID",
                table: "Booking",
                column: "RentalID",
                principalTable: "Rental",
                principalColumn: "RentalID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rental_Accounts_AccountID",
                table: "Rental",
                column: "AccountID",
                principalTable: "Accounts",
                principalColumn: "AccountID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Booking_Rental_RentalID",
                table: "Booking");

            migrationBuilder.DropForeignKey(
                name: "FK_Rental_Accounts_AccountID",
                table: "Rental");

            migrationBuilder.DropTable(
                name: "Facilities");

            migrationBuilder.DropTable(
                name: "Infrastructure");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Booking",
                table: "Booking");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Accounts",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "BookingID",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "AccountID",
                table: "Accounts");

            migrationBuilder.RenameColumn(
                name: "AccountID",
                table: "Rental",
                newName: "AccountId");

            migrationBuilder.RenameIndex(
                name: "IX_Rental_AccountID",
                table: "Rental",
                newName: "IX_Rental_AccountId");

            migrationBuilder.RenameColumn(
                name: "RentalID",
                table: "Booking",
                newName: "RentalId");

            migrationBuilder.RenameIndex(
                name: "IX_Booking_RentalID",
                table: "Booking",
                newName: "IX_Booking_RentalId");

            migrationBuilder.AddColumn<string>(
                name: "Furniture",
                table: "Rental",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Nearby",
                table: "Rental",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Booking",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Accounts",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Booking",
                table: "Booking",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Accounts",
                table: "Accounts",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_Rental_RentalId",
                table: "Booking",
                column: "RentalId",
                principalTable: "Rental",
                principalColumn: "RentalID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rental_Accounts_AccountId",
                table: "Rental",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
