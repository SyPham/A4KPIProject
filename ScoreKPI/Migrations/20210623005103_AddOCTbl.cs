using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ScoreKPI.Migrations
{
    public partial class AddOCTbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Periods");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedTime",
                table: "Periods",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "ModifiedBy",
                table: "Periods",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedTime",
                table: "Periods",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Months",
                table: "Periods",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PeriodTypeId",
                table: "Periods",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReportTime",
                table: "Periods",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Periods",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Value",
                table: "Periods",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Months",
                table: "PeriodReportTimes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PeriodOfYear",
                table: "PeriodReportTimes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PeriodTypeId",
                table: "PeriodReportTimes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "OC",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Level = table.Column<int>(type: "int", nullable: false),
                    ParentId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OC", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PeriodType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Position = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PeriodType", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Periods_PeriodTypeId",
                table: "Periods",
                column: "PeriodTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PeriodReportTimes_PeriodTypeId",
                table: "PeriodReportTimes",
                column: "PeriodTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_PeriodReportTimes_PeriodType_PeriodTypeId",
                table: "PeriodReportTimes",
                column: "PeriodTypeId",
                principalTable: "PeriodType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Periods_PeriodType_PeriodTypeId",
                table: "Periods",
                column: "PeriodTypeId",
                principalTable: "PeriodType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PeriodReportTimes_PeriodType_PeriodTypeId",
                table: "PeriodReportTimes");

            migrationBuilder.DropForeignKey(
                name: "FK_Periods_PeriodType_PeriodTypeId",
                table: "Periods");

            migrationBuilder.DropTable(
                name: "OC");

            migrationBuilder.DropTable(
                name: "PeriodType");

            migrationBuilder.DropIndex(
                name: "IX_Periods_PeriodTypeId",
                table: "Periods");

            migrationBuilder.DropIndex(
                name: "IX_PeriodReportTimes_PeriodTypeId",
                table: "PeriodReportTimes");

            migrationBuilder.DropColumn(
                name: "CreatedTime",
                table: "Periods");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "Periods");

            migrationBuilder.DropColumn(
                name: "ModifiedTime",
                table: "Periods");

            migrationBuilder.DropColumn(
                name: "Months",
                table: "Periods");

            migrationBuilder.DropColumn(
                name: "PeriodTypeId",
                table: "Periods");

            migrationBuilder.DropColumn(
                name: "ReportTime",
                table: "Periods");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Periods");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "Periods");

            migrationBuilder.DropColumn(
                name: "Months",
                table: "PeriodReportTimes");

            migrationBuilder.DropColumn(
                name: "PeriodOfYear",
                table: "PeriodReportTimes");

            migrationBuilder.DropColumn(
                name: "PeriodTypeId",
                table: "PeriodReportTimes");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Periods",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }
    }
}
