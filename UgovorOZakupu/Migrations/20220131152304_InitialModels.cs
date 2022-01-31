using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UgovorOZakupu.Migrations
{
    public partial class InitialModels : Migration
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
                    TipGarancijeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
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
                    UgovorOZakupuId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RokDospeca", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RokDospeca_UgovorOZakupu_UgovorOZakupuId",
                        column: x => x.UgovorOZakupuId,
                        principalTable: "UgovorOZakupu",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
