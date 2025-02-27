using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SMK.Data.Migrations
{
    public partial class IniDr_IniOp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "iniDrDtl",
                columns: table => new
                {
                    data_id = table.Column<string>(unicode: false, maxLength: 28, nullable: false),
                    fee_ym = table.Column<string>(unicode: false, fixedLength: true, maxLength: 6, nullable: false),
                    HospID = table.Column<string>(unicode: false, fixedLength: true, maxLength: 10, nullable: true, defaultValueSql: "('')"),
                    ExamYear = table.Column<string>(unicode: false, fixedLength: true, maxLength: 4, nullable: true, defaultValueSql: "('')"),
                    ExamTime = table.Column<int>(nullable: true),
                    FirstTreatDate = table.Column<string>(unicode: false, fixedLength: true, maxLength: 8, nullable: true, defaultValueSql: "('')"),
                    WeekCount = table.Column<int>(nullable: true),
                    InstructExamYear = table.Column<string>(unicode: false, fixedLength: true, maxLength: 4, nullable: true, defaultValueSql: "('')"),
                    InstructExamTime = table.Column<int>(nullable: true),
                    FirstInstructDate = table.Column<string>(unicode: false, fixedLength: true, maxLength: 8, nullable: true, defaultValueSql: "('')"),
                    InctructSerial = table.Column<int>(nullable: true),
                    MedApply = table.Column<string>(unicode: false, fixedLength: true, maxLength: 1, nullable: true, defaultValueSql: "('')"),
                    InstructApply = table.Column<string>(unicode: false, fixedLength: true, maxLength: 1, nullable: true, defaultValueSql: "('')"),
                    TraceApply = table.Column<string>(unicode: false, fixedLength: true, maxLength: 1, nullable: true, defaultValueSql: "('')"),
                    ReleaseApply = table.Column<string>(unicode: false, fixedLength: true, maxLength: 1, nullable: true, defaultValueSql: "('')"),
                    appl_type = table.Column<string>(unicode: false, fixedLength: true, maxLength: 1, nullable: true, defaultValueSql: "('')"),
                    appl_date = table.Column<string>(unicode: false, fixedLength: true, maxLength: 8, nullable: true, defaultValueSql: "('')"),
                    case_type = table.Column<string>(unicode: false, fixedLength: true, maxLength: 1, nullable: true, defaultValueSql: "('')"),
                    seq_no = table.Column<int>(nullable: true),
                    func_type = table.Column<string>(unicode: false, fixedLength: true, maxLength: 2, nullable: true, defaultValueSql: "('')"),
                    func_date = table.Column<string>(unicode: false, fixedLength: true, maxLength: 8, nullable: true, defaultValueSql: "('')"),
                    rel_date = table.Column<string>(unicode: false, fixedLength: true, maxLength: 8, nullable: true, defaultValueSql: "('')"),
                    birthday = table.Column<string>(unicode: false, fixedLength: true, maxLength: 8, nullable: true, defaultValueSql: "('')"),
                    id = table.Column<string>(unicode: false, fixedLength: true, maxLength: 10, nullable: true, defaultValueSql: "('')"),
                    func_seq_no = table.Column<string>(unicode: false, fixedLength: true, maxLength: 10, nullable: true, defaultValueSql: "('')"),
                    pay_type = table.Column<string>(unicode: false, fixedLength: true, maxLength: 1, nullable: true, defaultValueSql: "('')"),
                    part_code = table.Column<string>(unicode: false, fixedLength: true, maxLength: 3, nullable: true, defaultValueSql: "('')"),
                    icd9cm_code = table.Column<string>(unicode: false, maxLength: 9, nullable: true, defaultValueSql: "('')"),
                    icd9cm_code1 = table.Column<string>(unicode: false, maxLength: 9, nullable: true, defaultValueSql: "('')"),
                    icd9cm_code2 = table.Column<string>(unicode: false, maxLength: 9, nullable: true, defaultValueSql: "('')"),
                    drug_days = table.Column<int>(nullable: true),
                    prsn_id = table.Column<string>(unicode: false, fixedLength: true, maxLength: 10, nullable: true, defaultValueSql: "('')"),
                    drug_prsn_id = table.Column<string>(unicode: false, fixedLength: true, maxLength: 10, nullable: true, defaultValueSql: "('')"),
                    drug_dot = table.Column<int>(nullable: true),
                    cure_dot = table.Column<int>(nullable: true),
                    dsvc_code = table.Column<string>(unicode: false, fixedLength: true, maxLength: 12, nullable: true, defaultValueSql: "('')"),
                    dsvc_dot = table.Column<int>(nullable: true),
                    exp_dot = table.Column<int>(nullable: true),
                    part_amt = table.Column<int>(nullable: true),
                    appl_dot = table.Column<int>(nullable: true),
                    orig_hosp_id = table.Column<string>(unicode: false, fixedLength: true, maxLength: 10, nullable: true, defaultValueSql: "('')"),
                    Id_Sex = table.Column<string>(unicode: false, fixedLength: true, maxLength: 1, nullable: true, defaultValueSql: "('')"),
                    cure_item1 = table.Column<string>(unicode: false, fixedLength: true, maxLength: 2, nullable: true, defaultValueSql: "('')"),
                    cure_item2 = table.Column<string>(unicode: false, fixedLength: true, maxLength: 2, nullable: true, defaultValueSql: "('')"),
                    cure_item3 = table.Column<string>(unicode: false, fixedLength: true, maxLength: 2, nullable: true, defaultValueSql: "('')"),
                    cure_item4 = table.Column<string>(unicode: false, fixedLength: true, maxLength: 2, nullable: true, defaultValueSql: "('')"),
                    orig_case_type = table.Column<string>(unicode: false, fixedLength: true, maxLength: 2, nullable: true, defaultValueSql: "('')"),
                    other_part_amt = table.Column<int>(nullable: true),
                    appl_cause_mark = table.Column<string>(unicode: false, fixedLength: true, maxLength: 1, nullable: true, defaultValueSql: "('')"),
                    icd10cm_code2 = table.Column<string>(unicode: false, maxLength: 9, nullable: true, defaultValueSql: "('')"),
                    icd10cm_code3 = table.Column<string>(unicode: false, maxLength: 9, nullable: true, defaultValueSql: "('')"),
                    icd10cm_code4 = table.Column<string>(unicode: false, maxLength: 9, nullable: true, defaultValueSql: "('')"),
                    corr_hosp_id = table.Column<string>(unicode: false, fixedLength: true, maxLength: 10, nullable: true, defaultValueSql: "('')"),
                    area_service = table.Column<string>(unicode: false, fixedLength: true, maxLength: 2, nullable: true, defaultValueSql: "('')"),
                    tran_date = table.Column<string>(unicode: false, fixedLength: true, maxLength: 8, nullable: true),
                    name = table.Column<string>(fixedLength: true, maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_iniDrDtl", x => new { x.data_id, x.fee_ym });
                });

            migrationBuilder.CreateTable(
                name: "iniDrOrd",
                columns: table => new
                {
                    data_id = table.Column<string>(unicode: false, maxLength: 28, nullable: false),
                    order_seq_no = table.Column<int>(nullable: false),
                    fee_ym = table.Column<string>(unicode: false, fixedLength: true, maxLength: 6, nullable: false),
                    order_type = table.Column<string>(unicode: false, fixedLength: true, maxLength: 1, nullable: true, defaultValueSql: "('')"),
                    order_code = table.Column<string>(unicode: false, fixedLength: true, maxLength: 12, nullable: true, defaultValueSql: "('')"),
                    drug_num = table.Column<string>(unicode: false, fixedLength: true, maxLength: 6, nullable: true, defaultValueSql: "('')"),
                    drug_fre = table.Column<string>(unicode: false, fixedLength: true, maxLength: 18, nullable: true, defaultValueSql: "('')"),
                    drug_path = table.Column<string>(unicode: false, fixedLength: true, maxLength: 15, nullable: true, defaultValueSql: "('')"),
                    order_uprice = table.Column<decimal>(type: "decimal(9, 2)", nullable: true),
                    order_qty = table.Column<decimal>(type: "decimal(7, 2)", nullable: true),
                    order_dot = table.Column<int>(nullable: true),
                    order_drug_day = table.Column<int>(nullable: true),
                    exe_prsn_id = table.Column<string>(unicode: false, fixedLength: true, maxLength: 10, nullable: true, defaultValueSql: "('')"),
                    tran_date = table.Column<string>(unicode: false, maxLength: 8, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_iniDrOrd_1", x => new { x.data_id, x.order_seq_no, x.fee_ym });
                });

            migrationBuilder.CreateTable(
                name: "iniOpDtl",
                columns: table => new
                {
                    data_id = table.Column<string>(unicode: false, maxLength: 28, nullable: false),
                    fee_ym = table.Column<string>(unicode: false, fixedLength: true, maxLength: 6, nullable: false),
                    ExamYear = table.Column<string>(unicode: false, fixedLength: true, maxLength: 4, nullable: true, defaultValueSql: "('')"),
                    ExamTime = table.Column<int>(nullable: true),
                    FirstTreatDate = table.Column<string>(unicode: false, fixedLength: true, maxLength: 8, nullable: true, defaultValueSql: "('')"),
                    WeekCount = table.Column<int>(nullable: true),
                    InstructExamYear = table.Column<string>(unicode: false, fixedLength: true, maxLength: 4, nullable: true, defaultValueSql: "('')"),
                    InstructExamTime = table.Column<int>(nullable: true),
                    FirstInstructDate = table.Column<string>(unicode: false, fixedLength: true, maxLength: 8, nullable: true, defaultValueSql: "('')"),
                    InctructSerial = table.Column<int>(nullable: true),
                    MedApply = table.Column<string>(unicode: false, fixedLength: true, maxLength: 1, nullable: true, defaultValueSql: "('')"),
                    InstructApply = table.Column<string>(unicode: false, fixedLength: true, maxLength: 1, nullable: true, defaultValueSql: "('')"),
                    TraceApply = table.Column<string>(unicode: false, fixedLength: true, maxLength: 1, nullable: true, defaultValueSql: "('')"),
                    ReleaseApply = table.Column<string>(unicode: false, fixedLength: true, maxLength: 1, nullable: true, defaultValueSql: "('')"),
                    appl_type = table.Column<string>(unicode: false, fixedLength: true, maxLength: 1, nullable: true, defaultValueSql: "('')"),
                    HospID = table.Column<string>(unicode: false, fixedLength: true, maxLength: 10, nullable: true, defaultValueSql: "('')"),
                    appl_date = table.Column<string>(unicode: false, fixedLength: true, maxLength: 8, nullable: true, defaultValueSql: "('')"),
                    case_type = table.Column<string>(unicode: false, fixedLength: true, maxLength: 2, nullable: true, defaultValueSql: "('')"),
                    seq_no = table.Column<int>(nullable: true),
                    func_type = table.Column<string>(unicode: false, fixedLength: true, maxLength: 2, nullable: true, defaultValueSql: "('')"),
                    func_date = table.Column<string>(unicode: false, fixedLength: true, maxLength: 8, nullable: true, defaultValueSql: "('')"),
                    cure_e_date = table.Column<string>(unicode: false, fixedLength: true, maxLength: 8, nullable: true, defaultValueSql: "('')"),
                    birthday = table.Column<string>(unicode: false, fixedLength: true, maxLength: 8, nullable: true, defaultValueSql: "('')"),
                    id = table.Column<string>(unicode: false, fixedLength: true, maxLength: 10, nullable: true, defaultValueSql: "('')"),
                    func_seq_no = table.Column<string>(unicode: false, fixedLength: true, maxLength: 10, nullable: true, defaultValueSql: "('')"),
                    pay_type = table.Column<string>(unicode: false, fixedLength: true, maxLength: 1, nullable: true, defaultValueSql: "('')"),
                    part_code = table.Column<string>(unicode: false, fixedLength: true, maxLength: 3, nullable: true, defaultValueSql: "('')"),
                    icd9cm_code = table.Column<string>(unicode: false, maxLength: 9, nullable: true, defaultValueSql: "('')"),
                    icd9cm_code1 = table.Column<string>(unicode: false, maxLength: 9, nullable: true, defaultValueSql: "('')"),
                    icd9cm_code2 = table.Column<string>(unicode: false, maxLength: 9, nullable: true, defaultValueSql: "('')"),
                    drug_days = table.Column<int>(nullable: true),
                    rel_mode = table.Column<string>(unicode: false, fixedLength: true, maxLength: 1, nullable: true, defaultValueSql: "('')"),
                    prsn_id = table.Column<string>(unicode: false, fixedLength: true, maxLength: 10, nullable: true, defaultValueSql: "('')"),
                    drug_prsn_id = table.Column<string>(unicode: false, fixedLength: true, maxLength: 10, nullable: true, defaultValueSql: "('')"),
                    drug_dot = table.Column<int>(nullable: true),
                    cure_dot = table.Column<int>(nullable: true),
                    diag_code = table.Column<string>(unicode: false, fixedLength: true, maxLength: 12, nullable: true, defaultValueSql: "('')"),
                    diag_dot = table.Column<int>(nullable: true),
                    dsvc_code = table.Column<string>(unicode: false, fixedLength: true, maxLength: 12, nullable: true, defaultValueSql: "('')"),
                    dsvc_dot = table.Column<int>(nullable: true),
                    exp_dot = table.Column<int>(nullable: true),
                    part_amt = table.Column<int>(nullable: true),
                    appl_dot = table.Column<int>(nullable: true),
                    Id_Sex = table.Column<string>(unicode: false, fixedLength: true, maxLength: 1, nullable: true, defaultValueSql: "('')"),
                    cure_item1 = table.Column<string>(unicode: false, fixedLength: true, maxLength: 2, nullable: true, defaultValueSql: "('')"),
                    cure_item2 = table.Column<string>(unicode: false, fixedLength: true, maxLength: 2, nullable: true, defaultValueSql: "('')"),
                    cure_item3 = table.Column<string>(unicode: false, fixedLength: true, maxLength: 2, nullable: true, defaultValueSql: "('')"),
                    cure_item4 = table.Column<string>(unicode: false, fixedLength: true, maxLength: 2, nullable: true, defaultValueSql: "('')"),
                    area_service = table.Column<string>(unicode: false, fixedLength: true, maxLength: 2, nullable: true, defaultValueSql: "('')"),
                    supp_area = table.Column<string>(unicode: false, fixedLength: true, maxLength: 4, nullable: true, defaultValueSql: "('')"),
                    real_hosp_id = table.Column<string>(unicode: false, fixedLength: true, maxLength: 10, nullable: true, defaultValueSql: "('')"),
                    hosp_data_type = table.Column<string>(unicode: false, fixedLength: true, maxLength: 2, nullable: true, defaultValueSql: "('')"),
                    agency_part_amt = table.Column<decimal>(type: "decimal(9, 2)", nullable: true),
                    name = table.Column<string>(fixedLength: true, maxLength: 20, nullable: true, defaultValueSql: "('')"),
                    appl_cause_mark = table.Column<string>(unicode: false, fixedLength: true, maxLength: 1, nullable: true, defaultValueSql: "('')"),
                    icd10cm_code3 = table.Column<string>(unicode: false, maxLength: 9, nullable: true, defaultValueSql: "('')"),
                    icd10cm_code4 = table.Column<string>(unicode: false, maxLength: 9, nullable: true, defaultValueSql: "('')"),
                    met_dot = table.Column<int>(nullable: true),
                    corr_hosp_id = table.Column<string>(unicode: false, fixedLength: true, maxLength: 10, nullable: true, defaultValueSql: "('')"),
                    tran_date = table.Column<string>(unicode: false, fixedLength: true, maxLength: 8, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_iniOpDtl", x => new { x.data_id, x.fee_ym });
                });

            migrationBuilder.CreateTable(
                name: "iniOpOrd",
                columns: table => new
                {
                    data_id = table.Column<string>(unicode: false, maxLength: 28, nullable: false),
                    order_seq_no = table.Column<int>(nullable: false),
                    fee_ym = table.Column<string>(unicode: false, fixedLength: true, maxLength: 6, nullable: false),
                    order_type = table.Column<string>(unicode: false, fixedLength: true, maxLength: 1, nullable: true, defaultValueSql: "('')"),
                    order_code = table.Column<string>(unicode: false, fixedLength: true, maxLength: 12, nullable: true, defaultValueSql: "('')"),
                    rel_mode = table.Column<string>(unicode: false, fixedLength: true, maxLength: 1, nullable: true, defaultValueSql: "('')"),
                    chr_mark = table.Column<string>(unicode: false, fixedLength: true, maxLength: 1, nullable: true, defaultValueSql: "('')"),
                    drug_num = table.Column<string>(unicode: false, fixedLength: true, maxLength: 6, nullable: true, defaultValueSql: "('')"),
                    drug_fre = table.Column<string>(unicode: false, fixedLength: true, maxLength: 18, nullable: true, defaultValueSql: "('')"),
                    drug_path = table.Column<string>(unicode: false, fixedLength: true, maxLength: 15, nullable: true, defaultValueSql: "('')"),
                    order_uprice = table.Column<decimal>(type: "decimal(9, 2)", nullable: true),
                    order_qty = table.Column<decimal>(type: "decimal(7, 2)", nullable: true),
                    order_dot = table.Column<int>(nullable: true),
                    exe_prsn_id = table.Column<string>(unicode: false, fixedLength: true, maxLength: 10, nullable: true, defaultValueSql: "('')"),
                    cure_path = table.Column<string>(unicode: false, fixedLength: true, maxLength: 20, nullable: true, defaultValueSql: "('')"),
                    order_drug_day = table.Column<int>(nullable: true),
                    tran_date = table.Column<string>(unicode: false, maxLength: 8, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_iniOpOrd", x => new { x.data_id, x.order_seq_no, x.fee_ym });
                });

            migrationBuilder.UpdateData(
                table: "GenEmpData",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2020, 7, 20, 21, 44, 53, 404, DateTimeKind.Local).AddTicks(4864));

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2020, 7, 20, 21, 44, 53, 406, DateTimeKind.Local).AddTicks(4176));

            migrationBuilder.UpdateData(
                table: "RoleEmpMapping",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2020, 7, 20, 21, 44, 53, 406, DateTimeKind.Local).AddTicks(5750));

            migrationBuilder.CreateIndex(
                name: "INX_DrDtl",
                table: "iniDrDtl",
                columns: new[] { "id", "fee_ym", "HospID", "func_date", "birthday", "MedApply", "WeekCount", "drug_days", "orig_hosp_id", "tran_date" });

            migrationBuilder.CreateIndex(
                name: "INX_DrOrd",
                table: "iniDrOrd",
                columns: new[] { "fee_ym", "order_code" });

            migrationBuilder.CreateIndex(
                name: "INX_OpDtl",
                table: "iniOpDtl",
                columns: new[] { "id", "fee_ym", "HospID", "func_date", "birthday", "MedApply", "WeekCount", "drug_days", "tran_date" });

            migrationBuilder.CreateIndex(
                name: "INX_OpOrd",
                table: "iniOpOrd",
                columns: new[] { "fee_ym", "order_code" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "iniDrDtl");

            migrationBuilder.DropTable(
                name: "iniDrOrd");

            migrationBuilder.DropTable(
                name: "iniOpDtl");

            migrationBuilder.DropTable(
                name: "iniOpOrd");

            migrationBuilder.UpdateData(
                table: "GenEmpData",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2020, 6, 4, 0, 56, 3, 954, DateTimeKind.Local).AddTicks(7114));

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2020, 6, 4, 0, 56, 3, 956, DateTimeKind.Local).AddTicks(6231));

            migrationBuilder.UpdateData(
                table: "RoleEmpMapping",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2020, 6, 4, 0, 56, 3, 956, DateTimeKind.Local).AddTicks(7728));
        }
    }
}
