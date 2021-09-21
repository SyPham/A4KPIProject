using Microsoft.EntityFrameworkCore.Migrations;

namespace A4KPI.Migrations
{
    public partial class UpdateKPINewTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Submitted",
                table: "KPINew",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Submitted",
                table: "KPINew");
        }
    }
}
