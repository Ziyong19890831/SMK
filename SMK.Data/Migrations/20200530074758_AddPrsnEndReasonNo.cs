using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SMK.Data.Migrations
{
    public partial class AddPrsnEndReasonNo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EndReasonNo",
                table: "PrsnContract",
                maxLength: 2,
                nullable: true,
                defaultValueSql: "('')");

            migrationBuilder.CreateTable(
                name: "GenPrsnEndReason",
                columns: table => new
                {
                    EndReasonNo = table.Column<string>(unicode: false, fixedLength: true, maxLength: 2, nullable: false),
                    EndReasonName = table.Column<string>(maxLength: 20, nullable: true, defaultValueSql: "('')")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GenPrsnEndReason_1", x => x.EndReasonNo);
                });

            migrationBuilder.UpdateData(
                table: "GenEmpData",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2020, 5, 30, 15, 47, 58, 302, DateTimeKind.Local).AddTicks(8277));

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2020, 5, 30, 15, 47, 58, 304, DateTimeKind.Local).AddTicks(7995));

            migrationBuilder.UpdateData(
                table: "RoleEmpMapping",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2020, 5, 30, 15, 47, 58, 305, DateTimeKind.Local).AddTicks(5));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GenPrsnEndReason");

            migrationBuilder.DropColumn(
                name: "EndReasonNo",
                table: "PrsnContract");

            migrationBuilder.UpdateData(
                table: "GenEmpData",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2020, 5, 30, 1, 41, 40, 501, DateTimeKind.Local).AddTicks(2920));

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2020, 5, 30, 1, 41, 40, 503, DateTimeKind.Local).AddTicks(2247));

            migrationBuilder.UpdateData(
                table: "RoleEmpMapping",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2020, 5, 30, 1, 41, 40, 503, DateTimeKind.Local).AddTicks(3837));
        }
    }
}
