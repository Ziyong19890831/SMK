using Microsoft.EntityFrameworkCore.Migrations;

namespace SMK.Data.Migrations
{
    public partial class update_table_new_Colume_prsn_ForNew : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BranchName",
                table: "ApplyPrsnNew",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HospID",
                table: "ApplyPrsnNew",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "HospName",
                table: "ApplyPrsnNew",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HospSeqNo",
                table: "ApplyPrsnNew",
                type: "nvarchar(2)",
                maxLength: 2,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastContType",
                table: "ApplyPrsnNew",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MedicalID",
                table: "ApplyPrsnNew",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MedicalName",
                table: "ApplyPrsnNew",
                type: "nvarchar(80)",
                maxLength: 80,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BranchName",
                table: "ApplyPrsnNew");

            migrationBuilder.DropColumn(
                name: "HospID",
                table: "ApplyPrsnNew");

            migrationBuilder.DropColumn(
                name: "HospName",
                table: "ApplyPrsnNew");

            migrationBuilder.DropColumn(
                name: "HospSeqNo",
                table: "ApplyPrsnNew");

            migrationBuilder.DropColumn(
                name: "LastContType",
                table: "ApplyPrsnNew");

            migrationBuilder.DropColumn(
                name: "MedicalID",
                table: "ApplyPrsnNew");

            migrationBuilder.DropColumn(
                name: "MedicalName",
                table: "ApplyPrsnNew");
        }
    }
}
