using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SMK.Data.Migrations
{
    public partial class HospContractType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HospContractType",
                columns: table => new
                {
                    HospId = table.Column<string>(nullable: false),
                    HospContType = table.Column<string>(nullable: false),
                    CntSDate = table.Column<string>(nullable: false),
                    HospSeqNo = table.Column<string>(nullable: false),
                    CntEDate = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HospContractType", x => new { x.HospId, x.HospSeqNo, x.HospContType, x.CntSDate });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HospContractType");
        }
    }
}
