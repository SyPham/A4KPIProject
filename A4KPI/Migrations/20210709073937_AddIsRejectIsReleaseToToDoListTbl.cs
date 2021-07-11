using Microsoft.EntityFrameworkCore.Migrations;

namespace A4KPI.Migrations
{
    public partial class AddIsRejectIsReleaseToToDoListTbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsReject",
                table: "Objectives");

            migrationBuilder.DropColumn(
                name: "IsRelease",
                table: "Objectives");

            migrationBuilder.AddColumn<bool>(
                name: "IsReject",
                table: "ToDoList",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsRelease",
                table: "ToDoList",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsReject",
                table: "ToDoList");

            migrationBuilder.DropColumn(
                name: "IsRelease",
                table: "ToDoList");

            migrationBuilder.AddColumn<bool>(
                name: "IsReject",
                table: "Objectives",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsRelease",
                table: "Objectives",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
