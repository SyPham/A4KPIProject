using Microsoft.EntityFrameworkCore.Migrations;

namespace ScoreKPI.Migrations
{
    public partial class RemoveAccountIdObjectiveTbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Objectives_Accounts_AccountId",
                table: "Objectives");

            migrationBuilder.AlterColumn<int>(
                name: "AccountId",
                table: "Objectives",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "PIC",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ObjectiveId = table.Column<int>(type: "int", nullable: false),
                    AccountId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PIC", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PIC_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PIC_Objectives_ObjectiveId",
                        column: x => x.ObjectiveId,
                        principalTable: "Objectives",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PIC_AccountId",
                table: "PIC",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_PIC_ObjectiveId",
                table: "PIC",
                column: "ObjectiveId");

            migrationBuilder.AddForeignKey(
                name: "FK_Objectives_Accounts_AccountId",
                table: "Objectives",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Objectives_Accounts_AccountId",
                table: "Objectives");

            migrationBuilder.DropTable(
                name: "PIC");

            migrationBuilder.AlterColumn<int>(
                name: "AccountId",
                table: "Objectives",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Objectives_Accounts_AccountId",
                table: "Objectives",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
