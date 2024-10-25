using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class _1stMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Lenghth",
                table: "Regions");

            migrationBuilder.RenameColumn(
                name: "RegionImageUrl",
                table: "Regions",
                newName: "ImageUrl");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Regions",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                table: "Regions");

            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "Regions",
                newName: "RegionImageUrl");

            migrationBuilder.AddColumn<double>(
                name: "Lenghth",
                table: "Regions",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
