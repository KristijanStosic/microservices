using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KorisnikSistemaService.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TipKorisnika",
                columns: table => new
                {
                    TipKorisnikaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NazivTipaKorisnika = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipKorisnika", x => x.TipKorisnikaId);
                });

            migrationBuilder.CreateTable(
                name: "KorisnikSistema",
                columns: table => new
                {
                    KorisnikSistemaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Ime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Prezime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    KorisnickoIme = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Lozinka = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TipKorisnikaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KorisnikSistema", x => x.KorisnikSistemaId);
                    table.ForeignKey(
                        name: "FK_KorisnikSistema_TipKorisnika_TipKorisnikaId",
                        column: x => x.TipKorisnikaId,
                        principalTable: "TipKorisnika",
                        principalColumn: "TipKorisnikaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "TipKorisnika",
                columns: new[] { "TipKorisnikaId", "NazivTipaKorisnika" },
                values: new object[,]
                {
                    { new Guid("e98b3a1c-eb0c-4e16-8866-a811115cac7c"), "Operater" },
                    { new Guid("74e09d41-9b18-493b-9569-7069ea26beef"), "TehnickiSekretar" },
                    { new Guid("64591a2c-1aa9-4bb2-aded-deb0956e3e82"), "PrvaKomisija" },
                    { new Guid("f368886e-c58b-48fb-82b3-b1caf54275ba"), "Superuser" },
                    { new Guid("1e9a8348-c519-405d-b080-cbfdc60db8b6"), "OperaterNadmetanja" },
                    { new Guid("213cbfd7-821e-4496-86aa-a370f7804bb7"), "Licitant" },
                    { new Guid("909987de-f2fc-4bb8-bd99-3944434ff4cd"), "Menadzer" },
                    { new Guid("f76ffdb2-32d6-4e36-84a1-431c5158c028"), "Administrator" }
                });

            migrationBuilder.InsertData(
                table: "KorisnikSistema",
                columns: new[] { "KorisnikSistemaId", "Email", "Ime", "KorisnickoIme", "Lozinka", "Prezime", "TipKorisnikaId" },
                values: new object[,]
                {
                    { new Guid("523e45d9-f7a0-42a0-8850-f64ad734b8f3"), "Operater@gmail.com", "Operater", "Operater", "$2a$11$3yEazS9ipOrUw8bkAjykTu2S76ixA8ONbm6XUYnJBQqyfnJtB5d9O", "Operater", new Guid("e98b3a1c-eb0c-4e16-8866-a811115cac7c") },
                    { new Guid("0f7918d4-1fce-49c0-b638-9274c08c499d"), "TehnickiSekretar@gmail.com", "TehnickiSekretar", "TehnickiSekretar", "$2a$11$QSQ8ynEzPzB8zXxx2kx7wOnb1DQMGDKNFBH7iJU5bGo4S0V4MZc/y", "TehnickiSekretar", new Guid("74e09d41-9b18-493b-9569-7069ea26beef") },
                    { new Guid("d84f081c-0178-4f89-aec3-13bcb645c6a5"), "PrvaKomisija@gmail.com", "PrvaKomisija", "PrvaKomisija", "$2a$11$sA240A1NWd./XOGERzgIaemdmfc3mTMpzY3fENEeCTxRkXt2mwHj.", "PrvaKomisija", new Guid("64591a2c-1aa9-4bb2-aded-deb0956e3e82") },
                    { new Guid("2fa88a5b-372e-4c37-bb20-7b786e6d4317"), "Superuser@gmail.com", "Superuser", "Superuser", "$2a$11$JLNRe/erSEY1/KzoLPR2I.Pt9C7ghkKj412LqHkhGDesSo2e31JLe", "Superuser", new Guid("f368886e-c58b-48fb-82b3-b1caf54275ba") },
                    { new Guid("e4a42018-e71e-4e39-9c88-eae9199f46cd"), "OperaterNadmetanja@gmail.com", "OperaterNadmetanja", "OperaterNadmetanja", "$2a$11$.FVNptUs.TU0CfLpVAkGZux/T1eXglctk9cOaMhQ1UVNB0dmObsES", "OperaterNadmetanja", new Guid("1e9a8348-c519-405d-b080-cbfdc60db8b6") },
                    { new Guid("27e3d55f-e73f-4f7c-b49b-34a7e7209f44"), "Licitant@gmail.com", "Licitant", "Licitant", "$2a$11$TjKnLKjz3jeGAYO4/6gr8uLmlYtWviOrR7x0blKH2aw2bCLYEo5S6", "Licitant", new Guid("213cbfd7-821e-4496-86aa-a370f7804bb7") },
                    { new Guid("bb71d94a-e97c-4fb7-9e3c-55ea09a65a72"), "Menadzer@gmail.com", "Menadzer", "Menadzer", "$2a$11$qpgSQlGR/5pjS5aQi25GWe6D02fWY1kRq/OntvbmK02OfnybbXsY.", "Menadzer", new Guid("909987de-f2fc-4bb8-bd99-3944434ff4cd") },
                    { new Guid("bd3a7634-e992-47cd-989c-3ba9aa5368ea"), "Administrator@gmail.com", "Administrator", "Administrator", "$2a$11$e6ctOJTcIpfLKUVit/KOo.KS5NV.xKVxj49aCc/OOxVNf1mNW1/Te", "Administrator", new Guid("f76ffdb2-32d6-4e36-84a1-431c5158c028") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_KorisnikSistema_Email",
                table: "KorisnikSistema",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_KorisnikSistema_KorisnickoIme",
                table: "KorisnikSistema",
                column: "KorisnickoIme",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_KorisnikSistema_TipKorisnikaId",
                table: "KorisnikSistema",
                column: "TipKorisnikaId");

            migrationBuilder.CreateIndex(
                name: "IX_TipKorisnika_NazivTipaKorisnika",
                table: "TipKorisnika",
                column: "NazivTipaKorisnika",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KorisnikSistema");

            migrationBuilder.DropTable(
                name: "TipKorisnika");
        }
    }
}
