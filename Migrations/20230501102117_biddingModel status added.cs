using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Biddo.Migrations
{
    public partial class biddingModelstatusadded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAuctionCompleted",
                table: "BiddingTable",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "win",
                table: "BiddingTable",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAuctionCompleted",
                table: "BiddingTable");

            migrationBuilder.DropColumn(
                name: "win",
                table: "BiddingTable");
        }
    }
}
