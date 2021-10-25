using Microsoft.EntityFrameworkCore.Migrations;

namespace A4KPI.Migrations
{
    public partial class updateKPINewTableV11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KPINew_Policy_PolicyId",
                table: "KPINew");

            //migrationBuilder.DropIndex(
            //    name: "IX_KPINew_PolicyId",
            //    table: "KPINew");

            migrationBuilder.AlterColumn<int>(
                name: "PolicyId",
                table: "KPINew",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "PolicyId",
                table: "KPINew",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_KPINew_PolicyId",
                table: "KPINew",
                column: "PolicyId");

            migrationBuilder.AddForeignKey(
                name: "FK_KPINew_Policy_PolicyId",
                table: "KPINew",
                column: "PolicyId",
                principalTable: "Policy",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
