using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MvcForum.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Post",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsOP = table.Column<bool>(type: "bit", nullable: false),
                    Sage = table.Column<bool>(type: "bit", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Board = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdOnBoard = table.Column<int>(type: "int", nullable: false),
                    UserIP = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ThreadId = table.Column<int>(type: "int", nullable: false),
                    HasImage = table.Column<bool>(type: "bit", nullable: false),
                    ImageId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FullImagePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ThumbImagePath = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Post", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Post");
        }
    }
}
