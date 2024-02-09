using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IllegalLibAPI.Migrations
{
    /// <inheritdoc />
    public partial class UserIdDBGenerated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BookName",
                table: "BookRequests");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "BookRequests");

            migrationBuilder.AddColumn<string>(
                name: "Author",
                table: "BookRequests",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "BookRequests",
                type: "nvarchar(80)",
                maxLength: 80,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "BookRequests",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_BookRequests_UserId",
                table: "BookRequests",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookRequests_Users_UserId",
                table: "BookRequests",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookRequests_Users_UserId",
                table: "BookRequests");

            migrationBuilder.DropIndex(
                name: "IX_BookRequests_UserId",
                table: "BookRequests");

            migrationBuilder.DropColumn(
                name: "Author",
                table: "BookRequests");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "BookRequests");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "BookRequests");

            migrationBuilder.AddColumn<string>(
                name: "BookName",
                table: "BookRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "BookRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
