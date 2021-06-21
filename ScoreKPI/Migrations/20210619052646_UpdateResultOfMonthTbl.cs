using Microsoft.EntityFrameworkCore.Migrations;

namespace ScoreKPI.Migrations
{
    public partial class UpdateResultOfMonthTbl : Migration
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

        protected override void Down(MigrationBuilder migrationBuilder)
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
