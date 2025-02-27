using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SMK.Data.Migrations
{
    public partial class GenHospCont_QualityCount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "QualityDefaultCount",
                table: "GenHospCont",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "QualityImproveCount",
                table: "GenHospCont",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "GenEmpData",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2020, 11, 3, 0, 23, 36, 551, DateTimeKind.Local).AddTicks(7360));

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2020, 11, 3, 0, 23, 36, 553, DateTimeKind.Local).AddTicks(7469));

            migrationBuilder.UpdateData(
                table: "RoleEmpMapping",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2020, 11, 3, 0, 23, 36, 553, DateTimeKind.Local).AddTicks(9031));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QualityDefaultCount",
                table: "GenHospCont");

            migrationBuilder.DropColumn(
                name: "QualityImproveCount",
                table: "GenHospCont");

            migrationBuilder.UpdateData(
                table: "GenEmpData",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2020, 10, 25, 3, 17, 39, 176, DateTimeKind.Local).AddTicks(3566));

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2020, 10, 25, 3, 17, 39, 179, DateTimeKind.Local).AddTicks(5426));

            migrationBuilder.UpdateData(
                table: "RoleEmpMapping",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2020, 10, 25, 3, 17, 39, 179, DateTimeKind.Local).AddTicks(7801));
        }
    }
}
