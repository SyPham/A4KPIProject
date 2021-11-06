using Microsoft.EntityFrameworkCore.Migrations;

namespace A4KPI.Migrations
{
    public partial class AddNewTableTargetPICV11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TargetPIC",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    targetId = table.Column<int>(type: "int", nullable: false),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    IsSubmit = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TargetPIC", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TargetPIC_Targets_targetId",
                        column: x => x.targetId,
                        principalTable: "Targets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TargetPIC_targetId",
                table: "TargetPIC",
                column: "targetId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TargetPIC");
        }
    }
}
