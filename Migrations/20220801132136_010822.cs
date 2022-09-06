using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MvcForum.Migrations
{
    public partial class _010822 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ThumbImagePath",
                table: "Post",
                newName: "UserFileName");

            migrationBuilder.RenameColumn(
                name: "ImageId",
                table: "Post",
                newName: "ThumbImageName");

            migrationBuilder.RenameColumn(
                name: "FullImagePath",
                table: "Post",
                newName: "FullImageName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserFileName",
                table: "Post",
                newName: "ThumbImagePath");

            migrationBuilder.RenameColumn(
                name: "ThumbImageName",
                table: "Post",
                newName: "ImageId");

            migrationBuilder.RenameColumn(
                name: "FullImageName",
                table: "Post",
                newName: "FullImagePath");
        }
    }
}
