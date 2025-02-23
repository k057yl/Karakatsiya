using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Karakatsiya.Migrations
{
    /// <inheritdoc />
    public partial class AddedMainImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MainImageName",
                table: "Items",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MainImageName",
                table: "Items");
        }
    }
}
