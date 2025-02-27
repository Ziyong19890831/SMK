using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SMK.Data.Migrations
{
    public partial class HospBasicFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreateDate",
                table: "HospBasic",
                unicode: false,
                fixedLength: true,
                maxLength: 8,
                nullable: true,
                defaultValueSql: "('')");

            migrationBuilder.AddColumn<string>(
                name: "ModifyDate",
                table: "HospBasic",
                unicode: false,
                fixedLength: true,
                maxLength: 8,
                nullable: true,
                defaultValueSql: "('')");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "HospBasic");

            migrationBuilder.DropColumn(
                name: "ModifyDate",
                table: "HospBasic");

            migrationBuilder.UpdateData(
                table: "GenEmpData",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2020, 5, 18, 22, 2, 8, 614, DateTimeKind.Local).AddTicks(884));

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2020, 5, 18, 22, 2, 8, 615, DateTimeKind.Local).AddTicks(9244));

            migrationBuilder.UpdateData(
                table: "RoleEmpMapping",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2020, 5, 18, 22, 2, 8, 616, DateTimeKind.Local).AddTicks(727));
        }
    }
}
