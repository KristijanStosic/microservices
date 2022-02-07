using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ParcelaService.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KatastarskaOpstina",
                columns: table => new
                {
                    KatastarskaOpstinaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NazivKatastarskeOpstine = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KatastarskaOpstina", x => x.KatastarskaOpstinaId);
                });

            migrationBuilder.CreateTable(
                name: "Klasa",
                columns: table => new
                {
                    KlasaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    KlasaNaziv = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Klasa", x => x.KlasaId);
                });

            migrationBuilder.CreateTable(
                name: "Kultura",
                columns: table => new
                {
                    KulturaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NazivKulture = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kultura", x => x.KulturaId);
                });

            migrationBuilder.CreateTable(
                name: "OblikSvojine",
                columns: table => new
                {
                    OblikSvojineId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OpisOblikaSvojine = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OblikSvojine", x => x.OblikSvojineId);
                });

            migrationBuilder.CreateTable(
                name: "Obradivost",
                columns: table => new
                {
                    ObradivostId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OpisObradivosti = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Obradivost", x => x.ObradivostId);
                });

            migrationBuilder.CreateTable(
                name: "Odvodnjavanje",
                columns: table => new
                {
                    OdvodnjavanjeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OpisOdvodnjavanja = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Odvodnjavanje", x => x.OdvodnjavanjeId);
                });

            migrationBuilder.CreateTable(
                name: "ZasticenaZona",
                columns: table => new
                {
                    ZasticenaZonaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BrojZasticeneZone = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZasticenaZona", x => x.ZasticenaZonaId);
                });

            migrationBuilder.CreateTable(
                name: "Parcela",
                columns: table => new
                {
                    ParcelaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BrojParcele = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PovrsinaParcele = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BrojListeNepokretnosti = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ZasticenaZonaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OblikSvojineId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OdvodnjavanjeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    KatastarskaOpstinaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parcela", x => x.ParcelaId);
                    table.ForeignKey(
                        name: "FK_Parcela_KatastarskaOpstina_KatastarskaOpstinaId",
                        column: x => x.KatastarskaOpstinaId,
                        principalTable: "KatastarskaOpstina",
                        principalColumn: "KatastarskaOpstinaId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Parcela_OblikSvojine_OblikSvojineId",
                        column: x => x.OblikSvojineId,
                        principalTable: "OblikSvojine",
                        principalColumn: "OblikSvojineId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Parcela_Odvodnjavanje_OdvodnjavanjeId",
                        column: x => x.OdvodnjavanjeId,
                        principalTable: "Odvodnjavanje",
                        principalColumn: "OdvodnjavanjeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Parcela_ZasticenaZona_ZasticenaZonaId",
                        column: x => x.ZasticenaZonaId,
                        principalTable: "ZasticenaZona",
                        principalColumn: "ZasticenaZonaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeoParcele",
                columns: table => new
                {
                    DeoParceleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RedniBrojDela = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PovrsinaDela = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    KulturaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    KlasaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ObradivostId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ParcelaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    KupacId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeoParcele", x => x.DeoParceleId);
                    table.ForeignKey(
                        name: "FK_DeoParcele_Klasa_KlasaId",
                        column: x => x.KlasaId,
                        principalTable: "Klasa",
                        principalColumn: "KlasaId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DeoParcele_Kultura_KulturaId",
                        column: x => x.KulturaId,
                        principalTable: "Kultura",
                        principalColumn: "KulturaId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DeoParcele_Obradivost_ObradivostId",
                        column: x => x.ObradivostId,
                        principalTable: "Obradivost",
                        principalColumn: "ObradivostId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DeoParcele_Parcela_ParcelaId",
                        column: x => x.ParcelaId,
                        principalTable: "Parcela",
                        principalColumn: "ParcelaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "KatastarskaOpstina",
                columns: new[] { "KatastarskaOpstinaId", "NazivKatastarskeOpstine" },
                values: new object[,]
                {
                    { new Guid("9ef61f9a-a078-423c-96f3-c83bd2d1806f"), "Čantavir" },
                    { new Guid("2e101ea1-f68d-411c-934d-07970563474d"), "Bački Vinogradi" },
                    { new Guid("8772f89a-65ed-4240-95a8-cfd06775283b"), "Bikovo" },
                    { new Guid("ce8db946-e6a1-42e3-96b6-3ca9387a64c4"), "Đuđin" },
                    { new Guid("2fd6ea35-4bec-41a6-b705-87cb1fc4ff20"), "Žednik" },
                    { new Guid("e5ff87f4-6a6f-48ac-b3e4-2f324d2eb3cb"), "Tavankut" },
                    { new Guid("6905b37f-4449-41f4-8ac5-9e416dd17ff0"), "Bajmok" },
                    { new Guid("60e61296-b612-4950-b2d9-8766b82ebdb5"), "Donji Grad" },
                    { new Guid("571591d6-9d2c-4ca3-99f6-4f3d7df6863b"), "Stari Grad" },
                    { new Guid("78a28389-ef8f-4713-9640-ce047d99470e"), "Novi Grad" },
                    { new Guid("a8670b5a-41bc-4f90-9b37-6b78c06d2fd3"), "Palić" }
                });

            migrationBuilder.InsertData(
                table: "Klasa",
                columns: new[] { "KlasaId", "KlasaNaziv" },
                values: new object[,]
                {
                    { new Guid("0ec59e12-b271-471f-9a13-5c9c8ed0eda7"), "Druga" },
                    { new Guid("e496b563-abb9-48a9-8972-800f41a4a3a1"), "Treca" },
                    { new Guid("f39c9623-c6b6-48cb-b4dd-0340c7431870"), "Prva" }
                });

            migrationBuilder.InsertData(
                table: "Kultura",
                columns: new[] { "KulturaId", "NazivKulture" },
                values: new object[,]
                {
                    { new Guid("f32bf0e6-cb02-49b0-a035-79e350255742"), "Njiva" },
                    { new Guid("dcfc0f60-1683-4f16-919f-b6fbdf361fac"), "Vinograd" },
                    { new Guid("d89d9175-bdf0-4066-850a-4232318f80bb"), "Livada" }
                });

            migrationBuilder.InsertData(
                table: "OblikSvojine",
                columns: new[] { "OblikSvojineId", "OpisOblikaSvojine" },
                values: new object[,]
                {
                    { new Guid("f5133187-104c-4849-9d53-995e17e51094"), "Privatna svojina" },
                    { new Guid("03e3208e-eb61-40f1-b9d4-36fb3f63e4c6"), "Zajednicka svojina" }
                });

            migrationBuilder.InsertData(
                table: "Obradivost",
                columns: new[] { "ObradivostId", "OpisObradivosti" },
                values: new object[,]
                {
                    { new Guid("1c48c1d4-122b-4bd2-a8fe-188e54c5a88a"), "Moze se raditi redukovana obrada u odredjenoj meri" },
                    { new Guid("ec9e3d3e-193d-4de8-bdc4-acc3e4bf834d"), "Ne moze se raditi direktna setva" },
                    { new Guid("0d62386e-e188-49a9-a8e6-492fa14baeb4"), "Pogodno gajenje vecine kultura" }
                });

            migrationBuilder.InsertData(
                table: "Odvodnjavanje",
                columns: new[] { "OdvodnjavanjeId", "OpisOdvodnjavanja" },
                values: new object[,]
                {
                    { new Guid("b2f93ba3-ec00-41af-824e-fcd1f0c60c5c"), "Odvodnjavanje na parceli nije potrebno." },
                    { new Guid("d3c4ebaa-178c-4d62-afbc-05819e041021"), "Odvodnjavanje na parceli nije potrebno." },
                    { new Guid("6601b7a7-d1be-4216-9844-8d68d680847e"), "Odvodnjavanje na parceli nije potrebno." }
                });

            migrationBuilder.InsertData(
                table: "ZasticenaZona",
                columns: new[] { "ZasticenaZonaId", "BrojZasticeneZone" },
                values: new object[,]
                {
                    { new Guid("875fc830-5370-4d21-82bc-ceb8ce77e5d2"), "2" },
                    { new Guid("7663ebd7-8ad6-4e1a-9ed0-f0b3c07caa30"), "1" },
                    { new Guid("0bb57a3e-5ab2-4ab0-b33b-0e3c53d713d1"), "3" }
                });

            migrationBuilder.InsertData(
                table: "Parcela",
                columns: new[] { "ParcelaId", "BrojListeNepokretnosti", "BrojParcele", "KatastarskaOpstinaId", "OblikSvojineId", "OdvodnjavanjeId", "PovrsinaParcele", "ZasticenaZonaId" },
                values: new object[] { new Guid("5d99fd91-fcf4-4975-891b-a7ae3053ff52"), "8", "178", new Guid("9ef61f9a-a078-423c-96f3-c83bd2d1806f"), new Guid("f5133187-104c-4849-9d53-995e17e51094"), new Guid("b2f93ba3-ec00-41af-824e-fcd1f0c60c5c"), "250", new Guid("7663ebd7-8ad6-4e1a-9ed0-f0b3c07caa30") });

            migrationBuilder.InsertData(
                table: "Parcela",
                columns: new[] { "ParcelaId", "BrojListeNepokretnosti", "BrojParcele", "KatastarskaOpstinaId", "OblikSvojineId", "OdvodnjavanjeId", "PovrsinaParcele", "ZasticenaZonaId" },
                values: new object[] { new Guid("985eacda-be53-4506-a7b2-339fc6acf3d6"), "7", "289", new Guid("9ef61f9a-a078-423c-96f3-c83bd2d1806f"), new Guid("f5133187-104c-4849-9d53-995e17e51094"), new Guid("b2f93ba3-ec00-41af-824e-fcd1f0c60c5c"), "100", new Guid("7663ebd7-8ad6-4e1a-9ed0-f0b3c07caa30") });

            migrationBuilder.InsertData(
                table: "Parcela",
                columns: new[] { "ParcelaId", "BrojListeNepokretnosti", "BrojParcele", "KatastarskaOpstinaId", "OblikSvojineId", "OdvodnjavanjeId", "PovrsinaParcele", "ZasticenaZonaId" },
                values: new object[] { new Guid("73e47b70-c8fb-43e3-beb9-5f1b627a59bf"), "6", "158", new Guid("9ef61f9a-a078-423c-96f3-c83bd2d1806f"), new Guid("f5133187-104c-4849-9d53-995e17e51094"), new Guid("b2f93ba3-ec00-41af-824e-fcd1f0c60c5c"), "150", new Guid("875fc830-5370-4d21-82bc-ceb8ce77e5d2") });

            migrationBuilder.InsertData(
                table: "DeoParcele",
                columns: new[] { "DeoParceleId", "KlasaId", "KulturaId", "KupacId", "ObradivostId", "ParcelaId", "PovrsinaDela", "RedniBrojDela" },
                values: new object[] { new Guid("44302280-3611-4667-bcfc-08b4e272bb28"), new Guid("0ec59e12-b271-471f-9a13-5c9c8ed0eda7"), new Guid("dcfc0f60-1683-4f16-919f-b6fbdf361fac"), new Guid("4ba95c01-aa89-4d36-a467-c72b0fcc5b80"), new Guid("ec9e3d3e-193d-4de8-bdc4-acc3e4bf834d"), new Guid("5d99fd91-fcf4-4975-891b-a7ae3053ff52"), "15", "14" });

            migrationBuilder.InsertData(
                table: "DeoParcele",
                columns: new[] { "DeoParceleId", "KlasaId", "KulturaId", "KupacId", "ObradivostId", "ParcelaId", "PovrsinaDela", "RedniBrojDela" },
                values: new object[] { new Guid("c5bfe49c-518e-4216-a379-a5ec94b9efe2"), new Guid("e496b563-abb9-48a9-8972-800f41a4a3a1"), new Guid("d89d9175-bdf0-4066-850a-4232318f80bb"), new Guid("4ba95c01-aa89-4d36-a467-c72b0fcc5b80"), new Guid("0d62386e-e188-49a9-a8e6-492fa14baeb4"), new Guid("5d99fd91-fcf4-4975-891b-a7ae3053ff52"), "25", "20" });

            migrationBuilder.InsertData(
                table: "DeoParcele",
                columns: new[] { "DeoParceleId", "KlasaId", "KulturaId", "KupacId", "ObradivostId", "ParcelaId", "PovrsinaDela", "RedniBrojDela" },
                values: new object[] { new Guid("17894615-ca22-4943-87c8-16c246a35879"), new Guid("f39c9623-c6b6-48cb-b4dd-0340c7431870"), new Guid("f32bf0e6-cb02-49b0-a035-79e350255742"), new Guid("febd1c29-90e7-40c2-97f3-1e88495fe98d"), new Guid("1c48c1d4-122b-4bd2-a8fe-188e54c5a88a"), new Guid("73e47b70-c8fb-43e3-beb9-5f1b627a59bf"), "40", "12" });

            migrationBuilder.CreateIndex(
                name: "IX_DeoParcele_KlasaId",
                table: "DeoParcele",
                column: "KlasaId");

            migrationBuilder.CreateIndex(
                name: "IX_DeoParcele_KulturaId",
                table: "DeoParcele",
                column: "KulturaId");

            migrationBuilder.CreateIndex(
                name: "IX_DeoParcele_ObradivostId",
                table: "DeoParcele",
                column: "ObradivostId");

            migrationBuilder.CreateIndex(
                name: "IX_DeoParcele_ParcelaId",
                table: "DeoParcele",
                column: "ParcelaId");

            migrationBuilder.CreateIndex(
                name: "IX_Parcela_KatastarskaOpstinaId",
                table: "Parcela",
                column: "KatastarskaOpstinaId");

            migrationBuilder.CreateIndex(
                name: "IX_Parcela_OblikSvojineId",
                table: "Parcela",
                column: "OblikSvojineId");

            migrationBuilder.CreateIndex(
                name: "IX_Parcela_OdvodnjavanjeId",
                table: "Parcela",
                column: "OdvodnjavanjeId");

            migrationBuilder.CreateIndex(
                name: "IX_Parcela_ZasticenaZonaId",
                table: "Parcela",
                column: "ZasticenaZonaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeoParcele");

            migrationBuilder.DropTable(
                name: "Klasa");

            migrationBuilder.DropTable(
                name: "Kultura");

            migrationBuilder.DropTable(
                name: "Obradivost");

            migrationBuilder.DropTable(
                name: "Parcela");

            migrationBuilder.DropTable(
                name: "KatastarskaOpstina");

            migrationBuilder.DropTable(
                name: "OblikSvojine");

            migrationBuilder.DropTable(
                name: "Odvodnjavanje");

            migrationBuilder.DropTable(
                name: "ZasticenaZona");
        }
    }
}
