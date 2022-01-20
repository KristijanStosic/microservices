using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ZalbaService.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RadnjaZaZalbu",
                columns: table => new
                {
                    RadnjaZaZalbuId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NazivRadnjeZaZalbu = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RadnjaZaZalbu", x => x.RadnjaZaZalbuId);
                });

            migrationBuilder.CreateTable(
                name: "StatusZalbe",
                columns: table => new
                {
                    StatusZalbeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NazivStatusaZalbe = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatusZalbe", x => x.StatusZalbeId);
                });

            migrationBuilder.CreateTable(
                name: "TipZalbe",
                columns: table => new
                {
                    TipZalbeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NazivTipaZalbe = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipZalbe", x => x.TipZalbeId);
                });

            migrationBuilder.CreateTable(
                name: "Zalba",
                columns: table => new
                {
                    ZalbaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DatumPodnosenja = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DatumResenja = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RazlogZalbe = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Obrazlozenje = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BrojNadmetanja = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BrojResenja = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StatusZalbeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TipZalbeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RadnjaZaZalbuId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zalba", x => x.ZalbaId);
                    table.ForeignKey(
                        name: "FK_Zalba_RadnjaZaZalbu_RadnjaZaZalbuId",
                        column: x => x.RadnjaZaZalbuId,
                        principalTable: "RadnjaZaZalbu",
                        principalColumn: "RadnjaZaZalbuId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Zalba_StatusZalbe_StatusZalbeId",
                        column: x => x.StatusZalbeId,
                        principalTable: "StatusZalbe",
                        principalColumn: "StatusZalbeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Zalba_TipZalbe_TipZalbeId",
                        column: x => x.TipZalbeId,
                        principalTable: "TipZalbe",
                        principalColumn: "TipZalbeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "RadnjaZaZalbu",
                columns: new[] { "RadnjaZaZalbuId", "NazivRadnjeZaZalbu" },
                values: new object[,]
                {
                    { new Guid("009aa493-7786-4aad-9d1a-0f90d57ebbb4"), "JN ide u drugi krug sa novim uslovima" },
                    { new Guid("4ccb6d66-18b2-4791-8afe-b628a4f7c0af"), "JN ide u drugi krug sa starim uslovima" },
                    { new Guid("df645dd7-3e65-41cd-a1f4-81f936a7db49"), "JN ne ide u drugi krug" }
                });

            migrationBuilder.InsertData(
                table: "StatusZalbe",
                columns: new[] { "StatusZalbeId", "NazivStatusaZalbe" },
                values: new object[,]
                {
                    { new Guid("6e5e8a67-006b-4ac0-89d0-9711006c0d28"), "Usvojena" },
                    { new Guid("abdb833b-0706-4012-8689-c59bed59c6b4"), "Odbijena" },
                    { new Guid("2c1051d2-0ddf-41a9-ba08-24070a50f4b3"), "Otvorena" }
                });

            migrationBuilder.InsertData(
                table: "TipZalbe",
                columns: new[] { "TipZalbeId", "NazivTipaZalbe" },
                values: new object[,]
                {
                    { new Guid("10ea64f1-07ab-478d-b8f3-073bce4610f8"), "Zalba na tok javnog nadmetanja" },
                    { new Guid("b4947a43-42e4-4d20-a10b-169c5089aac6"), "Zalba na Odluku o davanju u zakup" },
                    { new Guid("018ac715-4588-4934-bb2d-8bb2f4d1049a"), "Zalba na Odluku o davanju na koriscenje" }
                });

            migrationBuilder.InsertData(
                table: "Zalba",
                columns: new[] { "ZalbaId", "BrojNadmetanja", "BrojResenja", "DatumPodnosenja", "DatumResenja", "Obrazlozenje", "RadnjaZaZalbuId", "RazlogZalbe", "StatusZalbeId", "TipZalbeId" },
                values: new object[] { new Guid("d81e3601-21b1-40cf-a595-1331d0e5212f"), "200-OO", "IIKK-55", new DateTime(2022, 1, 20, 15, 31, 4, 205, DateTimeKind.Local).AddTicks(44), new DateTime(2022, 3, 11, 15, 31, 4, 205, DateTimeKind.Local).AddTicks(71), "Dokumentacija nije potpuna kako bi se odrzalo javno nadmetanje", new Guid("009aa493-7786-4aad-9d1a-0f90d57ebbb4"), "Nepotpuna dokumentacija", new Guid("6e5e8a67-006b-4ac0-89d0-9711006c0d28"), new Guid("b4947a43-42e4-4d20-a10b-169c5089aac6") });

            migrationBuilder.InsertData(
                table: "Zalba",
                columns: new[] { "ZalbaId", "BrojNadmetanja", "BrojResenja", "DatumPodnosenja", "DatumResenja", "Obrazlozenje", "RadnjaZaZalbuId", "RazlogZalbe", "StatusZalbeId", "TipZalbeId" },
                values: new object[] { new Guid("7951acaa-ae36-410d-916c-df398713811f"), "999-AA", "QWOP44-MM", new DateTime(2022, 1, 20, 15, 31, 4, 205, DateTimeKind.Local).AddTicks(114), new DateTime(2022, 3, 11, 15, 31, 4, 205, DateTimeKind.Local).AddTicks(119), "Nema dovoljno novcanih sredstava za javno nadmetanje", new Guid("4ccb6d66-18b2-4791-8afe-b628a4f7c0af"), "Nedovoljno uplacenih novcanih sredstava", new Guid("2c1051d2-0ddf-41a9-ba08-24070a50f4b3"), new Guid("b4947a43-42e4-4d20-a10b-169c5089aac6") });

            migrationBuilder.InsertData(
                table: "Zalba",
                columns: new[] { "ZalbaId", "BrojNadmetanja", "BrojResenja", "DatumPodnosenja", "DatumResenja", "Obrazlozenje", "RadnjaZaZalbuId", "RazlogZalbe", "StatusZalbeId", "TipZalbeId" },
                values: new object[] { new Guid("26313a1b-0523-4638-957e-c7bc3015cf76"), "100-NN", "X9NN41HH", new DateTime(2022, 1, 20, 15, 31, 4, 201, DateTimeKind.Local).AddTicks(6535), new DateTime(2022, 3, 11, 15, 31, 4, 204, DateTimeKind.Local).AddTicks(6983), "Nema dovoljno licitanata da se odrzi javno nadmetanje", new Guid("009aa493-7786-4aad-9d1a-0f90d57ebbb4"), "Nedovoljno licitanata", new Guid("6e5e8a67-006b-4ac0-89d0-9711006c0d28"), new Guid("018ac715-4588-4934-bb2d-8bb2f4d1049a") });

            migrationBuilder.CreateIndex(
                name: "IX_RadnjaZaZalbu_NazivRadnjeZaZalbu",
                table: "RadnjaZaZalbu",
                column: "NazivRadnjeZaZalbu",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StatusZalbe_NazivStatusaZalbe",
                table: "StatusZalbe",
                column: "NazivStatusaZalbe",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TipZalbe_NazivTipaZalbe",
                table: "TipZalbe",
                column: "NazivTipaZalbe",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Zalba_RadnjaZaZalbuId",
                table: "Zalba",
                column: "RadnjaZaZalbuId");

            migrationBuilder.CreateIndex(
                name: "IX_Zalba_StatusZalbeId",
                table: "Zalba",
                column: "StatusZalbeId");

            migrationBuilder.CreateIndex(
                name: "IX_Zalba_TipZalbeId",
                table: "Zalba",
                column: "TipZalbeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Zalba");

            migrationBuilder.DropTable(
                name: "RadnjaZaZalbu");

            migrationBuilder.DropTable(
                name: "StatusZalbe");

            migrationBuilder.DropTable(
                name: "TipZalbe");
        }
    }
}
