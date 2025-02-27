using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SMK.Data.Migrations
{
    public partial class AddMhbtQsCureUpdateColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ID",
                table: "MhbtQsCure",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "Txt_Date",
                table: "MhbtQsCure",
                newName: "TxtDate");

            migrationBuilder.RenameColumn(
                name: "Cure_Num",
                table: "MhbtQsCure",
                newName: "CureNum");

            migrationBuilder.RenameColumn(
                name: "Adjust_User_ID",
                table: "MhbtQsCure",
                newName: "AdjustUserId");

            migrationBuilder.RenameColumn(
                name: "Hosp_Seq_No",
                table: "MhbtQsCure",
                newName: "HospSeqNo");

            migrationBuilder.RenameColumn(
                name: "Cure_Item",
                table: "MhbtQsCure",
                newName: "CureItem");

            migrationBuilder.RenameColumn(
                name: "Func_Date",
                table: "MhbtQsCure",
                newName: "FuncDate");

            migrationBuilder.RenameColumn(
                name: "Hosp_ID",
                table: "MhbtQsCure",
                newName: "HospId");

            migrationBuilder.AlterColumn<string>(
                name: "Birthday",
                table: "MhbtQsCure",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date");

            migrationBuilder.AlterColumn<string>(
                name: "TxtDate",
                table: "MhbtQsCure",
                type: "nvarchar(max)",
                nullable: true,
                defaultValueSql: "(('') collate Chinese_Taiwan_Stroke_CI_AS)",
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValueSql: "(('') collate Chinese_Taiwan_Stroke_CI_AS)");

            migrationBuilder.AlterColumn<string>(
                name: "FuncDate",
                table: "MhbtQsCure",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date");

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

            migrationBuilder.UpdateData(
                table: "GenEmpData",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2021, 7, 4, 22, 22, 33, 660, DateTimeKind.Local).AddTicks(112));

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2021, 7, 4, 22, 22, 33, 664, DateTimeKind.Local).AddTicks(3252));

            migrationBuilder.UpdateData(
                table: "RoleEmpMapping",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2021, 7, 4, 22, 22, 33, 664, DateTimeKind.Local).AddTicks(5368));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "MhbtQsCure");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "MhbtQsCure");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "MhbtQsCure",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "TxtDate",
                table: "MhbtQsCure",
                newName: "Txt_Date");

            migrationBuilder.RenameColumn(
                name: "CureNum",
                table: "MhbtQsCure",
                newName: "Cure_Num");

            migrationBuilder.RenameColumn(
                name: "AdjustUserId",
                table: "MhbtQsCure",
                newName: "Adjust_User_ID");

            migrationBuilder.RenameColumn(
                name: "HospSeqNo",
                table: "MhbtQsCure",
                newName: "Hosp_Seq_No");

            migrationBuilder.RenameColumn(
                name: "CureItem",
                table: "MhbtQsCure",
                newName: "Cure_Item");

            migrationBuilder.RenameColumn(
                name: "FuncDate",
                table: "MhbtQsCure",
                newName: "Func_Date");

            migrationBuilder.RenameColumn(
                name: "HospId",
                table: "MhbtQsCure",
                newName: "Hosp_ID");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Birthday",
                table: "MhbtQsCure",
                type: "date",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Txt_Date",
                table: "MhbtQsCure",
                type: "datetime",
                nullable: false,
                defaultValueSql: "(('') collate Chinese_Taiwan_Stroke_CI_AS)",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldDefaultValueSql: "(('') collate Chinese_Taiwan_Stroke_CI_AS)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Func_Date",
                table: "MhbtQsCure",
                type: "date",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

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
    }
}
