using Microsoft.EntityFrameworkCore.Migrations;

namespace SMK.Data.Migrations
{
    public partial class Update_ApplyPrsnNew_Contract_Effective_Date : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Contract_Effective_Date",
                table: "ApplyPrsnNew",
                type: "nvarchar(8)",
                maxLength: 8,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Contract_Effective_Date",
                table: "ApplyPrsnNew");
        }
    }
}
