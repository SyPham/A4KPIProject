using Microsoft.EntityFrameworkCore.Migrations;

namespace A4KPI.Migrations
{
    public partial class UpdatePerformanceV2Tbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Performances_Objectives_UploadBy",
                table: "Performances");

            migrationBuilder.CreateIndex(
                name: "IX_Performances_ObjectiveId",
                table: "Performances",
                column: "ObjectiveId");

            migrationBuilder.AddForeignKey(
                name: "FK_Performances_Objectives_ObjectiveId",
                table: "Performances",
                column: "ObjectiveId",
                principalTable: "Objectives",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Performances_Objectives_ObjectiveId",
                table: "Performances");

            migrationBuilder.DropIndex(
                name: "IX_Performances_ObjectiveId",
                table: "Performances");

            migrationBuilder.AddForeignKey(
                name: "FK_Performances_Objectives_UploadBy",
                table: "Performances",
                column: "UploadBy",
                principalTable: "Objectives",
                principalColumn: "Id");
        }
    }
}
