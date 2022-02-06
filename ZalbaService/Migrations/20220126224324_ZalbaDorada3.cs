using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ZalbaService.Migrations
{
    public partial class ZalbaDorada3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Zalba",
                keyColumn: "ZalbaId",
                keyValue: new Guid("0c9dac96-99b4-4c2e-b5bf-8f7a4f895a99"));

            migrationBuilder.DeleteData(
                table: "Zalba",
                keyColumn: "ZalbaId",
                keyValue: new Guid("ae4f5b3a-efb2-491e-8bb0-22b74ebf4c14"));

            migrationBuilder.DeleteData(
                table: "Zalba",
                keyColumn: "ZalbaId",
                keyValue: new Guid("b989728c-c026-4801-bf75-5a0855a4ebbe"));

            migrationBuilder.InsertData(
                table: "Zalba",
                columns: new[] { "ZalbaId", "BrojNadmetanja", "BrojResenja", "DatumPodnosenja", "DatumResenja", "KupacId", "Obrazlozenje", "RadnjaZaZalbuId", "RazlogZalbe", "StatusZalbeId", "TipZalbeId" },
                values: new object[] { new Guid("1048330a-bf7b-4744-8ae0-ebb2e95a0d79"), "100-NN", "X9NN41HH", new DateTime(2022, 1, 26, 23, 43, 24, 111, DateTimeKind.Local).AddTicks(5677), new DateTime(2022, 3, 17, 23, 43, 24, 114, DateTimeKind.Local).AddTicks(2858), null, "Nema dovoljno licitanata da se odrzi javno nadmetanje", new Guid("009aa493-7786-4aad-9d1a-0f90d57ebbb4"), "Nedovoljno licitanata", new Guid("6e5e8a67-006b-4ac0-89d0-9711006c0d28"), new Guid("018ac715-4588-4934-bb2d-8bb2f4d1049a") });

            migrationBuilder.InsertData(
                table: "Zalba",
                columns: new[] { "ZalbaId", "BrojNadmetanja", "BrojResenja", "DatumPodnosenja", "DatumResenja", "KupacId", "Obrazlozenje", "RadnjaZaZalbuId", "RazlogZalbe", "StatusZalbeId", "TipZalbeId" },
                values: new object[] { new Guid("768d040f-13e3-4da8-b4be-7a03213a4fb0"), "200-OO", "IIKK-55", new DateTime(2022, 1, 26, 23, 43, 24, 114, DateTimeKind.Local).AddTicks(5119), new DateTime(2022, 3, 17, 23, 43, 24, 114, DateTimeKind.Local).AddTicks(5134), null, "Dokumentacija nije potpuna kako bi se odrzalo javno nadmetanje", new Guid("009aa493-7786-4aad-9d1a-0f90d57ebbb4"), "Nepotpuna dokumentacija", new Guid("6e5e8a67-006b-4ac0-89d0-9711006c0d28"), new Guid("b4947a43-42e4-4d20-a10b-169c5089aac6") });

            migrationBuilder.InsertData(
                table: "Zalba",
                columns: new[] { "ZalbaId", "BrojNadmetanja", "BrojResenja", "DatumPodnosenja", "DatumResenja", "KupacId", "Obrazlozenje", "RadnjaZaZalbuId", "RazlogZalbe", "StatusZalbeId", "TipZalbeId" },
                values: new object[] { new Guid("57d86d26-3113-4188-881f-af86fe58015d"), "999-AA", "QWOP44-MM", new DateTime(2022, 1, 26, 23, 43, 24, 114, DateTimeKind.Local).AddTicks(5154), new DateTime(2022, 3, 17, 23, 43, 24, 114, DateTimeKind.Local).AddTicks(5158), null, "Nema dovoljno novcanih sredstava za javno nadmetanje", new Guid("4ccb6d66-18b2-4791-8afe-b628a4f7c0af"), "Nedovoljno uplacenih novcanih sredstava", new Guid("2c1051d2-0ddf-41a9-ba08-24070a50f4b3"), new Guid("b4947a43-42e4-4d20-a10b-169c5089aac6") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Zalba",
                keyColumn: "ZalbaId",
                keyValue: new Guid("1048330a-bf7b-4744-8ae0-ebb2e95a0d79"));

            migrationBuilder.DeleteData(
                table: "Zalba",
                keyColumn: "ZalbaId",
                keyValue: new Guid("57d86d26-3113-4188-881f-af86fe58015d"));

            migrationBuilder.DeleteData(
                table: "Zalba",
                keyColumn: "ZalbaId",
                keyValue: new Guid("768d040f-13e3-4da8-b4be-7a03213a4fb0"));

            migrationBuilder.InsertData(
                table: "Zalba",
                columns: new[] { "ZalbaId", "BrojNadmetanja", "BrojResenja", "DatumPodnosenja", "DatumResenja", "KupacId", "Obrazlozenje", "RadnjaZaZalbuId", "RazlogZalbe", "StatusZalbeId", "TipZalbeId" },
                values: new object[] { new Guid("0c9dac96-99b4-4c2e-b5bf-8f7a4f895a99"), "100-NN", "X9NN41HH", new DateTime(2022, 1, 26, 23, 39, 55, 736, DateTimeKind.Local).AddTicks(7346), new DateTime(2022, 3, 17, 23, 39, 55, 739, DateTimeKind.Local).AddTicks(941), null, "Nema dovoljno licitanata da se odrzi javno nadmetanje", new Guid("009aa493-7786-4aad-9d1a-0f90d57ebbb4"), "Nedovoljno licitanata", new Guid("6e5e8a67-006b-4ac0-89d0-9711006c0d28"), new Guid("018ac715-4588-4934-bb2d-8bb2f4d1049a") });

            migrationBuilder.InsertData(
                table: "Zalba",
                columns: new[] { "ZalbaId", "BrojNadmetanja", "BrojResenja", "DatumPodnosenja", "DatumResenja", "KupacId", "Obrazlozenje", "RadnjaZaZalbuId", "RazlogZalbe", "StatusZalbeId", "TipZalbeId" },
                values: new object[] { new Guid("b989728c-c026-4801-bf75-5a0855a4ebbe"), "200-OO", "IIKK-55", new DateTime(2022, 1, 26, 23, 39, 55, 739, DateTimeKind.Local).AddTicks(2814), new DateTime(2022, 3, 17, 23, 39, 55, 739, DateTimeKind.Local).AddTicks(2828), null, "Dokumentacija nije potpuna kako bi se odrzalo javno nadmetanje", new Guid("009aa493-7786-4aad-9d1a-0f90d57ebbb4"), "Nepotpuna dokumentacija", new Guid("6e5e8a67-006b-4ac0-89d0-9711006c0d28"), new Guid("b4947a43-42e4-4d20-a10b-169c5089aac6") });

            migrationBuilder.InsertData(
                table: "Zalba",
                columns: new[] { "ZalbaId", "BrojNadmetanja", "BrojResenja", "DatumPodnosenja", "DatumResenja", "KupacId", "Obrazlozenje", "RadnjaZaZalbuId", "RazlogZalbe", "StatusZalbeId", "TipZalbeId" },
                values: new object[] { new Guid("ae4f5b3a-efb2-491e-8bb0-22b74ebf4c14"), "999-AA", "QWOP44-MM", new DateTime(2022, 1, 26, 23, 39, 55, 739, DateTimeKind.Local).AddTicks(2848), new DateTime(2022, 3, 17, 23, 39, 55, 739, DateTimeKind.Local).AddTicks(2851), null, "Nema dovoljno novcanih sredstava za javno nadmetanje", new Guid("4ccb6d66-18b2-4791-8afe-b628a4f7c0af"), "Nedovoljno uplacenih novcanih sredstava", new Guid("2c1051d2-0ddf-41a9-ba08-24070a50f4b3"), new Guid("b4947a43-42e4-4d20-a10b-169c5089aac6") });
        }
    }
}
