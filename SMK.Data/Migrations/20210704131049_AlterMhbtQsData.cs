using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SMK.Data.Migrations
{
    public partial class AlterMhbtQsData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_MhbtQsData_1",
                table: "MhbtQsData");

            migrationBuilder.DropColumn(
                name: "CURE_STATE",
                table: "MhbtQsData");

            migrationBuilder.DropColumn(
                name: "TEL_D_ENTX",
                table: "MhbtAgentPatient");

            migrationBuilder.DropColumn(
                name: "TEL_N_ENTX",
                table: "MhbtAgentPatient");

            migrationBuilder.DropColumn(
                name: "Tel_D_AC",
                table: "MhbtAgentPatient");

            migrationBuilder.DropColumn(
                name: "Tel_N_AC",
                table: "MhbtAgentPatient");

            migrationBuilder.RenameColumn(
                name: "Week_Tot",
                table: "MhbtQsData",
                newName: "WeekTot");

            migrationBuilder.RenameColumn(
                name: "TRACE_STATE",
                table: "MhbtQsData",
                newName: "TraceState");

            migrationBuilder.RenameColumn(
                name: "TRACE_DATE",
                table: "MhbtQsData",
                newName: "TraceDate");

            migrationBuilder.RenameColumn(
                name: "TRACE_CO_CHECK",
                table: "MhbtQsData",
                newName: "TraceCoCheck");

            migrationBuilder.RenameColumn(
                name: "Smoke_Year",
                table: "MhbtQsData",
                newName: "SmokeYear");

            migrationBuilder.RenameColumn(
                name: "Smoke_Stop",
                table: "MhbtQsData",
                newName: "SmokeStop");

            migrationBuilder.RenameColumn(
                name: "Smoke_Sick",
                table: "MhbtQsData",
                newName: "SmokeSick");

            migrationBuilder.RenameColumn(
                name: "Smoke_Score",
                table: "MhbtQsData",
                newName: "SmokeScore");

            migrationBuilder.RenameColumn(
                name: "Smoke_No_Gp",
                table: "MhbtQsData",
                newName: "SmokeNoGp");

            migrationBuilder.RenameColumn(
                name: "Smoke_Much",
                table: "MhbtQsData",
                newName: "SmokeMuch");

            migrationBuilder.RenameColumn(
                name: "Smoke_Mon",
                table: "MhbtQsData",
                newName: "SmokeMon");

            migrationBuilder.RenameColumn(
                name: "Smoke_First",
                table: "MhbtQsData",
                newName: "SmokeFirst");

            migrationBuilder.RenameColumn(
                name: "Smoke_Day_Num",
                table: "MhbtQsData",
                newName: "SmokeDayNum");

            migrationBuilder.RenameColumn(
                name: "Smoke_Bed",
                table: "MhbtQsData",
                newName: "SmokeBed");

            migrationBuilder.RenameColumn(
                name: "PRSN_ID",
                table: "MhbtQsData",
                newName: "PrsnID");

            migrationBuilder.RenameColumn(
                name: "FEE_MARK",
                table: "MhbtQsData",
                newName: "FeeMark");

            migrationBuilder.RenameColumn(
                name: "Cure_Week",
                table: "MhbtQsData",
                newName: "CureWeek");

            migrationBuilder.RenameColumn(
                name: "Cure_Stage",
                table: "MhbtQsData",
                newName: "CureStage");

            migrationBuilder.RenameColumn(
                name: "Cure_Agree",
                table: "MhbtQsData",
                newName: "CureAgree");

            migrationBuilder.RenameColumn(
                name: "CO_CHECK",
                table: "MhbtQsData",
                newName: "CoCheck");

            migrationBuilder.RenameColumn(
                name: "Base_Weight",
                table: "MhbtQsData",
                newName: "BaseWeight");

            migrationBuilder.RenameColumn(
                name: "ADJUST_USER_ID",
                table: "MhbtQsData",
                newName: "AdjustUserID");

            migrationBuilder.RenameColumn(
                name: "TXT_DATE",
                table: "MhbtQsData",
                newName: "TxtDate");

            migrationBuilder.RenameColumn(
                name: "Branch_Code",
                table: "MhbtQsData",
                newName: "BranchCode");

            migrationBuilder.RenameColumn(
                name: "Hosp_ID",
                table: "MhbtQsData",
                newName: "HospId");

            migrationBuilder.RenameColumn(
                name: "BIRTHDAY",
                table: "MhbtAgentPatient",
                newName: "Birthday");

            migrationBuilder.RenameColumn(
                name: "Town_Name",
                table: "MhbtAgentPatient",
                newName: "TownName");

            migrationBuilder.RenameColumn(
                name: "Town_Code",
                table: "MhbtAgentPatient",
                newName: "TownCode");

            migrationBuilder.RenameColumn(
                name: "Tel_M",
                table: "MhbtAgentPatient",
                newName: "TelM");

            migrationBuilder.RenameColumn(
                name: "TEL_N",
                table: "MhbtAgentPatient",
                newName: "TelN");

            migrationBuilder.RenameColumn(
                name: "TEL_D",
                table: "MhbtAgentPatient",
                newName: "TelD");

            migrationBuilder.RenameColumn(
                name: "Seq_No",
                table: "MhbtAgentPatient",
                newName: "SeqNo");

            migrationBuilder.RenameColumn(
                name: "Inform_ADDR",
                table: "MhbtAgentPatient",
                newName: "InformAddr");

            migrationBuilder.RenameColumn(
                name: "Func_Mark",
                table: "MhbtAgentPatient",
                newName: "FuncMark");

            migrationBuilder.RenameColumn(
                name: "TXT_Date",
                table: "MhbtAgentPatient",
                newName: "TxtDate");

            migrationBuilder.RenameColumn(
                name: "Branch_Code",
                table: "MhbtAgentPatient",
                newName: "BranchCode");

            migrationBuilder.AlterColumn<string>(
                name: "Trace_Date2",
                table: "MhbtQsData",
                type: "nvarchar(max)",
                nullable: true,
                defaultValueSql: "(('') collate Chinese_Taiwan_Stroke_CI_AS)",
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldNullable: true,
                oldDefaultValueSql: "(('') collate Chinese_Taiwan_Stroke_CI_AS)");

            migrationBuilder.AlterColumn<string>(
                name: "ExamYear",
                table: "MhbtQsData",
                type: "varchar(900)",
                unicode: false,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(3)",
                oldUnicode: false,
                oldMaxLength: 3,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FuncDate",
                table: "MhbtQsData",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date");

            migrationBuilder.AlterColumn<string>(
                name: "Birthday",
                table: "MhbtQsData",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date");

            migrationBuilder.AlterColumn<string>(
                name: "TraceDate",
                table: "MhbtQsData",
                type: "nvarchar(max)",
                nullable: true,
                defaultValueSql: "(('') collate Chinese_Taiwan_Stroke_CI_AS)",
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldNullable: true,
                oldDefaultValueSql: "(('') collate Chinese_Taiwan_Stroke_CI_AS)");

            migrationBuilder.AlterColumn<string>(
                name: "CureStage",
                table: "MhbtQsData",
                type: "varchar(1)",
                unicode: false,
                maxLength: 1,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(1)",
                oldUnicode: false,
                oldMaxLength: 1,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "CoCheck",
                table: "MhbtQsData",
                type: "numeric(10,0)",
                nullable: true,
                defaultValueSql: "(('') collate Chinese_Taiwan_Stroke_CI_AS)",
                oldClrType: typeof(decimal),
                oldType: "numeric(2,0)",
                oldNullable: true,
                oldDefaultValueSql: "(('') collate Chinese_Taiwan_Stroke_CI_AS)");

            migrationBuilder.AlterColumn<decimal>(
                name: "BaseWeight",
                table: "MhbtQsData",
                type: "numeric(18,1)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(4,1)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TxtDate",
                table: "MhbtQsData",
                type: "nvarchar(max)",
                nullable: true,
                defaultValueSql: "(('') collate Chinese_Taiwan_Stroke_CI_AS)",
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValueSql: "(('') collate Chinese_Taiwan_Stroke_CI_AS)");

            migrationBuilder.AlterColumn<string>(
                name: "BranchCode",
                table: "MhbtQsData",
                type: "varchar(1)",
                unicode: false,
                maxLength: 1,
                nullable: true,
                defaultValueSql: "(('') collate Chinese_Taiwan_Stroke_CI_AS)",
                oldClrType: typeof(string),
                oldType: "varchar(1)",
                oldUnicode: false,
                oldMaxLength: 1,
                oldDefaultValueSql: "(('') collate Chinese_Taiwan_Stroke_CI_AS)");

            migrationBuilder.AddColumn<string>(
                name: "CurtState",
                table: "MhbtQsData",
                type: "varchar(900)",
                unicode: false,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ChType",
                table: "MhbtQsData",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CureStateOther",
                table: "MhbtQsData",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstTreatDate",
                table: "MhbtQsData",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SideEffect",
                table: "MhbtQsData",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SmokeLung",
                table: "MhbtQsData",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SmokeNico",
                table: "MhbtQsData",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "MhbtQsData",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Birthday",
                table: "MhbtAgentPatient",
                type: "nvarchar(450)",
                nullable: false,
                defaultValueSql: "(('') collate Chinese_Taiwan_Stroke_CI_AS)",
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldDefaultValueSql: "(('') collate Chinese_Taiwan_Stroke_CI_AS)");

            migrationBuilder.AlterColumn<string>(
                name: "TxtDate",
                table: "MhbtAgentPatient",
                type: "nvarchar(450)",
                nullable: false,
                defaultValueSql: "(('') collate Chinese_Taiwan_Stroke_CI_AS)",
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValueSql: "(('') collate Chinese_Taiwan_Stroke_CI_AS)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MhbtQsData_1",
                table: "MhbtQsData",
                columns: new[] { "HospId", "ID", "Birthday", "FuncDate", "CureStage", "ExamYear", "CurtState", "Cure_Type", "HospSeqNo" });

            migrationBuilder.CreateTable(
                name: "QuitDataAll",
                columns: table => new
                {
                    ID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CaseNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirstMonth = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TimeSpan = table.Column<int>(type: "int", nullable: false),
                    HospID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HospSeqNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Birthday = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Edition = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VisitDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Result = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QuitPnt = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QuitCtn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Edu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Job = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuitDataAll", x => x.ID);
                });

            migrationBuilder.UpdateData(
                table: "GenEmpData",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2021, 7, 4, 21, 10, 45, 427, DateTimeKind.Local).AddTicks(2672));

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2021, 7, 4, 21, 10, 45, 432, DateTimeKind.Local).AddTicks(7026));

            migrationBuilder.UpdateData(
                table: "RoleEmpMapping",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2021, 7, 4, 21, 10, 45, 432, DateTimeKind.Local).AddTicks(9856));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuitDataAll");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MhbtQsData_1",
                table: "MhbtQsData");

            migrationBuilder.DropColumn(
                name: "CurtState",
                table: "MhbtQsData");

            migrationBuilder.DropColumn(
                name: "ChType",
                table: "MhbtQsData");

            migrationBuilder.DropColumn(
                name: "CureStateOther",
                table: "MhbtQsData");

            migrationBuilder.DropColumn(
                name: "FirstTreatDate",
                table: "MhbtQsData");

            migrationBuilder.DropColumn(
                name: "SideEffect",
                table: "MhbtQsData");

            migrationBuilder.DropColumn(
                name: "SmokeLung",
                table: "MhbtQsData");

            migrationBuilder.DropColumn(
                name: "SmokeNico",
                table: "MhbtQsData");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "MhbtQsData");

            migrationBuilder.RenameColumn(
                name: "WeekTot",
                table: "MhbtQsData",
                newName: "Week_Tot");

            migrationBuilder.RenameColumn(
                name: "TxtDate",
                table: "MhbtQsData",
                newName: "TXT_DATE");

            migrationBuilder.RenameColumn(
                name: "TraceState",
                table: "MhbtQsData",
                newName: "TRACE_STATE");

            migrationBuilder.RenameColumn(
                name: "TraceDate",
                table: "MhbtQsData",
                newName: "TRACE_DATE");

            migrationBuilder.RenameColumn(
                name: "TraceCoCheck",
                table: "MhbtQsData",
                newName: "TRACE_CO_CHECK");

            migrationBuilder.RenameColumn(
                name: "SmokeYear",
                table: "MhbtQsData",
                newName: "Smoke_Year");

            migrationBuilder.RenameColumn(
                name: "SmokeStop",
                table: "MhbtQsData",
                newName: "Smoke_Stop");

            migrationBuilder.RenameColumn(
                name: "SmokeSick",
                table: "MhbtQsData",
                newName: "Smoke_Sick");

            migrationBuilder.RenameColumn(
                name: "SmokeScore",
                table: "MhbtQsData",
                newName: "Smoke_Score");

            migrationBuilder.RenameColumn(
                name: "SmokeNoGp",
                table: "MhbtQsData",
                newName: "Smoke_No_Gp");

            migrationBuilder.RenameColumn(
                name: "SmokeMuch",
                table: "MhbtQsData",
                newName: "Smoke_Much");

            migrationBuilder.RenameColumn(
                name: "SmokeMon",
                table: "MhbtQsData",
                newName: "Smoke_Mon");

            migrationBuilder.RenameColumn(
                name: "SmokeFirst",
                table: "MhbtQsData",
                newName: "Smoke_First");

            migrationBuilder.RenameColumn(
                name: "SmokeDayNum",
                table: "MhbtQsData",
                newName: "Smoke_Day_Num");

            migrationBuilder.RenameColumn(
                name: "SmokeBed",
                table: "MhbtQsData",
                newName: "Smoke_Bed");

            migrationBuilder.RenameColumn(
                name: "PrsnID",
                table: "MhbtQsData",
                newName: "PRSN_ID");

            migrationBuilder.RenameColumn(
                name: "FeeMark",
                table: "MhbtQsData",
                newName: "FEE_MARK");

            migrationBuilder.RenameColumn(
                name: "CureWeek",
                table: "MhbtQsData",
                newName: "Cure_Week");

            migrationBuilder.RenameColumn(
                name: "CureAgree",
                table: "MhbtQsData",
                newName: "Cure_Agree");

            migrationBuilder.RenameColumn(
                name: "CoCheck",
                table: "MhbtQsData",
                newName: "CO_CHECK");

            migrationBuilder.RenameColumn(
                name: "BranchCode",
                table: "MhbtQsData",
                newName: "Branch_Code");

            migrationBuilder.RenameColumn(
                name: "BaseWeight",
                table: "MhbtQsData",
                newName: "Base_Weight");

            migrationBuilder.RenameColumn(
                name: "AdjustUserID",
                table: "MhbtQsData",
                newName: "ADJUST_USER_ID");

            migrationBuilder.RenameColumn(
                name: "CureStage",
                table: "MhbtQsData",
                newName: "Cure_Stage");

            migrationBuilder.RenameColumn(
                name: "HospId",
                table: "MhbtQsData",
                newName: "Hosp_ID");

            migrationBuilder.RenameColumn(
                name: "Birthday",
                table: "MhbtAgentPatient",
                newName: "BIRTHDAY");

            migrationBuilder.RenameColumn(
                name: "TownName",
                table: "MhbtAgentPatient",
                newName: "Town_Name");

            migrationBuilder.RenameColumn(
                name: "TownCode",
                table: "MhbtAgentPatient",
                newName: "Town_Code");

            migrationBuilder.RenameColumn(
                name: "TelN",
                table: "MhbtAgentPatient",
                newName: "TEL_N");

            migrationBuilder.RenameColumn(
                name: "TelM",
                table: "MhbtAgentPatient",
                newName: "Tel_M");

            migrationBuilder.RenameColumn(
                name: "TelD",
                table: "MhbtAgentPatient",
                newName: "TEL_D");

            migrationBuilder.RenameColumn(
                name: "SeqNo",
                table: "MhbtAgentPatient",
                newName: "Seq_No");

            migrationBuilder.RenameColumn(
                name: "InformAddr",
                table: "MhbtAgentPatient",
                newName: "Inform_ADDR");

            migrationBuilder.RenameColumn(
                name: "FuncMark",
                table: "MhbtAgentPatient",
                newName: "Func_Mark");

            migrationBuilder.RenameColumn(
                name: "TxtDate",
                table: "MhbtAgentPatient",
                newName: "TXT_Date");

            migrationBuilder.RenameColumn(
                name: "BranchCode",
                table: "MhbtAgentPatient",
                newName: "Branch_Code");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Trace_Date2",
                table: "MhbtQsData",
                type: "date",
                nullable: true,
                defaultValueSql: "(('') collate Chinese_Taiwan_Stroke_CI_AS)",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldDefaultValueSql: "(('') collate Chinese_Taiwan_Stroke_CI_AS)");

            migrationBuilder.AlterColumn<string>(
                name: "ExamYear",
                table: "MhbtQsData",
                type: "varchar(3)",
                unicode: false,
                maxLength: 3,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(900)",
                oldUnicode: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "FuncDate",
                table: "MhbtQsData",
                type: "date",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Birthday",
                table: "MhbtQsData",
                type: "date",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "TXT_DATE",
                table: "MhbtQsData",
                type: "datetime",
                nullable: false,
                defaultValueSql: "(('') collate Chinese_Taiwan_Stroke_CI_AS)",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldDefaultValueSql: "(('') collate Chinese_Taiwan_Stroke_CI_AS)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "TRACE_DATE",
                table: "MhbtQsData",
                type: "date",
                nullable: true,
                defaultValueSql: "(('') collate Chinese_Taiwan_Stroke_CI_AS)",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldDefaultValueSql: "(('') collate Chinese_Taiwan_Stroke_CI_AS)");

            migrationBuilder.AlterColumn<decimal>(
                name: "CO_CHECK",
                table: "MhbtQsData",
                type: "numeric(2,0)",
                nullable: true,
                defaultValueSql: "(('') collate Chinese_Taiwan_Stroke_CI_AS)",
                oldClrType: typeof(decimal),
                oldType: "numeric(10,0)",
                oldNullable: true,
                oldDefaultValueSql: "(('') collate Chinese_Taiwan_Stroke_CI_AS)");

            migrationBuilder.AlterColumn<string>(
                name: "Branch_Code",
                table: "MhbtQsData",
                type: "varchar(1)",
                unicode: false,
                maxLength: 1,
                nullable: false,
                defaultValueSql: "(('') collate Chinese_Taiwan_Stroke_CI_AS)",
                oldClrType: typeof(string),
                oldType: "varchar(1)",
                oldUnicode: false,
                oldMaxLength: 1,
                oldNullable: true,
                oldDefaultValueSql: "(('') collate Chinese_Taiwan_Stroke_CI_AS)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Base_Weight",
                table: "MhbtQsData",
                type: "numeric(4,1)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,1)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Cure_Stage",
                table: "MhbtQsData",
                type: "varchar(1)",
                unicode: false,
                maxLength: 1,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(1)",
                oldUnicode: false,
                oldMaxLength: 1);

            migrationBuilder.AddColumn<string>(
                name: "CURE_STATE",
                table: "MhbtQsData",
                type: "varchar(2)",
                unicode: false,
                maxLength: 2,
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "BIRTHDAY",
                table: "MhbtAgentPatient",
                type: "date",
                nullable: false,
                defaultValueSql: "(('') collate Chinese_Taiwan_Stroke_CI_AS)",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldDefaultValueSql: "(('') collate Chinese_Taiwan_Stroke_CI_AS)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "TXT_Date",
                table: "MhbtAgentPatient",
                type: "datetime",
                nullable: false,
                defaultValueSql: "(('') collate Chinese_Taiwan_Stroke_CI_AS)",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldDefaultValueSql: "(('') collate Chinese_Taiwan_Stroke_CI_AS)");

            migrationBuilder.AddColumn<string>(
                name: "TEL_D_ENTX",
                table: "MhbtAgentPatient",
                type: "varchar(5)",
                unicode: false,
                maxLength: 5,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TEL_N_ENTX",
                table: "MhbtAgentPatient",
                type: "varchar(5)",
                unicode: false,
                maxLength: 5,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Tel_D_AC",
                table: "MhbtAgentPatient",
                type: "varchar(4)",
                unicode: false,
                maxLength: 4,
                nullable: true,
                defaultValueSql: "(('') collate Chinese_Taiwan_Stroke_CI_AS)");

            migrationBuilder.AddColumn<string>(
                name: "Tel_N_AC",
                table: "MhbtAgentPatient",
                type: "varchar(4)",
                unicode: false,
                maxLength: 4,
                nullable: true,
                defaultValueSql: "(('') collate Chinese_Taiwan_Stroke_CI_AS)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MhbtQsData_1",
                table: "MhbtQsData",
                columns: new[] { "Hosp_ID", "ID", "Birthday", "FuncDate", "Branch_Code", "TXT_DATE", "Cure_Type", "HospSeqNo" });

            migrationBuilder.UpdateData(
                table: "GenEmpData",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2021, 6, 30, 0, 58, 27, 765, DateTimeKind.Local).AddTicks(6855));

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2021, 6, 30, 0, 58, 27, 791, DateTimeKind.Local).AddTicks(9924));

            migrationBuilder.UpdateData(
                table: "RoleEmpMapping",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2021, 6, 30, 0, 58, 27, 792, DateTimeKind.Local).AddTicks(2592));
        }
    }
}
