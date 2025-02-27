using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SMK.Data.Migrations
{
    public partial class AddQsQuota : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.CreateTable(
                name: "CST_QS_QUOTA",
                columns: table => new
                {
                    YEARS = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    HOSP_ID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    HOSP_SEQ_NO = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CURE_TYPE = table.Column<int>(type: "int", nullable: false),
                    QUOTA = table.Column<int>(type: "int", nullable: true),
                    TXT_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ADJUST_USER_ID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VALID_S_DATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VALID_E_DATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    REMARK = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CREAT_USER_ID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CREAT_DATE = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CST_QS_QUOTA", x => new { x.YEARS, x.HOSP_ID, x.HOSP_SEQ_NO, x.CURE_TYPE });
                });

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CST_QS_QUOTA");


        }
    }
}
