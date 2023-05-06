using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Biddo.Migrations
{
    public partial class chatmodelupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ConversionId",
                table: "TimelineCommentModel",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TimelineCommentModel_ConversionId",
                table: "TimelineCommentModel",
                column: "ConversionId");

            migrationBuilder.AddForeignKey(
                name: "FK_TimelineCommentModel_ConvensationTable_ConversionId",
                table: "TimelineCommentModel",
                column: "ConversionId",
                principalTable: "ConvensationTable",
                principalColumn: "ConversationId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TimelineCommentModel_ConvensationTable_ConversionId",
                table: "TimelineCommentModel");

            migrationBuilder.DropIndex(
                name: "IX_TimelineCommentModel_ConversionId",
                table: "TimelineCommentModel");

            migrationBuilder.DropColumn(
                name: "ConversionId",
                table: "TimelineCommentModel");
        }
    }
}
