using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SMK.Data.Migrations
{
    public partial class prsnFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PrsnContract",
                table: "PrsnContract");

            migrationBuilder.AlterColumn<string>(
                name: "PrsnStartDate",
                table: "PrsnContract",
                unicode: false,
                fixedLength: true,
                maxLength: 8,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldType: "char(8)",
                oldUnicode: false,
                oldFixedLength: true,
                oldMaxLength: 8,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "SMKContractType",
                table: "PrsnContract",
                unicode: false,
                fixedLength: true,
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(10)",
                oldUnicode: false,
                oldFixedLength: true,
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<string>(
                name: "PrsnID",
                table: "PrsnContract",
                unicode: false,
                fixedLength: true,
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(10)",
                oldUnicode: false,
                oldFixedLength: true,
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<string>(
                name: "HospSeqNo",
                table: "PrsnContract",
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
                table: "PrsnContract",
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
                table: "PrsnContract",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "PEmail",
                table: "PrsnBasic",
                unicode: false,
                maxLength: 80,
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PrsnContract",
                table: "PrsnContract",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "PrsnEmail",
                columns: table => new
                {
                    PrsnID = table.Column<string>(unicode: false, fixedLength: true, maxLength: 10, nullable: false),
                    PEmail = table.Column<string>(unicode: false, maxLength: 80, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrsnEmail", x => new { x.PrsnID, x.PEmail });
                });

            migrationBuilder.UpdateData(
                table: "GenEmpData",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2020, 5, 27, 21, 3, 3, 975, DateTimeKind.Local).AddTicks(7159));

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2020, 5, 27, 21, 3, 3, 977, DateTimeKind.Local).AddTicks(5874));

            migrationBuilder.UpdateData(
                table: "RoleEmpMapping",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2020, 5, 27, 21, 3, 3, 977, DateTimeKind.Local).AddTicks(7372));

            migrationBuilder.CreateIndex(
                name: "IX_PrsnContract_HospID_HospSeqNo_PrsnID_SMKContractType_PrsnStartDate",
                table: "PrsnContract",
                columns: new[] { "HospID", "HospSeqNo", "PrsnID", "SMKContractType", "PrsnStartDate" },
                unique: true,
                filter: "[HospID] IS NOT NULL AND [HospSeqNo] IS NOT NULL AND [PrsnID] IS NOT NULL AND [SMKContractType] IS NOT NULL AND [PrsnStartDate] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PrsnEmail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PrsnContract",
                table: "PrsnContract");

            migrationBuilder.DropIndex(
                name: "IX_PrsnContract_HospID_HospSeqNo_PrsnID_SMKContractType_PrsnStartDate",
                table: "PrsnContract");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "PrsnContract");

            migrationBuilder.DropColumn(
                name: "PEmail",
                table: "PrsnBasic");

            migrationBuilder.AlterColumn<string>(
                name: "SMKContractType",
                table: "PrsnContract",
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

            migrationBuilder.AlterColumn<string>(
                name: "PrsnStartDate",
                table: "PrsnContract",
                type: "char(8)",
                unicode: false,
                fixedLength: true,
                maxLength: 8,
                nullable: false,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldUnicode: false,
                oldFixedLength: true,
                oldMaxLength: 8,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "PrsnID",
                table: "PrsnContract",
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

            migrationBuilder.AlterColumn<string>(
                name: "HospSeqNo",
                table: "PrsnContract",
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
                table: "PrsnContract",
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
                name: "PK_PrsnContract",
                table: "PrsnContract",
                columns: new[] { "HospID", "HospSeqNo", "PrsnID", "SMKContractType", "PrsnStartDate" });

            migrationBuilder.UpdateData(
                table: "GenEmpData",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2020, 5, 25, 23, 37, 57, 881, DateTimeKind.Local).AddTicks(9149));

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2020, 5, 25, 23, 37, 57, 883, DateTimeKind.Local).AddTicks(8478));

            migrationBuilder.UpdateData(
                table: "RoleEmpMapping",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2020, 5, 25, 23, 37, 57, 884, DateTimeKind.Local).AddTicks(196));
        }
    }
}
