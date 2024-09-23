using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_New_Column : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserProfile_Culture_CultureId",
                table: "UserProfile");

            migrationBuilder.DropIndex(
                name: "IX_UserProfile_CultureId",
                table: "UserProfile");

            migrationBuilder.DropColumn(
                name: "CultureId",
                table: "UserProfile");

            migrationBuilder.DropColumn(
                name: "IsBot",
                table: "UserProfile");

            migrationBuilder.DropColumn(
                name: "IsPremium",
                table: "UserProfile");

            migrationBuilder.DropColumn(
                name: "DiscountPercent",
                table: "Subscription");

            migrationBuilder.AddColumn<DateOnly>(
                name: "DateBirth",
                table: "UserProfile",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "UserProfile",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NickName",
                table: "User",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateBirth",
                table: "UserProfile");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "UserProfile");

            migrationBuilder.AddColumn<long>(
                name: "CultureId",
                table: "UserProfile",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<bool>(
                name: "IsBot",
                table: "UserProfile",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPremium",
                table: "UserProfile",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "NickName",
                table: "User",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "DiscountPercent",
                table: "Subscription",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_UserProfile_CultureId",
                table: "UserProfile",
                column: "CultureId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserProfile_Culture_CultureId",
                table: "UserProfile",
                column: "CultureId",
                principalTable: "Culture",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
