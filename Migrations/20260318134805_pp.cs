using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlexFit.Migrations
{
    /// <inheritdoc />
    public partial class pp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MembershipCards_Users_MemberId",
                table: "MembershipCards");

            migrationBuilder.DropIndex(
                name: "IX_MembershipCards_MemberId",
                table: "MembershipCards");

            migrationBuilder.DropColumn(
                name: "CardType",
                table: "MembershipCards");

            migrationBuilder.DropColumn(
                name: "PurchaseTime",
                table: "MembershipCards");

            migrationBuilder.DropColumn(
                name: "SerialNumber",
                table: "MembershipCards");

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "Users",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "MemberId",
                table: "MembershipCards",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "MembershipCards",
                type: "character varying(21)",
                maxLength: 21,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "MembershipCards",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_MembershipCards_MemberId",
                table: "MembershipCards",
                column: "MemberId");

            migrationBuilder.AddForeignKey(
                name: "FK_MembershipCards_Users_MemberId",
                table: "MembershipCards",
                column: "MemberId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MembershipCards_Users_MemberId1",
                table: "MembershipCards",
                column: "MemberId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MembershipCards_Users_MemberId",
                table: "MembershipCards");

            migrationBuilder.DropForeignKey(
                name: "FK_MembershipCards_Users_MemberId1",
                table: "MembershipCards");

            migrationBuilder.DropIndex(
                name: "IX_MembershipCards_MemberId",
                table: "MembershipCards");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "MembershipCards");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "MembershipCards");

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MemberId",
                table: "MembershipCards",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CardType",
                table: "MembershipCards",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "PurchaseTime",
                table: "MembershipCards",
                type: "interval",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SerialNumber",
                table: "MembershipCards",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MembershipCards_MemberId",
                table: "MembershipCards",
                column: "MemberId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_MembershipCards_Users_MemberId",
                table: "MembershipCards",
                column: "MemberId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
