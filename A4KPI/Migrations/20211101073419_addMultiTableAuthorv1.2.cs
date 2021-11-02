using Microsoft.EntityFrameworkCore.Migrations;

namespace A4KPI.Migrations
{
    public partial class addMultiTableAuthorv12 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OptionInFunctionSystems_Options_OptionID",
                table: "OptionInFunctionSystems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OptionInFunctionSystems",
                table: "OptionInFunctionSystems");

            migrationBuilder.DropIndex(
                name: "IX_OptionInFunctionSystems_OptionID",
                table: "OptionInFunctionSystems");

            migrationBuilder.DropColumn(
                name: "ActionID",
                table: "OptionInFunctionSystems");

            migrationBuilder.AlterColumn<int>(
                name: "OptionID",
                table: "OptionInFunctionSystems",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_OptionInFunctionSystems",
                table: "OptionInFunctionSystems",
                columns: new[] { "OptionID", "FunctionSystemID" });

            migrationBuilder.AddForeignKey(
                name: "FK_OptionInFunctionSystems_Options_OptionID",
                table: "OptionInFunctionSystems",
                column: "OptionID",
                principalTable: "Options",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OptionInFunctionSystems_Options_OptionID",
                table: "OptionInFunctionSystems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OptionInFunctionSystems",
                table: "OptionInFunctionSystems");

            migrationBuilder.AlterColumn<int>(
                name: "OptionID",
                table: "OptionInFunctionSystems",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "ActionID",
                table: "OptionInFunctionSystems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_OptionInFunctionSystems",
                table: "OptionInFunctionSystems",
                columns: new[] { "ActionID", "FunctionSystemID" });

            migrationBuilder.CreateIndex(
                name: "IX_OptionInFunctionSystems_OptionID",
                table: "OptionInFunctionSystems",
                column: "OptionID");

            migrationBuilder.AddForeignKey(
                name: "FK_OptionInFunctionSystems_Options_OptionID",
                table: "OptionInFunctionSystems",
                column: "OptionID",
                principalTable: "Options",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
