using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SMK.Data.Migrations
{
    public partial class fixHospContract_HospId_HospSeqno : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_HospContract_HospID_HospSeqNo_SMKContractType_HospStartDate",
                table: "HospContract");

            migrationBuilder.AlterColumn<string>(
                name: "HospSeqNo",
                table: "HospContract",
                unicode: false,
                fixedLength: true,
                maxLength: 2,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "char(2)",
                oldUnicode: false,
                oldFixedLength: true,
                oldMaxLength: 2,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "HospID",
                table: "HospContract",
                unicode: false,
                fixedLength: true,
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "char(10)",
                oldUnicode: false,
                oldFixedLength: true,
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "GenEmpData",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2020, 6, 1, 0, 16, 29, 417, DateTimeKind.Local).AddTicks(2510));

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2020, 6, 1, 0, 16, 29, 419, DateTimeKind.Local).AddTicks(2261));

            migrationBuilder.UpdateData(
                table: "RoleEmpMapping",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2020, 6, 1, 0, 16, 29, 419, DateTimeKind.Local).AddTicks(3975));

            migrationBuilder.CreateIndex(
                name: "IX_HospContract_HospID_HospSeqNo_SMKContractType_HospStartDate",
                table: "HospContract",
                columns: new[] { "HospID", "HospSeqNo", "SMKContractType", "HospStartDate" },
                unique: true,
                filter: "[SMKContractType] IS NOT NULL AND [HospStartDate] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_HospContract_HospID_HospSeqNo_SMKContractType_HospStartDate",
                table: "HospContract");

            migrationBuilder.AlterColumn<string>(
                name: "HospSeqNo",
                table: "HospContract",
                type: "char(2)",
                unicode: false,
                fixedLength: true,
                maxLength: 2,
                nullable: true,
                oldClrType: typeof(string),
                oldUnicode: false,
                oldFixedLength: true,
                oldMaxLength: 2);

            migrationBuilder.AlterColumn<string>(
                name: "HospID",
                table: "HospContract",
                type: "char(10)",
                unicode: false,
                fixedLength: true,
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldUnicode: false,
                oldFixedLength: true,
                oldMaxLength: 10);

            migrationBuilder.UpdateData(
                table: "GenEmpData",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2020, 5, 30, 15, 47, 58, 302, DateTimeKind.Local).AddTicks(8277));

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2020, 5, 30, 15, 47, 58, 304, DateTimeKind.Local).AddTicks(7995));

            migrationBuilder.UpdateData(
                table: "RoleEmpMapping",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2020, 5, 30, 15, 47, 58, 305, DateTimeKind.Local).AddTicks(5));

            migrationBuilder.CreateIndex(
                name: "IX_HospContract_HospID_HospSeqNo_SMKContractType_HospStartDate",
                table: "HospContract",
                columns: new[] { "HospID", "HospSeqNo", "SMKContractType", "HospStartDate" },
                unique: true,
                filter: "[HospID] IS NOT NULL AND [HospSeqNo] IS NOT NULL AND [SMKContractType] IS NOT NULL AND [HospStartDate] IS NOT NULL");
        }
    }
}
