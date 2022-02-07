using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LicnostService.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Licnost",
                columns: table => new
                {
                    LicnostId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Ime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Prezime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Funkcija = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Licnost", x => x.LicnostId);
                });

            migrationBuilder.CreateTable(
                name: "Komisija",
                columns: table => new
                {
                    KomisijaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NazivKomisije = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PredsednikKomisijeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DokumentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Komisija", x => x.KomisijaId);
                    table.ForeignKey(
                        name: "FK_Komisija_Licnost_PredsednikKomisijeId",
                        column: x => x.PredsednikKomisijeId,
                        principalTable: "Licnost",
                        principalColumn: "LicnostId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "KomisijaLicnost",
                columns: table => new
                {
                    ClanoviKomisijeLicnostId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    KomisijeKomisijaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KomisijaLicnost", x => new { x.ClanoviKomisijeLicnostId, x.KomisijeKomisijaId });
                    table.ForeignKey(
                        name: "FK_KomisijaLicnost_Komisija_KomisijeKomisijaId",
                        column: x => x.KomisijeKomisijaId,
                        principalTable: "Komisija",
                        principalColumn: "KomisijaId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_KomisijaLicnost_Licnost_ClanoviKomisijeLicnostId",
                        column: x => x.ClanoviKomisijeLicnostId,
                        principalTable: "Licnost",
                        principalColumn: "LicnostId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Licnost",
                columns: new[] { "LicnostId", "Funkcija", "Ime", "Prezime" },
                values: new object[] { new Guid("95dbbc3b-b961-4364-9bf5-b6f2b4256393"), "Sef", "Stefan", "Bulaja" });

            migrationBuilder.InsertData(
                table: "Licnost",
                columns: new[] { "LicnostId", "Funkcija", "Ime", "Prezime" },
                values: new object[] { new Guid("4da64d71-ee63-4886-bcd3-fb7ae004a384"), "Licitant", "Marko", "Markovic" });

            migrationBuilder.InsertData(
                table: "Licnost",
                columns: new[] { "LicnostId", "Funkcija", "Ime", "Prezime" },
                values: new object[] { new Guid("e3db1e95-c4db-4e11-ac52-9b9e26207e1c"), "Licitant", "Pera", "Peric" });

            migrationBuilder.InsertData(
                table: "Komisija",
                columns: new[] { "KomisijaId", "DokumentId", "NazivKomisije", "PredsednikKomisijeId" },
                values: new object[] { new Guid("25410d44-6d96-486e-afd5-1409b906b3de"), null, "Subotica_Komisija", new Guid("95dbbc3b-b961-4364-9bf5-b6f2b4256393") });

            migrationBuilder.InsertData(
                table: "Komisija",
                columns: new[] { "KomisijaId", "DokumentId", "NazivKomisije", "PredsednikKomisijeId" },
                values: new object[] { new Guid("8b2e2f05-3796-444a-9dc2-2c62372c0fa9"), null, "Subotica_Komisija2", new Guid("4da64d71-ee63-4886-bcd3-fb7ae004a384") });

            migrationBuilder.InsertData(
                table: "Komisija",
                columns: new[] { "KomisijaId", "DokumentId", "NazivKomisije", "PredsednikKomisijeId" },
                values: new object[] { new Guid("289d3f53-8584-49e1-9511-d994a6dda9a9"), null, "Subotica_Komisija", new Guid("4da64d71-ee63-4886-bcd3-fb7ae004a384") });

            migrationBuilder.InsertData(
                table: "KomisijaLicnost",
                columns: new[] { "ClanoviKomisijeLicnostId", "KomisijeKomisijaId" },
                values: new object[] { new Guid("e3db1e95-c4db-4e11-ac52-9b9e26207e1c"), new Guid("25410d44-6d96-486e-afd5-1409b906b3de") });

            migrationBuilder.InsertData(
                table: "KomisijaLicnost",
                columns: new[] { "ClanoviKomisijeLicnostId", "KomisijeKomisijaId" },
                values: new object[] { new Guid("e3db1e95-c4db-4e11-ac52-9b9e26207e1c"), new Guid("289d3f53-8584-49e1-9511-d994a6dda9a9") });

            migrationBuilder.InsertData(
                table: "KomisijaLicnost",
                columns: new[] { "ClanoviKomisijeLicnostId", "KomisijeKomisijaId" },
                values: new object[] { new Guid("95dbbc3b-b961-4364-9bf5-b6f2b4256393"), new Guid("289d3f53-8584-49e1-9511-d994a6dda9a9") });

            migrationBuilder.CreateIndex(
                name: "IX_Komisija_PredsednikKomisijeId",
                table: "Komisija",
                column: "PredsednikKomisijeId");

            migrationBuilder.CreateIndex(
                name: "IX_KomisijaLicnost_KomisijeKomisijaId",
                table: "KomisijaLicnost",
                column: "KomisijeKomisijaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KomisijaLicnost");

            migrationBuilder.DropTable(
                name: "Komisija");

            migrationBuilder.DropTable(
                name: "Licnost");
        }
    }
}
