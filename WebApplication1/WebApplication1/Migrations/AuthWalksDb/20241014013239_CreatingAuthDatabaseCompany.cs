using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebApplication1.Migrations.AuthWalksDb
{
    /// <inheritdoc />
    public partial class CreatingAuthDatabaseCompany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "22fb894e-ee49-4d03-8cc8-36ad6ac79eb3");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6da7b2c2-5bb2-42f2-9163-6a0cb0d46a41");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "AspNetUsers");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "8feaa11d-4858-412b-85e9-b9e547fe078f", "8feaa11d-4858-412b-85e9-b9e547fe078f", "Reader", "READER" },
                    { "e71a903d-66a4-48ab-9acf-4f07fc4dcd11", "e71a903d-66a4-48ab-9acf-4f07fc4dcd11", "Writer", "WRITER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8feaa11d-4858-412b-85e9-b9e547fe078f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e71a903d-66a4-48ab-9acf-4f07fc4dcd11");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "AspNetUsers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfBirth",
                table: "AspNetUsers",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "AspNetUsers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "22fb894e-ee49-4d03-8cc8-36ad6ac79eb3", "22fb894e-ee49-4d03-8cc8-36ad6ac79eb3", "Writer", "WRITER" },
                    { "6da7b2c2-5bb2-42f2-9163-6a0cb0d46a41", "6da7b2c2-5bb2-42f2-9163-6a0cb0d46a41", "Reader", "READER" }
                });
        }
    }
}
