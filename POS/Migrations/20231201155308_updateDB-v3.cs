using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace POS.Migrations
{
    public partial class updateDBv3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreateAt",
                table: "SellHistories",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateAt",
                table: "SellHistories");
        }
    }
}
