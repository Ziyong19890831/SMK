using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SMK.Data.Migrations
{
    public partial class update_ApplyPrsnNew_ChangeType_SMKLogDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SingleNote",
                table: "ApplyPrsnNew");

            migrationBuilder.AlterColumn<DateTime>(
                name: "SMKLogDate",
                table: "ApplyPrsnNew",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(string),
                oldType: "nvarchar(8)",
                oldMaxLength: 8,
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SMKLogDate",
                table: "ApplyPrsnNew",
                type: "nvarchar(8)",
                maxLength: 8,
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "SingleNote",
                table: "ApplyPrsnNew",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }
    }
}
