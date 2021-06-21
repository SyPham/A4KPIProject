using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ScoreKPI.Migrations
{
    public partial class InitialDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccountGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AccountTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Progresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Progresses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Periods",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    AccountGroupId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Periods", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Periods_AccountGroups_AccountGroupId",
                        column: x => x.AccountGroupId,
                        principalTable: "AccountGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Password = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    AccountTypeId = table.Column<int>(type: "int", nullable: true),
                    AccountGroupId = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ProgressId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Accounts_AccountGroups_AccountGroupId",
                        column: x => x.AccountGroupId,
                        principalTable: "AccountGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Accounts_AccountTypes_AccountTypeId",
                        column: x => x.AccountTypeId,
                        principalTable: "AccountTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Accounts_Progresses_ProgressId",
                        column: x => x.ProgressId,
                        principalTable: "Progresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ToDoList",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProgressId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ToDoList", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ToDoList_Progresses_ProgressId",
                        column: x => x.ProgressId,
                        principalTable: "Progresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PeriodReportTimes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PeriodId = table.Column<int>(type: "int", nullable: false),
                    ReportTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Createdy = table.Column<int>(type: "int", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PeriodReportTimes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PeriodReportTimes_Periods_PeriodId",
                        column: x => x.PeriodId,
                        principalTable: "Periods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Mailings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Url = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Frequency = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Report = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    TimeSend = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mailings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Mailings_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Plans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Topic = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Plans_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_AccountGroupId",
                table: "Accounts",
                column: "AccountGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_AccountTypeId",
                table: "Accounts",
                column: "AccountTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_ProgressId",
                table: "Accounts",
                column: "ProgressId");

            migrationBuilder.CreateIndex(
                name: "IX_Mailings_AccountId",
                table: "Mailings",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_PeriodReportTimes_PeriodId",
                table: "PeriodReportTimes",
                column: "PeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_Periods_AccountGroupId",
                table: "Periods",
                column: "AccountGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Plans_AccountId",
                table: "Plans",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_ToDoList_ProgressId",
                table: "ToDoList",
                column: "ProgressId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Mailings");

            migrationBuilder.DropTable(
                name: "PeriodReportTimes");

            migrationBuilder.DropTable(
                name: "Plans");

            migrationBuilder.DropTable(
                name: "ToDoList");

            migrationBuilder.DropTable(
                name: "Periods");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "AccountGroups");

            migrationBuilder.DropTable(
                name: "AccountTypes");

            migrationBuilder.DropTable(
                name: "Progresses");
        }
    }
}
