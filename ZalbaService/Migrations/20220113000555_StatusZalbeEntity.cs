using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ZalbaService.Migrations
{
    public partial class StatusZalbeEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StatusZalbe",
                columns: table => new
                {
                    StatusZalbeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NazivStatusaZalbe = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatusZalbe", x => x.StatusZalbeId);
                });

            migrationBuilder.InsertData(
                table: "StatusZalbe",
                columns: new[] { "StatusZalbeId", "NazivStatusaZalbe" },
                values: new object[] { new Guid("7d1878b3-b31d-46a3-91a8-a2bbc833eda4"), "Usvojena" });

            migrationBuilder.InsertData(
                table: "StatusZalbe",
                columns: new[] { "StatusZalbeId", "NazivStatusaZalbe" },
                values: new object[] { new Guid("2ccd78ba-8383-4f92-9c15-06d0373d65dd"), "Odbijena" });

            migrationBuilder.InsertData(
                table: "StatusZalbe",
                columns: new[] { "StatusZalbeId", "NazivStatusaZalbe" },
                values: new object[] { new Guid("dba4f831-20c2-418a-a5fb-da69aa8588bb"), "Otvorena" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StatusZalbe");
        }
    }
}
