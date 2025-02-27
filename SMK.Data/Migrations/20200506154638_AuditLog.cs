using Microsoft.EntityFrameworkCore.Migrations;

namespace SMK.Data.Migrations
{
    public partial class AuditLog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuditLog",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 36, nullable: false),
                    Account = table.Column<string>(maxLength: 128, nullable: true),
                    SourceTable = table.Column<string>(maxLength: 64, nullable: true),
                    ActionType = table.Column<string>(nullable: true),
                    RecordId = table.Column<string>(maxLength: 36, nullable: true),
                    OriginalRecord = table.Column<string>(type: "text", nullable: true),
                    CurrentRecord = table.Column<string>(type: "text", nullable: true),
                    InvolvedColumns = table.Column<string>(maxLength: 1024, nullable: true),
                    ActionRemark = table.Column<string>(maxLength: 1024, nullable: true),
                    Description = table.Column<string>(maxLength: 511, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLog", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuditLog_Account",
                table: "AuditLog",
                column: "Account");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLog_RecordId",
                table: "AuditLog",
                column: "RecordId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLog_SourceTable",
                table: "AuditLog",
                column: "SourceTable");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditLog");
        }
    }
}
