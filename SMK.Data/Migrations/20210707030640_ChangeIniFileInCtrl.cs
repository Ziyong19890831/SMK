using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SMK.Data.Migrations
{
    public partial class ChangeIniFileInCtrl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IniDrDtlStatus",
                table: "IniFileInCtrl");

            migrationBuilder.DropColumn(
                name: "IniDrDtlStatusUpdatedAt",
                table: "IniFileInCtrl");

            migrationBuilder.DropColumn(
                name: "IniDrOrdStatus",
                table: "IniFileInCtrl");

            migrationBuilder.DropColumn(
                name: "IniDrOrdStatusUpdatedAt",
                table: "IniFileInCtrl");

            migrationBuilder.DropColumn(
                name: "IniOpDtlStatus",
                table: "IniFileInCtrl");

            migrationBuilder.DropColumn(
                name: "IniOpDtlStatusUpdatedAt",
                table: "IniFileInCtrl");

            migrationBuilder.RenameColumn(
                name: "IniOpOrdStatusUpdatedAt",
                table: "IniFileInCtrl",
                newName: "StatusUpdatedAt");

            migrationBuilder.RenameColumn(
                name: "IniOpOrdStatus",
                table: "IniFileInCtrl",
                newName: "Status");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StatusUpdatedAt",
                table: "IniFileInCtrl",
                newName: "IniOpOrdStatusUpdatedAt");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "IniFileInCtrl",
                newName: "IniOpOrdStatus");

            migrationBuilder.AddColumn<string>(
                name: "IniDrDtlStatus",
                table: "IniFileInCtrl",
                type: "varchar(20)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "IniDrDtlStatusUpdatedAt",
                table: "IniFileInCtrl",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IniDrOrdStatus",
                table: "IniFileInCtrl",
                type: "varchar(20)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "IniDrOrdStatusUpdatedAt",
                table: "IniFileInCtrl",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IniOpDtlStatus",
                table: "IniFileInCtrl",
                type: "varchar(20)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "IniOpDtlStatusUpdatedAt",
                table: "IniFileInCtrl",
                type: "datetime2",
                nullable: true);
        }
    }
}
