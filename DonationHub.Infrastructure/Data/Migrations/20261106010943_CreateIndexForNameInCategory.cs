using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DonationHub.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateIndexForNameInCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NameEn",
                table: "Categories",
                newName: "Name_NameEn");

            migrationBuilder.RenameColumn(
                name: "NameAr",
                table: "Categories",
                newName: "Name_NameAr");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Name_NameAr_Name_NameEn",
                table: "Categories",
                columns: new[] { "Name_NameAr", "Name_NameEn" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Categories_Name_NameAr_Name_NameEn",
                table: "Categories");

            migrationBuilder.RenameColumn(
                name: "Name_NameEn",
                table: "Categories",
                newName: "NameEn");

            migrationBuilder.RenameColumn(
                name: "Name_NameAr",
                table: "Categories",
                newName: "NameAr");
        }
    }
}
