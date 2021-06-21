using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ScoreKPI.Migrations
{
    public partial class AddDateTimeToKPIScoreTbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedTime",
                table: "KPIScore",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedTime",
                table: "KPIScore",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedTime",
                table: "KPIScore");

            migrationBuilder.DropColumn(
                name: "ModifiedTime",
                table: "KPIScore");
        }
    }
}
