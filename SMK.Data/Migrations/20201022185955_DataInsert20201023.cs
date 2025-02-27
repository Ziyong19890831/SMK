using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SMK.Data.Migrations
{
    public partial class DataInsert20201023 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_DataInsert",
                table: "DataInsert");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ISNO_1",
                table: "DataInsert",
                column: "ISNO");

            migrationBuilder.UpdateData(
                table: "GenEmpData",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2020, 10, 23, 2, 59, 54, 451, DateTimeKind.Local).AddTicks(6185));

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2020, 10, 23, 2, 59, 54, 455, DateTimeKind.Local).AddTicks(2633));

            migrationBuilder.UpdateData(
                table: "RoleEmpMapping",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2020, 10, 23, 2, 59, 54, 455, DateTimeKind.Local).AddTicks(6120));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ISNO_1",
                table: "DataInsert");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DataInsert",
                table: "DataInsert",
                column: "ISNO");

            migrationBuilder.UpdateData(
                table: "GenEmpData",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2020, 10, 23, 2, 24, 17, 294, DateTimeKind.Local).AddTicks(2958));

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2020, 10, 23, 2, 24, 17, 297, DateTimeKind.Local).AddTicks(235));

            migrationBuilder.UpdateData(
                table: "RoleEmpMapping",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2020, 10, 23, 2, 24, 17, 297, DateTimeKind.Local).AddTicks(2767));
        }
    }
}
