using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Biddo.Migrations
{
    public partial class renameinTimelineComment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TimelineCommentModel_ConvensationTable_ConversionId",
                table: "TimelineCommentModel");

            migrationBuilder.RenameColumn(
                name: "ConversionId",
                table: "TimelineCommentModel",
                newName: "ConversationId");

            migrationBuilder.RenameIndex(
                name: "IX_TimelineCommentModel_ConversionId",
                table: "TimelineCommentModel",
                newName: "IX_TimelineCommentModel_ConversationId");

            migrationBuilder.AddForeignKey(
                name: "FK_TimelineCommentModel_ConvensationTable_ConversationId",
                table: "TimelineCommentModel",
                column: "ConversationId",
                principalTable: "ConvensationTable",
                principalColumn: "ConversationId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TimelineCommentModel_ConvensationTable_ConversationId",
                table: "TimelineCommentModel");

            migrationBuilder.RenameColumn(
                name: "ConversationId",
                table: "TimelineCommentModel",
                newName: "ConversionId");

            migrationBuilder.RenameIndex(
                name: "IX_TimelineCommentModel_ConversationId",
                table: "TimelineCommentModel",
                newName: "IX_TimelineCommentModel_ConversionId");

            migrationBuilder.AddForeignKey(
                name: "FK_TimelineCommentModel_ConvensationTable_ConversionId",
                table: "TimelineCommentModel",
                column: "ConversionId",
                principalTable: "ConvensationTable",
                principalColumn: "ConversationId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
