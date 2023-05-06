using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Biddo.Migrations
{
    public partial class fromRoleindb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FromRole",
                table: "TimelineCommentModel",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FromRole",
                table: "TimelineCommentModel");
        }
    }
}
