using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DocumentService.Migrations
{
    public partial class IntialModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TipDokumenta",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NazivTipa = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                    ZavodniBroj = table.Column<string>(type: "nvarchar(max)", nullable: true),
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

            migrationBuilder.CreateIndex(
                name: "IX_Dokument_TipDokumentaId",
                table: "Dokument",
                column: "TipDokumentaId");
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
