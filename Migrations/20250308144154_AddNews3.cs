using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Karakatsiya.Migrations
{
    /// <inheritdoc />
    public partial class AddNews3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WelcomeMessage",
                table: "HomePageSettings",
                newName: "NewsTitle3");

            migrationBuilder.RenameColumn(
                name: "Welcome",
                table: "HomePageSettings",
                newName: "NewsSummary3");

            migrationBuilder.AddColumn<DateTime>(
                name: "NewsDate3",
                table: "HomePageSettings",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NewsDate3",
                table: "HomePageSettings");

            migrationBuilder.RenameColumn(
                name: "NewsTitle3",
                table: "HomePageSettings",
                newName: "WelcomeMessage");

            migrationBuilder.RenameColumn(
                name: "NewsSummary3",
                table: "HomePageSettings",
                newName: "Welcome");
        }
    }
}
