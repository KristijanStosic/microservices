using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UgovorOZakupu.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TipGarancije",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NazivTipa = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipGarancije", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UgovorOZakupu",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ZavodniBroj = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DatumZavodjenja = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RokZaVracanje = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MestoPotpisivanja = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DatumPotpisivanja = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TipGarancijeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DokumentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    JavnoNadmetanjeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    KupacId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LicnostId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UgovorOZakupu", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UgovorOZakupu_TipGarancije_TipGarancijeId",
                        column: x => x.TipGarancijeId,
                        principalTable: "TipGarancije",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RokDospeca",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Rok = table.Column<int>(type: "int", nullable: false),
                    UgovorOZakupuId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RokDospeca", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RokDospeca_UgovorOZakupu_UgovorOZakupuId",
                        column: x => x.UgovorOZakupuId,
                        principalTable: "UgovorOZakupu",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "TipGarancije",
                columns: new[] { "Id", "NazivTipa" },
                values: new object[] { new Guid("3085ea9a-ea2a-4577-9e03-467812d8ef56"), "Činidbena garancija" });

            migrationBuilder.InsertData(
                table: "TipGarancije",
                columns: new[] { "Id", "NazivTipa" },
                values: new object[] { new Guid("be38618a-783c-4850-af91-9645105f54f8"), "Avansna garancija" });

            migrationBuilder.InsertData(
                table: "UgovorOZakupu",
                columns: new[] { "Id", "DatumPotpisivanja", "DatumZavodjenja", "DokumentId", "JavnoNadmetanjeId", "KupacId", "LicnostId", "MestoPotpisivanja", "RokZaVracanje", "TipGarancijeId", "ZavodniBroj" },
                values: new object[] { new Guid("ea9e8d12-0af9-4926-b022-fd1bd951a3b6"), new DateTime(2022, 2, 4, 20, 18, 2, 896, DateTimeKind.Local).AddTicks(3068), new DateTime(2022, 2, 4, 20, 21, 2, 891, DateTimeKind.Local).AddTicks(5899), new Guid("854a5603-31c6-4815-b393-a2b61887ba5e"), new Guid("56a7cff5-cb0a-46b8-bfc1-93db415feeb4"), new Guid("4ba95c01-aa89-4d36-a467-c72b0fcc5b80"), new Guid("95dbbc3b-b961-4364-9bf5-b6f2b4256393"), "Subotica", new DateTime(2022, 2, 18, 20, 21, 2, 896, DateTimeKind.Local).AddTicks(1641), new Guid("3085ea9a-ea2a-4577-9e03-467812d8ef56"), "UZ1/02-2022" });

            migrationBuilder.InsertData(
                table: "UgovorOZakupu",
                columns: new[] { "Id", "DatumPotpisivanja", "DatumZavodjenja", "DokumentId", "JavnoNadmetanjeId", "KupacId", "LicnostId", "MestoPotpisivanja", "RokZaVracanje", "TipGarancijeId", "ZavodniBroj" },
                values: new object[] { new Guid("a0dcd0f0-a6b2-4564-922a-b54880363c39"), new DateTime(2022, 2, 7, 20, 18, 2, 896, DateTimeKind.Local).AddTicks(6588), new DateTime(2022, 2, 7, 20, 20, 2, 896, DateTimeKind.Local).AddTicks(6554), new Guid("13b6c9df-03b9-4313-af6a-6c2e076e8a0a"), new Guid("6849bbbe-5798-4c04-aa20-58de420aa578"), new Guid("4ba95c01-aa89-4d36-a467-c72b0fcc5b80"), new Guid("4da64d71-ee63-4886-bcd3-fb7ae004a384"), "Novi Sad", new DateTime(2022, 2, 20, 20, 21, 2, 896, DateTimeKind.Local).AddTicks(6582), new Guid("3085ea9a-ea2a-4577-9e03-467812d8ef56"), "UZ2/02-2022" });

            migrationBuilder.InsertData(
                table: "UgovorOZakupu",
                columns: new[] { "Id", "DatumPotpisivanja", "DatumZavodjenja", "DokumentId", "JavnoNadmetanjeId", "KupacId", "LicnostId", "MestoPotpisivanja", "RokZaVracanje", "TipGarancijeId", "ZavodniBroj" },
                values: new object[] { new Guid("42988239-b116-4100-bd15-06869c0d6fba"), new DateTime(2022, 2, 7, 20, 24, 2, 896, DateTimeKind.Local).AddTicks(6628), new DateTime(2022, 2, 7, 20, 26, 2, 896, DateTimeKind.Local).AddTicks(6618), new Guid("7147fd52-b938-4cec-8b81-dfa51f123f0c"), new Guid("b195c4c2-2b26-40ad-8ff6-c212474acc83"), new Guid("4ba95c01-aa89-4d36-a467-c72b0fcc5b80"), new Guid("e3db1e95-c4db-4e11-ac52-9b9e26207e1c"), "Beograd", new DateTime(2022, 2, 21, 20, 21, 2, 896, DateTimeKind.Local).AddTicks(6623), new Guid("be38618a-783c-4850-af91-9645105f54f8"), "UZ3/02-2022" });

            migrationBuilder.InsertData(
                table: "RokDospeca",
                columns: new[] { "Id", "Rok", "UgovorOZakupuId" },
                values: new object[,]
                {
                    { new Guid("5cc7b507-9fc7-4d71-9a5a-198e77ead9d3"), 3, new Guid("ea9e8d12-0af9-4926-b022-fd1bd951a3b6") },
                    { new Guid("999f0824-0de9-4c1b-9951-3477c3808f3c"), 6, new Guid("ea9e8d12-0af9-4926-b022-fd1bd951a3b6") },
                    { new Guid("99866822-1176-485b-9561-5f4e210922fe"), 7, new Guid("a0dcd0f0-a6b2-4564-922a-b54880363c39") },
                    { new Guid("b47767a5-34d4-4b46-90a1-dfc7baa67823"), 10, new Guid("42988239-b116-4100-bd15-06869c0d6fba") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_RokDospeca_UgovorOZakupuId",
                table: "RokDospeca",
                column: "UgovorOZakupuId");

            migrationBuilder.CreateIndex(
                name: "IX_UgovorOZakupu_TipGarancijeId",
                table: "UgovorOZakupu",
                column: "TipGarancijeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RokDospeca");

            migrationBuilder.DropTable(
                name: "UgovorOZakupu");

            migrationBuilder.DropTable(
                name: "TipGarancije");
        }
    }
}
