using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Karakatsiya.Migrations
{
    /// <inheritdoc />
    public partial class AddCategoryResourceKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ResourceKey",
                table: "Categories",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResourceKey",
                table: "Categories");
        }
    }
}
