using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Yourttoo.Api.Migrations
{
    /// <inheritdoc />
    public partial class ZoneCountryRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Continent",
                table: "Countries",
                type: "TEXT",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "ZoneId",
                table: "Countries",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Countries_Continent",
                table: "Countries",
                column: "Continent");

            migrationBuilder.CreateIndex(
                name: "IX_Countries_ZoneId",
                table: "Countries",
                column: "ZoneId");

            migrationBuilder.AddForeignKey(
                name: "FK_Countries_Zones_ZoneId",
                table: "Countries",
                column: "ZoneId",
                principalTable: "Zones",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Countries_Zones_ZoneId",
                table: "Countries");

            migrationBuilder.DropIndex(
                name: "IX_Countries_Continent",
                table: "Countries");

            migrationBuilder.DropIndex(
                name: "IX_Countries_ZoneId",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "Continent",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "ZoneId",
                table: "Countries");
        }
    }
}
