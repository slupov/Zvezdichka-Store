using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Zvezdichka.Data.Migrations
{
    public partial class EmailVerificationFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "DistributorShipments",
                nullable: false,
                defaultValue: new DateTime(2019, 6, 20, 0, 59, 44, 745, DateTimeKind.Local).AddTicks(3757),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 6, 20, 0, 43, 51, 974, DateTimeKind.Local).AddTicks(4785));

            migrationBuilder.AddColumn<bool>(
                name: "IsEmailVerified",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsEmailVerified",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "DistributorShipments",
                nullable: false,
                defaultValue: new DateTime(2019, 6, 20, 0, 43, 51, 974, DateTimeKind.Local).AddTicks(4785),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 6, 20, 0, 59, 44, 745, DateTimeKind.Local).AddTicks(3757));
        }
    }
}
