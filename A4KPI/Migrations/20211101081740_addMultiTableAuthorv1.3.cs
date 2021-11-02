using Microsoft.EntityFrameworkCore.Migrations;

namespace A4KPI.Migrations
{
    public partial class addMultiTableAuthorv13 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Permisions_Options_OptionID",
                table: "Permisions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Permisions",
                table: "Permisions");

            migrationBuilder.DropIndex(
                name: "IX_Permisions_OptionID",
                table: "Permisions");

            migrationBuilder.DropColumn(
                name: "ActionID",
                table: "Permisions");

            migrationBuilder.AlterColumn<int>(
                name: "OptionID",
                table: "Permisions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Permisions",
                table: "Permisions",
                columns: new[] { "OptionID", "FunctionSystemID", "RoleID" });

            migrationBuilder.AddForeignKey(
                name: "FK_Permisions_Options_OptionID",
                table: "Permisions",
                column: "OptionID",
                principalTable: "Options",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Permisions_Options_OptionID",
                table: "Permisions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Permisions",
                table: "Permisions");

            migrationBuilder.AlterColumn<int>(
                name: "OptionID",
                table: "Permisions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "ActionID",
                table: "Permisions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Permisions",
                table: "Permisions",
                columns: new[] { "ActionID", "FunctionSystemID", "RoleID" });

            migrationBuilder.CreateIndex(
                name: "IX_Permisions_OptionID",
                table: "Permisions",
                column: "OptionID");

            migrationBuilder.AddForeignKey(
                name: "FK_Permisions_Options_OptionID",
                table: "Permisions",
                column: "OptionID",
                principalTable: "Options",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
