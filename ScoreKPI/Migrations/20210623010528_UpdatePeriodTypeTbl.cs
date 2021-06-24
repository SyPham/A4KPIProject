using Microsoft.EntityFrameworkCore.Migrations;

namespace ScoreKPI.Migrations
{
    public partial class UpdatePeriodTypeTbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PeriodReportTimes_PeriodType_PeriodTypeId",
                table: "PeriodReportTimes");

            migrationBuilder.DropIndex(
                name: "IX_PeriodReportTimes_PeriodTypeId",
                table: "PeriodReportTimes");

            migrationBuilder.DropColumn(
                name: "PeriodTypeId",
                table: "PeriodReportTimes");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PeriodTypeId",
                table: "PeriodReportTimes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PeriodReportTimes_PeriodTypeId",
                table: "PeriodReportTimes",
                column: "PeriodTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_PeriodReportTimes_PeriodType_PeriodTypeId",
                table: "PeriodReportTimes",
                column: "PeriodTypeId",
                principalTable: "PeriodType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
