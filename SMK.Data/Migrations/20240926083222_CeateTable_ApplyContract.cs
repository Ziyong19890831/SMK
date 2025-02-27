using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SMK.Data.Migrations
{
    public partial class CeateTable_ApplyContract : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApplyContract",
                columns: table => new
                {
                    HOSP_ID = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    HOSP_SEQ_NO = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    Cure_Type = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    CONT_S_DATE = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CONT_E_DATE = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    CREATE_USER_ID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TXT_USER_ID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TXT_DATE = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplyContract", x => new { x.HOSP_ID, x.HOSP_SEQ_NO, x.Cure_Type, x.CONT_S_DATE });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplyContract");
        }
    }
}
