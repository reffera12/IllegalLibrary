using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IllegalLibAPI.Migrations
{
    /// <inheritdoc />
    public partial class SomeModelModif : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_AuthUsers_UserId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Isbn",
                table: "Books");

            migrationBuilder.AddColumn<string>(
                name: "Bio",
                table: "Users",
                type: "nvarchar(max)",
                maxLength: 5000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "AuthUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ResetToken",
                table: "AuthUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId1",
                table: "AuthUsers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuthUsers_UserId1",
                table: "AuthUsers",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_AuthUsers_Users_UserId",
                table: "AuthUsers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AuthUsers_Users_UserId1",
                table: "AuthUsers",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuthUsers_Users_UserId",
                table: "AuthUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AuthUsers_Users_UserId1",
                table: "AuthUsers");

            migrationBuilder.DropIndex(
                name: "IX_AuthUsers_UserId1",
                table: "AuthUsers");

            migrationBuilder.DropColumn(
                name: "Bio",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "AuthUsers");

            migrationBuilder.DropColumn(
                name: "ResetToken",
                table: "AuthUsers");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "AuthUsers");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Isbn",
                table: "Books",
                type: "nvarchar(17)",
                maxLength: 17,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_AuthUsers_UserId",
                table: "Users",
                column: "UserId",
                principalTable: "AuthUsers",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
