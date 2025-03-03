using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Karakatsiya.Migrations
{
    /// <inheritdoc />
    public partial class AddHomePageSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MainImageName",
                table: "Items");

            migrationBuilder.CreateTable(
                name: "HomePageSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Welcome = table.Column<string>(type: "text", nullable: true),
                    WelcomeMessage = table.Column<string>(type: "text", nullable: true),
                    Address = table.Column<string>(type: "text", nullable: true),
                    Phone = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    NewsTitle1 = table.Column<string>(type: "text", nullable: true),
                    NewsSummary1 = table.Column<string>(type: "text", nullable: true),
                    NewsDate1 = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    NewsTitle2 = table.Column<string>(type: "text", nullable: true),
                    NewsSummary2 = table.Column<string>(type: "text", nullable: true),
                    NewsDate2 = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Image1 = table.Column<string>(type: "text", nullable: true),
                    Image2 = table.Column<string>(type: "text", nullable: true),
                    Image3 = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomePageSettings", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HomePageSettings");

            migrationBuilder.AddColumn<string>(
                name: "MainImageName",
                table: "Items",
                type: "text",
                nullable: true);
        }
    }
}
