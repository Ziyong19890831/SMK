using Microsoft.EntityFrameworkCore.Migrations;

namespace SMK.Data.Migrations
{
    public partial class CreateNewTable_MhbtQsData2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MhbtQsData2",
                columns: table => new
                {
                    HospId = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    ID = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Birthday = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    FuncDate = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    CureStage = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    ExamYear = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    Cure_Type = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    HospSeqNo = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    BaseWeight = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Height = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PrsnID = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Trace_Co_Check3 = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    Trace_Date3 = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: true),
                    Cure_State3 = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    Trace_State3 = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MhbtQsData2", x => new { x.HospId, x.ID, x.Birthday, x.FuncDate, x.CureStage, x.ExamYear, x.Cure_Type, x.HospSeqNo });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MhbtQsData2");
        }
    }
}
