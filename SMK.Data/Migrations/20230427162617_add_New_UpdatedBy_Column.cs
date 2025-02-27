using Microsoft.EntityFrameworkCore.Migrations;

namespace SMK.Data.Migrations
{
    public partial class add_New_UpdatedBy_Column : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "ApplyHospNew",
                type: "nvarchar(127)",
                maxLength: 127,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "ApplyHospEnd",
                type: "nvarchar(127)",
                maxLength: 127,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "ApplyHospChange",
                type: "nvarchar(127)",
                maxLength: 127,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "ApplyHospNew");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "ApplyHospEnd");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "ApplyHospChange");
        }
    }
}
