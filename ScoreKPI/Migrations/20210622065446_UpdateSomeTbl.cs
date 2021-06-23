using Microsoft.EntityFrameworkCore.Migrations;

namespace ScoreKPI.Migrations
{
    public partial class UpdateSomeTbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AttitudeScore_Objectives_ObjectiveId",
                table: "AttitudeScore");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Objectives_ObjectiveId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Contributions_Objectives_ObjectiveId",
                table: "Contributions");

            migrationBuilder.DropForeignKey(
                name: "FK_KPIScore_Objectives_ObjectiveId",
                table: "KPIScore");

            migrationBuilder.DropColumn(
                name: "PeriodType",
                table: "KPIScore");

            migrationBuilder.DropColumn(
                name: "PeriodType",
                table: "AttitudeScore");

            migrationBuilder.RenameColumn(
                name: "ObjectiveId",
                table: "KPIScore",
                newName: "PeriodTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_KPIScore_ObjectiveId",
                table: "KPIScore",
                newName: "IX_KPIScore_PeriodTypeId");

            migrationBuilder.RenameColumn(
                name: "ObjectiveId",
                table: "Contributions",
                newName: "PeriodTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Contributions_ObjectiveId",
                table: "Contributions",
                newName: "IX_Contributions_PeriodTypeId");

            migrationBuilder.RenameColumn(
                name: "ObjectiveId",
                table: "Comments",
                newName: "PeriodTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_ObjectiveId",
                table: "Comments",
                newName: "IX_Comments_PeriodTypeId");

            migrationBuilder.RenameColumn(
                name: "ObjectiveId",
                table: "AttitudeScore",
                newName: "PeriodTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_AttitudeScore_ObjectiveId",
                table: "AttitudeScore",
                newName: "IX_AttitudeScore_PeriodTypeId");

            migrationBuilder.AddColumn<int>(
                name: "AccountId",
                table: "KPIScore",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AccountId",
                table: "Contributions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Period",
                table: "Contributions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AccountId",
                table: "Comments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Period",
                table: "Comments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AccountId",
                table: "AttitudeScore",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_KPIScore_AccountId",
                table: "KPIScore",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_KPIScore_ScoreBy",
                table: "KPIScore",
                column: "ScoreBy");

            migrationBuilder.CreateIndex(
                name: "IX_Contributions_AccountId",
                table: "Contributions",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_AccountId",
                table: "Comments",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AttitudeScore_AccountId",
                table: "AttitudeScore",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AttitudeScore_ScoreBy",
                table: "AttitudeScore",
                column: "ScoreBy");

            migrationBuilder.AddForeignKey(
                name: "FK_AttitudeScore_Accounts_AccountId",
                table: "AttitudeScore",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_AttitudeScore_Accounts_ScoreBy",
                table: "AttitudeScore",
                column: "ScoreBy",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_AttitudeScore_Periods_PeriodTypeId",
                table: "AttitudeScore",
                column: "PeriodTypeId",
                principalTable: "Periods",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Accounts_AccountId",
                table: "Comments",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Periods_PeriodTypeId",
                table: "Comments",
                column: "PeriodTypeId",
                principalTable: "Periods",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Contributions_Accounts_AccountId",
                table: "Contributions",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Contributions_Periods_PeriodTypeId",
                table: "Contributions",
                column: "PeriodTypeId",
                principalTable: "Periods",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_KPIScore_Accounts_AccountId",
                table: "KPIScore",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_KPIScore_Accounts_ScoreBy",
                table: "KPIScore",
                column: "ScoreBy",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_KPIScore_Periods_PeriodTypeId",
                table: "KPIScore",
                column: "PeriodTypeId",
                principalTable: "Periods",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AttitudeScore_Accounts_AccountId",
                table: "AttitudeScore");

            migrationBuilder.DropForeignKey(
                name: "FK_AttitudeScore_Accounts_ScoreBy",
                table: "AttitudeScore");

            migrationBuilder.DropForeignKey(
                name: "FK_AttitudeScore_Periods_PeriodTypeId",
                table: "AttitudeScore");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Accounts_AccountId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Periods_PeriodTypeId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Contributions_Accounts_AccountId",
                table: "Contributions");

            migrationBuilder.DropForeignKey(
                name: "FK_Contributions_Periods_PeriodTypeId",
                table: "Contributions");

            migrationBuilder.DropForeignKey(
                name: "FK_KPIScore_Accounts_AccountId",
                table: "KPIScore");

            migrationBuilder.DropForeignKey(
                name: "FK_KPIScore_Accounts_ScoreBy",
                table: "KPIScore");

            migrationBuilder.DropForeignKey(
                name: "FK_KPIScore_Periods_PeriodTypeId",
                table: "KPIScore");

            migrationBuilder.DropIndex(
                name: "IX_KPIScore_AccountId",
                table: "KPIScore");

            migrationBuilder.DropIndex(
                name: "IX_KPIScore_ScoreBy",
                table: "KPIScore");

            migrationBuilder.DropIndex(
                name: "IX_Contributions_AccountId",
                table: "Contributions");

            migrationBuilder.DropIndex(
                name: "IX_Comments_AccountId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_AttitudeScore_AccountId",
                table: "AttitudeScore");

            migrationBuilder.DropIndex(
                name: "IX_AttitudeScore_ScoreBy",
                table: "AttitudeScore");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "KPIScore");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "Contributions");

            migrationBuilder.DropColumn(
                name: "Period",
                table: "Contributions");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "Period",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "AttitudeScore");

            migrationBuilder.RenameColumn(
                name: "PeriodTypeId",
                table: "KPIScore",
                newName: "ObjectiveId");

            migrationBuilder.RenameIndex(
                name: "IX_KPIScore_PeriodTypeId",
                table: "KPIScore",
                newName: "IX_KPIScore_ObjectiveId");

            migrationBuilder.RenameColumn(
                name: "PeriodTypeId",
                table: "Contributions",
                newName: "ObjectiveId");

            migrationBuilder.RenameIndex(
                name: "IX_Contributions_PeriodTypeId",
                table: "Contributions",
                newName: "IX_Contributions_ObjectiveId");

            migrationBuilder.RenameColumn(
                name: "PeriodTypeId",
                table: "Comments",
                newName: "ObjectiveId");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_PeriodTypeId",
                table: "Comments",
                newName: "IX_Comments_ObjectiveId");

            migrationBuilder.RenameColumn(
                name: "PeriodTypeId",
                table: "AttitudeScore",
                newName: "ObjectiveId");

            migrationBuilder.RenameIndex(
                name: "IX_AttitudeScore_PeriodTypeId",
                table: "AttitudeScore",
                newName: "IX_AttitudeScore_ObjectiveId");

            migrationBuilder.AddColumn<string>(
                name: "PeriodType",
                table: "KPIScore",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PeriodType",
                table: "AttitudeScore",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AttitudeScore_Objectives_ObjectiveId",
                table: "AttitudeScore",
                column: "ObjectiveId",
                principalTable: "Objectives",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Objectives_ObjectiveId",
                table: "Comments",
                column: "ObjectiveId",
                principalTable: "Objectives",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Contributions_Objectives_ObjectiveId",
                table: "Contributions",
                column: "ObjectiveId",
                principalTable: "Objectives",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_KPIScore_Objectives_ObjectiveId",
                table: "KPIScore",
                column: "ObjectiveId",
                principalTable: "Objectives",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
