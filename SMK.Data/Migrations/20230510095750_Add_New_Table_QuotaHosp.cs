using Microsoft.EntityFrameworkCore.Migrations;

namespace SMK.Data.Migrations
{
    public partial class Add_New_Table_QuotaHosp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QuotaHosp",
                columns: table => new
                {
                    Serial_Number = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    QuotaYear = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    FeeYM = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: false),
                    FeeYMD = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    HospID = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    HospSeqNo = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    HospName = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    LastContType = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    ApplyTreat = table.Column<bool>(type: "bit", nullable: false),
                    ApplyTreatPeople = table.Column<int>(type: "int", nullable: true),
                    ApplyHealthEdu = table.Column<bool>(type: "bit", nullable: false),
                    ApplyHealthEduPeople = table.Column<int>(type: "int", nullable: true),
                    DesignedTreat = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DesignedTreatPeople = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ResultTreat = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ResultTreatPeople = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    VPNChangeNote = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DocumentNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IssueDate = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Treatment = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ScanFileName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(127)", maxLength: 127, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuotaHosp", x => x.Serial_Number);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuotaHosp");
        }
    }
}
