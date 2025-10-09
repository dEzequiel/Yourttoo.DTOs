using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Yourttoo.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TagCategory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Code = table.Column<string>(type: "TEXT", nullable: false),
                    Filtering = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "datetime('now')"),
                    UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true, defaultValueSql: "datetime('now')"),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    Name = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TagCategory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Code = table.Column<string>(type: "TEXT", nullable: false),
                    Slug = table.Column<string>(type: "TEXT", nullable: false),
                    Status = table.Column<string>(type: "TEXT", nullable: false, defaultValue: "active"),
                    Icon = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "datetime('now')"),
                    UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true, defaultValueSql: "datetime('now')"),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    Label = table.Column<string>(type: "TEXT", nullable: true),
                    Name = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TagCategoryTag",
                columns: table => new
                {
                    CategoriesId = table.Column<Guid>(type: "TEXT", nullable: false),
                    TagsId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TagCategoryTag", x => new { x.CategoriesId, x.TagsId });
                    table.ForeignKey(
                        name: "FK_TagCategoryTag_TagCategory_CategoriesId",
                        column: x => x.CategoriesId,
                        principalTable: "TagCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TagCategoryTag_Tags_TagsId",
                        column: x => x.TagsId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TagCategory_Code",
                table: "TagCategory",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TagCategoryTag_TagsId",
                table: "TagCategoryTag",
                column: "TagsId");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_Code",
                table: "Tags",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tags_CreatedBy_CreatedAt",
                table: "Tags",
                columns: new[] { "CreatedBy", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Tags_Slug",
                table: "Tags",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tags_Status",
                table: "Tags",
                column: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TagCategoryTag");

            migrationBuilder.DropTable(
                name: "TagCategory");

            migrationBuilder.DropTable(
                name: "Tags");
        }
    }
}
