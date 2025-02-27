using Microsoft.EntityFrameworkCore.Migrations;

namespace SMK.Data.Migrations
{
    public partial class change_QuotaHosp_ResultHealthEdu : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ResultTreatPeople",
                table: "QuotaHosp",
                newName: "ResultHealthEdu");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ResultHealthEdu",
                table: "QuotaHosp",
                newName: "ResultTreatPeople");
        }
    }
}
