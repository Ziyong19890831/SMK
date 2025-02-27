using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SMK.Data.Migrations
{
    public partial class PrsnContract : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PrsnContract",
                columns: table => new
                {
                    HospID = table.Column<string>(unicode: false, fixedLength: true, maxLength: 10, nullable: false),
                    PrsnID = table.Column<string>(unicode: false, fixedLength: true, maxLength: 10, nullable: false),
                    SMKContractType = table.Column<string>(unicode: false, fixedLength: true, maxLength: 10, nullable: false),
                    PrsnStartDate = table.Column<string>(unicode: false, fixedLength: true, maxLength: 8, nullable: false, defaultValueSql: "('')"),
                    HospSeqNo = table.Column<string>(unicode: false, fixedLength: true, maxLength: 2, nullable: false),
                    PrsnEndDate = table.Column<string>(unicode: false, fixedLength: true, maxLength: 8, nullable: true, defaultValueSql: "('')"),
                    CreateDate = table.Column<string>(unicode: false, fixedLength: true, maxLength: 8, nullable: true, defaultValueSql: "('')"),
                    ModifyDate = table.Column<string>(unicode: false, fixedLength: true, maxLength: 8, nullable: true, defaultValueSql: "('')"),
                    ModifyPersonNo = table.Column<int>(nullable: true),
                    Remark = table.Column<string>(unicode: false, maxLength: 200, nullable: true, defaultValueSql: "('')"),
                    CouldTreat = table.Column<string>(unicode: false, fixedLength: true, maxLength: 1, nullable: true, defaultValueSql: "('')"),
                    CouldInstruct = table.Column<string>(unicode: false, fixedLength: true, maxLength: 1, nullable: true, defaultValueSql: "('')")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrsnContract", x => new { x.HospID, x.HospSeqNo, x.PrsnID, x.SMKContractType, x.PrsnStartDate });
                });

            migrationBuilder.UpdateData(
                table: "GenEmpData",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2020, 5, 12, 2, 45, 57, 681, DateTimeKind.Local).AddTicks(2942));

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2020, 5, 12, 2, 45, 57, 683, DateTimeKind.Local).AddTicks(1565));

            migrationBuilder.UpdateData(
                table: "RoleEmpMapping",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2020, 5, 12, 2, 45, 57, 683, DateTimeKind.Local).AddTicks(3137));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PrsnContract");

            migrationBuilder.UpdateData(
                table: "GenEmpData",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2020, 5, 8, 0, 14, 17, 676, DateTimeKind.Local).AddTicks(3004));

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2020, 5, 8, 0, 14, 17, 678, DateTimeKind.Local).AddTicks(1398));

            migrationBuilder.UpdateData(
                table: "RoleEmpMapping",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2020, 5, 8, 0, 14, 17, 678, DateTimeKind.Local).AddTicks(2888));
        }
    }
}
