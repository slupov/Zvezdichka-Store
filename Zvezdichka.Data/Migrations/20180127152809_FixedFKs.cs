using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Zvezdichka.Data.Migrations
{
    public partial class FixedFKs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_DistributorShipments_DistributorShipmentId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_DistributorShipmentId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "DistributorShipmentId",
                table: "Products");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DistributorShipmentId",
                table: "Products",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_DistributorShipmentId",
                table: "Products",
                column: "DistributorShipmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_DistributorShipments_DistributorShipmentId",
                table: "Products",
                column: "DistributorShipmentId",
                principalTable: "DistributorShipments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
