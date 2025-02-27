using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SMK.Data.Migrations
{
    public partial class prsnLicence : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PrsnLicence",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PrsnID = table.Column<string>(unicode: false, fixedLength: true, maxLength: 10, nullable: true),
                    LicenceType = table.Column<string>(unicode: false, fixedLength: true, maxLength: 2, nullable: true),
                    LicenceNo = table.Column<string>(unicode: false, fixedLength: true, maxLength: 20, nullable: true),
                    LicenceEndDate = table.Column<string>(unicode: false, fixedLength: true, maxLength: 8, nullable: true, defaultValueSql: "('')")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrsnLicence", x => x.Id);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_PrsnLicence_PrsnID_LicenceType_LicenceNo",
                table: "PrsnLicence",
                columns: new[] { "PrsnID", "LicenceType", "LicenceNo" },
                unique: true,
                filter: "[PrsnID] IS NOT NULL AND [LicenceType] IS NOT NULL AND [LicenceNo] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PrsnLicence");

            migrationBuilder.UpdateData(
                table: "GenEmpData",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2020, 5, 27, 21, 3, 3, 975, DateTimeKind.Local).AddTicks(7159));

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2020, 5, 27, 21, 3, 3, 977, DateTimeKind.Local).AddTicks(5874));

            migrationBuilder.UpdateData(
                table: "RoleEmpMapping",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2020, 5, 27, 21, 3, 3, 977, DateTimeKind.Local).AddTicks(7372));
        }
    }
}
