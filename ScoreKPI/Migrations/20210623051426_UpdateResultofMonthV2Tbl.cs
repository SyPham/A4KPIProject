using Microsoft.EntityFrameworkCore.Migrations;

namespace ScoreKPI.Migrations
{
    public partial class UpdateResultofMonthV2Tbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ResultOfMonth_Objectives_ObjectiveId",
                table: "ResultOfMonth");

            migrationBuilder.AlterColumn<int>(
                name: "ObjectiveId",
                table: "ResultOfMonth",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_ResultOfMonth_CreatedBy",
                table: "ResultOfMonth",
                column: "CreatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_ResultOfMonth_Accounts_CreatedBy",
                table: "ResultOfMonth",
                column: "CreatedBy",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ResultOfMonth_Objectives_ObjectiveId",
                table: "ResultOfMonth",
                column: "ObjectiveId",
                principalTable: "Objectives",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ResultOfMonth_Accounts_CreatedBy",
                table: "ResultOfMonth");

            migrationBuilder.DropForeignKey(
                name: "FK_ResultOfMonth_Objectives_ObjectiveId",
                table: "ResultOfMonth");

            migrationBuilder.DropIndex(
                name: "IX_ResultOfMonth_CreatedBy",
                table: "ResultOfMonth");

            migrationBuilder.AlterColumn<int>(
                name: "ObjectiveId",
                table: "ResultOfMonth",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ResultOfMonth_Objectives_ObjectiveId",
                table: "ResultOfMonth",
                column: "ObjectiveId",
                principalTable: "Objectives",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
