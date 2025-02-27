using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SMK.Data.Migrations
{
    public partial class Drop_DataInsert_Then_Create_DataInsertLog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DataInsert");

            migrationBuilder.CreateTable(
                name: "DataInsertLog",
                columns: table => new
                {
                    ISNO = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(nullable: true),
                    FinishDate = table.Column<DateTime>(nullable: true),
                    RecordCount = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ISNO_1", x => x.ISNO);
                });

            migrationBuilder.UpdateData(
                table: "GenEmpData",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2020, 10, 25, 2, 57, 25, 716, DateTimeKind.Local).AddTicks(1535));

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2020, 10, 25, 2, 57, 25, 719, DateTimeKind.Local).AddTicks(1046));

            migrationBuilder.UpdateData(
                table: "RoleEmpMapping",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2020, 10, 25, 2, 57, 25, 719, DateTimeKind.Local).AddTicks(5645));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DataInsertLog");

            migrationBuilder.CreateTable(
                name: "DataInsert",
                columns: table => new
                {
                    ISNO = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FinishDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RecordCount = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ISNO_1", x => x.ISNO);
                });

            migrationBuilder.UpdateData(
                table: "GenEmpData",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2020, 10, 23, 2, 59, 54, 451, DateTimeKind.Local).AddTicks(6185));

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2020, 10, 23, 2, 59, 54, 455, DateTimeKind.Local).AddTicks(2633));

            migrationBuilder.UpdateData(
                table: "RoleEmpMapping",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2020, 10, 23, 2, 59, 54, 455, DateTimeKind.Local).AddTicks(6120));
        }
    }
}
