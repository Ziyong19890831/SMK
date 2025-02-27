using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SMK.Data.Migrations
{
    public partial class HospContract : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_HospContract",
                table: "HospContract");

            migrationBuilder.AlterColumn<string>(
                name: "HospStartDate",
                table: "HospContract",
                unicode: false,
                fixedLength: true,
                maxLength: 8,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(8)",
                oldUnicode: false,
                oldFixedLength: true,
                oldMaxLength: 8);

            migrationBuilder.AlterColumn<string>(
                name: "SMKContractType",
                table: "HospContract",
                unicode: false,
                fixedLength: true,
                maxLength: 2,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(2)",
                oldUnicode: false,
                oldFixedLength: true,
                oldMaxLength: 2);

            migrationBuilder.AlterColumn<string>(
                name: "HospSeqNo",
                table: "HospContract",
                unicode: false,
                fixedLength: true,
                maxLength: 2,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(2)",
                oldUnicode: false,
                oldFixedLength: true,
                oldMaxLength: 2);

            migrationBuilder.AlterColumn<string>(
                name: "HospID",
                table: "HospContract",
                unicode: false,
                fixedLength: true,
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(10)",
                oldUnicode: false,
                oldFixedLength: true,
                oldMaxLength: 10);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "HospContract",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HospContract",
                table: "HospContract",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "GenEmpData",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2020, 5, 25, 1, 22, 16, 611, DateTimeKind.Local).AddTicks(4800));

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2020, 5, 25, 1, 22, 16, 613, DateTimeKind.Local).AddTicks(3583));

            migrationBuilder.UpdateData(
                table: "RoleEmpMapping",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2020, 5, 25, 1, 22, 16, 613, DateTimeKind.Local).AddTicks(5103));

            migrationBuilder.CreateIndex(
                name: "IX_HospContract_HospID_HospSeqNo_SMKContractType_HospStartDate",
                table: "HospContract",
                columns: new[] { "HospID", "HospSeqNo", "SMKContractType", "HospStartDate" },
                unique: true,
                filter: "[HospID] IS NOT NULL AND [HospSeqNo] IS NOT NULL AND [SMKContractType] IS NOT NULL AND [HospStartDate] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_HospContract",
                table: "HospContract");

            migrationBuilder.DropIndex(
                name: "IX_HospContract_HospID_HospSeqNo_SMKContractType_HospStartDate",
                table: "HospContract");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "HospContract");

            migrationBuilder.AlterColumn<string>(
                name: "SMKContractType",
                table: "HospContract",
                type: "char(2)",
                unicode: false,
                fixedLength: true,
                maxLength: 2,
                nullable: false,
                oldClrType: typeof(string),
                oldUnicode: false,
                oldFixedLength: true,
                oldMaxLength: 2,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "HospStartDate",
                table: "HospContract",
                type: "char(8)",
                unicode: false,
                fixedLength: true,
                maxLength: 8,
                nullable: false,
                oldClrType: typeof(string),
                oldUnicode: false,
                oldFixedLength: true,
                oldMaxLength: 8,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "HospSeqNo",
                table: "HospContract",
                type: "char(2)",
                unicode: false,
                fixedLength: true,
                maxLength: 2,
                nullable: false,
                oldClrType: typeof(string),
                oldUnicode: false,
                oldFixedLength: true,
                oldMaxLength: 2,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "HospID",
                table: "HospContract",
                type: "char(10)",
                unicode: false,
                fixedLength: true,
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldUnicode: false,
                oldFixedLength: true,
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_HospContract",
                table: "HospContract",
                columns: new[] { "HospID", "HospSeqNo", "SMKContractType", "HospStartDate" });

            migrationBuilder.UpdateData(
                table: "GenEmpData",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2020, 5, 19, 1, 53, 8, 497, DateTimeKind.Local).AddTicks(9901));

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2020, 5, 19, 1, 53, 8, 499, DateTimeKind.Local).AddTicks(8076));

            migrationBuilder.UpdateData(
                table: "RoleEmpMapping",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2020, 5, 19, 1, 53, 8, 499, DateTimeKind.Local).AddTicks(9557));
        }
    }
}
