using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SMK.Data.Migrations
{
    public partial class AddMhbtQsStateUpdateColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ID",
                table: "MhbtQsState",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "Txt_Date",
                table: "MhbtQsState",
                newName: "TxtDate");

            migrationBuilder.RenameColumn(
                name: "Cure_State_Other",
                table: "MhbtQsState",
                newName: "CureStateOther");

            migrationBuilder.RenameColumn(
                name: "Adjust_User_ID",
                table: "MhbtQsState",
                newName: "AdjustUserId");

            migrationBuilder.RenameColumn(
                name: "Hosp_Seq_No",
                table: "MhbtQsState",
                newName: "HospSeqNo");

            migrationBuilder.RenameColumn(
                name: "Cure_State",
                table: "MhbtQsState",
                newName: "CureState");

            migrationBuilder.RenameColumn(
                name: "Func_Date",
                table: "MhbtQsState",
                newName: "FuncDate");

            migrationBuilder.RenameColumn(
                name: "Hosp_ID",
                table: "MhbtQsState",
                newName: "HospId");

            migrationBuilder.AlterColumn<string>(
                name: "Birthday",
                table: "MhbtQsState",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date");

            migrationBuilder.AlterColumn<string>(
                name: "TxtDate",
                table: "MhbtQsState",
                type: "nvarchar(max)",
                nullable: true,
                defaultValueSql: "(('') collate Chinese_Taiwan_Stroke_CI_AS)",
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValueSql: "(('') collate Chinese_Taiwan_Stroke_CI_AS)");

            migrationBuilder.AlterColumn<string>(
                name: "FuncDate",
                table: "MhbtQsState",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "MhbtQsState",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Seqno",
                table: "MhbtQsState",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "MhbtQsState",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "MhbtQsState");

            migrationBuilder.DropColumn(
                name: "Seqno",
                table: "MhbtQsState");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "MhbtQsState");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "MhbtQsState",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "TxtDate",
                table: "MhbtQsState",
                newName: "Txt_Date");

            migrationBuilder.RenameColumn(
                name: "CureStateOther",
                table: "MhbtQsState",
                newName: "Cure_State_Other");

            migrationBuilder.RenameColumn(
                name: "AdjustUserId",
                table: "MhbtQsState",
                newName: "Adjust_User_ID");

            migrationBuilder.RenameColumn(
                name: "HospSeqNo",
                table: "MhbtQsState",
                newName: "Hosp_Seq_No");

            migrationBuilder.RenameColumn(
                name: "CureState",
                table: "MhbtQsState",
                newName: "Cure_State");

            migrationBuilder.RenameColumn(
                name: "FuncDate",
                table: "MhbtQsState",
                newName: "Func_Date");

            migrationBuilder.RenameColumn(
                name: "HospId",
                table: "MhbtQsState",
                newName: "Hosp_ID");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Birthday",
                table: "MhbtQsState",
                type: "date",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Txt_Date",
                table: "MhbtQsState",
                type: "datetime",
                nullable: false,
                defaultValueSql: "(('') collate Chinese_Taiwan_Stroke_CI_AS)",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldDefaultValueSql: "(('') collate Chinese_Taiwan_Stroke_CI_AS)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Func_Date",
                table: "MhbtQsState",
                type: "date",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.UpdateData(
                table: "GenEmpData",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2021, 7, 4, 23, 44, 37, 382, DateTimeKind.Local).AddTicks(1942));

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2021, 7, 4, 23, 44, 37, 385, DateTimeKind.Local).AddTicks(7417));

            migrationBuilder.UpdateData(
                table: "RoleEmpMapping",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2021, 7, 4, 23, 44, 37, 385, DateTimeKind.Local).AddTicks(9426));
        }
    }
}
