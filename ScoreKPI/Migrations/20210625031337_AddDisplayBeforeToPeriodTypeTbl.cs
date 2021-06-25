using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ScoreKPI.Migrations
{
    public partial class AddDisplayBeforeToPeriodTypeTbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DisplayTime",
                table: "PeriodType");

            migrationBuilder.AddColumn<int>(
                name: "DisplayBefore",
                table: "PeriodType",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DisplayBefore",
                table: "PeriodType");

            migrationBuilder.AddColumn<DateTime>(
                name: "DisplayTime",
                table: "PeriodType",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
