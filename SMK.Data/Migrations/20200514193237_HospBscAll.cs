using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SMK.Data.Migrations
{
    public partial class HospBscAll : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HospBscAll",
                columns: table => new
                {
                    HospID = table.Column<string>(unicode: false, fixedLength: true, maxLength: 10, nullable: false),
                    HospName = table.Column<string>(maxLength: 80, nullable: true, defaultValueSql: "('')"),
                    HospTelArea = table.Column<string>(unicode: false, fixedLength: true, maxLength: 5, nullable: true, defaultValueSql: "('')"),
                    HospTel = table.Column<string>(unicode: false, fixedLength: true, maxLength: 20, nullable: true, defaultValueSql: "('')"),
                    BranchNo = table.Column<string>(unicode: false, fixedLength: true, maxLength: 1, nullable: true, defaultValueSql: "('')"),
                    HospAddress = table.Column<string>(unicode: false, maxLength: 80, nullable: true, defaultValueSql: "('')"),
                    ContType = table.Column<string>(unicode: false, fixedLength: true, maxLength: 5, nullable: true, defaultValueSql: "('')"),
                    HospType = table.Column<string>(unicode: false, fixedLength: true, maxLength: 5, nullable: true, defaultValueSql: "('')"),
                    HospKind = table.Column<string>(unicode: false, fixedLength: true, maxLength: 5, nullable: true, defaultValueSql: "('')"),
                    HospEndDate = table.Column<string>(unicode: false, fixedLength: true, maxLength: 8, nullable: true, defaultValueSql: "('')")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HospBscAll", x => x.HospID);
                });

            migrationBuilder.UpdateData(
                table: "GenEmpData",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2020, 5, 15, 3, 32, 37, 515, DateTimeKind.Local).AddTicks(5309));

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2020, 5, 15, 3, 32, 37, 517, DateTimeKind.Local).AddTicks(4130));

            migrationBuilder.UpdateData(
                table: "RoleEmpMapping",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2020, 5, 15, 3, 32, 37, 517, DateTimeKind.Local).AddTicks(5667));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HospBscAll");

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
    }
}
