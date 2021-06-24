using Microsoft.EntityFrameworkCore.Migrations;

namespace ScoreKPI.Migrations
{
    public partial class UpdateForeignKeyPeriodTypeTbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Periods_PeriodTypeId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Contributions_Periods_PeriodTypeId",
                table: "Contributions");

            migrationBuilder.DropForeignKey(
                name: "FK_KPIScore_Periods_PeriodTypeId",
                table: "KPIScore");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_PeriodType_PeriodTypeId",
                table: "Comments",
                column: "PeriodTypeId",
                principalTable: "PeriodType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Contributions_PeriodType_PeriodTypeId",
                table: "Contributions",
                column: "PeriodTypeId",
                principalTable: "PeriodType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_KPIScore_PeriodType_PeriodTypeId",
                table: "KPIScore",
                column: "PeriodTypeId",
                principalTable: "PeriodType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_PeriodType_PeriodTypeId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Contributions_PeriodType_PeriodTypeId",
                table: "Contributions");

            migrationBuilder.DropForeignKey(
                name: "FK_KPIScore_PeriodType_PeriodTypeId",
                table: "KPIScore");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Periods_PeriodTypeId",
                table: "Comments",
                column: "PeriodTypeId",
                principalTable: "Periods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Contributions_Periods_PeriodTypeId",
                table: "Contributions",
                column: "PeriodTypeId",
                principalTable: "Periods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_KPIScore_Periods_PeriodTypeId",
                table: "KPIScore",
                column: "PeriodTypeId",
                principalTable: "Periods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
