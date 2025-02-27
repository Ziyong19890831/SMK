using Microsoft.EntityFrameworkCore.Migrations;

namespace SMK.Data.Migrations
{
    public partial class add_new_table_rfp_quit_smoking : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApplyPrsnChange",
                columns: table => new
                {
                    Serial_Number = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    FeeYM = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: false),
                    ChangeID = table.Column<bool>(type: "bit", nullable: false),
                    ChangeName = table.Column<bool>(type: "bit", nullable: false),
                    StartYMD = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: true),
                    Application_Type = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    ID = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    NewID = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    NewName = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    SingleChangeDate = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: true),
                    SingleNote = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(127)", maxLength: 127, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplyPrsnChange", x => x.Serial_Number);
                });

            migrationBuilder.CreateTable(
                name: "ApplyPrsnEnd",
                columns: table => new
                {
                    Serial_Number = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    FeeYM = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: false),
                    FeeYMD = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    Application_Type = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    HospID = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    HospSeqNo = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    HospName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MedicalID = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    MedicalName = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    Professional_Title = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    SingleChangeDate = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: true),
                    SingleNote = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(127)", maxLength: 127, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplyPrsnEnd", x => x.Serial_Number);
                });

            migrationBuilder.CreateTable(
                name: "ApplyPrsnNew",
                columns: table => new
                {
                    Serial_Number = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    FeeYM = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: false),
                    FeeYMD = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    Application_Type = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Birthday = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: true),
                    Smoking_Expiration_Date = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    Professional_Title = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    HospUserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    HospUserID = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    HospUserMail = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    Medical_Certificate = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    Smoking_Cessation_Certificate = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    Treatment = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    SMKLogDate = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(127)", maxLength: 127, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplyPrsnNew", x => x.Serial_Number);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplyPrsnChange");

            migrationBuilder.DropTable(
                name: "ApplyPrsnEnd");

            migrationBuilder.DropTable(
                name: "ApplyPrsnNew");
        }
    }
}
