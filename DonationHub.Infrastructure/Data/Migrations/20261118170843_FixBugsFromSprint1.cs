using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DonationHub.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixBugsFromSprint1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Campaigns_Categories_CategoryId1",
                table: "Campaigns");

            migrationBuilder.DropForeignKey(
                name: "FK_PaymentAttempts_Donations_DonationId1",
                table: "PaymentAttempts");

            migrationBuilder.DropIndex(
                name: "IX_PaymentAttempts_DonationId1",
                table: "PaymentAttempts");

            migrationBuilder.DropIndex(
                name: "IX_Categories_Name_NameAr_Name_NameEn",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Campaigns_CategoryId1",
                table: "Campaigns");

            migrationBuilder.DropColumn(
                name: "DonationId1",
                table: "PaymentAttempts");

            migrationBuilder.DropColumn(
                name: "CategoryId1",
                table: "Campaigns");

            migrationBuilder.AddColumn<Guid>(
                name: "FamilyId",
                table: "RefreshTokens",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<Guid>(
                name: "DonationId",
                table: "PaymentAttempts",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedAt",
                table: "Categories",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Categories",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_PaymentAttempts_DonationId",
                table: "PaymentAttempts",
                column: "DonationId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Name_NameAr",
                table: "Categories",
                column: "Name_NameAr",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Name_NameEn",
                table: "Categories",
                column: "Name_NameEn",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentAttempts_Donations_DonationId",
                table: "PaymentAttempts",
                column: "DonationId",
                principalTable: "Donations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentAttempts_Donations_DonationId",
                table: "PaymentAttempts");

            migrationBuilder.DropIndex(
                name: "IX_PaymentAttempts_DonationId",
                table: "PaymentAttempts");

            migrationBuilder.DropIndex(
                name: "IX_Categories_Name_NameAr",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Categories_Name_NameEn",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "FamilyId",
                table: "RefreshTokens");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Categories");

            migrationBuilder.AlterColumn<string>(
                name: "DonationId",
                table: "PaymentAttempts",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "DonationId1",
                table: "PaymentAttempts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "CategoryId1",
                table: "Campaigns",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PaymentAttempts_DonationId1",
                table: "PaymentAttempts",
                column: "DonationId1");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Name_NameAr_Name_NameEn",
                table: "Categories",
                columns: new[] { "Name_NameAr", "Name_NameEn" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Campaigns_CategoryId1",
                table: "Campaigns",
                column: "CategoryId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Campaigns_Categories_CategoryId1",
                table: "Campaigns",
                column: "CategoryId1",
                principalTable: "Categories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentAttempts_Donations_DonationId1",
                table: "PaymentAttempts",
                column: "DonationId1",
                principalTable: "Donations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
