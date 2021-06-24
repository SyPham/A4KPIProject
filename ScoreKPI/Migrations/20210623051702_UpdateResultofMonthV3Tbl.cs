using Microsoft.EntityFrameworkCore.Migrations;

namespace ScoreKPI.Migrations
{
    public partial class UpdateResultofMonthV3Tbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ResultOfMonth_Objectives_ObjectiveId",
                table: "ResultOfMonth");

            migrationBuilder.DropIndex(
                name: "IX_ResultOfMonth_ObjectiveId",
                table: "ResultOfMonth");

            migrationBuilder.DropColumn(
                name: "ObjectiveId",
                table: "ResultOfMonth");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ObjectiveId",
                table: "ResultOfMonth",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ResultOfMonth_ObjectiveId",
                table: "ResultOfMonth",
                column: "ObjectiveId");

            migrationBuilder.AddForeignKey(
                name: "FK_ResultOfMonth_Objectives_ObjectiveId",
                table: "ResultOfMonth",
                column: "ObjectiveId",
                principalTable: "Objectives",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
