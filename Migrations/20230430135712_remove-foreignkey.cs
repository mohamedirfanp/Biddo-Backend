using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Biddo.Migrations
{
    public partial class removeforeignkey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProvidedServiceId",
                table: "SelectedServiceTable",
                newName: "ProvidedServiceName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProvidedServiceName",
                table: "SelectedServiceTable",
                newName: "ProvidedServiceId");
        }
    }
}
