using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class idsisstings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop foreign key constraint
            migrationBuilder.DropForeignKey(
                name: "FK_EventUser_AspNetUsers_LikeListId",
                table: "EventUser");

            // Drop primary key constraint
            migrationBuilder.DropPrimaryKey(
                name: "PK_EventUser",
                table: "EventUser");

            // Alter the column
            migrationBuilder.AlterColumn<string>(
                name: "LikeListId",
                table: "EventUser",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            // Recreate primary key constraint
            migrationBuilder.AddPrimaryKey(
                name: "PK_EventUser",
                table: "EventUser",
                column: "LikeListId");

            // Recreate foreign key constraint
            migrationBuilder.AddForeignKey(
                name: "FK_EventUser_AspNetUsers_LikeListId",
                table: "EventUser",
                column: "LikeListId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop foreign key constraint
            migrationBuilder.DropForeignKey(
                name: "FK_EventUser_AspNetUsers_LikeListId",
                table: "EventUser");

            // Drop primary key constraint
            migrationBuilder.DropPrimaryKey(
                name: "PK_EventUser",
                table: "EventUser");

            // Revert the column alteration
            migrationBuilder.AlterColumn<Guid>(
                name: "LikeListId",
                table: "EventUser",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            // Recreate primary key constraint
            migrationBuilder.AddPrimaryKey(
                name: "PK_EventUser",
                table: "EventUser",
                column: "LikeListId");

            // Recreate foreign key constraint
            migrationBuilder.AddForeignKey(
                name: "FK_EventUser_AspNetUsers_LikeListId",
                table: "EventUser",
                column: "LikeListId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
