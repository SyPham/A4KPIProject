using Microsoft.EntityFrameworkCore.Migrations;

namespace A4KPI.Migrations
{
    public partial class updateTypeTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OCAccounts_OC_OCId",
                table: "OCAccounts");

            migrationBuilder.AddColumn<string>(
                name: "NameEn",
                table: "Types",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameZh",
                table: "Types",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OCAccounts_OC_OCId",
                table: "OCAccounts",
                column: "OCId",
                principalTable: "OC",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OCAccounts_OC_OCId",
                table: "OCAccounts");

            migrationBuilder.DropColumn(
                name: "NameEn",
                table: "Types");

            migrationBuilder.DropColumn(
                name: "NameZh",
                table: "Types");

            migrationBuilder.AddForeignKey(
                name: "FK_OCAccounts_OC_OCId",
                table: "OCAccounts",
                column: "OCId",
                principalTable: "OC",
                principalColumn: "Id");
        }
    }
}
