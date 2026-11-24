using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DonationHub.Infrastructure.Data.Configurations
{
    /// <inheritdoc />
    public partial class FixNamesCoulmnInCategory_Name : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name_NameEn",
                table: "Categories",
                newName: "NameEn");

            migrationBuilder.RenameColumn(
                name: "Name_NameAr",
                table: "Categories",
                newName: "NameAr");

            migrationBuilder.RenameIndex(
                name: "IX_Categories_Name_NameEn",
                table: "Categories",
                newName: "IX_Categories_NameEn");

            migrationBuilder.RenameIndex(
                name: "IX_Categories_Name_NameAr",
                table: "Categories",
                newName: "IX_Categories_NameAr");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NameEn",
                table: "Categories",
                newName: "Name_NameEn");

            migrationBuilder.RenameColumn(
                name: "NameAr",
                table: "Categories",
                newName: "Name_NameAr");

            migrationBuilder.RenameIndex(
                name: "IX_Categories_NameEn",
                table: "Categories",
                newName: "IX_Categories_Name_NameEn");

            migrationBuilder.RenameIndex(
                name: "IX_Categories_NameAr",
                table: "Categories",
                newName: "IX_Categories_Name_NameAr");
        }
    }
}
