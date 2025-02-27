using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SMK.Data.Migrations
{
    public partial class UpdateGenEmpData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LoginError",
                table: "GenEmpData",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "LoginErrorAt",
                table: "GenEmpData",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PasswordModifyAt",
                table: "GenEmpData",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LoginError",
                table: "GenEmpData");

            migrationBuilder.DropColumn(
                name: "LoginErrorAt",
                table: "GenEmpData");

            migrationBuilder.DropColumn(
                name: "PasswordModifyAt",
                table: "GenEmpData");
        }
    }
}
