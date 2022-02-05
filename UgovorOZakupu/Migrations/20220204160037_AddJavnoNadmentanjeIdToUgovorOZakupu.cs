using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UgovorOZakupu.Migrations
{
    public partial class AddJavnoNadmentanjeIdToUgovorOZakupu : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "JavnoNadmetanjeId",
                table: "UgovorOZakupu",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JavnoNadmetanjeId",
                table: "UgovorOZakupu");
        }
    }
}
