using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UgovorOZakupu.Migrations
{
    public partial class AddDokumentIdToUgovorOZakupu : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RokDospeca_UgovorOZakupu_UgovorOZakupuId",
                table: "RokDospeca");

            migrationBuilder.AddColumn<Guid>(
                name: "DokumentId",
                table: "UgovorOZakupu",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<Guid>(
                name: "UgovorOZakupuId",
                table: "RokDospeca",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_RokDospeca_UgovorOZakupu_UgovorOZakupuId",
                table: "RokDospeca",
                column: "UgovorOZakupuId",
                principalTable: "UgovorOZakupu",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RokDospeca_UgovorOZakupu_UgovorOZakupuId",
                table: "RokDospeca");

            migrationBuilder.DropColumn(
                name: "DokumentId",
                table: "UgovorOZakupu");

            migrationBuilder.AlterColumn<Guid>(
                name: "UgovorOZakupuId",
                table: "RokDospeca",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_RokDospeca_UgovorOZakupu_UgovorOZakupuId",
                table: "RokDospeca",
                column: "UgovorOZakupuId",
                principalTable: "UgovorOZakupu",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
