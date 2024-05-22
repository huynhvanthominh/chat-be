using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace chat_be.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMessageUserModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_MessageGroupUserModels_MessageGroupId",
                table: "MessageGroupUserModels",
                column: "MessageGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageGroupUserModels_UserId",
                table: "MessageGroupUserModels",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_MessageGroupUserModels_MessageGroupModels_MessageGroupId",
                table: "MessageGroupUserModels",
                column: "MessageGroupId",
                principalTable: "MessageGroupModels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MessageGroupUserModels_Users_UserId",
                table: "MessageGroupUserModels",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MessageGroupUserModels_MessageGroupModels_MessageGroupId",
                table: "MessageGroupUserModels");

            migrationBuilder.DropForeignKey(
                name: "FK_MessageGroupUserModels_Users_UserId",
                table: "MessageGroupUserModels");

            migrationBuilder.DropIndex(
                name: "IX_MessageGroupUserModels_MessageGroupId",
                table: "MessageGroupUserModels");

            migrationBuilder.DropIndex(
                name: "IX_MessageGroupUserModels_UserId",
                table: "MessageGroupUserModels");
        }
    }
}
