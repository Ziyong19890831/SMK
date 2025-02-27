using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SMK.Data.Migrations
{
    public partial class prsnLicenceFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Remark",
                table: "PrsnLicence",
                unicode: false,
                fixedLength: true,
                maxLength: 200,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "GenEmpData",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2020, 5, 28, 1, 23, 13, 201, DateTimeKind.Local).AddTicks(3644));

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2020, 5, 28, 1, 23, 13, 203, DateTimeKind.Local).AddTicks(2741));

            migrationBuilder.UpdateData(
                table: "RoleEmpMapping",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2020, 5, 28, 1, 23, 13, 203, DateTimeKind.Local).AddTicks(4318));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Remark",
                table: "PrsnLicence");

            migrationBuilder.UpdateData(
                table: "GenEmpData",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2020, 5, 28, 1, 15, 8, 90, DateTimeKind.Local).AddTicks(6280));

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2020, 5, 28, 1, 15, 8, 92, DateTimeKind.Local).AddTicks(5426));

            migrationBuilder.UpdateData(
                table: "RoleEmpMapping",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2020, 5, 28, 1, 15, 8, 92, DateTimeKind.Local).AddTicks(6976));
        }
    }
}
