using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OvlascenoLiceService.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OvlascenoLice",
                columns: table => new
                {
                    OvlascenoLiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Ime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Prezime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JMBG = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: true),
                    BrojPasosa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdresaId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DrzavaId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OvlascenoLice", x => x.OvlascenoLiceId);
                });

            migrationBuilder.CreateTable(
                name: "BrojTable",
                columns: table => new
                {
                    BrojTableId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RbTable = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OznakaTable = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OvlascenoLiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrojTable", x => x.BrojTableId);
                    table.ForeignKey(
                        name: "FK_BrojTable_OvlascenoLice_OvlascenoLiceId",
                        column: x => x.OvlascenoLiceId,
                        principalTable: "OvlascenoLice",
                        principalColumn: "OvlascenoLiceId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "OvlascenoLice",
                columns: new[] { "OvlascenoLiceId", "AdresaId", "BrojPasosa", "DrzavaId", "Ime", "JMBG", "Prezime" },
                values: new object[] { new Guid("5ed44cab-255d-4bb7-9cc9-828ec90bfaf5"), new Guid("1c989ee3-13b2-4d3b-abeb-c4e6343eace7"), null, null, "Petar", "1002987800035", "Petrovic" });

            migrationBuilder.InsertData(
                table: "OvlascenoLice",
                columns: new[] { "OvlascenoLiceId", "AdresaId", "BrojPasosa", "DrzavaId", "Ime", "JMBG", "Prezime" },
                values: new object[] { new Guid("5e1bfffc-1aee-4662-bc04-341c35b9ebdc"), new Guid("37371ef6-4f25-48b3-9bf2-fe72a81f88d2"), null, null, "Marko", "2004983800022", "Markovic" });

            migrationBuilder.InsertData(
                table: "OvlascenoLice",
                columns: new[] { "OvlascenoLiceId", "AdresaId", "BrojPasosa", "DrzavaId", "Ime", "JMBG", "Prezime" },
                values: new object[] { new Guid("e22f999d-5c61-4dce-965b-9c6667efc74d"), null, "0252624", new Guid("9ce21ce2-7809-4e28-ba74-fd2f1bc6466a"), "Nemanja", null, "Nenic" });

            migrationBuilder.InsertData(
                table: "BrojTable",
                columns: new[] { "BrojTableId", "OvlascenoLiceId", "OznakaTable" },
                values: new object[] { new Guid("33dbd6be-207b-43a5-ab4f-65aa47c6ee3c"), new Guid("5e1bfffc-1aee-4662-bc04-341c35b9ebdc"), "Tabla25" });

            migrationBuilder.InsertData(
                table: "BrojTable",
                columns: new[] { "BrojTableId", "OvlascenoLiceId", "OznakaTable" },
                values: new object[] { new Guid("f7837323-afeb-4aa9-ba28-cd0912bb1fac"), new Guid("e22f999d-5c61-4dce-965b-9c6667efc74d"), "Talba3" });

            migrationBuilder.InsertData(
                table: "BrojTable",
                columns: new[] { "BrojTableId", "OvlascenoLiceId", "OznakaTable" },
                values: new object[] { new Guid("97a9c3ec-6af3-4b02-ad18-097702f62fa0"), new Guid("e22f999d-5c61-4dce-965b-9c6667efc74d"), "Tabla12" });

            migrationBuilder.CreateIndex(
                name: "IX_BrojTable_OvlascenoLiceId",
                table: "BrojTable",
                column: "OvlascenoLiceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BrojTable");

            migrationBuilder.DropTable(
                name: "OvlascenoLice");
        }
    }
}
