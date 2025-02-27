using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SMK.Data.Migrations
{
    public partial class change_ICCardData_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "HealthCarePrice",
                table: "GenDrugBasic",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.CreateTable(
                name: "GenSubDivision",
                columns: table => new
                {
                    SubdivisionNo = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    SubdivisionName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    DivisionNo = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    DivisionNoOld = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    ZIP = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    Type = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    StatSubDivisionCode = table.Column<string>(type: "nvarchar(7)", maxLength: 7, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GenSubDivision", x => x.SubdivisionNo);
                });

            migrationBuilder.CreateTable(
                name: "ICCardData",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataType = table.Column<int>(type: "int", nullable: true),
                    PersonID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Birthday = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HospitalCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhysicianPersonID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReadCardDatetime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MedicalSeries = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReissueNote = table.Column<int>(type: "int", nullable: true),
                    MedicalType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MainMedicalCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MinorMedicalCodeFirst = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MinorMedicalCodeSecond = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MinorMedicalCodeThird = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MinorMedicalCodeFourth = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MinorMedicalCodeFifth = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MedicalDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PhysicianOrderType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TreatCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MedicineMethod = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MedicineDay = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MedicineCount = table.Column<int>(type: "int", nullable: true),
                    CreateDT = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ICCardData", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GenSubDivision");

            migrationBuilder.DropTable(
                name: "ICCardData");

            migrationBuilder.AlterColumn<bool>(
                name: "HealthCarePrice",
                table: "GenDrugBasic",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);
        }
    }
}
