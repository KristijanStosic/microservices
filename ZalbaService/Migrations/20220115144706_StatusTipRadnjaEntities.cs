using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ZalbaService.Migrations
{
    public partial class StatusTipRadnjaEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RadnjaZaZalbu",
                columns: table => new
                {
                    RadnjaZaZalbuId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NazivRadnjeZaZalbu = table.Column<string>(type: "nvarchar(max)", nullable: false)
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
                    NazivStatusaZalbe = table.Column<string>(type: "nvarchar(max)", nullable: false)
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
                    NazivTipaZalbe = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipZalbe", x => x.TipZalbeId);
                });

            migrationBuilder.InsertData(
                table: "RadnjaZaZalbu",
                columns: new[] { "RadnjaZaZalbuId", "NazivRadnjeZaZalbu" },
                values: new object[,]
                {
                    { new Guid("0c9f2e15-4271-408d-8d40-e0cf57b33285"), "JN ide u drugi krug sa novim uslovima" },
                    { new Guid("6ac4f54a-d8f4-4d67-9b53-b97e240cbfb9"), "JN ide u drugi krug sa starim uslovima" },
                    { new Guid("4688f1f3-e7cf-4772-b1a4-1ac19fadef00"), "JN ne ide u drugi krug" }
                });

            migrationBuilder.InsertData(
                table: "StatusZalbe",
                columns: new[] { "StatusZalbeId", "NazivStatusaZalbe" },
                values: new object[,]
                {
                    { new Guid("646f2074-e10e-4078-be21-403d6ac873c7"), "Usvojena" },
                    { new Guid("c677ec37-647f-421a-b68d-df8ef37b7edc"), "Odbijena" },
                    { new Guid("083df6f6-200e-45fd-be27-2b88cb8ee7b3"), "Otvorena" }
                });

            migrationBuilder.InsertData(
                table: "TipZalbe",
                columns: new[] { "TipZalbeId", "NazivTipaZalbe" },
                values: new object[,]
                {
                    { new Guid("01c0db0a-6906-4f21-8d40-efe95e0748a0"), "Zalba na tok javnog nadmetanja" },
                    { new Guid("a1168e04-72f5-4ad8-9441-e1edcb533406"), "Zalba na Odluku o davanju u zakup" },
                    { new Guid("7bac6453-dac7-4192-a5de-ffe48dd1501c"), "Zalba na Odluku o davanju na koriscenje" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RadnjaZaZalbu");

            migrationBuilder.DropTable(
                name: "StatusZalbe");

            migrationBuilder.DropTable(
                name: "TipZalbe");
        }
    }
}
