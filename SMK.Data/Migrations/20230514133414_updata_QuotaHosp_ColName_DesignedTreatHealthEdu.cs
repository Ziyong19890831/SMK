using Microsoft.EntityFrameworkCore.Migrations;

namespace SMK.Data.Migrations
{
    public partial class updata_QuotaHosp_ColName_DesignedTreatHealthEdu : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DesignedTreatPeople",
                table: "QuotaHosp",
                newName: "DesignedTreatHealthEdu");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DesignedTreatHealthEdu",
                table: "QuotaHosp",
                newName: "DesignedTreatPeople");
        }
    }
}
