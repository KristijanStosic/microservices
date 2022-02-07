using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KupacService.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KontaktOsobe",
                columns: table => new
                {
                    KontaktOsobaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Ime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Prezime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Telefon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Funkcija = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KontaktOsobe", x => x.KontaktOsobaId);
                });

            migrationBuilder.CreateTable(
                name: "Kupci",
                columns: table => new
                {
                    KupacId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OstvarenaPovrsina = table.Column<double>(type: "float", nullable: false),
                    ImaZabranu = table.Column<bool>(type: "bit", nullable: false),
                    DatumPocetkaZabrane = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DuzinaTrajanjaZabraneGod = table.Column<int>(type: "int", nullable: false),
                    BrojTelefona = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BrojTelefona2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BrojRacuna = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdresaId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kupci", x => x.KupacId);
                });

            migrationBuilder.CreateTable(
                name: "Prioriteti",
                columns: table => new
                {
                    PrioritetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Opis = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prioriteti", x => x.PrioritetId);
                });

            migrationBuilder.CreateTable(
                name: "FizickoLice",
                columns: table => new
                {
                    KupacId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Ime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Prezime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JMBG = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FizickoLice", x => x.KupacId);
                    table.ForeignKey(
                        name: "FK_FizickoLice_Kupci_KupacId",
                        column: x => x.KupacId,
                        principalTable: "Kupci",
                        principalColumn: "KupacId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "kupacOvlascenoLice",
                columns: table => new
                {
                    KupacId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OvlascenoLiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_kupacOvlascenoLice", x => new { x.KupacId, x.OvlascenoLiceId });
                    table.ForeignKey(
                        name: "FK_kupacOvlascenoLice_Kupci_KupacId",
                        column: x => x.KupacId,
                        principalTable: "Kupci",
                        principalColumn: "KupacId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "kupacUplata",
                columns: table => new
                {
                    UplataId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    KupacId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_kupacUplata", x => x.UplataId);
                    table.ForeignKey(
                        name: "FK_kupacUplata_Kupci_KupacId",
                        column: x => x.KupacId,
                        principalTable: "Kupci",
                        principalColumn: "KupacId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PravnoLice",
                columns: table => new
                {
                    KupacId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Naziv = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaticniBroj = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Faks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KontaktOsobaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PravnoLice", x => x.KupacId);
                    table.ForeignKey(
                        name: "FK_PravnoLice_KontaktOsobe_KontaktOsobaId",
                        column: x => x.KontaktOsobaId,
                        principalTable: "KontaktOsobe",
                        principalColumn: "KontaktOsobaId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PravnoLice_Kupci_KupacId",
                        column: x => x.KupacId,
                        principalTable: "Kupci",
                        principalColumn: "KupacId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "KupacPrioritet",
                columns: table => new
                {
                    KupacId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PrioritetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KupacPrioritet", x => new { x.KupacId, x.PrioritetId });
                    table.ForeignKey(
                        name: "FK_KupacPrioritet_Kupci_KupacId",
                        column: x => x.KupacId,
                        principalTable: "Kupci",
                        principalColumn: "KupacId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KupacPrioritet_Prioriteti_PrioritetId",
                        column: x => x.PrioritetId,
                        principalTable: "Prioriteti",
                        principalColumn: "PrioritetId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "KontaktOsobe",
                columns: new[] { "KontaktOsobaId", "Funkcija", "Ime", "Prezime", "Telefon" },
                values: new object[,]
                {
                    { new Guid("244fb7c4-aab8-4ec4-8960-e48e017bad37"), "Funkcija1", "Milan", "Drazic", "0693432534" },
                    { new Guid("da2197a4-891f-4a40-a1f2-313962701627"), "Funkcija2", "Mark", "Todor", "0693432534" }
                });

            migrationBuilder.InsertData(
                table: "Kupci",
                columns: new[] { "KupacId", "AdresaId", "BrojRacuna", "BrojTelefona", "BrojTelefona2", "DatumPocetkaZabrane", "DuzinaTrajanjaZabraneGod", "Email", "ImaZabranu", "OstvarenaPovrsina" },
                values: new object[,]
                {
                    { new Guid("febd1c29-90e7-40c2-97f3-1e88495fe98d"), new Guid("1c989ee3-13b2-4d3b-abeb-c4e6343eace7"), "908 ‑ 10501 ‑ 97", "069453432543", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, "filip@gmail.com", false, 500.0 },
                    { new Guid("4ba95c01-aa89-4d36-a467-c72b0fcc5b80"), null, "934 ‑ 10501 ‑ 97", "069453232543", null, new DateTime(2022, 2, 7, 19, 28, 20, 899, DateTimeKind.Local).AddTicks(996), 2, "Firma@gmail.com", true, 200.0 }
                });

            migrationBuilder.InsertData(
                table: "Prioriteti",
                columns: new[] { "PrioritetId", "Opis" },
                values: new object[,]
                {
                    { new Guid("2578e81b-3f01-479a-b790-f52106f639f7"), "Vlasnik sistema za navodnjavanje" },
                    { new Guid("f2b8faa4-732c-4480-8b0a-34d65e483930"), "Vlasnik zemljišta koje se graniči sazemljištem koje se daje u zakup" }
                });

            migrationBuilder.InsertData(
                table: "FizickoLice",
                columns: new[] { "KupacId", "Ime", "JMBG", "Prezime" },
                values: new object[] { new Guid("febd1c29-90e7-40c2-97f3-1e88495fe98d"), "Filip", "1253627363526", "Ivanic" });

            migrationBuilder.InsertData(
                table: "KupacPrioritet",
                columns: new[] { "KupacId", "PrioritetId" },
                values: new object[] { new Guid("febd1c29-90e7-40c2-97f3-1e88495fe98d"), new Guid("2578e81b-3f01-479a-b790-f52106f639f7") });

            migrationBuilder.InsertData(
                table: "PravnoLice",
                columns: new[] { "KupacId", "Faks", "KontaktOsobaId", "MaticniBroj", "Naziv" },
                values: new object[] { new Guid("4ba95c01-aa89-4d36-a467-c72b0fcc5b80"), "+1-212-9876543", new Guid("da2197a4-891f-4a40-a1f2-313962701627"), "1254327363526", "Firma" });

            migrationBuilder.InsertData(
                table: "kupacOvlascenoLice",
                columns: new[] { "KupacId", "OvlascenoLiceId" },
                values: new object[,]
                {
                    { new Guid("febd1c29-90e7-40c2-97f3-1e88495fe98d"), new Guid("5ed44cab-255d-4bb7-9cc9-828ec90bfaf5") },
                    { new Guid("4ba95c01-aa89-4d36-a467-c72b0fcc5b80"), new Guid("5ed44cab-255d-4bb7-9cc9-828ec90bfaf5") },
                    { new Guid("4ba95c01-aa89-4d36-a467-c72b0fcc5b80"), new Guid("5e1bfffc-1aee-4662-bc04-341c35b9ebdc") }
                });

            migrationBuilder.InsertData(
                table: "kupacUplata",
                columns: new[] { "UplataId", "KupacId" },
                values: new object[,]
                {
                    { new Guid("5ed44cab-255d-4bb7-9cc9-828ec90bfaf5"), new Guid("febd1c29-90e7-40c2-97f3-1e88495fe98d") },
                    { new Guid("5e1bfffc-1aee-4662-bc04-341c35b9ebdc"), new Guid("4ba95c01-aa89-4d36-a467-c72b0fcc5b80") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_KupacPrioritet_PrioritetId",
                table: "KupacPrioritet",
                column: "PrioritetId");

            migrationBuilder.CreateIndex(
                name: "IX_kupacUplata_KupacId",
                table: "kupacUplata",
                column: "KupacId");

            migrationBuilder.CreateIndex(
                name: "IX_PravnoLice_KontaktOsobaId",
                table: "PravnoLice",
                column: "KontaktOsobaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FizickoLice");

            migrationBuilder.DropTable(
                name: "kupacOvlascenoLice");

            migrationBuilder.DropTable(
                name: "KupacPrioritet");

            migrationBuilder.DropTable(
                name: "kupacUplata");

            migrationBuilder.DropTable(
                name: "PravnoLice");

            migrationBuilder.DropTable(
                name: "Prioriteti");

            migrationBuilder.DropTable(
                name: "KontaktOsobe");

            migrationBuilder.DropTable(
                name: "Kupci");
        }
    }
}
