using Microsoft.EntityFrameworkCore.Migrations;

namespace A4KPI.Migrations
{
    public partial class UpdateKPINewTablev16 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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
                table: "KPINew");

            migrationBuilder.DropColumn(
                name: "DeptId",
                table: "KPINew");

            migrationBuilder.DropColumn(
                name: "FactId",
                table: "KPINew");

            migrationBuilder.DropColumn(
                name: "LevelOcPolicy",
                table: "KPINew");

            migrationBuilder.DropColumn(
                name: "OcIdPolicy",
                table: "KPINew");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "KPIAccountId",
                table: "OC",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CenterId",
                table: "KPINew",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeptId",
                table: "KPINew",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FactId",
                table: "KPINew",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LevelOcPolicy",
                table: "KPINew",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OcIdPolicy",
                table: "KPINew",
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
    }
}
