using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace A4KPI.Migrations
{
    public partial class AddSpecialScoreTbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SpecialContributionScore",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Period = table.Column<int>(type: "int", nullable: false),
                    Point = table.Column<double>(type: "float", nullable: false),
                    PeriodTypeId = table.Column<int>(type: "int", nullable: false),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    ScoreBy = table.Column<int>(type: "int", nullable: false),
                    ScoreType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecialContributionScore", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpecialContributionScore_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_SpecialContributionScore_Accounts_ScoreBy",
                        column: x => x.ScoreBy,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_SpecialContributionScore_Periods_PeriodTypeId",
                        column: x => x.PeriodTypeId,
                        principalTable: "Periods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "SpecialScore",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Point = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecialScore", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SpecialContributionScore_AccountId",
                table: "SpecialContributionScore",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_SpecialContributionScore_PeriodTypeId",
                table: "SpecialContributionScore",
                column: "PeriodTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_SpecialContributionScore_ScoreBy",
                table: "SpecialContributionScore",
                column: "ScoreBy");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SpecialContributionScore");

            migrationBuilder.DropTable(
                name: "SpecialScore");
        }
    }
}
