using Microsoft.EntityFrameworkCore.Migrations;

namespace SMK.Data.Migrations
{
    public partial class AddGenDrugBasic : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GenDrugBasic",
                columns: table => new
                {
                    DrugNo = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DrugType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrderChiName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrderEngName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DrugCompany = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DrugIngredient = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DrugContent = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OrderStartDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrderEndDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    prescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HealthCarePrice = table.Column<bool>(type: "bit",nullable:true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GenDrugBasic", x => x.DrugNo);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GenDrugBasic_DrugNo",
                table: "GenDrugBasic",
                column: "DrugNo");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GenDrugBasic");
        }
    }
}
