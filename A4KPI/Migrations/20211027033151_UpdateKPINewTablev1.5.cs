using Microsoft.EntityFrameworkCore.Migrations;

namespace A4KPI.Migrations
{
    public partial class UpdateKPINewTablev15 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "KPIAccountId",
                table: "OC",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CenterId",
                table: "KPIAccount",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeptId",
                table: "KPIAccount",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FactId",
                table: "KPIAccount",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OC_KPIAccountId",
                table: "OC",
                column: "KPIAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_OC_KPIAccount_KPIAccountId",
                table: "OC",
                column: "KPIAccountId",
                principalTable: "KPIAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OC_KPIAccount_KPIAccountId",
                table: "OC");

            migrationBuilder.DropIndex(
                name: "IX_OC_KPIAccountId",
                table: "OC");

            migrationBuilder.DropColumn(
                name: "KPIAccountId",
                table: "OC");

            migrationBuilder.DropColumn(
                name: "CenterId",
                table: "KPIAccount");

            migrationBuilder.DropColumn(
                name: "DeptId",
                table: "KPIAccount");

            migrationBuilder.DropColumn(
                name: "FactId",
                table: "KPIAccount");
        }
    }
}
