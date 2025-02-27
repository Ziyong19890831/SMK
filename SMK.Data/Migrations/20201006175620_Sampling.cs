using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SMK.Data.Migrations
{
    public partial class Sampling : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DtlWithSet",
                columns: table => new
                {
                    Data_ID = table.Column<string>(unicode: false, fixedLength: true, maxLength: 28, nullable: false),
                    Fee_YM = table.Column<string>(unicode: false, fixedLength: true, maxLength: 6, nullable: false),
                    ID = table.Column<string>(unicode: false, fixedLength: true, maxLength: 10, nullable: true),
                    Func_Date = table.Column<string>(unicode: false, fixedLength: true, maxLength: 8, nullable: true),
                    LastHospID = table.Column<string>(unicode: false, fixedLength: true, maxLength: 10, nullable: true),
                    LastHospSeqNo = table.Column<string>(unicode: false, fixedLength: true, maxLength: 2, nullable: true),
                    HospID = table.Column<string>(unicode: false, fixedLength: true, maxLength: 10, nullable: true),
                    HospSeqNo = table.Column<string>(unicode: false, fixedLength: true, maxLength: 2, nullable: true),
                    ID_Sex = table.Column<string>(unicode: false, fixedLength: true, maxLength: 1, nullable: true),
                    Birthday = table.Column<string>(unicode: false, fixedLength: true, maxLength: 8, nullable: true),
                    Prsn_ID = table.Column<string>(unicode: false, fixedLength: true, maxLength: 10, nullable: true),
                    ExePrsnID = table.Column<string>(unicode: false, fixedLength: true, maxLength: 10, nullable: true),
                    HospContType = table.Column<string>(unicode: false, fixedLength: true, maxLength: 1, nullable: true),
                    MedQlty = table.Column<string>(unicode: false, fixedLength: true, maxLength: 1, nullable: false),
                    InsQlty = table.Column<string>(unicode: false, fixedLength: true, maxLength: 1, nullable: false),
                    Part_Code = table.Column<string>(unicode: false, fixedLength: true, maxLength: 3, nullable: true),
                    Func_Type = table.Column<string>(unicode: false, fixedLength: true, maxLength: 2, nullable: true),
                    Rel_Mode = table.Column<string>(unicode: false, fixedLength: true, maxLength: 1, nullable: true),
                    Drug_Days = table.Column<int>(nullable: true),
                    MedApply = table.Column<string>(unicode: false, fixedLength: true, maxLength: 1, nullable: true),
                    InstructApply = table.Column<string>(unicode: false, fixedLength: true, maxLength: 1, nullable: true),
                    TraceApply = table.Column<string>(unicode: false, fixedLength: true, maxLength: 1, nullable: true),
                    ReleaseApply = table.Column<string>(unicode: false, fixedLength: true, maxLength: 1, nullable: true),
                    Referral = table.Column<string>(unicode: false, fixedLength: true, maxLength: 1, nullable: true),
                    LowIncome = table.Column<string>(unicode: false, fixedLength: true, maxLength: 1, nullable: false),
                    Correction = table.Column<string>(unicode: false, fixedLength: true, maxLength: 1, nullable: false),
                    Drug = table.Column<string>(unicode: false, maxLength: 20, nullable: true),
                    Exp_Dot = table.Column<int>(nullable: true),
                    Part_Amt = table.Column<int>(nullable: true),
                    Appl_Dot = table.Column<int>(nullable: true),
                    ServiceFee = table.Column<int>(nullable: true),
                    DrugFee = table.Column<int>(nullable: true),
                    DispensingFee = table.Column<int>(nullable: true),
                    InstructionFee = table.Column<int>(nullable: true),
                    TracingFee = table.Column<int>(nullable: true),
                    ReferralFee = table.Column<int>(nullable: true),
                    ExamYear = table.Column<string>(unicode: false, fixedLength: true, maxLength: 4, nullable: true),
                    ExamTime = table.Column<int>(nullable: true),
                    FirstTreatDate = table.Column<string>(unicode: false, fixedLength: true, maxLength: 8, nullable: true),
                    WeekCount = table.Column<int>(nullable: true),
                    InstructExamYear = table.Column<string>(unicode: false, fixedLength: true, maxLength: 4, nullable: true),
                    InstructExamTime = table.Column<int>(nullable: true),
                    FirstInstructDate = table.Column<string>(unicode: false, fixedLength: true, maxLength: 8, nullable: true),
                    InctructSerial = table.Column<int>(nullable: true),
                    Orig_Hosp_ID = table.Column<string>(unicode: false, fixedLength: true, maxLength: 10, nullable: true),
                    Patch_N = table.Column<string>(unicode: false, fixedLength: true, maxLength: 1, nullable: false),
                    Patch_S = table.Column<string>(unicode: false, fixedLength: true, maxLength: 1, nullable: false),
                    Gum_Lozenge = table.Column<string>(unicode: false, fixedLength: true, maxLength: 1, nullable: false),
                    Inhaler = table.Column<string>(unicode: false, fixedLength: true, maxLength: 1, nullable: false),
                    Bupropion = table.Column<string>(unicode: false, fixedLength: true, maxLength: 1, nullable: false),
                    Varenicline = table.Column<string>(unicode: false, fixedLength: true, maxLength: 1, nullable: false),
                    PrsnType = table.Column<string>(unicode: false, maxLength: 20, nullable: true),
                    Visits = table.Column<int>(nullable: true),
                    DrTreat = table.Column<string>(unicode: false, fixedLength: true, maxLength: 1, nullable: false),
                    DentistTreat = table.Column<string>(unicode: false, fixedLength: true, maxLength: 1, nullable: false),
                    PharTreat = table.Column<string>(unicode: false, fixedLength: true, maxLength: 1, nullable: false),
                    EduTreat = table.Column<string>(unicode: false, fixedLength: true, maxLength: 1, nullable: false),
                    Remark = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    UnCount = table.Column<string>(unicode: false, maxLength: 10, nullable: false),
                    Func_Month = table.Column<string>(unicode: false, fixedLength: true, maxLength: 6, nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "MhbtAgentPatient",
                columns: table => new
                {
                    HospID = table.Column<string>(unicode: false, maxLength: 10, nullable: false),
                    HospAgentCode = table.Column<string>(unicode: false, maxLength: 5, nullable: false),
                    ID = table.Column<string>(unicode: false, maxLength: 10, nullable: false),
                    BIRTHDAY = table.Column<DateTime>(type: "date", nullable: false, defaultValueSql: "(('') collate Chinese_Taiwan_Stroke_CI_AS)"),
                    Name = table.Column<string>(maxLength: 12, nullable: false, defaultValueSql: "(('') collate Chinese_Taiwan_Stroke_CI_AS)"),
                    Sex = table.Column<string>(unicode: false, maxLength: 1, nullable: false, defaultValueSql: "(('') collate Chinese_Taiwan_Stroke_CI_AS)"),
                    Inform_ADDR = table.Column<string>(maxLength: 120, nullable: true, defaultValueSql: "(('') collate Chinese_Taiwan_Stroke_CI_AS)"),
                    Tel_D_AC = table.Column<string>(unicode: false, maxLength: 4, nullable: true, defaultValueSql: "(('') collate Chinese_Taiwan_Stroke_CI_AS)"),
                    TEL_D = table.Column<string>(unicode: false, maxLength: 10, nullable: true),
                    TEL_D_ENTX = table.Column<string>(unicode: false, maxLength: 5, nullable: true),
                    Tel_N_AC = table.Column<string>(unicode: false, maxLength: 4, nullable: true, defaultValueSql: "(('') collate Chinese_Taiwan_Stroke_CI_AS)"),
                    TEL_N = table.Column<string>(unicode: false, maxLength: 10, nullable: true),
                    TEL_N_ENTX = table.Column<string>(unicode: false, maxLength: 5, nullable: true),
                    Tel_M = table.Column<string>(unicode: false, maxLength: 15, nullable: true, defaultValueSql: "(('') collate Chinese_Taiwan_Stroke_CI_AS)"),
                    Seq_No = table.Column<string>(unicode: false, maxLength: 20, nullable: true, defaultValueSql: "(('') collate Chinese_Taiwan_Stroke_CI_AS)"),
                    Branch_Code = table.Column<string>(unicode: false, maxLength: 1, nullable: false, defaultValueSql: "(('') collate Chinese_Taiwan_Stroke_CI_AS)"),
                    TXT_Date = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(('') collate Chinese_Taiwan_Stroke_CI_AS)"),
                    Func_Mark = table.Column<string>(unicode: false, maxLength: 1, nullable: true, defaultValueSql: "(('') collate Chinese_Taiwan_Stroke_CI_AS)"),
                    Town_Code = table.Column<string>(unicode: false, maxLength: 4, nullable: true, defaultValueSql: "(('') collate Chinese_Taiwan_Stroke_CI_AS)"),
                    Town_Name = table.Column<string>(maxLength: 10, nullable: true, defaultValueSql: "(('') collate Chinese_Taiwan_Stroke_CI_AS)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MhbtAgentPatient_1", x => new { x.HospID, x.HospAgentCode, x.ID, x.BIRTHDAY });
                });

            migrationBuilder.CreateTable(
                name: "MhbtQsCure",
                columns: table => new
                {
                    Hosp_ID = table.Column<string>(unicode: false, maxLength: 10, nullable: false),
                    ID = table.Column<string>(unicode: false, maxLength: 10, nullable: false),
                    Birthday = table.Column<DateTime>(type: "date", nullable: false),
                    Func_Date = table.Column<DateTime>(type: "date", nullable: false),
                    Cure_Item = table.Column<string>(unicode: false, maxLength: 10, nullable: false),
                    Hosp_Seq_No = table.Column<string>(unicode: false, maxLength: 2, nullable: false),
                    Cure_Num = table.Column<decimal>(type: "numeric(5, 1)", nullable: false),
                    Txt_Date = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(('') collate Chinese_Taiwan_Stroke_CI_AS)"),
                    Adjust_User_ID = table.Column<string>(unicode: false, maxLength: 30, nullable: false, defaultValueSql: "(('') collate Chinese_Taiwan_Stroke_CI_AS)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MhbtQsCure", x => new { x.Hosp_ID, x.ID, x.Birthday, x.Func_Date, x.Cure_Item, x.Hosp_Seq_No });
                });

            migrationBuilder.CreateTable(
                name: "MhbtQsData",
                columns: table => new
                {
                    Hosp_ID = table.Column<string>(unicode: false, maxLength: 10, nullable: false),
                    ID = table.Column<string>(unicode: false, maxLength: 10, nullable: false),
                    Birthday = table.Column<DateTime>(type: "date", nullable: false),
                    FuncDate = table.Column<DateTime>(type: "date", nullable: false),
                    Cure_Type = table.Column<string>(unicode: false, maxLength: 1, nullable: false),
                    HospSeqNo = table.Column<string>(unicode: false, maxLength: 2, nullable: false),
                    PRSN_ID = table.Column<string>(unicode: false, maxLength: 10, nullable: true, defaultValueSql: "(('') collate Chinese_Taiwan_Stroke_CI_AS)"),
                    Cure_Stage = table.Column<string>(unicode: false, maxLength: 1, nullable: true),
                    ExamYear = table.Column<string>(unicode: false, maxLength: 3, nullable: true),
                    Smoke_Year = table.Column<decimal>(type: "numeric(3, 0)", nullable: true),
                    Smoke_Mon = table.Column<decimal>(type: "numeric(2, 0)", nullable: true),
                    Smoke_Day_Num = table.Column<decimal>(type: "numeric(4, 0)", nullable: true),
                    Base_Weight = table.Column<decimal>(type: "numeric(4, 1)", nullable: true),
                    Cure_Week = table.Column<decimal>(type: "numeric(1, 0)", nullable: true),
                    Week_Tot = table.Column<decimal>(type: "numeric(2, 0)", nullable: true),
                    Smoke_First = table.Column<string>(unicode: false, maxLength: 1, nullable: true, defaultValueSql: "(('') collate Chinese_Taiwan_Stroke_CI_AS)"),
                    Smoke_Stop = table.Column<string>(unicode: false, maxLength: 1, nullable: true, defaultValueSql: "(('') collate Chinese_Taiwan_Stroke_CI_AS)"),
                    Smoke_No_Gp = table.Column<string>(unicode: false, maxLength: 1, nullable: true, defaultValueSql: "(('') collate Chinese_Taiwan_Stroke_CI_AS)"),
                    Smoke_Much = table.Column<string>(unicode: false, maxLength: 1, nullable: true, defaultValueSql: "(('') collate Chinese_Taiwan_Stroke_CI_AS)"),
                    Smoke_Bed = table.Column<string>(unicode: false, maxLength: 1, nullable: true, defaultValueSql: "(('') collate Chinese_Taiwan_Stroke_CI_AS)"),
                    Smoke_Sick = table.Column<string>(unicode: false, maxLength: 1, nullable: true, defaultValueSql: "(('') collate Chinese_Taiwan_Stroke_CI_AS)"),
                    Smoke_Score = table.Column<decimal>(type: "numeric(2, 0)", nullable: true, defaultValueSql: "(('') collate Chinese_Taiwan_Stroke_CI_AS)"),
                    Cure_Agree = table.Column<string>(unicode: false, maxLength: 1, nullable: true, defaultValueSql: "(('') collate Chinese_Taiwan_Stroke_CI_AS)"),
                    Branch_Code = table.Column<string>(unicode: false, maxLength: 1, nullable: false, defaultValueSql: "(('') collate Chinese_Taiwan_Stroke_CI_AS)"),
                    TXT_DATE = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(('') collate Chinese_Taiwan_Stroke_CI_AS)"),
                    ADJUST_USER_ID = table.Column<string>(unicode: false, maxLength: 30, nullable: false, defaultValueSql: "(('') collate Chinese_Taiwan_Stroke_CI_AS)"),
                    FEE_MARK = table.Column<string>(unicode: false, maxLength: 1, nullable: true, defaultValueSql: "(('') collate Chinese_Taiwan_Stroke_CI_AS)"),
                    CO_CHECK = table.Column<decimal>(type: "numeric(2, 0)", nullable: true, defaultValueSql: "(('') collate Chinese_Taiwan_Stroke_CI_AS)"),
                    TRACE_DATE = table.Column<DateTime>(type: "date", nullable: true, defaultValueSql: "(('') collate Chinese_Taiwan_Stroke_CI_AS)"),
                    TRACE_STATE = table.Column<string>(unicode: false, maxLength: 3, nullable: true),
                    CURE_STATE = table.Column<string>(unicode: false, maxLength: 2, nullable: true),
                    TRACE_CO_CHECK = table.Column<decimal>(type: "numeric(2, 0)", nullable: true, defaultValueSql: "(('') collate Chinese_Taiwan_Stroke_CI_AS)"),
                    Trace_Date2 = table.Column<DateTime>(type: "date", nullable: true, defaultValueSql: "(('') collate Chinese_Taiwan_Stroke_CI_AS)"),
                    Trace_State2 = table.Column<string>(unicode: false, maxLength: 3, nullable: true, defaultValueSql: "(('') collate Chinese_Taiwan_Stroke_CI_AS)"),
                    Cure_State2 = table.Column<string>(unicode: false, maxLength: 2, nullable: true, defaultValueSql: "(('') collate Chinese_Taiwan_Stroke_CI_AS)"),
                    Trace_Co_Check2 = table.Column<decimal>(type: "numeric(2, 0)", nullable: true, defaultValueSql: "(('') collate Chinese_Taiwan_Stroke_CI_AS)"),
                    Case_Source = table.Column<string>(unicode: false, maxLength: 1, nullable: true, defaultValueSql: "(('') collate Chinese_Taiwan_Stroke_CI_AS)"),
                    Case_Kind = table.Column<string>(unicode: false, maxLength: 1, nullable: true, defaultValueSql: "(('') collate Chinese_Taiwan_Stroke_CI_AS)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MhbtQsData", x => new { x.Hosp_ID, x.ID, x.Birthday, x.FuncDate, x.Cure_Type, x.HospSeqNo });
                });

            migrationBuilder.CreateTable(
                name: "MhbtQsState",
                columns: table => new
                {
                    Hosp_ID = table.Column<string>(unicode: false, maxLength: 10, nullable: false),
                    ID = table.Column<string>(unicode: false, maxLength: 10, nullable: false),
                    Birthday = table.Column<DateTime>(type: "date", nullable: false),
                    Func_Date = table.Column<DateTime>(type: "date", nullable: false),
                    Cure_State = table.Column<string>(unicode: false, maxLength: 1, nullable: false),
                    Cure_Type = table.Column<string>(unicode: false, maxLength: 1, nullable: false),
                    Hosp_Seq_No = table.Column<string>(unicode: false, maxLength: 2, nullable: false),
                    Cure_State_Other = table.Column<string>(maxLength: 60, nullable: true, defaultValueSql: "(('') collate Chinese_Taiwan_Stroke_CI_AS)"),
                    Txt_Date = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(('') collate Chinese_Taiwan_Stroke_CI_AS)"),
                    Adjust_User_ID = table.Column<string>(unicode: false, maxLength: 30, nullable: false, defaultValueSql: "(('') collate Chinese_Taiwan_Stroke_CI_AS)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MhbtQsState", x => new { x.Hosp_ID, x.ID, x.Birthday, x.Func_Date, x.Cure_State, x.Cure_Type, x.Hosp_Seq_No });
                });

            migrationBuilder.CreateTable(
                name: "SamplingData",
                columns: table => new
                {
                    fee_ym = table.Column<string>(unicode: false, fixedLength: true, maxLength: 6, nullable: false),
                    data_id = table.Column<string>(unicode: false, maxLength: 28, nullable: false),
                    order_seq_no = table.Column<int>(nullable: false),
                    review = table.Column<string>(unicode: false, maxLength: 20, nullable: true),
                    reviewdate = table.Column<string>(unicode: false, maxLength: 8, nullable: true),
                    reviewremark = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    appeals = table.Column<string>(unicode: false, maxLength: 20, nullable: true),
                    appealsdate = table.Column<string>(unicode: false, maxLength: 8, nullable: true),
                    appealsremark = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    reviewamt = table.Column<int>(nullable: true),
                    appealsamt = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SamplingData", x => new { x.fee_ym, x.data_id, x.order_seq_no });
                });

            migrationBuilder.CreateTable(
                name: "SamplingList",
                columns: table => new
                {
                    fee_ym = table.Column<string>(unicode: false, fixedLength: true, maxLength: 6, nullable: false),
                    data_id = table.Column<string>(unicode: false, maxLength: 28, nullable: false),
                    SamplingNo = table.Column<string>(unicode: false, fixedLength: true, maxLength: 7, nullable: true),
                    ChkFlg = table.Column<string>(unicode: false, fixedLength: true, maxLength: 1, nullable: true),
                    accessdate = table.Column<string>(unicode: false, maxLength: 7, nullable: true),
                    accessno = table.Column<string>(unicode: false, maxLength: 6, nullable: true),
                    replydate = table.Column<string>(unicode: false, maxLength: 8, nullable: true),
                    replyno = table.Column<string>(unicode: false, maxLength: 6, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SamplingList", x => new { x.fee_ym, x.data_id });
                });

            migrationBuilder.UpdateData(
                table: "GenEmpData",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2020, 10, 7, 1, 56, 19, 714, DateTimeKind.Local).AddTicks(9551));

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2020, 10, 7, 1, 56, 19, 716, DateTimeKind.Local).AddTicks(9808));

            migrationBuilder.UpdateData(
                table: "RoleEmpMapping",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2020, 10, 7, 1, 56, 19, 717, DateTimeKind.Local).AddTicks(1940));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DtlWithSet");

            migrationBuilder.DropTable(
                name: "MhbtAgentPatient");

            migrationBuilder.DropTable(
                name: "MhbtQsCure");

            migrationBuilder.DropTable(
                name: "MhbtQsData");

            migrationBuilder.DropTable(
                name: "MhbtQsState");

            migrationBuilder.DropTable(
                name: "SamplingData");

            migrationBuilder.DropTable(
                name: "SamplingList");

            migrationBuilder.UpdateData(
                table: "GenEmpData",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2020, 7, 20, 23, 59, 12, 43, DateTimeKind.Local).AddTicks(7807));

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2020, 7, 20, 23, 59, 12, 45, DateTimeKind.Local).AddTicks(6502));

            migrationBuilder.UpdateData(
                table: "RoleEmpMapping",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2020, 7, 20, 23, 59, 12, 45, DateTimeKind.Local).AddTicks(7995));
        }
    }
}
