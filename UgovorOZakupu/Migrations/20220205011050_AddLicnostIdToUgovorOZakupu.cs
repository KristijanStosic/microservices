using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UgovorOZakupu.Migrations
{
    public partial class AddLicnostIdToUgovorOZakupu : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "LicnostId",
                table: "UgovorOZakupu",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LicnostId",
                table: "UgovorOZakupu");
        }
    }
}
