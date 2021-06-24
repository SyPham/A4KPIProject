using Microsoft.EntityFrameworkCore.Migrations;

namespace ScoreKPI.Migrations
{
    public partial class UpdateRemoveForeignKeyProgressTbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ToDoList_Progresses_ProgressId",
                table: "ToDoList");

            migrationBuilder.DropIndex(
                name: "IX_ToDoList_ProgressId",
                table: "ToDoList");

            migrationBuilder.DropColumn(
                name: "ProgressId",
                table: "ToDoList");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProgressId",
                table: "ToDoList",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ToDoList_ProgressId",
                table: "ToDoList",
                column: "ProgressId");

            migrationBuilder.AddForeignKey(
                name: "FK_ToDoList_Progresses_ProgressId",
                table: "ToDoList",
                column: "ProgressId",
                principalTable: "Progresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
