using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LiquorShop.Migrations
{
    public partial class iyzico : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CardName",
                table: "OrderHeader",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CardNo",
                table: "OrderHeader",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Cvc",
                table: "OrderHeader",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ExpirationMonth",
                table: "OrderHeader",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ExpirationYear",
                table: "OrderHeader",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CardName",
                table: "OrderHeader");

            migrationBuilder.DropColumn(
                name: "CardNo",
                table: "OrderHeader");

            migrationBuilder.DropColumn(
                name: "Cvc",
                table: "OrderHeader");

            migrationBuilder.DropColumn(
                name: "ExpirationMonth",
                table: "OrderHeader");

            migrationBuilder.DropColumn(
                name: "ExpirationYear",
                table: "OrderHeader");
        }
    }
}
