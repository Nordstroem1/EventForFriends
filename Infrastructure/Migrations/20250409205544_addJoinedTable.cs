using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addJoinedTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventUser_AspNetUsers_LikeListId",
                table: "EventUser");

            migrationBuilder.DropForeignKey(
                name: "FK_EventUser_Events_EventsEventId",
                table: "EventUser");

            migrationBuilder.RenameColumn(
                name: "LikeListId",
                table: "EventUser",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "EventsEventId",
                table: "EventUser",
                newName: "EventId");

            migrationBuilder.RenameIndex(
                name: "IX_EventUser_LikeListId",
                table: "EventUser",
                newName: "IX_EventUser_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_EventUser_AspNetUsers_UserId",
                table: "EventUser",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EventUser_Events_EventId",
                table: "EventUser",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "EventId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventUser_AspNetUsers_UserId",
                table: "EventUser");

            migrationBuilder.DropForeignKey(
                name: "FK_EventUser_Events_EventId",
                table: "EventUser");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "EventUser",
                newName: "LikeListId");

            migrationBuilder.RenameColumn(
                name: "EventId",
                table: "EventUser",
                newName: "EventsEventId");

            migrationBuilder.RenameIndex(
                name: "IX_EventUser_UserId",
                table: "EventUser",
                newName: "IX_EventUser_LikeListId");

            migrationBuilder.AddForeignKey(
                name: "FK_EventUser_AspNetUsers_LikeListId",
                table: "EventUser",
                column: "LikeListId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EventUser_Events_EventsEventId",
                table: "EventUser",
                column: "EventsEventId",
                principalTable: "Events",
                principalColumn: "EventId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
