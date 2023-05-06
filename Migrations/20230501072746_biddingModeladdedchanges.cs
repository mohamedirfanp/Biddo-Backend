using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Biddo.Migrations
{
    public partial class biddingModeladdedchanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BiddingTable_SelectedServiceTable_SelectedServicesId",
                table: "BiddingTable");

            migrationBuilder.DropIndex(
                name: "IX_BiddingTable_SelectedServicesId",
                table: "BiddingTable");

            migrationBuilder.DropColumn(
                name: "SelectedServicesId",
                table: "BiddingTable");

            migrationBuilder.AddColumn<string>(
                name: "SelectedServiceName",
                table: "BiddingTable",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SelectedServiceName",
                table: "BiddingTable");

            migrationBuilder.AddColumn<int>(
                name: "SelectedServicesId",
                table: "BiddingTable",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_BiddingTable_SelectedServicesId",
                table: "BiddingTable",
                column: "SelectedServicesId");

            migrationBuilder.AddForeignKey(
                name: "FK_BiddingTable_SelectedServiceTable_SelectedServicesId",
                table: "BiddingTable",
                column: "SelectedServicesId",
                principalTable: "SelectedServiceTable",
                principalColumn: "SelectedServiceId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
