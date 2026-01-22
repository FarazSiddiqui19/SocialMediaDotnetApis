using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialMedia.Migrations
{
    /// <inheritdoc />
    public partial class FriendRequestTableStructureChanged : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FriendRequest_Users_FriendId",
                table: "FriendRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_FriendRequest_Users_UserId",
                table: "FriendRequest");

            migrationBuilder.RenameColumn(
                name: "FriendId",
                table: "FriendRequest",
                newName: "RecieverId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "FriendRequest",
                newName: "SenderId");

            migrationBuilder.RenameIndex(
                name: "IX_FriendRequest_FriendId",
                table: "FriendRequest",
                newName: "IX_FriendRequest_RecieverId");

            migrationBuilder.AddForeignKey(
                name: "FK_FriendRequest_Users_RecieverId",
                table: "FriendRequest",
                column: "RecieverId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FriendRequest_Users_SenderId",
                table: "FriendRequest",
                column: "SenderId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FriendRequest_Users_RecieverId",
                table: "FriendRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_FriendRequest_Users_SenderId",
                table: "FriendRequest");

            migrationBuilder.RenameColumn(
                name: "RecieverId",
                table: "FriendRequest",
                newName: "FriendId");

            migrationBuilder.RenameColumn(
                name: "SenderId",
                table: "FriendRequest",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_FriendRequest_RecieverId",
                table: "FriendRequest",
                newName: "IX_FriendRequest_FriendId");

            migrationBuilder.AddForeignKey(
                name: "FK_FriendRequest_Users_FriendId",
                table: "FriendRequest",
                column: "FriendId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FriendRequest_Users_UserId",
                table: "FriendRequest",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
