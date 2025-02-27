using Microsoft.EntityFrameworkCore.Migrations;

namespace SMK.Data.Migrations
{
    public partial class Insert_New_table_For_RFP_Hosp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApplyHospChange",
                columns: table => new
                {
                    Serial_Number = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    ChangeHospID = table.Column<bool>(type: "bit", nullable: false),
                    ChangeHospName = table.Column<bool>(type: "bit", nullable: false),
                    ChangeHospUserName = table.Column<bool>(type: "bit", nullable: false),
                    ChangeHospAddress = table.Column<bool>(type: "bit", nullable: false),
                    FeeYM = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: false),
                    FeeYMD = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    StartYMD = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    Application_Type = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    HospID = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    HospSeqNo = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    HospName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    HospUserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    HospAddress = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    NewHospID = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    NewHospSeqNo = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    NewHospName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NewHospUserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NewHospAddress = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplyHospChange", x => x.Serial_Number);
                });

            migrationBuilder.CreateTable(
                name: "ApplyHospEnd",
                columns: table => new
                {
                    Serial_Number = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    FeeYM = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: false),
                    FeeYMD = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    Application_Type = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    HospID = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    HospSeqNo = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    HospName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LastContType = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    BranchName = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    HospUserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    HospAddress = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    MinHospStartDate = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: true),
                    MaxHospEndDate = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: true),
                    Reason = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Note = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TTCNote = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SingleChangeDate = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: true),
                    SingleNote = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplyHospEnd", x => x.Serial_Number);
                });

            migrationBuilder.CreateTable(
                name: "ApplyHospNew",
                columns: table => new
                {
                    Serial_Number = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    FeeYM = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: false),
                    FeeYMD = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    Application_Type = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    HospID = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    HospSeqNo = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    HospName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LastContType = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    BranchName = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    SMKLogDate = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: true),
                    Note = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplyHospNew", x => x.Serial_Number);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplyHospChange");

            migrationBuilder.DropTable(
                name: "ApplyHospEnd");

            migrationBuilder.DropTable(
                name: "ApplyHospNew");
        }
    }
}
