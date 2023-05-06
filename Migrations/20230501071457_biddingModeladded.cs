using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Biddo.Migrations
{
    public partial class biddingModeladded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BiddingTable",
                columns: table => new
                {
                    BidId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SelectedServicesId = table.Column<int>(type: "int", nullable: false),
                    EventId = table.Column<int>(type: "int", nullable: false),
                    Bid = table.Column<int>(type: "int", nullable: false),
                    VendorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BiddingTable", x => x.BidId);
                    table.ForeignKey(
                        name: "FK_BiddingTable_EventModelTable_EventId",
                        column: x => x.EventId,
                        principalTable: "EventModelTable",
                        principalColumn: "EventId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BiddingTable_SelectedServiceTable_SelectedServicesId",
                        column: x => x.SelectedServicesId,
                        principalTable: "SelectedServiceTable",
                        principalColumn: "SelectedServiceId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BiddingTable_VendorModel_VendorId",
                        column: x => x.VendorId,
                        principalTable: "VendorModel",
                        principalColumn: "VendorId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BiddingTable_EventId",
                table: "BiddingTable",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_BiddingTable_SelectedServicesId",
                table: "BiddingTable",
                column: "SelectedServicesId");

            migrationBuilder.CreateIndex(
                name: "IX_BiddingTable_VendorId",
                table: "BiddingTable",
                column: "VendorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BiddingTable");
        }
    }
}
