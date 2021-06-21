using Microsoft.EntityFrameworkCore.Migrations;

namespace ScoreKPI.Migrations
{
    public partial class AddAccountGroupPeriodTbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Periods_AccountGroups_AccountGroupId",
                table: "Periods");

            migrationBuilder.AlterColumn<int>(
                name: "AccountGroupId",
                table: "Periods",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "AccountGroupPeriods",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountGroupId = table.Column<int>(type: "int", nullable: false),
                    PeriodId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountGroupPeriods", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccountGroupPeriods_AccountGroups_AccountGroupId",
                        column: x => x.AccountGroupId,
                        principalTable: "AccountGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccountGroupPeriods_Periods_PeriodId",
                        column: x => x.PeriodId,
                        principalTable: "Periods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountGroupPeriods_AccountGroupId",
                table: "AccountGroupPeriods",
                column: "AccountGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountGroupPeriods_PeriodId",
                table: "AccountGroupPeriods",
                column: "PeriodId");

            migrationBuilder.AddForeignKey(
                name: "FK_Periods_AccountGroups_AccountGroupId",
                table: "Periods",
                column: "AccountGroupId",
                principalTable: "AccountGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Periods_AccountGroups_AccountGroupId",
                table: "Periods");

            migrationBuilder.DropTable(
                name: "AccountGroupPeriods");

            migrationBuilder.AlterColumn<int>(
                name: "AccountGroupId",
                table: "Periods",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Periods_AccountGroups_AccountGroupId",
                table: "Periods",
                column: "AccountGroupId",
                principalTable: "AccountGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
