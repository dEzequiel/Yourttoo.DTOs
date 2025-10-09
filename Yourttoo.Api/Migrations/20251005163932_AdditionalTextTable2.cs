using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Yourttoo.Api.Migrations
{
    /// <inheritdoc />
    public partial class AdditionalTextTable2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdditionalText",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Status = table.Column<string>(type: "TEXT", nullable: false, defaultValue: "published"),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "datetime('now')"),
                    UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true, defaultValueSql: "datetime('now')"),
                    Content = table.Column<string>(type: "TEXT", nullable: true),
                    Title = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdditionalText", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdditionalText_CreatedBy_CreatedAt",
                table: "AdditionalText",
                columns: new[] { "CreatedBy", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_AdditionalText_Status",
                table: "AdditionalText",
                column: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdditionalText");
        }
    }
}
