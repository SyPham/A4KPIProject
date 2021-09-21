using Microsoft.EntityFrameworkCore.Migrations;

namespace A4KPI.Migrations
{
    public partial class UpdateKpinew : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KPINew_Policy_PolicyId",
                table: "KPINew");

            migrationBuilder.DropIndex(
                name: "IX_KPINew_PolicyId",
                table: "KPINew");
        }
    }
}
