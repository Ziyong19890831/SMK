using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SMK.Data.Migrations
{
    public partial class AddIniFileInCtrl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_QuitDataAll",
                table: "QuitDataAll");

            migrationBuilder.DropIndex(
                name: "IX_MhbtQsCure_UpdatedAt",
                table: "MhbtQsCure");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "MhbtQsState");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "MhbtQsState");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "MhbtQsData");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "MhbtQsCure");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "MhbtQsCure");

            migrationBuilder.RenameColumn(
                name: "AdjustUserId",
                table: "MhbtQsState",
                newName: "AdjustUserID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "MhbtQsState",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "HospId",
                table: "MhbtQsState",
                newName: "HospID");

            migrationBuilder.RenameColumn(
                name: "AdjustUserId",
                table: "MhbtQsCure",
                newName: "AdjustUserID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "MhbtQsCure",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "HospId",
                table: "MhbtQsCure",
                newName: "HospID");

            migrationBuilder.RenameColumn(
                name: "InformAddr",
                table: "MhbtAgentPatient",
                newName: "InformADDR");

            migrationBuilder.AlterColumn<string>(
                name: "FirstMonth",
                table: "QuitDataAll",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CaseNo",
                table: "QuitDataAll",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ID",
                table: "QuitDataAll",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<decimal>(
                name: "WeekTot",
                table: "MhbtQsData",
                type: "numeric(2,0)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "numeric(2,0)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Trace_Co_Check2",
                table: "MhbtQsData",
                type: "numeric(2,0)",
                nullable: false,
                defaultValueSql: "(('') collate Chinese_Taiwan_Stroke_CI_AS)",
                oldClrType: typeof(decimal),
                oldType: "numeric(2,0)",
                oldNullable: true,
                oldDefaultValueSql: "(('') collate Chinese_Taiwan_Stroke_CI_AS)");

            migrationBuilder.AlterColumn<decimal>(
                name: "TraceCoCheck",
                table: "MhbtQsData",
                type: "numeric(2,0)",
                nullable: false,
                defaultValueSql: "(('') collate Chinese_Taiwan_Stroke_CI_AS)",
                oldClrType: typeof(decimal),
                oldType: "numeric(2,0)",
                oldNullable: true,
                oldDefaultValueSql: "(('') collate Chinese_Taiwan_Stroke_CI_AS)");

            migrationBuilder.AlterColumn<decimal>(
                name: "SmokeYear",
                table: "MhbtQsData",
                type: "numeric(3,0)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "numeric(3,0)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "SmokeScore",
                table: "MhbtQsData",
                type: "numeric(2,0)",
                nullable: false,
                defaultValueSql: "(('') collate Chinese_Taiwan_Stroke_CI_AS)",
                oldClrType: typeof(decimal),
                oldType: "numeric(2,0)",
                oldNullable: true,
                oldDefaultValueSql: "(('') collate Chinese_Taiwan_Stroke_CI_AS)");

            migrationBuilder.AlterColumn<decimal>(
                name: "SmokeMon",
                table: "MhbtQsData",
                type: "numeric(2,0)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "numeric(2,0)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "SmokeDayNum",
                table: "MhbtQsData",
                type: "numeric(4,0)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "numeric(4,0)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "CureWeek",
                table: "MhbtQsData",
                type: "numeric(1,0)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "numeric(1,0)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "CoCheck",
                table: "MhbtQsData",
                type: "numeric(10,0)",
                nullable: false,
                defaultValueSql: "(('') collate Chinese_Taiwan_Stroke_CI_AS)",
                oldClrType: typeof(decimal),
                oldType: "numeric(10,0)",
                oldNullable: true,
                oldDefaultValueSql: "(('') collate Chinese_Taiwan_Stroke_CI_AS)");

            migrationBuilder.AlterColumn<decimal>(
                name: "BaseWeight",
                table: "MhbtQsData",
                type: "numeric(18,1)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,1)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_QuitDataAll",
                table: "QuitDataAll",
                columns: new[] { "CaseNo", "FirstMonth", "TimeSpan" });

            migrationBuilder.CreateTable(
                name: "IniFileInCtl",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Filename = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    FileDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IniDrDtlStatus = table.Column<string>(type: "varchar(20)", nullable: false),
                    IniDrOrdStatus = table.Column<string>(type: "varchar(20)", nullable: false),
                    IniOpDtlStatus = table.Column<string>(type: "varchar(20)", nullable: false),
                    IniOpOrdStatus = table.Column<string>(type: "varchar(20)", nullable: false),
                    IniDrDtlStatusUpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IniDrOrdStatusUpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IniOpDtlStatusUpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IniOpOrdStatusUpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IniFileInCtrl", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "GenEmpData",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2021, 7, 6, 13, 39, 5, 627, DateTimeKind.Local).AddTicks(8438));

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2021, 7, 6, 13, 39, 5, 632, DateTimeKind.Local).AddTicks(1710));

            migrationBuilder.UpdateData(
                table: "RoleEmpMapping",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2021, 7, 6, 13, 39, 5, 632, DateTimeKind.Local).AddTicks(3958));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IniFileInCtl");

            migrationBuilder.DropPrimaryKey(
                name: "PK_QuitDataAll",
                table: "QuitDataAll");

            migrationBuilder.RenameColumn(
                name: "AdjustUserID",
                table: "MhbtQsState",
                newName: "AdjustUserId");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "MhbtQsState",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "HospID",
                table: "MhbtQsState",
                newName: "HospId");

            migrationBuilder.RenameColumn(
                name: "AdjustUserID",
                table: "MhbtQsCure",
                newName: "AdjustUserId");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "MhbtQsCure",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "HospID",
                table: "MhbtQsCure",
                newName: "HospId");

            migrationBuilder.RenameColumn(
                name: "InformADDR",
                table: "MhbtAgentPatient",
                newName: "InformAddr");

            migrationBuilder.AlterColumn<string>(
                name: "ID",
                table: "QuitDataAll",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FirstMonth",
                table: "QuitDataAll",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "CaseNo",
                table: "QuitDataAll",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "MhbtQsState",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "MhbtQsState",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<decimal>(
                name: "WeekTot",
                table: "MhbtQsData",
                type: "numeric(2,0)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(2,0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Trace_Co_Check2",
                table: "MhbtQsData",
                type: "numeric(2,0)",
                nullable: true,
                defaultValueSql: "(('') collate Chinese_Taiwan_Stroke_CI_AS)",
                oldClrType: typeof(decimal),
                oldType: "numeric(2,0)",
                oldDefaultValueSql: "(('') collate Chinese_Taiwan_Stroke_CI_AS)");

            migrationBuilder.AlterColumn<decimal>(
                name: "TraceCoCheck",
                table: "MhbtQsData",
                type: "numeric(2,0)",
                nullable: true,
                defaultValueSql: "(('') collate Chinese_Taiwan_Stroke_CI_AS)",
                oldClrType: typeof(decimal),
                oldType: "numeric(2,0)",
                oldDefaultValueSql: "(('') collate Chinese_Taiwan_Stroke_CI_AS)");

            migrationBuilder.AlterColumn<decimal>(
                name: "SmokeYear",
                table: "MhbtQsData",
                type: "numeric(3,0)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(3,0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "SmokeScore",
                table: "MhbtQsData",
                type: "numeric(2,0)",
                nullable: true,
                defaultValueSql: "(('') collate Chinese_Taiwan_Stroke_CI_AS)",
                oldClrType: typeof(decimal),
                oldType: "numeric(2,0)",
                oldDefaultValueSql: "(('') collate Chinese_Taiwan_Stroke_CI_AS)");

            migrationBuilder.AlterColumn<decimal>(
                name: "SmokeMon",
                table: "MhbtQsData",
                type: "numeric(2,0)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(2,0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "SmokeDayNum",
                table: "MhbtQsData",
                type: "numeric(4,0)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(4,0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "CureWeek",
                table: "MhbtQsData",
                type: "numeric(1,0)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(1,0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "CoCheck",
                table: "MhbtQsData",
                type: "numeric(10,0)",
                nullable: true,
                defaultValueSql: "(('') collate Chinese_Taiwan_Stroke_CI_AS)",
                oldClrType: typeof(decimal),
                oldType: "numeric(10,0)",
                oldDefaultValueSql: "(('') collate Chinese_Taiwan_Stroke_CI_AS)");

            migrationBuilder.AlterColumn<decimal>(
                name: "BaseWeight",
                table: "MhbtQsData",
                type: "numeric(18,1)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,1)");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "MhbtQsData",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "MhbtQsCure",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "MhbtQsCure",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_QuitDataAll",
                table: "QuitDataAll",
                column: "ID");

            migrationBuilder.UpdateData(
                table: "GenEmpData",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2021, 7, 5, 2, 41, 8, 288, DateTimeKind.Local).AddTicks(4300));

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2021, 7, 5, 2, 41, 8, 292, DateTimeKind.Local).AddTicks(5349));

            migrationBuilder.UpdateData(
                table: "RoleEmpMapping",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2021, 7, 5, 2, 41, 8, 292, DateTimeKind.Local).AddTicks(9161));

            migrationBuilder.CreateIndex(
                name: "IX_MhbtQsCure_UpdatedAt",
                table: "MhbtQsCure",
                column: "UpdatedAt");
        }
    }
}
