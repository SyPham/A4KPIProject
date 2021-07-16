using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace A4KPI.Migrations
{
    public partial class UpdateOCAccountsTbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "OCAccounts",
                newName: "CreatedTime");

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedTime",
                table: "OCAccounts",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ModifiedTime",
                table: "OCAccounts");

            migrationBuilder.RenameColumn(
                name: "CreatedTime",
                table: "OCAccounts",
                newName: "CreatedDate");
        }
    }
}
