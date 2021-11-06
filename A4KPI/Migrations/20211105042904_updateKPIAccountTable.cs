using Microsoft.EntityFrameworkCore.Migrations;

namespace A4KPI.Migrations
{
    public partial class updateKPIAccountTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSubmit",
                table: "KPIAccount",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "KPIAccountId",
                table: "Actions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Actions_KPIAccountId",
                table: "Actions",
                column: "KPIAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Actions_KPIAccount_KPIAccountId",
                table: "Actions",
                column: "KPIAccountId",
                principalTable: "KPIAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Actions_KPIAccount_KPIAccountId",
                table: "Actions");

            migrationBuilder.DropIndex(
                name: "IX_Actions_KPIAccountId",
                table: "Actions");

            migrationBuilder.DropColumn(
                name: "IsSubmit",
                table: "KPIAccount");

            migrationBuilder.DropColumn(
                name: "KPIAccountId",
                table: "Actions");
        }
    }
}
