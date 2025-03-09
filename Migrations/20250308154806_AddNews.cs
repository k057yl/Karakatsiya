using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Karakatsiya.Migrations
{
    /// <inheritdoc />
    public partial class AddNews : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image1",
                table: "HomePageSettings");

            migrationBuilder.DropColumn(
                name: "Image2",
                table: "HomePageSettings");

            migrationBuilder.DropColumn(
                name: "Image3",
                table: "HomePageSettings");

            migrationBuilder.DropColumn(
                name: "NewsDate1",
                table: "HomePageSettings");

            migrationBuilder.DropColumn(
                name: "NewsDate2",
                table: "HomePageSettings");

            migrationBuilder.DropColumn(
                name: "NewsDate3",
                table: "HomePageSettings");

            migrationBuilder.DropColumn(
                name: "NewsSummary1",
                table: "HomePageSettings");

            migrationBuilder.DropColumn(
                name: "NewsSummary2",
                table: "HomePageSettings");

            migrationBuilder.DropColumn(
                name: "NewsSummary3",
                table: "HomePageSettings");

            migrationBuilder.DropColumn(
                name: "NewsTitle1",
                table: "HomePageSettings");

            migrationBuilder.DropColumn(
                name: "NewsTitle2",
                table: "HomePageSettings");

            migrationBuilder.DropColumn(
                name: "NewsTitle3",
                table: "HomePageSettings");

            migrationBuilder.CreateTable(
                name: "News",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Summary = table.Column<string>(type: "text", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ImagePath = table.Column<string>(type: "text", nullable: true),
                    HomePageSettingsId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_News", x => x.Id);
                    table.ForeignKey(
                        name: "FK_News_HomePageSettings_HomePageSettingsId",
                        column: x => x.HomePageSettingsId,
                        principalTable: "HomePageSettings",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_News_HomePageSettingsId",
                table: "News",
                column: "HomePageSettingsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "News");

            migrationBuilder.AddColumn<string>(
                name: "Image1",
                table: "HomePageSettings",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Image2",
                table: "HomePageSettings",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Image3",
                table: "HomePageSettings",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "NewsDate1",
                table: "HomePageSettings",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "NewsDate2",
                table: "HomePageSettings",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "NewsDate3",
                table: "HomePageSettings",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "NewsSummary1",
                table: "HomePageSettings",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NewsSummary2",
                table: "HomePageSettings",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NewsSummary3",
                table: "HomePageSettings",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NewsTitle1",
                table: "HomePageSettings",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NewsTitle2",
                table: "HomePageSettings",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NewsTitle3",
                table: "HomePageSettings",
                type: "text",
                nullable: true);
        }
    }
}
