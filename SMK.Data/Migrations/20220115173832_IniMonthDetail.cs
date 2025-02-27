using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SMK.Data.Migrations
{
    public partial class IniMonthDetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IniMonthDetail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContractYM = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: true),
                    ContractTotal = table.Column<int>(type: "int", nullable: false),
                    ContractAllTotal = table.Column<int>(type: "int", nullable: false),
                    RunTimeContractAllTotal = table.Column<int>(type: "int", nullable: false),
                    ContractPersonTotal = table.Column<int>(type: "int", nullable: false),
                    ContractPersonAllTotal = table.Column<int>(type: "int", nullable: false),
                    RunTimeContractPersonAllTotal = table.Column<int>(type: "int", nullable: false),
                    NhiYM = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: true),
                    TreatInstructCnt = table.Column<int>(type: "int", nullable: false),
                    TreatCnt = table.Column<int>(type: "int", nullable: false),
                    InstructCnt = table.Column<int>(type: "int", nullable: false),
                    TreatInstructSum = table.Column<int>(type: "int", nullable: false),
                    TreatSum = table.Column<int>(type: "int", nullable: false),
                    InstructSum = table.Column<int>(type: "int", nullable: false),
                    TreatAddInstruct = table.Column<int>(type: "int", nullable: false),
                    TreatWeek = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IniMonthDetail", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IniMonthDetail_ContractYM",
                table: "IniMonthDetail",
                column: "ContractYM",
                unique: true,
                filter: "[ContractYM] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IniMonthDetail");
        }
    }
}
