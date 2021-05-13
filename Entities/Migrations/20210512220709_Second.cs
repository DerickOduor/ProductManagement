using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Entities.Migrations
{
    public partial class Second : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompositeProducts_Products_ProductId",
                table: "CompositeProducts");

            migrationBuilder.DropIndex(
                name: "IX_CompositeProducts_ProductId",
                table: "CompositeProducts");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Variants",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "SKU",
                table: "Variants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Value",
                table: "Variants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "ProductId",
                table: "CompositeProducts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "Variants");

            migrationBuilder.DropColumn(
                name: "SKU",
                table: "Variants");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "Variants");

            migrationBuilder.AlterColumn<Guid>(
                name: "ProductId",
                table: "CompositeProducts",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.CreateIndex(
                name: "IX_CompositeProducts_ProductId",
                table: "CompositeProducts",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_CompositeProducts_Products_ProductId",
                table: "CompositeProducts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
