using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Zvezdichka.Data.Migrations
{
    public partial class ProductSalePriceAndDefaultsAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Products",
                nullable: true,
                defaultValue: "",
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsInSale",
                table: "Products",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "SalePrice",
                table: "Products",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Message",
                table: "Comments",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<bool>(
                name: "IsEdited",
                table: "Comments",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsInSale",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "SalePrice",
                table: "Products");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Products",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true,
                oldDefaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Message",
                table: "Comments",
                nullable: false,
                oldClrType: typeof(string),
                oldDefaultValue: "");

            migrationBuilder.AlterColumn<bool>(
                name: "IsEdited",
                table: "Comments",
                nullable: false,
                oldClrType: typeof(bool),
                oldDefaultValue: false);
        }
    }
}
