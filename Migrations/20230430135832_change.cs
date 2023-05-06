using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Biddo.Migrations
{
    public partial class change : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProvidedServiceName",
                table: "SelectedServiceTable");

            migrationBuilder.AddColumn<string>(
                name: "SelectServiceName",
                table: "SelectedServiceTable",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SelectServiceName",
                table: "SelectedServiceTable");

            migrationBuilder.AddColumn<int>(
                name: "ProvidedServiceName",
                table: "SelectedServiceTable",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
