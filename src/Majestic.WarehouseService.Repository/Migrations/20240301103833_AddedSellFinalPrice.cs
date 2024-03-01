using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Majestic.WarehouseService.Repository.Migrations
{
    public partial class AddedSellFinalPrice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "SellFinalPrice",
                table: "CarEntity",
                type: "decimal(18,2)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SellFinalPrice",
                table: "CarEntity");
        }
    }
}
