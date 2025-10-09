using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Yourttoo.Api.Migrations
{
    /// <inheritdoc />
    public partial class Geolocations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Currency = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    CurrencySymbol = table.Column<string>(type: "TEXT", maxLength: 5, nullable: false),
                    Language = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    LanguageCode = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    TimeZone = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Latitude = table.Column<long>(type: "INTEGER", nullable: false),
                    Longitude = table.Column<long>(type: "INTEGER", nullable: false),
                    AveragePrice = table.Column<double>(type: "REAL", nullable: false),
                    ImageUrl = table.Column<string>(type: "TEXT", nullable: false),
                    IconUrl = table.Column<string>(type: "TEXT", nullable: false),
                    BackgroundColor = table.Column<string>(type: "TEXT", nullable: false),
                    Status = table.Column<string>(type: "TEXT", nullable: false),
                    ThumbnailUrl = table.Column<string>(type: "TEXT", nullable: false),
                    Category = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    Name = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Zones",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    PromotionArea = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    PromotionAreaPriority = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Latitude = table.Column<long>(type: "INTEGER", nullable: false),
                    Longitude = table.Column<long>(type: "INTEGER", nullable: false),
                    AveragePrice = table.Column<double>(type: "REAL", nullable: false),
                    ImageUrl = table.Column<string>(type: "TEXT", nullable: false),
                    IconUrl = table.Column<string>(type: "TEXT", nullable: false),
                    BackgroundColor = table.Column<string>(type: "TEXT", nullable: false),
                    Status = table.Column<string>(type: "TEXT", nullable: false),
                    ThumbnailUrl = table.Column<string>(type: "TEXT", nullable: false),
                    Category = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    Name = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zones", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Regions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    CountryId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ZoneId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Latitude = table.Column<long>(type: "INTEGER", nullable: false),
                    Longitude = table.Column<long>(type: "INTEGER", nullable: false),
                    AveragePrice = table.Column<double>(type: "REAL", nullable: false),
                    ImageUrl = table.Column<string>(type: "TEXT", nullable: false),
                    IconUrl = table.Column<string>(type: "TEXT", nullable: false),
                    BackgroundColor = table.Column<string>(type: "TEXT", nullable: false),
                    Status = table.Column<string>(type: "TEXT", nullable: false),
                    ThumbnailUrl = table.Column<string>(type: "TEXT", nullable: false),
                    Category = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    Name = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Regions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Regions_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Regions_Zones_ZoneId",
                        column: x => x.ZoneId,
                        principalTable: "Zones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    RegionId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CountryId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CountryCode = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    ZoneId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Latitude = table.Column<long>(type: "INTEGER", nullable: false),
                    Longitude = table.Column<long>(type: "INTEGER", nullable: false),
                    AveragePrice = table.Column<double>(type: "REAL", nullable: false),
                    ImageUrl = table.Column<string>(type: "TEXT", nullable: false),
                    IconUrl = table.Column<string>(type: "TEXT", nullable: false),
                    BackgroundColor = table.Column<string>(type: "TEXT", nullable: false),
                    Status = table.Column<string>(type: "TEXT", nullable: false),
                    ThumbnailUrl = table.Column<string>(type: "TEXT", nullable: false),
                    Category = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    Name = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cities_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Cities_Regions_RegionId",
                        column: x => x.RegionId,
                        principalTable: "Regions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Cities_Zones_ZoneId",
                        column: x => x.ZoneId,
                        principalTable: "Zones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Airports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    CityId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ZoneId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CountryId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CountryCode = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    RegionId = table.Column<Guid>(type: "TEXT", nullable: false),
                    IATACode = table.Column<string>(type: "TEXT", maxLength: 3, nullable: false),
                    ICAOCode = table.Column<string>(type: "TEXT", maxLength: 4, nullable: false),
                    TimeZone = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    GMTOffset = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                    DSTOffset = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Latitude = table.Column<long>(type: "INTEGER", nullable: false),
                    Longitude = table.Column<long>(type: "INTEGER", nullable: false),
                    AveragePrice = table.Column<double>(type: "REAL", nullable: false),
                    ImageUrl = table.Column<string>(type: "TEXT", nullable: false),
                    IconUrl = table.Column<string>(type: "TEXT", nullable: false),
                    BackgroundColor = table.Column<string>(type: "TEXT", nullable: false),
                    Status = table.Column<string>(type: "TEXT", nullable: false),
                    ThumbnailUrl = table.Column<string>(type: "TEXT", nullable: false),
                    Category = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    Name = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Airports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Airports_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Airports_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Airports_Regions_RegionId",
                        column: x => x.RegionId,
                        principalTable: "Regions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Airports_Zones_ZoneId",
                        column: x => x.ZoneId,
                        principalTable: "Zones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Airports_CityId",
                table: "Airports",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Airports_CountryCode",
                table: "Airports",
                column: "CountryCode");

            migrationBuilder.CreateIndex(
                name: "IX_Airports_CountryId",
                table: "Airports",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Airports_IATACode",
                table: "Airports",
                column: "IATACode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Airports_ICAOCode",
                table: "Airports",
                column: "ICAOCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Airports_RegionId",
                table: "Airports",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_Airports_TimeZone",
                table: "Airports",
                column: "TimeZone");

            migrationBuilder.CreateIndex(
                name: "IX_Airports_ZoneId",
                table: "Airports",
                column: "ZoneId");

            migrationBuilder.CreateIndex(
                name: "IX_Cities_CountryCode",
                table: "Cities",
                column: "CountryCode");

            migrationBuilder.CreateIndex(
                name: "IX_Cities_CountryId",
                table: "Cities",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Cities_RegionId",
                table: "Cities",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_Cities_ZoneId",
                table: "Cities",
                column: "ZoneId");

            migrationBuilder.CreateIndex(
                name: "IX_Countries_Currency",
                table: "Countries",
                column: "Currency");

            migrationBuilder.CreateIndex(
                name: "IX_Countries_LanguageCode",
                table: "Countries",
                column: "LanguageCode");

            migrationBuilder.CreateIndex(
                name: "IX_Countries_TimeZone",
                table: "Countries",
                column: "TimeZone");

            migrationBuilder.CreateIndex(
                name: "IX_Regions_CountryId",
                table: "Regions",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Regions_ZoneId",
                table: "Regions",
                column: "ZoneId");

            migrationBuilder.CreateIndex(
                name: "IX_Zones_PromotionArea",
                table: "Zones",
                column: "PromotionArea");

            migrationBuilder.CreateIndex(
                name: "IX_Zones_PromotionAreaPriority",
                table: "Zones",
                column: "PromotionAreaPriority");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Airports");

            migrationBuilder.DropTable(
                name: "Cities");

            migrationBuilder.DropTable(
                name: "Regions");

            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropTable(
                name: "Zones");
        }
    }
}
