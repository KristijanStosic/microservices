using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UplataService.Migrations
{
    public partial class Uplata : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Uplata",
                columns: table => new
                {
                    UplataId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BrojRacuna = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PozivNaBroj = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Iznos = table.Column<double>(type: "float", nullable: false),
                    SvrhaUplate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DatumUplate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    JavnoNadmetanjeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Uplata", x => x.UplataId);
                });

            migrationBuilder.InsertData(
                table: "Uplata",
                columns: new[] { "UplataId", "BrojRacuna", "DatumUplate", "Iznos", "JavnoNadmetanjeId", "PozivNaBroj", "SvrhaUplate" },
                values: new object[] { new Guid("8e297ad0-9072-4941-b951-5970eaed18f3"), "Usvojena", new DateTime(2020, 5, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), 1999.99, null, "90-555258-552-559", "Uplata na racun" });

            migrationBuilder.InsertData(
                table: "Uplata",
                columns: new[] { "UplataId", "BrojRacuna", "DatumUplate", "Iznos", "JavnoNadmetanjeId", "PozivNaBroj", "SvrhaUplate" },
                values: new object[] { new Guid("19d28646-6779-4896-a9ff-6e7b7b70d87a"), "150-2541485965214-99", new DateTime(2020, 5, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), 2052.4699999999998, null, "90-555258-552-559", "Uplata na racun" });

            migrationBuilder.InsertData(
                table: "Uplata",
                columns: new[] { "UplataId", "BrojRacuna", "DatumUplate", "Iznos", "JavnoNadmetanjeId", "PozivNaBroj", "SvrhaUplate" },
                values: new object[] { new Guid("633140e2-3fbc-4402-bd4d-ec6e06ec6627"), "150-3333385965214-99", new DateTime(2020, 5, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), 8482.9899999999998, null, "90-555258-552-559", "Uplata na racun" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Uplata");
        }
    }
}
