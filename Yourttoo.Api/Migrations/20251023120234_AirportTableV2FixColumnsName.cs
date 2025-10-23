using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Yourttoo.Api.Migrations
{
    /// <inheritdoc />
    public partial class AirportTableV2FixColumnsName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ICAOCode",
                table: "Airports",
                newName: "IcaoCode");

            migrationBuilder.RenameColumn(
                name: "IATACode",
                table: "Airports",
                newName: "IataCode");

            migrationBuilder.RenameIndex(
                name: "IX_Airports_ICAOCode",
                table: "Airports",
                newName: "IX_Airports_IcaoCode");

            migrationBuilder.RenameIndex(
                name: "IX_Airports_IATACode",
                table: "Airports",
                newName: "IX_Airports_IataCode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IcaoCode",
                table: "Airports",
                newName: "ICAOCode");

            migrationBuilder.RenameColumn(
                name: "IataCode",
                table: "Airports",
                newName: "IATACode");

            migrationBuilder.RenameIndex(
                name: "IX_Airports_IcaoCode",
                table: "Airports",
                newName: "IX_Airports_ICAOCode");

            migrationBuilder.RenameIndex(
                name: "IX_Airports_IataCode",
                table: "Airports",
                newName: "IX_Airports_IATACode");
        }
    }
}
