using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Biddo.Migrations
{
    public partial class auctionadded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuctionModelTable",
                columns: table => new
                {
                    AuctionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventId = table.Column<int>(type: "int", nullable: false),
                    VendorId = table.Column<int>(type: "int", nullable: false),
                    BidId = table.Column<int>(type: "int", nullable: false),
                    SelectedServiceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuctionModelTable", x => x.AuctionID);
                    table.ForeignKey(
                        name: "FK_AuctionModelTable_BiddingTable_BidId",
                        column: x => x.BidId,
                        principalTable: "BiddingTable",
                        principalColumn: "BidId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuctionModelTable_SelectedServiceTable_SelectedServiceId",
                        column: x => x.SelectedServiceId,
                        principalTable: "SelectedServiceTable",
                        principalColumn: "SelectedServiceId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuctionModelTable_BidId",
                table: "AuctionModelTable",
                column: "BidId");

            migrationBuilder.CreateIndex(
                name: "IX_AuctionModelTable_SelectedServiceId",
                table: "AuctionModelTable",
                column: "SelectedServiceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuctionModelTable");
        }
    }
}
