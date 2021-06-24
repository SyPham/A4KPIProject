using Microsoft.EntityFrameworkCore.Migrations;

namespace ScoreKPI.Migrations
{
    public partial class UpdateObjectiveTbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Objectives_Accounts_AccountId",
                table: "Objectives");

            migrationBuilder.DropIndex(
                name: "IX_Objectives_AccountId",
                table: "Objectives");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "Objectives");

            migrationBuilder.CreateIndex(
                name: "IX_Objectives_CreatedBy",
                table: "Objectives",
                column: "CreatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_Objectives_Accounts_CreatedBy",
                table: "Objectives",
                column: "CreatedBy",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Objectives_Accounts_CreatedBy",
                table: "Objectives");

            migrationBuilder.DropIndex(
                name: "IX_Objectives_CreatedBy",
                table: "Objectives");

            migrationBuilder.AddColumn<int>(
                name: "AccountId",
                table: "Objectives",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Objectives_AccountId",
                table: "Objectives",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Objectives_Accounts_AccountId",
                table: "Objectives",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
