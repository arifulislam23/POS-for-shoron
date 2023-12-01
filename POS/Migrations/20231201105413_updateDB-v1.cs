using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace POS.Migrations
{
    public partial class updateDBv1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CustomerId",
                table: "SellHistories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    CustomerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.CustomerId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SellHistories_CustomerId",
                table: "SellHistories",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_SellHistories_Customer_CustomerId",
                table: "SellHistories",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "CustomerId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SellHistories_Customer_CustomerId",
                table: "SellHistories");

            migrationBuilder.DropTable(
                name: "Customer");

            migrationBuilder.DropIndex(
                name: "IX_SellHistories_CustomerId",
                table: "SellHistories");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "SellHistories");
        }
    }
}
