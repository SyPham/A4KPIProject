using Microsoft.EntityFrameworkCore.Migrations;

namespace ScoreKPI.Migrations
{
    public partial class AddKPIScoreTbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Position",
                table: "AccountGroups",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "KPIScore",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PeriodType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Period = table.Column<int>(type: "int", nullable: false),
                    Point = table.Column<double>(type: "float", nullable: false),
                    ObjectiveId = table.Column<int>(type: "int", nullable: false),
                    ScoreBy = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KPIScore", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KPIScore_Objectives_ObjectiveId",
                        column: x => x.ObjectiveId,
                        principalTable: "Objectives",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_KPIScore_ObjectiveId",
                table: "KPIScore",
                column: "ObjectiveId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KPIScore");

            migrationBuilder.DropColumn(
                name: "Position",
                table: "AccountGroups");
        }
    }
}
