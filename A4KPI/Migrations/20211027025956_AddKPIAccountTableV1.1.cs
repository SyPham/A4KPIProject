using Microsoft.EntityFrameworkCore.Migrations;

namespace A4KPI.Migrations
{
    public partial class AddKPIAccountTableV11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KPIAccount_Accounts_AccountId",
                table: "KPIAccount");

            migrationBuilder.DropForeignKey(
                name: "FK_KPIAccount_KPINew_KpiId",
                table: "KPIAccount");

            migrationBuilder.DropIndex(
                name: "IX_KPIAccount_AccountId",
                table: "KPIAccount");

            migrationBuilder.DropIndex(
                name: "IX_KPIAccount_KpiId",
                table: "KPIAccount");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_KPIAccount_AccountId",
                table: "KPIAccount",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_KPIAccount_KpiId",
                table: "KPIAccount",
                column: "KpiId");

            migrationBuilder.AddForeignKey(
                name: "FK_KPIAccount_Accounts_AccountId",
                table: "KPIAccount",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_KPIAccount_KPINew_KpiId",
                table: "KPIAccount",
                column: "KpiId",
                principalTable: "KPINew",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
