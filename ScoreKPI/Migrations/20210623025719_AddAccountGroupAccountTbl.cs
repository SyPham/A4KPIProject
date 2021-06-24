using Microsoft.EntityFrameworkCore.Migrations;

namespace ScoreKPI.Migrations
{
    public partial class AddAccountGroupAccountTbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccountGroupAccount",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountGroupId = table.Column<int>(type: "int", nullable: false),
                    AccountId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountGroupAccount", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccountGroupAccount_AccountGroups_AccountGroupId",
                        column: x => x.AccountGroupId,
                        principalTable: "AccountGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccountGroupAccount_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountGroupAccount_AccountGroupId",
                table: "AccountGroupAccount",
                column: "AccountGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountGroupAccount_AccountId",
                table: "AccountGroupAccount",
                column: "AccountId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountGroupAccount");
        }
    }
}
