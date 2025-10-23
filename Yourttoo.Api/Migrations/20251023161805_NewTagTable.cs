using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Yourttoo.Api.Migrations
{
    /// <inheritdoc />
    public partial class NewTagTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TagCategoryTag");

            migrationBuilder.DropTable(
                name: "TagCategory");

            migrationBuilder.DropIndex(
                name: "IX_Tags_Code",
                table: "Tags");

            migrationBuilder.DropIndex(
                name: "IX_Tags_CreatedBy_CreatedAt",
                table: "Tags");

            migrationBuilder.DropIndex(
                name: "IX_Tags_Slug",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "Icon",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "Label",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "Slug",
                table: "Tags");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Tags",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true,
                oldDefaultValueSql: "datetime('now')");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Tags",
                type: "TEXT",
                maxLength: 50,
                nullable: false,
                defaultValue: "Active",
                comment: "Current status of the tag",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldDefaultValue: "active");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Tags",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Tags",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Tags",
                type: "TEXT",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValueSql: "datetime('now')");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Tags",
                type: "TEXT",
                maxLength: 100,
                nullable: true,
                comment: "Main category classification of the tag");

            migrationBuilder.AddColumn<string>(
                name: "IconUrl",
                table: "Tags",
                type: "TEXT",
                maxLength: 500,
                nullable: true,
                comment: "URL to the icon image for the tag");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Tags",
                type: "TEXT",
                maxLength: 500,
                nullable: true,
                comment: "URL to the main image for the tag");

            migrationBuilder.AddColumn<string>(
                name: "Key",
                table: "Tags",
                type: "TEXT",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                comment: "Unique identifier key for the tag");

            migrationBuilder.AddColumn<string>(
                name: "SubCategory",
                table: "Tags",
                type: "TEXT",
                maxLength: 100,
                nullable: true,
                comment: "Sub-category classification of the tag");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Tags",
                type: "TEXT",
                maxLength: 50,
                nullable: false,
                defaultValue: "SearchKeyword",
                comment: "Type of tag (Category, Feature, Theme, etc.)");

            migrationBuilder.AddColumn<string>(
                name: "Value",
                table: "Tags",
                type: "TEXT",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                comment: "Display value of the tag");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_Category",
                table: "Tags",
                column: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_Category_SubCategory",
                table: "Tags",
                columns: new[] { "Category", "SubCategory" });

            migrationBuilder.CreateIndex(
                name: "IX_Tags_CreatedAt",
                table: "Tags",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_Key",
                table: "Tags",
                column: "Key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tags_SubCategory",
                table: "Tags",
                column: "SubCategory");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_Type",
                table: "Tags",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_Type_Status",
                table: "Tags",
                columns: new[] { "Type", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_Tags_UpdatedAt",
                table: "Tags",
                column: "UpdatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_Value",
                table: "Tags",
                column: "Value");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Tags_Status",
                table: "Tags",
                sql: "Status IN ('Active', 'Inactive', 'Pending', 'Deleted', 'Archived', 'Draft', 'Approved', 'Rejected', 'Suspended', 'Expired', 'Completed', 'Cancelled', 'Processing', 'On_Hold', 'New', 'Updated', 'Verified', 'Unverified')");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Tags_Type",
                table: "Tags",
                sql: "Type IN ('Category', 'Feature', 'Theme', 'Topic', 'Destination', 'Service', 'Audience', 'Style', 'PriceRange', 'SearchKeyword')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Tags_Category",
                table: "Tags");

            migrationBuilder.DropIndex(
                name: "IX_Tags_Category_SubCategory",
                table: "Tags");

            migrationBuilder.DropIndex(
                name: "IX_Tags_CreatedAt",
                table: "Tags");

            migrationBuilder.DropIndex(
                name: "IX_Tags_Key",
                table: "Tags");

            migrationBuilder.DropIndex(
                name: "IX_Tags_SubCategory",
                table: "Tags");

            migrationBuilder.DropIndex(
                name: "IX_Tags_Type",
                table: "Tags");

            migrationBuilder.DropIndex(
                name: "IX_Tags_Type_Status",
                table: "Tags");

            migrationBuilder.DropIndex(
                name: "IX_Tags_UpdatedAt",
                table: "Tags");

            migrationBuilder.DropIndex(
                name: "IX_Tags_Value",
                table: "Tags");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Tags_Status",
                table: "Tags");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Tags_Type",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "IconUrl",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "Key",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "SubCategory",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "Tags");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Tags",
                type: "TEXT",
                nullable: true,
                defaultValueSql: "datetime('now')",
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Tags",
                type: "TEXT",
                nullable: false,
                defaultValue: "active",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 50,
                oldDefaultValue: "Active",
                oldComment: "Current status of the tag");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Tags",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Tags",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Tags",
                type: "TEXT",
                nullable: false,
                defaultValueSql: "datetime('now')",
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Tags",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Icon",
                table: "Tags",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Label",
                table: "Tags",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "Tags",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "TagCategory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Code = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "datetime('now')"),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                    Filtering = table.Column<bool>(type: "INTEGER", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true, defaultValueSql: "datetime('now')"),
                    UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TagCategory", x => x.Id);
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
                name: "IX_TagCategory_Code",
                table: "TagCategory",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TagCategoryTag_TagsId",
                table: "TagCategoryTag",
                column: "TagsId");
        }
    }
}
