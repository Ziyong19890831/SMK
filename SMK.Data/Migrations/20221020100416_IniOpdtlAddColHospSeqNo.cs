using Microsoft.EntityFrameworkCore.Migrations;

namespace SMK.Data.Migrations
{
    public partial class IniOpdtlAddColHospSeqNo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HospSeqNo",
                table: "iniOpDtl",
                type: "char(2)",
                unicode: false,
                fixedLength: true,
                maxLength: 2,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HospSeqNo",
                table: "iniDrDtl",
                type: "char(2)",
                unicode: false,
                fixedLength: true,
                maxLength: 2,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HospSeqNo",
                table: "iniOpDtl");

            migrationBuilder.DropColumn(
                name: "HospSeqNo",
                table: "iniDrDtl");
        }
    }
}
