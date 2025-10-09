using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Yourttoo.Api.Migrations
{
    /// <inheritdoc />
    public partial class FAQTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FAQSection",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Category = table.Column<string>(type: "TEXT", nullable: true),
                    Type = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "datetime('now')"),
                    UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true, defaultValueSql: "datetime('now')"),
                    Title = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FAQSection", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FAQContent",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Slug = table.Column<string>(type: "TEXT", nullable: false),
                    Status = table.Column<string>(type: "TEXT", nullable: false),
                    FAQSectionId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "datetime('now')"),
                    UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true, defaultValueSql: "datetime('now')"),
                    Content = table.Column<string>(type: "TEXT", nullable: true),
                    Title = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FAQContent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FAQContent_FAQSection_FAQSectionId",
                        column: x => x.FAQSectionId,
                        principalTable: "FAQSection",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FAQContent_CreatedBy_CreatedAt",
                table: "FAQContent",
                columns: new[] { "CreatedBy", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_FAQContent_FAQSectionId",
                table: "FAQContent",
                column: "FAQSectionId");

            migrationBuilder.CreateIndex(
                name: "IX_FAQSection_CreatedBy_CreatedAt",
                table: "FAQSection",
                columns: new[] { "CreatedBy", "CreatedAt" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FAQContent");

            migrationBuilder.DropTable(
                name: "FAQSection");
        }
    }
}
