using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UplataService.Migrations
{
    public partial class KursVO : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SvrhaUplate",
                table: "Uplata",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "PozivNaBroj",
                table: "Uplata",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BrojRacuna",
                table: "Uplata",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Kurs_VrednostKursa",
                table: "Uplata",
                type: "float",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Uplata",
                keyColumn: "UplataId",
                keyValue: new Guid("8e297ad0-9072-4941-b951-5970eaed18f3"),
                column: "BrojRacuna",
                value: "100-4777487000005-66");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Kurs_VrednostKursa",
                table: "Uplata");

            migrationBuilder.AlterColumn<string>(
                name: "SvrhaUplate",
                table: "Uplata",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldDefaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "PozivNaBroj",
                table: "Uplata",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true,
                oldDefaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "BrojRacuna",
                table: "Uplata",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true,
                oldDefaultValue: "");

            migrationBuilder.UpdateData(
                table: "Uplata",
                keyColumn: "UplataId",
                keyValue: new Guid("8e297ad0-9072-4941-b951-5970eaed18f3"),
                column: "BrojRacuna",
                value: "Usvojena");
        }
    }
}
