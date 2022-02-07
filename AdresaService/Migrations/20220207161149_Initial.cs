using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AdresaService.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Drzave",
                columns: table => new
                {
                    DrzavaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NazivDrzave = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Drzave", x => x.DrzavaId);
                });

            migrationBuilder.CreateTable(
                name: "Adrese",
                columns: table => new
                {
                    AdresaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Ulica = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Broj = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Mesto = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostanskiBroj = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DrzavaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Adrese", x => x.AdresaId);
                    table.ForeignKey(
                        name: "FK_Adrese_Drzave_DrzavaId",
                        column: x => x.DrzavaId,
                        principalTable: "Drzave",
                        principalColumn: "DrzavaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Drzave",
                columns: new[] { "DrzavaId", "NazivDrzave" },
                values: new object[] { new Guid("f320743f-6c87-47ca-9f82-50191c1d31ac"), "Srbija" });

            migrationBuilder.InsertData(
                table: "Drzave",
                columns: new[] { "DrzavaId", "NazivDrzave" },
                values: new object[] { new Guid("9ce21ce2-7809-4e28-ba74-fd2f1bc6466a"), "Bosna i hercegovina" });

            migrationBuilder.InsertData(
                table: "Adrese",
                columns: new[] { "AdresaId", "Broj", "DrzavaId", "Mesto", "PostanskiBroj", "Ulica" },
                values: new object[] { new Guid("1c989ee3-13b2-4d3b-abeb-c4e6343eace7"), "1", new Guid("f320743f-6c87-47ca-9f82-50191c1d31ac"), "Novi Sad", "21000", "Branka Ilica" });

            migrationBuilder.InsertData(
                table: "Adrese",
                columns: new[] { "AdresaId", "Broj", "DrzavaId", "Mesto", "PostanskiBroj", "Ulica" },
                values: new object[] { new Guid("37371ef6-4f25-48b3-9bf2-fe72a81f88d2"), "23", new Guid("9ce21ce2-7809-4e28-ba74-fd2f1bc6466a"), "Bijeljna", "76300", "Solunska" });

            migrationBuilder.CreateIndex(
                name: "IX_Adrese_DrzavaId",
                table: "Adrese",
                column: "DrzavaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Adrese");

            migrationBuilder.DropTable(
                name: "Drzave");
        }
    }
}
