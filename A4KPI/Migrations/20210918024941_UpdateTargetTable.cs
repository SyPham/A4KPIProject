using Microsoft.EntityFrameworkCore.Migrations;

namespace A4KPI.Migrations
{
    public partial class UpdateTargetTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Submitted",
                table: "Targets",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Submitted",
                table: "Targets");
        }
    }
}
