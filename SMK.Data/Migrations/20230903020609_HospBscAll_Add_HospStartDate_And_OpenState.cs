using Microsoft.EntityFrameworkCore.Migrations;

namespace SMK.Data.Migrations
{
    public partial class HospBscAll_Add_HospStartDate_And_OpenState : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HospStartDate",
                table: "HospBscAll",
                type: "nvarchar(8)",
                maxLength: 8,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OpenState",
                table: "HospBscAll",
                type: "nvarchar(1)",
                maxLength: 1,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HospStartDate",
                table: "HospBscAll");

            migrationBuilder.DropColumn(
                name: "OpenState",
                table: "HospBscAll");
        }
    }
}
