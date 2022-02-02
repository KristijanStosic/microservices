using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DokumentService.Migrations
{
    public partial class SeedDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                values: new object[,]
                {
                    { new Guid("c4bcdc43-e7ed-4364-90b1-fa5f84e666f3"), new DateTime(2022, 2, 2, 15, 16, 48, 584, DateTimeKind.Local).AddTicks(6641), new DateTime(2022, 2, 5, 15, 16, 48, 584, DateTimeKind.Local).AddTicks(7037), new Guid("77cbe573-4ad7-4db8-b9b1-7a9f45bec5db"), "PSPG-1/2022" },
                    { new Guid("ce0244e0-e7be-40c3-b558-bcebcf7b2466"), new DateTime(2022, 2, 2, 15, 16, 48, 584, DateTimeKind.Local).AddTicks(7753), new DateTime(2022, 2, 5, 15, 16, 48, 584, DateTimeKind.Local).AddTicks(7756), new Guid("77cbe573-4ad7-4db8-b9b1-7a9f45bec5db"), "PSPG-2/2022" },
                    { new Guid("b2f3c1aa-4855-4fd7-bf17-0824280acada"), new DateTime(2022, 2, 2, 15, 16, 48, 584, DateTimeKind.Local).AddTicks(7736), new DateTime(2022, 2, 5, 15, 16, 48, 584, DateTimeKind.Local).AddTicks(7738), new Guid("3e649b2d-5569-4463-80f3-04382ef4a7eb"), "IJEN-1/2022" },
                    { new Guid("a2fa2d1e-ed38-4b63-b0c2-41fb70eaadc5"), new DateTime(2022, 2, 2, 15, 16, 48, 584, DateTimeKind.Local).AddTicks(7776), new DateTime(2022, 2, 5, 15, 16, 48, 584, DateTimeKind.Local).AddTicks(7778), new Guid("3e649b2d-5569-4463-80f3-04382ef4a7eb"), "IJEN-2/2022" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Dokument",
                keyColumn: "Id",
                keyValue: new Guid("a2fa2d1e-ed38-4b63-b0c2-41fb70eaadc5"));

            migrationBuilder.DeleteData(
                table: "Dokument",
                keyColumn: "Id",
                keyValue: new Guid("b2f3c1aa-4855-4fd7-bf17-0824280acada"));

            migrationBuilder.DeleteData(
                table: "Dokument",
                keyColumn: "Id",
                keyValue: new Guid("c4bcdc43-e7ed-4364-90b1-fa5f84e666f3"));

            migrationBuilder.DeleteData(
                table: "Dokument",
                keyColumn: "Id",
                keyValue: new Guid("ce0244e0-e7be-40c3-b558-bcebcf7b2466"));

            migrationBuilder.DeleteData(
                table: "TipDokumenta",
                keyColumn: "Id",
                keyValue: new Guid("3e649b2d-5569-4463-80f3-04382ef4a7eb"));

            migrationBuilder.DeleteData(
                table: "TipDokumenta",
                keyColumn: "Id",
                keyValue: new Guid("77cbe573-4ad7-4db8-b9b1-7a9f45bec5db"));
        }
    }
}
