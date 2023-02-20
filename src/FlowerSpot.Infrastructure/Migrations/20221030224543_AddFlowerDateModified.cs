using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlowerSpot.Infrastructure.Migrations
{
    public partial class AddFlowerDateModified : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateModified",
                table: "Flowers",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Flowers",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Flowers_DateModified",
                table: "Flowers",
                column: "DateModified");

            migrationBuilder.CreateIndex(
                name: "IX_Flowers_UserId",
                table: "Flowers",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Flowers_Users_UserId",
                table: "Flowers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Flowers_Users_UserId",
                table: "Flowers");

            migrationBuilder.DropIndex(
                name: "IX_Flowers_DateModified",
                table: "Flowers");

            migrationBuilder.DropIndex(
                name: "IX_Flowers_UserId",
                table: "Flowers");

            migrationBuilder.DropColumn(
                name: "DateModified",
                table: "Flowers");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Flowers");
        }
    }
}
