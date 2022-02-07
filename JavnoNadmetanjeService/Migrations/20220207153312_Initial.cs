using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace JavnoNadmetanjeService.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Status",
                columns: table => new
                {
                    StatusId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NazivStatusa = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Status", x => x.StatusId);
                });

            migrationBuilder.CreateTable(
                name: "Tip",
                columns: table => new
                {
                    TipId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NazivTipa = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tip", x => x.TipId);
                });

            migrationBuilder.CreateTable(
                name: "JavnoNadmetanje",
                columns: table => new
                {
                    JavnoNadmetanjeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PocetnaCenaHektar = table.Column<double>(type: "float", nullable: false),
                    VisinaDopuneDepozita = table.Column<int>(type: "int", nullable: false),
                    PeriodZakupa = table.Column<int>(type: "int", nullable: false),
                    IzlicitiranaCena = table.Column<int>(type: "int", nullable: false),
                    BrojUcesnika = table.Column<int>(type: "int", nullable: false),
                    Krug = table.Column<int>(type: "int", nullable: false),
                    Izuzeto = table.Column<bool>(type: "bit", nullable: false),
                    StatusId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TipId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    KupacId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AdresaId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JavnoNadmetanje", x => x.JavnoNadmetanjeId);
                    table.ForeignKey(
                        name: "FK_JavnoNadmetanje_Status_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Status",
                        principalColumn: "StatusId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JavnoNadmetanje_Tip_TipId",
                        column: x => x.TipId,
                        principalTable: "Tip",
                        principalColumn: "TipId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Etapa",
                columns: table => new
                {
                    EtapaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Datum = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DanPoRedu = table.Column<int>(type: "int", nullable: false),
                    VremePocetka = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VremeKraja = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ZavrsenaUspesno = table.Column<bool>(type: "bit", nullable: false),
                    JavnoNadmetanjeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Etapa", x => x.EtapaId);
                    table.ForeignKey(
                        name: "FK_Etapa_JavnoNadmetanje_JavnoNadmetanjeId",
                        column: x => x.JavnoNadmetanjeId,
                        principalTable: "JavnoNadmetanje",
                        principalColumn: "JavnoNadmetanjeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JavnoNadmetanjeDeoParcele",
                columns: table => new
                {
                    JavnoNadmetanjeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeoParceleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JavnoNadmetanjeDeoParcele", x => new { x.JavnoNadmetanjeId, x.DeoParceleId });
                    table.ForeignKey(
                        name: "FK_JavnoNadmetanjeDeoParcele_JavnoNadmetanje_JavnoNadmetanjeId",
                        column: x => x.JavnoNadmetanjeId,
                        principalTable: "JavnoNadmetanje",
                        principalColumn: "JavnoNadmetanjeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JavnoNadmetanjeKupac",
                columns: table => new
                {
                    JavnoNadmetanjeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    KupacId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JavnoNadmetanjeKupac", x => new { x.JavnoNadmetanjeId, x.KupacId });
                    table.ForeignKey(
                        name: "FK_JavnoNadmetanjeKupac_JavnoNadmetanje_JavnoNadmetanjeId",
                        column: x => x.JavnoNadmetanjeId,
                        principalTable: "JavnoNadmetanje",
                        principalColumn: "JavnoNadmetanjeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JavnoNadmetanjeOvlascenoLice",
                columns: table => new
                {
                    JavnoNadmetanjeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OvlascenoLiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JavnoNadmetanjeOvlascenoLice", x => new { x.JavnoNadmetanjeId, x.OvlascenoLiceId });
                    table.ForeignKey(
                        name: "FK_JavnoNadmetanjeOvlascenoLice_JavnoNadmetanje_JavnoNadmetanjeId",
                        column: x => x.JavnoNadmetanjeId,
                        principalTable: "JavnoNadmetanje",
                        principalColumn: "JavnoNadmetanjeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Status",
                columns: new[] { "StatusId", "NazivStatusa" },
                values: new object[,]
                {
                    { new Guid("8457e79a-d24a-4623-a34c-cde32435df23"), "Prvi krug" },
                    { new Guid("3b7ee65f-eb68-4a32-ae69-df7fdf463188"), "Drugi krug sa starim uslovima" },
                    { new Guid("1588803d-c08c-4d14-9343-954f6dc785e8"), "Drugi krug sa novim uslovima" }
                });

            migrationBuilder.InsertData(
                table: "Tip",
                columns: new[] { "TipId", "NazivTipa" },
                values: new object[,]
                {
                    { new Guid("d6d56b98-3672-4bdb-a0cb-e916ffe053c8"), "Javna licitacija" },
                    { new Guid("ab5b1fe9-d09f-471d-8e4c-e55ebd7e87b3"), "Otvaranje zatvorenih ponuda" }
                });

            migrationBuilder.InsertData(
                table: "JavnoNadmetanje",
                columns: new[] { "JavnoNadmetanjeId", "AdresaId", "BrojUcesnika", "IzlicitiranaCena", "Izuzeto", "Krug", "KupacId", "PeriodZakupa", "PocetnaCenaHektar", "StatusId", "TipId", "VisinaDopuneDepozita" },
                values: new object[] { new Guid("6849bbbe-5798-4c04-aa20-58de420aa578"), null, 15, 0, false, 1, null, 3, 550.0, new Guid("3b7ee65f-eb68-4a32-ae69-df7fdf463188"), new Guid("d6d56b98-3672-4bdb-a0cb-e916ffe053c8"), 150 });

            migrationBuilder.InsertData(
                table: "JavnoNadmetanje",
                columns: new[] { "JavnoNadmetanjeId", "AdresaId", "BrojUcesnika", "IzlicitiranaCena", "Izuzeto", "Krug", "KupacId", "PeriodZakupa", "PocetnaCenaHektar", "StatusId", "TipId", "VisinaDopuneDepozita" },
                values: new object[] { new Guid("b195c4c2-2b26-40ad-8ff6-c212474acc83"), new Guid("1c989ee3-13b2-4d3b-abeb-c4e6343eace7"), 7, 0, true, 5, null, 1, 1350.5, new Guid("8457e79a-d24a-4623-a34c-cde32435df23"), new Guid("d6d56b98-3672-4bdb-a0cb-e916ffe053c8"), 3 });

            migrationBuilder.InsertData(
                table: "JavnoNadmetanje",
                columns: new[] { "JavnoNadmetanjeId", "AdresaId", "BrojUcesnika", "IzlicitiranaCena", "Izuzeto", "Krug", "KupacId", "PeriodZakupa", "PocetnaCenaHektar", "StatusId", "TipId", "VisinaDopuneDepozita" },
                values: new object[] { new Guid("56a7cff5-cb0a-46b8-bfc1-93db415feeb4"), new Guid("1c989ee3-13b2-4d3b-abeb-c4e6343eace7"), 5, 400, false, 2, new Guid("febd1c29-90e7-40c2-97f3-1e88495fe98d"), 2, 350.5, new Guid("8457e79a-d24a-4623-a34c-cde32435df23"), new Guid("ab5b1fe9-d09f-471d-8e4c-e55ebd7e87b3"), 50 });

            migrationBuilder.InsertData(
                table: "Etapa",
                columns: new[] { "EtapaId", "DanPoRedu", "Datum", "JavnoNadmetanjeId", "VremeKraja", "VremePocetka", "ZavrsenaUspesno" },
                values: new object[,]
                {
                    { new Guid("0b410560-5868-4f34-8695-098bfc2b53e1"), 1, new DateTime(2022, 2, 12, 16, 33, 11, 451, DateTimeKind.Local).AddTicks(2685), new Guid("6849bbbe-5798-4c04-aa20-58de420aa578"), "18:33", "16:33", false },
                    { new Guid("baf25a2f-68af-4f36-a300-f4f78ba1d10c"), 1, new DateTime(2022, 2, 7, 16, 33, 11, 433, DateTimeKind.Local).AddTicks(4190), new Guid("56a7cff5-cb0a-46b8-bfc1-93db415feeb4"), "17:33", "16:33", false },
                    { new Guid("759775e2-7e4b-43d3-a0e5-395cb22f9bb5"), 2, new DateTime(2022, 2, 9, 16, 33, 11, 451, DateTimeKind.Local).AddTicks(2775), new Guid("56a7cff5-cb0a-46b8-bfc1-93db415feeb4"), "20:33", "16:33", true }
                });

            migrationBuilder.InsertData(
                table: "JavnoNadmetanjeDeoParcele",
                columns: new[] { "DeoParceleId", "JavnoNadmetanjeId" },
                values: new object[,]
                {
                    { new Guid("44302280-3611-4667-bcfc-08b4e272bb28"), new Guid("b195c4c2-2b26-40ad-8ff6-c212474acc83") },
                    { new Guid("17894615-ca22-4943-87c8-16c246a35879"), new Guid("56a7cff5-cb0a-46b8-bfc1-93db415feeb4") },
                    { new Guid("44302280-3611-4667-bcfc-08b4e272bb28"), new Guid("56a7cff5-cb0a-46b8-bfc1-93db415feeb4") }
                });

            migrationBuilder.InsertData(
                table: "JavnoNadmetanjeKupac",
                columns: new[] { "JavnoNadmetanjeId", "KupacId" },
                values: new object[,]
                {
                    { new Guid("b195c4c2-2b26-40ad-8ff6-c212474acc83"), new Guid("4ba95c01-aa89-4d36-a467-c72b0fcc5b80") },
                    { new Guid("56a7cff5-cb0a-46b8-bfc1-93db415feeb4"), new Guid("febd1c29-90e7-40c2-97f3-1e88495fe98d") },
                    { new Guid("56a7cff5-cb0a-46b8-bfc1-93db415feeb4"), new Guid("4ba95c01-aa89-4d36-a467-c72b0fcc5b80") }
                });

            migrationBuilder.InsertData(
                table: "JavnoNadmetanjeOvlascenoLice",
                columns: new[] { "JavnoNadmetanjeId", "OvlascenoLiceId" },
                values: new object[,]
                {
                    { new Guid("b195c4c2-2b26-40ad-8ff6-c212474acc83"), new Guid("5ed44cab-255d-4bb7-9cc9-828ec90bfaf5") },
                    { new Guid("56a7cff5-cb0a-46b8-bfc1-93db415feeb4"), new Guid("e22f999d-5c61-4dce-965b-9c6667efc74d") },
                    { new Guid("56a7cff5-cb0a-46b8-bfc1-93db415feeb4"), new Guid("5ed44cab-255d-4bb7-9cc9-828ec90bfaf5") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Etapa_JavnoNadmetanjeId",
                table: "Etapa",
                column: "JavnoNadmetanjeId");

            migrationBuilder.CreateIndex(
                name: "IX_JavnoNadmetanje_StatusId",
                table: "JavnoNadmetanje",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_JavnoNadmetanje_TipId",
                table: "JavnoNadmetanje",
                column: "TipId");

            migrationBuilder.CreateIndex(
                name: "IX_Status_NazivStatusa",
                table: "Status",
                column: "NazivStatusa",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tip_NazivTipa",
                table: "Tip",
                column: "NazivTipa",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Etapa");

            migrationBuilder.DropTable(
                name: "JavnoNadmetanjeDeoParcele");

            migrationBuilder.DropTable(
                name: "JavnoNadmetanjeKupac");

            migrationBuilder.DropTable(
                name: "JavnoNadmetanjeOvlascenoLice");

            migrationBuilder.DropTable(
                name: "JavnoNadmetanje");

            migrationBuilder.DropTable(
                name: "Status");

            migrationBuilder.DropTable(
                name: "Tip");
        }
    }
}
