using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MvcForum.Migrations
{
    public partial class ImageHeightWidth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Height",
                table: "Files",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Width",
                table: "Files",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Height",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "Width",
                table: "Files");
        }
    }
}
