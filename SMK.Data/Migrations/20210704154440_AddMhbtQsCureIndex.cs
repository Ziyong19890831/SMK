using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SMK.Data.Migrations
{
    public partial class AddMhbtQsCureIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "GenEmpData",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2021, 7, 4, 23, 44, 37, 382, DateTimeKind.Local).AddTicks(1942));

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2021, 7, 4, 23, 44, 37, 385, DateTimeKind.Local).AddTicks(7417));

            migrationBuilder.UpdateData(
                table: "RoleEmpMapping",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2021, 7, 4, 23, 44, 37, 385, DateTimeKind.Local).AddTicks(9426));

            migrationBuilder.CreateIndex(
                name: "IX_MhbtQsCure_UpdatedAt",
                table: "MhbtQsCure",
                column: "UpdatedAt");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MhbtQsCure_UpdatedAt",
                table: "MhbtQsCure");

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
    }
}
