using Microsoft.EntityFrameworkCore.Migrations;

namespace SMK.Data.Migrations
{
    public partial class AddIniDrDtlIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_iniDrDtl_tran_date",
                table: "iniDrDtl",
                column: "tran_date");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_iniDrDtl_tran_date",
                table: "iniDrDtl");
        }
    }
}
