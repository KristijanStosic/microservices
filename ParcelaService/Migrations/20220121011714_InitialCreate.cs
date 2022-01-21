using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ParcelaService.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Klasa",
                columns: table => new
                {
                    ID_Klasa = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    KlasaNaziv = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Klasa", x => x.ID_Klasa);
                });

            migrationBuilder.InsertData(
                table: "Klasa",
                columns: new[] { "ID_Klasa", "KlasaNaziv" },
                values: new object[] { new Guid("f39c9623-c6b6-48cb-b4dd-0340c7431870"), "Prva" });

            migrationBuilder.InsertData(
                table: "Klasa",
                columns: new[] { "ID_Klasa", "KlasaNaziv" },
                values: new object[] { new Guid("0ec59e12-b271-471f-9a13-5c9c8ed0eda7"), "Druga" });

            migrationBuilder.InsertData(
                table: "Klasa",
                columns: new[] { "ID_Klasa", "KlasaNaziv" },
                values: new object[] { new Guid("e496b563-abb9-48a9-8972-800f41a4a3a1"), "Treca" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Klasa");
        }
    }
}
