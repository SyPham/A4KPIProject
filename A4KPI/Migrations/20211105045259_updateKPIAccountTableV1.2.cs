using Microsoft.EntityFrameworkCore.Migrations;

namespace A4KPI.Migrations
{
    public partial class updateKPIAccountTableV12 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsSubmit",
                table: "KPIAccount",
                newName: "IsPDCASubmit");

            migrationBuilder.AddColumn<bool>(
                name: "IsActionSubmit",
                table: "KPIAccount",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActionSubmit",
                table: "KPIAccount");

            migrationBuilder.RenameColumn(
                name: "IsPDCASubmit",
                table: "KPIAccount",
                newName: "IsSubmit");
        }
    }
}
