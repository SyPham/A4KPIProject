using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ScoreKPI.Migrations
{
    public partial class AddAttitudeScoreTbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AttitudeScore",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PeriodType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Period = table.Column<int>(type: "int", nullable: false),
                    Point = table.Column<double>(type: "float", nullable: false),
                    ObjectiveId = table.Column<int>(type: "int", nullable: false),
                    ScoreBy = table.Column<int>(type: "int", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttitudeScore", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AttitudeScore_Objectives_ObjectiveId",
                        column: x => x.ObjectiveId,
                        principalTable: "Objectives",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AttitudeScore_ObjectiveId",
                table: "AttitudeScore",
                column: "ObjectiveId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AttitudeScore");
        }
    }
}
