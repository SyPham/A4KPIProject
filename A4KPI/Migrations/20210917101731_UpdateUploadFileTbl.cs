using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace A4KPI.Migrations
{
    public partial class UpdateUploadFileTbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "UploadFile");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "UploadFile",
                newName: "KPIId");

            migrationBuilder.AddColumn<DateTime>(
                name: "UploadTime",
                table: "UploadFile",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UploadTime",
                table: "UploadFile");

            migrationBuilder.RenameColumn(
                name: "KPIId",
                table: "UploadFile",
                newName: "CreatedBy");

            migrationBuilder.AddColumn<int>(
                name: "ModifiedBy",
                table: "UploadFile",
                type: "int",
                nullable: true);
        }
    }
}
