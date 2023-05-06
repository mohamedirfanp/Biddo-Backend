using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Biddo.Migrations
{
    public partial class changesinProvidedService : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VendorId",
                table: "ProvidedServiceTable",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ProvidedServiceTable_VendorId",
                table: "ProvidedServiceTable",
                column: "VendorId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProvidedServiceTable_VendorModel_VendorId",
                table: "ProvidedServiceTable",
                column: "VendorId",
                principalTable: "VendorModel",
                principalColumn: "VendorId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProvidedServiceTable_VendorModel_VendorId",
                table: "ProvidedServiceTable");

            migrationBuilder.DropIndex(
                name: "IX_ProvidedServiceTable_VendorId",
                table: "ProvidedServiceTable");

            migrationBuilder.DropColumn(
                name: "VendorId",
                table: "ProvidedServiceTable");
        }
    }
}
