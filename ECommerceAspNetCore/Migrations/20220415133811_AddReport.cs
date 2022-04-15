using Microsoft.EntityFrameworkCore.Migrations;

namespace ECommerceAspNetCore.Migrations
{
    public partial class AddReport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSold",
                table: "CartProducts");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSold",
                table: "CartProducts",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }
    }
}
