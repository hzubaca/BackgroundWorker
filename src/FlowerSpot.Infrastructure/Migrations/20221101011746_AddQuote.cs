using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlowerSpot.Infrastructure.Migrations
{
    public partial class AddQuote : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Quote",
                table: "Sightings",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quote",
                table: "Sightings");
        }
    }
}
