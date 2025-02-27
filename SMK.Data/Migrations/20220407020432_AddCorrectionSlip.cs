using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SMK.Data.Migrations
{
    public partial class AddCorrectionSlip : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CorrectionSlip",
                columns: table => new
                {
                    CaseNo = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: false),
                    ReceiveDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HospId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HospName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ID = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Birthday = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CorrectBasic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CorrectHosp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CorrectHealth = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CorrectOther = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CorrectItems = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CorrectItems2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    source = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Memo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(127)", maxLength: 127, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CorrectionSlip", x => x.CaseNo);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CorrectionSlip_CaseNo",
                table: "CorrectionSlip",
                column: "CaseNo");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CorrectionSlip");
        }
    }
}
