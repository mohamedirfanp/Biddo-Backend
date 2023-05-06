using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Biddo.Migrations
{
    public partial class chatmodel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ConvensationTable",
                columns: table => new
                {
                    ConversationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    VendorId = table.Column<int>(type: "int", nullable: false),
                    AdminId = table.Column<int>(type: "int", nullable: false),
                    IsBlocked = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConvensationTable", x => x.ConversationId);
                    table.ForeignKey(
                        name: "FK_ConvensationTable_UserModel_UserId",
                        column: x => x.UserId,
                        principalTable: "UserModel",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ConvensationTable_VendorModel_VendorId",
                        column: x => x.VendorId,
                        principalTable: "VendorModel",
                        principalColumn: "VendorId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TimelineCommentModel",
                columns: table => new
                {
                    TimelineCommentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    From = table.Column<int>(type: "int", nullable: false),
                    TimeStamp = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimelineCommentModel", x => x.TimelineCommentId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConvensationTable_UserId",
                table: "ConvensationTable",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ConvensationTable_VendorId",
                table: "ConvensationTable",
                column: "VendorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConvensationTable");

            migrationBuilder.DropTable(
                name: "TimelineCommentModel");
        }
    }
}
