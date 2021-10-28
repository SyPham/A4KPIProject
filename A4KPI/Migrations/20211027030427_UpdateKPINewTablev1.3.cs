using Microsoft.EntityFrameworkCore.Migrations;

namespace A4KPI.Migrations
{
    public partial class UpdateKPINewTablev13 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CenterId",
                table: "KPINew");

            migrationBuilder.DropColumn(
                name: "DeptId",
                table: "KPINew");

            migrationBuilder.DropColumn(
                name: "FactId",
                table: "KPINew");
        }
    }
}
