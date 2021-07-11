using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace A4KPI.Migrations
{
    public partial class AddDeadlineToToDoListTbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "YourObjective",
                table: "ToDoList");

            migrationBuilder.AddColumn<DateTime>(
                name: "Deadline",
                table: "ToDoList",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Deadline",
                table: "ToDoList");

            migrationBuilder.AddColumn<string>(
                name: "YourObjective",
                table: "ToDoList",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
