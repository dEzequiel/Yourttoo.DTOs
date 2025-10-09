using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Yourttoo.Api.Migrations
{
    /// <inheritdoc />
    public partial class SessionTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Sessions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    SessionId = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    UserId = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: true),
                    IpAddress = table.Column<string>(type: "TEXT", maxLength: 45, nullable: true),
                    UserAgent = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sessions", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_ExpiresAt",
                table: "Sessions",
                column: "ExpiresAt");

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_IsActive",
                table: "Sessions",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_SessionId",
                table: "Sessions",
                column: "SessionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_UserId",
                table: "Sessions",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Sessions");
        }
    }
}
