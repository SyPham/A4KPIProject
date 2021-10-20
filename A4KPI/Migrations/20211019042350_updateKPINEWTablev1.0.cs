using Microsoft.EntityFrameworkCore.Migrations;

namespace A4KPI.Migrations
{
    public partial class updateKPINEWTablev10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreateBy",
                table: "KPINew",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LevelOcCreateBy",
                table: "KPINew",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LevelOcPolicy",
                table: "KPINew",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OcIdCreateBy",
                table: "KPINew",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OcIdPolicy",
                table: "KPINew",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateBy",
                table: "KPINew");

            migrationBuilder.DropColumn(
                name: "LevelOcCreateBy",
                table: "KPINew");

            migrationBuilder.DropColumn(
                name: "LevelOcPolicy",
                table: "KPINew");

            migrationBuilder.DropColumn(
                name: "OcIdCreateBy",
                table: "KPINew");

            migrationBuilder.DropColumn(
                name: "OcIdPolicy",
                table: "KPINew");
        }
    }
}
