using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SMK.Data.Migrations
{
    public partial class initEmpData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "GenEmpData",
                columns: new[] { "Id", "Account", "CreatedAt", "Enable", "LastLoginDate", "Name", "Pwd", "UpdatedAt", "UpdatedBy" },
                values: new object[] { "00000000-0000-0000-0000-000000000000", "Admin", new DateTime(2020, 5, 8, 0, 14, 17, 676, DateTimeKind.Local).AddTicks(3004), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "系統管理員", "ZHACEBb7ESmBmYj7XqLotw==", null, null });

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "CreatedAt", "Name", "UpdatedAt", "UpdatedBy" },
                values: new object[] { "00000000-0000-0000-0000-000000000000", new DateTime(2020, 5, 8, 0, 14, 17, 678, DateTimeKind.Local).AddTicks(1398), "SuperAdmin", null, null });

            migrationBuilder.InsertData(
                table: "RoleEmpMapping",
                columns: new[] { "Id", "CreatedAt", "EmpId", "RoleId", "UpdatedAt", "UpdatedBy" },
                values: new object[] { "00000000-0000-0000-0000-000000000000", new DateTime(2020, 5, 8, 0, 14, 17, 678, DateTimeKind.Local).AddTicks(2888), "00000000-0000-0000-0000-000000000000", "00000000-0000-0000-0000-000000000000", null, null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "GenEmpData",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000");

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000");

            migrationBuilder.DeleteData(
                table: "RoleEmpMapping",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000");
        }
    }
}
