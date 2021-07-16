using Microsoft.EntityFrameworkCore.Migrations;

namespace A4KPI.Migrations
{
    public partial class UpdateOCAccountsV2Tbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OCAccounts_OC_AccountId",
                table: "OCAccounts");

            migrationBuilder.CreateIndex(
                name: "IX_OCAccounts_OCId",
                table: "OCAccounts",
                column: "OCId");

            migrationBuilder.AddForeignKey(
                name: "FK_OCAccounts_OC_OCId",
                table: "OCAccounts",
                column: "OCId",
                principalTable: "OC",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OCAccounts_OC_OCId",
                table: "OCAccounts");

            migrationBuilder.DropIndex(
                name: "IX_OCAccounts_OCId",
                table: "OCAccounts");

            migrationBuilder.AddForeignKey(
                name: "FK_OCAccounts_OC_AccountId",
                table: "OCAccounts",
                column: "AccountId",
                principalTable: "OC",
                principalColumn: "Id");
        }
    }
}
