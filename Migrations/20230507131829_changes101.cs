using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Biddo.Migrations
{
    public partial class changes101 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QueryTable_UserModel_UserId",
                table: "QueryTable");

            migrationBuilder.DropForeignKey(
                name: "FK_TimelineCommentModel_ConvensationTable_ConversationId",
                table: "TimelineCommentModel");

            migrationBuilder.DropIndex(
                name: "IX_TimelineCommentModel_ConversationId",
                table: "TimelineCommentModel");

            migrationBuilder.DropIndex(
                name: "IX_QueryTable_UserId",
                table: "QueryTable");

            migrationBuilder.DropColumn(
                name: "AdminId",
                table: "ConvensationTable");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "QueryTable",
                newName: "CreatedId");

            migrationBuilder.AddColumn<int>(
                name: "QueryId",
                table: "TimelineCommentModel",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QueryId",
                table: "TimelineCommentModel");

            migrationBuilder.RenameColumn(
                name: "CreatedId",
                table: "QueryTable",
                newName: "UserId");

            migrationBuilder.AddColumn<int>(
                name: "AdminId",
                table: "ConvensationTable",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TimelineCommentModel_ConversationId",
                table: "TimelineCommentModel",
                column: "ConversationId");

            migrationBuilder.CreateIndex(
                name: "IX_QueryTable_UserId",
                table: "QueryTable",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_QueryTable_UserModel_UserId",
                table: "QueryTable",
                column: "UserId",
                principalTable: "UserModel",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TimelineCommentModel_ConvensationTable_ConversationId",
                table: "TimelineCommentModel",
                column: "ConversationId",
                principalTable: "ConvensationTable",
                principalColumn: "ConversationId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
