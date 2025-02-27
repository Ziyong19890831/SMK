using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SMK.Data.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GenBranch",
                columns: table => new
                {
                    BranchNo = table.Column<string>(unicode: false, fixedLength: true, maxLength: 1, nullable: false),
                    BranchName = table.Column<string>(unicode: false, maxLength: 20, nullable: true, defaultValueSql: "('')")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GenBranch_1", x => x.BranchNo);
                });

            migrationBuilder.CreateTable(
                name: "GenEmpData",
                columns: table => new
                {
                    EmpNo = table.Column<string>(maxLength: 10, nullable: false),
                    EmpName = table.Column<string>(maxLength: 100, nullable: true),
                    LoginID = table.Column<string>(maxLength: 20, nullable: true),
                    LoginPWD = table.Column<string>(maxLength: 50, nullable: true),
                    Enable = table.Column<bool>(nullable: false),
                    CreateAt = table.Column<DateTime>(nullable: false),
                    UpdateAt = table.Column<DateTime>(nullable: true),
                    UpdateBy = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GenEmpData", x => x.EmpNo);
                });

            migrationBuilder.CreateTable(
                name: "GenEndReason",
                columns: table => new
                {
                    EndReasonNo = table.Column<string>(unicode: false, fixedLength: true, maxLength: 2, nullable: false),
                    EndReasonName = table.Column<string>(unicode: false, maxLength: 20, nullable: true, defaultValueSql: "('')")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GenEndReason_1", x => x.EndReasonNo);
                });

            migrationBuilder.CreateTable(
                name: "GenHospCont",
                columns: table => new
                {
                    HospContType = table.Column<string>(unicode: false, fixedLength: true, maxLength: 1, nullable: false),
                    HospContName = table.Column<string>(unicode: false, maxLength: 20, nullable: true, defaultValueSql: "('')")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GenHospCont_1", x => x.HospContType);
                });

            migrationBuilder.CreateTable(
                name: "GenLicenceType",
                columns: table => new
                {
                    LicenceType = table.Column<string>(unicode: false, fixedLength: true, maxLength: 2, nullable: false),
                    LicenceName = table.Column<string>(unicode: false, maxLength: 20, nullable: true, defaultValueSql: "('')")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GenLicenceType_1", x => x.LicenceType);
                });

            migrationBuilder.CreateTable(
                name: "GenProgramData",
                columns: table => new
                {
                    ProNo = table.Column<string>(maxLength: 30, nullable: false),
                    ParentId = table.Column<string>(maxLength: 30, nullable: true),
                    ProName = table.Column<string>(maxLength: 30, nullable: true),
                    ControllerName = table.Column<string>(maxLength: 30, nullable: true),
                    ActionName = table.Column<string>(maxLength: 30, nullable: true),
                    QueryParams = table.Column<string>(maxLength: 30, nullable: true),
                    Sort = table.Column<int>(nullable: false),
                    CreateAt = table.Column<DateTime>(nullable: false),
                    UpdateAt = table.Column<DateTime>(nullable: true),
                    UpdateBy = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GenProgramData", x => x.ProNo);
                });

            migrationBuilder.CreateTable(
                name: "GenPrsnType",
                columns: table => new
                {
                    PrsnType = table.Column<string>(unicode: false, fixedLength: true, maxLength: 1, nullable: false),
                    PrsnTypeName = table.Column<string>(unicode: false, maxLength: 20, nullable: true, defaultValueSql: "('')")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GenPrsnType_1", x => x.PrsnType);
                });

            migrationBuilder.CreateTable(
                name: "GenSMKContract",
                columns: table => new
                {
                    SMKContractType = table.Column<string>(unicode: false, fixedLength: true, maxLength: 2, nullable: false),
                    SMKContractName = table.Column<string>(unicode: false, maxLength: 20, nullable: true, defaultValueSql: "('')")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GenSMKContract_1", x => x.SMKContractType);
                });

            migrationBuilder.CreateTable(
                name: "GenSpecial",
                columns: table => new
                {
                    SpecialistNo = table.Column<string>(unicode: false, maxLength: 5, nullable: false),
                    SpecialistName = table.Column<string>(unicode: false, maxLength: 20, nullable: true, defaultValueSql: "('')")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GenSpecial_2", x => x.SpecialistNo);
                });

            migrationBuilder.CreateTable(
                name: "HospBasic",
                columns: table => new
                {
                    HospID = table.Column<string>(unicode: false, fixedLength: true, maxLength: 10, nullable: false),
                    HospSeqNo = table.Column<string>(unicode: false, fixedLength: true, maxLength: 2, nullable: false, defaultValueSql: "('')"),
                    HospName = table.Column<string>(maxLength: 80, nullable: true, defaultValueSql: "('')"),
                    HospTel = table.Column<string>(unicode: false, fixedLength: true, maxLength: 20, nullable: true, defaultValueSql: "('')"),
                    HospFax = table.Column<string>(unicode: false, fixedLength: true, maxLength: 20, nullable: true, defaultValueSql: "('')"),
                    HospEmail = table.Column<string>(unicode: false, maxLength: 60, nullable: true, defaultValueSql: "('')"),
                    Contact1 = table.Column<string>(fixedLength: true, maxLength: 20, nullable: true, defaultValueSql: "('')"),
                    ContactTel1 = table.Column<string>(unicode: false, fixedLength: true, maxLength: 20, nullable: true, defaultValueSql: "('')"),
                    ContactFax1 = table.Column<string>(unicode: false, fixedLength: true, maxLength: 20, nullable: true, defaultValueSql: "('')"),
                    ContactEmail1 = table.Column<string>(unicode: false, maxLength: 60, nullable: true, defaultValueSql: "('')"),
                    Contact2 = table.Column<string>(fixedLength: true, maxLength: 20, nullable: true, defaultValueSql: "('')"),
                    ContactTel2 = table.Column<string>(unicode: false, fixedLength: true, maxLength: 20, nullable: true, defaultValueSql: "('')"),
                    ContactFax2 = table.Column<string>(unicode: false, fixedLength: true, maxLength: 20, nullable: true, defaultValueSql: "('')"),
                    ContactEmail2 = table.Column<string>(unicode: false, maxLength: 60, nullable: true, defaultValueSql: "('')"),
                    HospOwnName = table.Column<string>(fixedLength: true, maxLength: 20, nullable: true, defaultValueSql: "('')"),
                    HospOwnID = table.Column<string>(unicode: false, fixedLength: true, maxLength: 10, nullable: true, defaultValueSql: "('')"),
                    BranchNo = table.Column<string>(unicode: false, fixedLength: true, maxLength: 1, nullable: true, defaultValueSql: "('')"),
                    ZIP = table.Column<string>(unicode: false, fixedLength: true, maxLength: 5, nullable: true, defaultValueSql: "('')"),
                    DivisionNo = table.Column<string>(unicode: false, fixedLength: true, maxLength: 2, nullable: true, defaultValueSql: "('')"),
                    SubDivisionNo = table.Column<string>(unicode: false, fixedLength: true, maxLength: 10, nullable: true, defaultValueSql: "('')"),
                    HospAddress = table.Column<string>(unicode: false, maxLength: 80, nullable: true, defaultValueSql: "('')"),
                    HospStatus = table.Column<string>(unicode: false, fixedLength: true, maxLength: 1, nullable: true, defaultValueSql: "('')"),
                    FirstHospID = table.Column<string>(unicode: false, fixedLength: true, maxLength: 10, nullable: true, defaultValueSql: "('')"),
                    PrevHospID = table.Column<string>(unicode: false, fixedLength: true, maxLength: 10, nullable: true, defaultValueSql: "('')"),
                    LastHospID = table.Column<string>(unicode: false, fixedLength: true, maxLength: 10, nullable: true, defaultValueSql: "('')"),
                    LastContType = table.Column<string>(unicode: false, fixedLength: true, maxLength: 1, nullable: true, defaultValueSql: "('')"),
                    Remark = table.Column<string>(unicode: false, maxLength: 200, nullable: true, defaultValueSql: "('')"),
                    chFlg1 = table.Column<string>(unicode: false, fixedLength: true, maxLength: 1, nullable: true, defaultValueSql: "('')"),
                    chFlg2 = table.Column<string>(unicode: false, fixedLength: true, maxLength: 1, nullable: true, defaultValueSql: "('')"),
                    chFlg3 = table.Column<string>(unicode: false, fixedLength: true, maxLength: 1, nullable: true, defaultValueSql: "('')"),
                    PrevHospSeqNo = table.Column<string>(unicode: false, fixedLength: true, maxLength: 2, nullable: true, defaultValueSql: "('')"),
                    LastHospSeqNo = table.Column<string>(unicode: false, fixedLength: true, maxLength: 2, nullable: true, defaultValueSql: "('')"),
                    FirstHospSeqNo = table.Column<string>(unicode: false, fixedLength: true, maxLength: 2, nullable: true, defaultValueSql: "('')"),
                    HospAbbr = table.Column<string>(maxLength: 10, nullable: true, defaultValueSql: "('')")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SMKHospBasic", x => new { x.HospID, x.HospSeqNo });
                });

            migrationBuilder.CreateTable(
                name: "HospContract",
                columns: table => new
                {
                    HospID = table.Column<string>(unicode: false, fixedLength: true, maxLength: 10, nullable: false),
                    SMKContractType = table.Column<string>(unicode: false, fixedLength: true, maxLength: 2, nullable: false),
                    HospStartDate = table.Column<string>(unicode: false, fixedLength: true, maxLength: 8, nullable: false),
                    HospSeqNo = table.Column<string>(unicode: false, fixedLength: true, maxLength: 2, nullable: false),
                    HospEndDate = table.Column<string>(unicode: false, fixedLength: true, maxLength: 8, nullable: true, defaultValueSql: "('')"),
                    EndReasonNo = table.Column<string>(unicode: false, fixedLength: true, maxLength: 2, nullable: true, defaultValueSql: "('')"),
                    CreateDate = table.Column<string>(unicode: false, fixedLength: true, maxLength: 8, nullable: true, defaultValueSql: "('')"),
                    ModifyDate = table.Column<string>(unicode: false, fixedLength: true, maxLength: 8, nullable: true, defaultValueSql: "('')"),
                    ModifyPersonNo = table.Column<int>(nullable: true),
                    Remark = table.Column<string>(unicode: false, maxLength: 200, nullable: true, defaultValueSql: "('')")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HospContract", x => new { x.HospID, x.HospSeqNo, x.SMKContractType, x.HospStartDate });
                });

            migrationBuilder.CreateTable(
                name: "PrsnBasic",
                columns: table => new
                {
                    PrsnID = table.Column<string>(unicode: false, fixedLength: true, maxLength: 10, nullable: false),
                    PrsnName = table.Column<string>(fixedLength: true, maxLength: 20, nullable: true, defaultValueSql: "('')"),
                    PrsnBirthday = table.Column<string>(unicode: false, fixedLength: true, maxLength: 8, nullable: true, defaultValueSql: "('')"),
                    PrsnType = table.Column<string>(unicode: false, fixedLength: true, maxLength: 1, nullable: true, defaultValueSql: "('')"),
                    MajorSpecialistNo = table.Column<string>(unicode: false, fixedLength: true, maxLength: 5, nullable: true, defaultValueSql: "('')"),
                    SubSpecialistNo = table.Column<string>(unicode: false, fixedLength: true, maxLength: 5, nullable: true, defaultValueSql: "('')"),
                    Remark = table.Column<string>(unicode: false, maxLength: 200, nullable: true, defaultValueSql: "('')")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrsnBasic", x => x.PrsnID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GenBranch");

            migrationBuilder.DropTable(
                name: "GenEmpData");

            migrationBuilder.DropTable(
                name: "GenEndReason");

            migrationBuilder.DropTable(
                name: "GenHospCont");

            migrationBuilder.DropTable(
                name: "GenLicenceType");

            migrationBuilder.DropTable(
                name: "GenProgramData");

            migrationBuilder.DropTable(
                name: "GenPrsnType");

            migrationBuilder.DropTable(
                name: "GenSMKContract");

            migrationBuilder.DropTable(
                name: "GenSpecial");

            migrationBuilder.DropTable(
                name: "HospBasic");

            migrationBuilder.DropTable(
                name: "HospContract");

            migrationBuilder.DropTable(
                name: "PrsnBasic");
        }
    }
}
