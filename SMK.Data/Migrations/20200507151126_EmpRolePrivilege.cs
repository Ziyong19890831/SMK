using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SMK.Data.Migrations
{
    public partial class EmpRolePrivilege : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GenProgramData");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GenEmpData",
                table: "GenEmpData");

            migrationBuilder.DropColumn(
                name: "EmpNo",
                table: "GenEmpData");

            migrationBuilder.DropColumn(
                name: "CreateAt",
                table: "GenEmpData");

            migrationBuilder.DropColumn(
                name: "EmpName",
                table: "GenEmpData");

            migrationBuilder.DropColumn(
                name: "LoginID",
                table: "GenEmpData");

            migrationBuilder.DropColumn(
                name: "LoginPWD",
                table: "GenEmpData");

            migrationBuilder.DropColumn(
                name: "UpdateAt",
                table: "GenEmpData");

            migrationBuilder.DropColumn(
                name: "UpdateBy",
                table: "GenEmpData");

            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "GenEmpData",
                maxLength: 36,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Account",
                table: "GenEmpData",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "GenEmpData",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastLoginDate",
                table: "GenEmpData",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "GenEmpData",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Pwd",
                table: "GenEmpData",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "GenEmpData",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "GenEmpData",
                maxLength: 127,
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_GenEmpData",
                table: "GenEmpData",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Privilege",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 36, nullable: false),
                    ParentId = table.Column<string>(maxLength: 36, nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Sort = table.Column<int>(nullable: false),
                    Type = table.Column<string>(nullable: true),
                    ControllerName = table.Column<string>(nullable: true),
                    ActionName = table.Column<string>(nullable: true),
                    QueryParams = table.Column<string>(maxLength: 512, nullable: true),
                    Remark = table.Column<string>(maxLength: 512, nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 127, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Privilege", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 36, nullable: false),
                    Name = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 127, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RoleEmpMapping",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 36, nullable: false),
                    EmpId = table.Column<string>(maxLength: 36, nullable: true),
                    RoleId = table.Column<string>(maxLength: 36, nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 127, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleEmpMapping", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RolePrivilegeMapping",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 36, nullable: false),
                    RoleId = table.Column<string>(maxLength: 36, nullable: true),
                    PrivilegeId = table.Column<string>(maxLength: 36, nullable: true),
                    EnableEntry = table.Column<bool>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 127, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePrivilegeMapping", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GenEmpData_Account",
                table: "GenEmpData",
                column: "Account",
                unique: true,
                filter: "[Account] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Privilege_ParentId",
                table: "Privilege",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Role_Name",
                table: "Role",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_RoleEmpMapping_EmpId_RoleId",
                table: "RoleEmpMapping",
                columns: new[] { "EmpId", "RoleId" },
                unique: true,
                filter: "[EmpId] IS NOT NULL AND [RoleId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_RolePrivilegeMapping_PrivilegeId_RoleId",
                table: "RolePrivilegeMapping",
                columns: new[] { "PrivilegeId", "RoleId" },
                unique: true,
                filter: "[PrivilegeId] IS NOT NULL AND [RoleId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Privilege");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "RoleEmpMapping");

            migrationBuilder.DropTable(
                name: "RolePrivilegeMapping");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GenEmpData",
                table: "GenEmpData");

            migrationBuilder.DropIndex(
                name: "IX_GenEmpData_Account",
                table: "GenEmpData");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "GenEmpData");

            migrationBuilder.DropColumn(
                name: "Account",
                table: "GenEmpData");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "GenEmpData");

            migrationBuilder.DropColumn(
                name: "LastLoginDate",
                table: "GenEmpData");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "GenEmpData");

            migrationBuilder.DropColumn(
                name: "Pwd",
                table: "GenEmpData");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "GenEmpData");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "GenEmpData");

            migrationBuilder.AddColumn<string>(
                name: "EmpNo",
                table: "GenEmpData",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateAt",
                table: "GenEmpData",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "EmpName",
                table: "GenEmpData",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LoginID",
                table: "GenEmpData",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LoginPWD",
                table: "GenEmpData",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateAt",
                table: "GenEmpData",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdateBy",
                table: "GenEmpData",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_GenEmpData",
                table: "GenEmpData",
                column: "EmpNo");

            migrationBuilder.CreateTable(
                name: "GenProgramData",
                columns: table => new
                {
                    ProNo = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    ActionName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    ControllerName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ParentId = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    ProName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    QueryParams = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Sort = table.Column<int>(type: "int", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GenProgramData", x => x.ProNo);
                });
        }
    }
}
