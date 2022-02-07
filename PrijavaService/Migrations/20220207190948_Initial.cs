using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PrijavaService.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Prijava",
                columns: table => new
                {
                    PrijavaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BrojPrijave = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DatumPrijave = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MestoPrijave = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SatPrijema = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ZatvorenaPonuda = table.Column<bool>(type: "bit", nullable: false),
                    KupacId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prijava", x => x.PrijavaId);
                });

            migrationBuilder.CreateTable(
                name: "DokFizickaLica",
                columns: table => new
                {
                    DokFizickaLicaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NazivDokumenta = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PrijavaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DokFizickaLica", x => x.DokFizickaLicaId);
                    table.ForeignKey(
                        name: "FK_DokFizickaLica_Prijava_PrijavaId",
                        column: x => x.PrijavaId,
                        principalTable: "Prijava",
                        principalColumn: "PrijavaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DokPravnaLica",
                columns: table => new
                {
                    DokPravnaLicaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NazivDokumenta = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PrijavaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DokPravnaLica", x => x.DokPravnaLicaId);
                    table.ForeignKey(
                        name: "FK_DokPravnaLica_Prijava_PrijavaId",
                        column: x => x.PrijavaId,
                        principalTable: "Prijava",
                        principalColumn: "PrijavaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PrijavaJavnoNadmetanje",
                columns: table => new
                {
                    PrijavaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    JavnoNadmetanjeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrijavaJavnoNadmetanje", x => new { x.PrijavaId, x.JavnoNadmetanjeId });
                    table.ForeignKey(
                        name: "FK_PrijavaJavnoNadmetanje_Prijava_PrijavaId",
                        column: x => x.PrijavaId,
                        principalTable: "Prijava",
                        principalColumn: "PrijavaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Prijava",
                columns: new[] { "PrijavaId", "BrojPrijave", "DatumPrijave", "KupacId", "MestoPrijave", "SatPrijema", "ZatvorenaPonuda" },
                values: new object[] { new Guid("3040da81-b4b5-47bd-a47c-f1474341f162"), "B22", new DateTime(2022, 2, 7, 20, 9, 47, 936, DateTimeKind.Local).AddTicks(1384), new Guid("febd1c29-90e7-40c2-97f3-1e88495fe98d"), "Mesto 1", null, true });

            migrationBuilder.InsertData(
                table: "Prijava",
                columns: new[] { "PrijavaId", "BrojPrijave", "DatumPrijave", "KupacId", "MestoPrijave", "SatPrijema", "ZatvorenaPonuda" },
                values: new object[] { new Guid("a370bc58-2cb2-4d8d-9cfb-b444841aeb80"), "B255", new DateTime(2022, 2, 7, 20, 9, 47, 957, DateTimeKind.Local).AddTicks(3059), new Guid("febd1c29-90e7-40c2-97f3-1e88495fe98d"), "Mesto 2", null, false });

            migrationBuilder.InsertData(
                table: "DokFizickaLica",
                columns: new[] { "DokFizickaLicaId", "NazivDokumenta", "PrijavaId" },
                values: new object[,]
                {
                    { new Guid("522de090-3873-4113-821c-c000f6ef7ae5"), "Dokument FL 1", new Guid("a370bc58-2cb2-4d8d-9cfb-b444841aeb80") },
                    { new Guid("dd8e0e7b-0c07-45e8-a1ad-1517853e8940"), "Dokument FL 2", new Guid("a370bc58-2cb2-4d8d-9cfb-b444841aeb80") }
                });

            migrationBuilder.InsertData(
                table: "DokPravnaLica",
                columns: new[] { "DokPravnaLicaId", "NazivDokumenta", "PrijavaId" },
                values: new object[,]
                {
                    { new Guid("6edac589-e1f6-4733-b9d3-da55256ee707"), "Dokument PL 1", new Guid("3040da81-b4b5-47bd-a47c-f1474341f162") },
                    { new Guid("de1f2617-6def-48c1-a523-218a2b062916"), "Dokument PL 2", new Guid("3040da81-b4b5-47bd-a47c-f1474341f162") }
                });

            migrationBuilder.InsertData(
                table: "PrijavaJavnoNadmetanje",
                columns: new[] { "JavnoNadmetanjeId", "PrijavaId" },
                values: new object[,]
                {
                    { new Guid("e22f999d-5c61-4dce-965b-9c6667efc74d"), new Guid("3040da81-b4b5-47bd-a47c-f1474341f162") },
                    { new Guid("5ed44cab-255d-4bb7-9cc9-828ec90bfaf5"), new Guid("3040da81-b4b5-47bd-a47c-f1474341f162") },
                    { new Guid("5ed44cab-255d-4bb7-9cc9-828ec90bfaf5"), new Guid("a370bc58-2cb2-4d8d-9cfb-b444841aeb80") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_DokFizickaLica_PrijavaId",
                table: "DokFizickaLica",
                column: "PrijavaId");

            migrationBuilder.CreateIndex(
                name: "IX_DokPravnaLica_PrijavaId",
                table: "DokPravnaLica",
                column: "PrijavaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DokFizickaLica");

            migrationBuilder.DropTable(
                name: "DokPravnaLica");

            migrationBuilder.DropTable(
                name: "PrijavaJavnoNadmetanje");

            migrationBuilder.DropTable(
                name: "Prijava");
        }
    }
}
