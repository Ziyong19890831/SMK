using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SMK.Data.Migrations
{
    public partial class MhbtTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_MhbtQsData",
                table: "MhbtQsData");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MhbtAgentPatient_1",
                table: "MhbtAgentPatient");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MhbtQsData_1",
                table: "MhbtQsData",
                columns: new[] { "Hosp_ID", "ID", "Birthday", "FuncDate", "Branch_Code", "TXT_DATE", "Cure_Type", "HospSeqNo" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_MhbtAgentPatient",
                table: "MhbtAgentPatient",
                columns: new[] { "HospID", "HospAgentCode", "ID", "BIRTHDAY", "Branch_Code", "TXT_Date" });

            migrationBuilder.UpdateData(
                table: "GenEmpData",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2020, 10, 13, 23, 38, 51, 505, DateTimeKind.Local).AddTicks(6363));

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2020, 10, 13, 23, 38, 51, 507, DateTimeKind.Local).AddTicks(7317));

            migrationBuilder.UpdateData(
                table: "RoleEmpMapping",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2020, 10, 13, 23, 38, 51, 507, DateTimeKind.Local).AddTicks(9021));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_MhbtQsData_1",
                table: "MhbtQsData");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MhbtAgentPatient",
                table: "MhbtAgentPatient");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MhbtQsData",
                table: "MhbtQsData",
                columns: new[] { "Hosp_ID", "ID", "Birthday", "FuncDate", "Cure_Type", "HospSeqNo" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_MhbtAgentPatient_1",
                table: "MhbtAgentPatient",
                columns: new[] { "HospID", "HospAgentCode", "ID", "BIRTHDAY" });

            migrationBuilder.UpdateData(
                table: "GenEmpData",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2020, 10, 7, 1, 56, 19, 714, DateTimeKind.Local).AddTicks(9551));

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2020, 10, 7, 1, 56, 19, 716, DateTimeKind.Local).AddTicks(9808));

            migrationBuilder.UpdateData(
                table: "RoleEmpMapping",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2020, 10, 7, 1, 56, 19, 717, DateTimeKind.Local).AddTicks(1940));
        }
    }
}
