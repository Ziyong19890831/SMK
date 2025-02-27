using Microsoft.EntityFrameworkCore.Migrations;

namespace SMK.Data.Migrations
{
    public partial class Create_Table_Feat_RFP_Health_insurance_card_passing : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ICcardByMonth",
                columns: table => new
                {
                    HospID = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    ICCard_YM = table.Column<string>(type: "nvarchar(7)", maxLength: 7, nullable: false),
                    LastContType = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    HospName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ICCard_Times = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ICcardByMonth", x => new { x.HospID, x.ICCard_YM });
                });

            migrationBuilder.CreateTable(
                name: "ICNotFound",
                columns: table => new
                {
                    FeeYM = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: false),
                    Data_id = table.Column<string>(type: "nvarchar(28)", maxLength: 28, nullable: false),
                    CureType = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    BranchName = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    LastContType = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    HospID = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    HospSeqNo = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    HospName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    HospDataType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ID = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Birthday = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: true),
                    FuncDate = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: true),
                    CaseType = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    SeqNo = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Real_HospID = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    ExpDot = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Note = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ICNotFound", x => new { x.FeeYM, x.Data_id, x.CureType });
                });

            migrationBuilder.CreateTable(
                name: "ICRateByMonth",
                columns: table => new
                {
                    HospID = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    HospSeqNo = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    FeeYM = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: false),
                    CureType = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    LastContType = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    HospDataType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    HospName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    samples = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Rate = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    NoRate = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ICRateByMonth", x => new { x.HospID, x.HospSeqNo, x.FeeYM, x.CureType });
                });

            migrationBuilder.CreateTable(
                name: "ICRateLately",
                columns: table => new
                {
                    HospID = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    HospSeqNo = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    FeeYM = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: false),
                    LastContType = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    HospDataType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    HospName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MedIC = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    MedApply = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    MedRate = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    InsIC = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    InsApply = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    InsRate = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ICRateLately", x => new { x.HospID, x.HospSeqNo, x.FeeYM });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ICcardByMonth");

            migrationBuilder.DropTable(
                name: "ICNotFound");

            migrationBuilder.DropTable(
                name: "ICRateByMonth");

            migrationBuilder.DropTable(
                name: "ICRateLately");
        }
    }
}
