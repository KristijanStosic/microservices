using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DokumentService.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TipDokumenta",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NazivTipa = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipDokumenta", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Dokument",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ZavodniBroj = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Datum = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DatumDonosenjaDokumenta = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TipDokumentaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dokument", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Dokument_TipDokumenta_TipDokumentaId",
                        column: x => x.TipDokumentaId,
                        principalTable: "TipDokumenta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "TipDokumenta",
                columns: new[] { "Id", "NazivTipa" },
                values: new object[] { new Guid("77cbe573-4ad7-4db8-b9b1-7a9f45bec5db"), "Potvrda o statusu poljoprivrednog gazdinstava u registru poljoprivrednih gazdinstava" });

            migrationBuilder.InsertData(
                table: "TipDokumenta",
                columns: new[] { "Id", "NazivTipa" },
                values: new object[] { new Guid("3e649b2d-5569-4463-80f3-04382ef4a7eb"), "Izvod iz javne evidencije o nepokretnosti" });

            migrationBuilder.InsertData(
                table: "Dokument",
                columns: new[] { "Id", "Datum", "DatumDonosenjaDokumenta", "TipDokumentaId", "ZavodniBroj" },
                values: new object[] { new Guid("854a5603-31c6-4815-b393-a2b61887ba5e"), new DateTime(2022, 2, 7, 19, 14, 35, 236, DateTimeKind.Local).AddTicks(3207), new DateTime(2022, 2, 10, 19, 14, 35, 236, DateTimeKind.Local).AddTicks(3882), new Guid("77cbe573-4ad7-4db8-b9b1-7a9f45bec5db"), "PSPG-1/2022" });

            migrationBuilder.InsertData(
                table: "Dokument",
                columns: new[] { "Id", "Datum", "DatumDonosenjaDokumenta", "TipDokumentaId", "ZavodniBroj" },
                values: new object[] { new Guid("7147fd52-b938-4cec-8b81-dfa51f123f0c"), new DateTime(2022, 2, 7, 19, 14, 35, 236, DateTimeKind.Local).AddTicks(5126), new DateTime(2022, 2, 10, 19, 14, 35, 236, DateTimeKind.Local).AddTicks(5130), new Guid("77cbe573-4ad7-4db8-b9b1-7a9f45bec5db"), "PSPG-2/2022" });

            migrationBuilder.InsertData(
                table: "Dokument",
                columns: new[] { "Id", "Datum", "DatumDonosenjaDokumenta", "TipDokumentaId", "ZavodniBroj" },
                values: new object[] { new Guid("13b6c9df-03b9-4313-af6a-6c2e076e8a0a"), new DateTime(2022, 2, 7, 19, 14, 35, 236, DateTimeKind.Local).AddTicks(5100), new DateTime(2022, 2, 10, 19, 14, 35, 236, DateTimeKind.Local).AddTicks(5106), new Guid("3e649b2d-5569-4463-80f3-04382ef4a7eb"), "IJEN-1/2022" });

            migrationBuilder.CreateIndex(
                name: "IX_Dokument_TipDokumentaId",
                table: "Dokument",
                column: "TipDokumentaId");

            migrationBuilder.CreateIndex(
                name: "IX_Dokument_ZavodniBroj",
                table: "Dokument",
                column: "ZavodniBroj",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TipDokumenta_NazivTipa",
                table: "TipDokumenta",
                column: "NazivTipa",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Dokument");

            migrationBuilder.DropTable(
                name: "TipDokumenta");
        }
    }
}
