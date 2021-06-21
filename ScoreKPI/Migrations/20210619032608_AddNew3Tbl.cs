using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ScoreKPI.Migrations
{
    public partial class AddNew3Tbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ToDoList_Progresses_ProgressId",
                table: "ToDoList");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "ToDoList",
                newName: "YourObjective");

            migrationBuilder.RenameColumn(
                name: "Createdy",
                table: "PeriodReportTimes",
                newName: "CreatedBy");

            migrationBuilder.AlterColumn<int>(
                name: "ProgressId",
                table: "ToDoList",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Action",
                table: "ToDoList",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ObjectiveId",
                table: "ToDoList",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Attitudes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Point = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attitudes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KPIs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Point = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KPIs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ResultOfMonth",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Month = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ObjectiveId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResultOfMonth", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResultOfMonth_Objectives_ObjectiveId",
                        column: x => x.ObjectiveId,
                        principalTable: "Objectives",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ToDoList_ObjectiveId",
                table: "ToDoList",
                column: "ObjectiveId");

            migrationBuilder.CreateIndex(
                name: "IX_ResultOfMonth_ObjectiveId",
                table: "ResultOfMonth",
                column: "ObjectiveId");

            migrationBuilder.AddForeignKey(
                name: "FK_ToDoList_Objectives_ObjectiveId",
                table: "ToDoList",
                column: "ObjectiveId",
                principalTable: "Objectives",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ToDoList_Progresses_ProgressId",
                table: "ToDoList",
                column: "ProgressId",
                principalTable: "Progresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ToDoList_Objectives_ObjectiveId",
                table: "ToDoList");

            migrationBuilder.DropForeignKey(
                name: "FK_ToDoList_Progresses_ProgressId",
                table: "ToDoList");

            migrationBuilder.DropTable(
                name: "Attitudes");

            migrationBuilder.DropTable(
                name: "KPIs");

            migrationBuilder.DropTable(
                name: "ResultOfMonth");

            migrationBuilder.DropIndex(
                name: "IX_ToDoList_ObjectiveId",
                table: "ToDoList");

            migrationBuilder.DropColumn(
                name: "Action",
                table: "ToDoList");

            migrationBuilder.DropColumn(
                name: "ObjectiveId",
                table: "ToDoList");

            migrationBuilder.RenameColumn(
                name: "YourObjective",
                table: "ToDoList",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "PeriodReportTimes",
                newName: "Createdy");

            migrationBuilder.AlterColumn<int>(
                name: "ProgressId",
                table: "ToDoList",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ToDoList_Progresses_ProgressId",
                table: "ToDoList",
                column: "ProgressId",
                principalTable: "Progresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
