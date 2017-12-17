using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Zvezdichka.Data.Migrations
{
    public partial class CommentsDateAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateAdded",
                table: "Comments",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateEdited",
                table: "Comments",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsEdited",
                table: "Comments",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateAdded",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "DateEdited",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "IsEdited",
                table: "Comments");
        }
    }
}
