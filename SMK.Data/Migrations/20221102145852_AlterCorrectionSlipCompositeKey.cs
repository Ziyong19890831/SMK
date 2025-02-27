using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SMK.Data.Migrations
{
    public partial class AlterCorrectionSlipCompositeKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CorrectionSlip",
                table: "CorrectionSlip");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ReceiveDate",
                table: "CorrectionSlip",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CorrectionSlip",
                table: "CorrectionSlip",
                columns: new[] { "CaseNo", "ReceiveDate" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CorrectionSlip",
                table: "CorrectionSlip");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ReceiveDate",
                table: "CorrectionSlip",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CorrectionSlip",
                table: "CorrectionSlip",
                column: "CaseNo");
        }
    }
}
