using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Biddo.Migrations
{
    public partial class auctionStatusadded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAuctionCompleted",
                table: "EventModelTable",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAuctionCompleted",
                table: "EventModelTable");
        }
    }
}
