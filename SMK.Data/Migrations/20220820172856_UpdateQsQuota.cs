using Microsoft.EntityFrameworkCore.Migrations;

namespace SMK.Data.Migrations
{
    public partial class UpdateQsQuota : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CST_QS_QUOTA",
                table: "CST_QS_QUOTA");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CST_QS_QUOTA",
                table: "CST_QS_QUOTA",
                columns: new[] { "YEARS", "HOSP_ID", "HOSP_SEQ_NO", "CURE_TYPE", "VALID_S_DATE", "VALID_E_DATE" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CST_QS_QUOTA",
                table: "CST_QS_QUOTA");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CST_QS_QUOTA",
                table: "CST_QS_QUOTA",
                columns: new[] { "YEARS", "HOSP_ID", "HOSP_SEQ_NO", "CURE_TYPE" });
        }
    }
}
