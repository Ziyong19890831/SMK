using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SMK.Data.Migrations
{
    public partial class Create_UpdDrDtl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "updDrDtl",
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
                    ModifyDate = table.Column<string>(unicode: false, fixedLength: true, maxLength: 8, nullable: true, defaultValueSql: "('')"),
                    ModifyPersonNo = table.Column<int>(nullable: true),
                    Remark = table.Column<string>(unicode: false, maxLength: 50, nullable: true, defaultValueSql: "('')"),
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
                    table.PrimaryKey("PK_updDrDtl", x => new { x.data_id, x.fee_ym });
                });

            migrationBuilder.UpdateData(
                table: "GenEmpData",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2020, 10, 25, 3, 17, 39, 176, DateTimeKind.Local).AddTicks(3566));

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2020, 10, 25, 3, 17, 39, 179, DateTimeKind.Local).AddTicks(5426));

            migrationBuilder.UpdateData(
                table: "RoleEmpMapping",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2020, 10, 25, 3, 17, 39, 179, DateTimeKind.Local).AddTicks(7801));

            migrationBuilder.CreateIndex(
                name: "INX_UDrDtl",
                table: "updDrDtl",
                columns: new[] { "id", "fee_ym", "HospID", "func_date", "birthday", "MedApply", "WeekCount", "drug_days", "orig_hosp_id" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "updDrDtl");

            migrationBuilder.UpdateData(
                table: "GenEmpData",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2020, 10, 25, 3, 8, 56, 143, DateTimeKind.Local).AddTicks(2745));

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2020, 10, 25, 3, 8, 56, 148, DateTimeKind.Local).AddTicks(9917));

            migrationBuilder.UpdateData(
                table: "RoleEmpMapping",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2020, 10, 25, 3, 8, 56, 149, DateTimeKind.Local).AddTicks(1937));
        }
    }
}
