using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialMedia.Migrations
{
    /// <inheritdoc />
    public partial class FinalizeFriendRequestRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop existing foreign keys to be safe
            migrationBuilder.DropForeignKey(
                name: "FK_FriendRequest_Users_RecieverId",
                table: "FriendRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_FriendRequest_Users_SenderId",
                table: "FriendRequest");

            // Re-add them with the correct OnDelete behavior
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

            migrationBuilder.AddForeignKey(
                name: "FK_FriendRequest_Users_RecieverId",
                table: "FriendRequest",
                column: "RecieverId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FriendRequest_Users_SenderId",
                table: "FriendRequest",
                column: "SenderId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
