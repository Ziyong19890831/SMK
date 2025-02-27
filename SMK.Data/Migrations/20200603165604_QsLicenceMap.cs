using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SMK.Data.Migrations
{
    public partial class QsLicenceMap : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LicenceEndDate",
                table: "PrsnLicence");

            migrationBuilder.AlterColumn<string>(
                name: "LicenceNo",
                table: "PrsnLicence",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CertEndDate",
                table: "PrsnLicence",
                maxLength: 8,
                nullable: true,
                defaultValueSql: "('')");

            migrationBuilder.AddColumn<string>(
                name: "CertPublicDate",
                table: "PrsnLicence",
                maxLength: 8,
                nullable: true,
                defaultValueSql: "('')");

            migrationBuilder.AddColumn<string>(
                name: "CertStartDate",
                table: "PrsnLicence",
                maxLength: 8,
                nullable: true,
                defaultValueSql: "('')");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "PrsnLicence",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "PrsnLicence",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "PrsnLicence",
                maxLength: 127,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "QsLicenceMap",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LicenceType = table.Column<string>(maxLength: 2, nullable: true),
                    CTypeSNO = table.Column<int>(maxLength: 10, nullable: false),
                    CTypeName = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QsLicenceMap", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "GenEmpData",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2020, 6, 4, 0, 56, 3, 954, DateTimeKind.Local).AddTicks(7114));

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2020, 6, 4, 0, 56, 3, 956, DateTimeKind.Local).AddTicks(6231));

            migrationBuilder.UpdateData(
                table: "RoleEmpMapping",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2020, 6, 4, 0, 56, 3, 956, DateTimeKind.Local).AddTicks(7728));

            migrationBuilder.CreateIndex(
                name: "IX_QsLicenceMap_LicenceType_CTypeSNO",
                table: "QsLicenceMap",
                columns: new[] { "LicenceType", "CTypeSNO" },
                unique: true,
                filter: "[LicenceType] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QsLicenceMap");

            migrationBuilder.DropColumn(
                name: "CertEndDate",
                table: "PrsnLicence");

            migrationBuilder.DropColumn(
                name: "CertPublicDate",
                table: "PrsnLicence");

            migrationBuilder.DropColumn(
                name: "CertStartDate",
                table: "PrsnLicence");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "PrsnLicence");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "PrsnLicence");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "PrsnLicence");

            migrationBuilder.AlterColumn<string>(
                name: "LicenceNo",
                table: "PrsnLicence",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LicenceEndDate",
                table: "PrsnLicence",
                type: "nvarchar(8)",
                maxLength: 8,
                nullable: true,
                defaultValueSql: "('')");

            migrationBuilder.UpdateData(
                table: "GenEmpData",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2020, 6, 1, 0, 16, 29, 417, DateTimeKind.Local).AddTicks(2510));

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2020, 6, 1, 0, 16, 29, 419, DateTimeKind.Local).AddTicks(2261));

            migrationBuilder.UpdateData(
                table: "RoleEmpMapping",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2020, 6, 1, 0, 16, 29, 419, DateTimeKind.Local).AddTicks(3975));
        }
    }
}
