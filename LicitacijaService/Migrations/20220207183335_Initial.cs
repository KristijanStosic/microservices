using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LicitacijaService.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Licitacija",
                columns: table => new
                {
                    LicitacijaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BrojLicitacije = table.Column<int>(type: "int", nullable: false),
                    GodinaLicitacije = table.Column<int>(type: "int", nullable: false),
                    OgranicenjeLicitacije = table.Column<int>(type: "int", nullable: false),
                    RokLicitacije = table.Column<DateTime>(type: "datetime2", nullable: false),
                    KorakCeneLicitacije = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Licitacija", x => x.LicitacijaId);
                });

            migrationBuilder.CreateTable(
                name: "LicitacijaJavnoNadmetanje",
                columns: table => new
                {
                    LicitacijaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    JavnoNadmetanjeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LicitacijaJavnoNadmetanje", x => new { x.LicitacijaId, x.JavnoNadmetanjeId });
                    table.ForeignKey(
                        name: "FK_LicitacijaJavnoNadmetanje_Licitacija_LicitacijaId",
                        column: x => x.LicitacijaId,
                        principalTable: "Licitacija",
                        principalColumn: "LicitacijaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Licitacija",
                columns: new[] { "LicitacijaId", "BrojLicitacije", "GodinaLicitacije", "KorakCeneLicitacije", "OgranicenjeLicitacije", "RokLicitacije" },
                values: new object[] { new Guid("cac0e0a2-852a-4265-ac71-49c25fb5559b"), 42, 2012, 3, 12, new DateTime(2020, 9, 25, 19, 33, 34, 894, DateTimeKind.Local).AddTicks(8874) });

            migrationBuilder.InsertData(
                table: "Licitacija",
                columns: new[] { "LicitacijaId", "BrojLicitacije", "GodinaLicitacije", "KorakCeneLicitacije", "OgranicenjeLicitacije", "RokLicitacije" },
                values: new object[] { new Guid("2ff32eb3-a7a1-4e8b-a9e1-8ec51f3eca4c"), 43, 2012, 2, 13, new DateTime(2020, 10, 6, 19, 33, 34, 899, DateTimeKind.Local).AddTicks(5896) });

            migrationBuilder.InsertData(
                table: "Licitacija",
                columns: new[] { "LicitacijaId", "BrojLicitacije", "GodinaLicitacije", "KorakCeneLicitacije", "OgranicenjeLicitacije", "RokLicitacije" },
                values: new object[] { new Guid("fb96a27d-f87f-49b5-98f3-ef6b57e84c3c"), 44, 2012, 4, 14, new DateTime(2020, 10, 15, 19, 33, 34, 899, DateTimeKind.Local).AddTicks(5964) });

            migrationBuilder.InsertData(
                table: "LicitacijaJavnoNadmetanje",
                columns: new[] { "JavnoNadmetanjeId", "LicitacijaId" },
                values: new object[] { new Guid("56a7cff5-cb0a-46b8-bfc1-93db415feeb4"), new Guid("cac0e0a2-852a-4265-ac71-49c25fb5559b") });

            migrationBuilder.InsertData(
                table: "LicitacijaJavnoNadmetanje",
                columns: new[] { "JavnoNadmetanjeId", "LicitacijaId" },
                values: new object[] { new Guid("6849bbbe-5798-4c04-aa20-58de420aa578"), new Guid("cac0e0a2-852a-4265-ac71-49c25fb5559b") });

            migrationBuilder.InsertData(
                table: "LicitacijaJavnoNadmetanje",
                columns: new[] { "JavnoNadmetanjeId", "LicitacijaId" },
                values: new object[] { new Guid("b195c4c2-2b26-40ad-8ff6-c212474acc83"), new Guid("2ff32eb3-a7a1-4e8b-a9e1-8ec51f3eca4c") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LicitacijaJavnoNadmetanje");

            migrationBuilder.DropTable(
                name: "Licitacija");
        }
    }
}
