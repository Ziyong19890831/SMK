using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SMK.Data.Migrations
{
    public partial class fixIsUnicode_IsFixedLength : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Remark",
                table: "PrsnLicence",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(200)",
                oldUnicode: false,
                oldFixedLength: true,
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PrsnID",
                table: "PrsnLicence",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(10)",
                oldUnicode: false,
                oldFixedLength: true,
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LicenceType",
                table: "PrsnLicence",
                maxLength: 2,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(2)",
                oldUnicode: false,
                oldFixedLength: true,
                oldMaxLength: 2,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LicenceNo",
                table: "PrsnLicence",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(20)",
                oldUnicode: false,
                oldFixedLength: true,
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LicenceEndDate",
                table: "PrsnLicence",
                maxLength: 8,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldType: "char(8)",
                oldUnicode: false,
                oldFixedLength: true,
                oldMaxLength: 8,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "SMKContractType",
                table: "PrsnContract",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(10)",
                oldUnicode: false,
                oldFixedLength: true,
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Remark",
                table: "PrsnContract",
                maxLength: 200,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldType: "varchar(200)",
                oldUnicode: false,
                oldMaxLength: 200,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "PrsnStartDate",
                table: "PrsnContract",
                maxLength: 8,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldType: "char(8)",
                oldUnicode: false,
                oldFixedLength: true,
                oldMaxLength: 8,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "PrsnEndDate",
                table: "PrsnContract",
                maxLength: 8,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldType: "char(8)",
                oldUnicode: false,
                oldFixedLength: true,
                oldMaxLength: 8,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "ModifyDate",
                table: "PrsnContract",
                maxLength: 8,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldType: "char(8)",
                oldUnicode: false,
                oldFixedLength: true,
                oldMaxLength: 8,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "CreateDate",
                table: "PrsnContract",
                maxLength: 8,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldType: "char(8)",
                oldUnicode: false,
                oldFixedLength: true,
                oldMaxLength: 8,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "CouldTreat",
                table: "PrsnContract",
                maxLength: 1,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldType: "char(1)",
                oldUnicode: false,
                oldFixedLength: true,
                oldMaxLength: 1,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "CouldInstruct",
                table: "PrsnContract",
                maxLength: 1,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldType: "char(1)",
                oldUnicode: false,
                oldFixedLength: true,
                oldMaxLength: 1,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "SubSpecialistNo",
                table: "PrsnBasic",
                maxLength: 5,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldType: "char(5)",
                oldUnicode: false,
                oldFixedLength: true,
                oldMaxLength: 5,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "Remark",
                table: "PrsnBasic",
                maxLength: 200,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldType: "varchar(200)",
                oldUnicode: false,
                oldMaxLength: 200,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "PrsnType",
                table: "PrsnBasic",
                maxLength: 1,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldType: "char(1)",
                oldUnicode: false,
                oldFixedLength: true,
                oldMaxLength: 1,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "PrsnName",
                table: "PrsnBasic",
                maxLength: 20,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldType: "nchar(20)",
                oldFixedLength: true,
                oldMaxLength: 20,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "PrsnBirthday",
                table: "PrsnBasic",
                maxLength: 8,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldType: "char(8)",
                oldUnicode: false,
                oldFixedLength: true,
                oldMaxLength: 8,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "PEmail",
                table: "PrsnBasic",
                maxLength: 80,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(80)",
                oldUnicode: false,
                oldMaxLength: 80,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MajorSpecialistNo",
                table: "PrsnBasic",
                maxLength: 5,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldType: "char(5)",
                oldUnicode: false,
                oldFixedLength: true,
                oldMaxLength: 5,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "SMKContractType",
                table: "HospContract",
                maxLength: 2,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(2)",
                oldUnicode: false,
                oldFixedLength: true,
                oldMaxLength: 2,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Remark",
                table: "HospContract",
                maxLength: 200,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldType: "varchar(200)",
                oldUnicode: false,
                oldMaxLength: 200,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "ModifyDate",
                table: "HospContract",
                maxLength: 8,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldType: "char(8)",
                oldUnicode: false,
                oldFixedLength: true,
                oldMaxLength: 8,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "HospStartDate",
                table: "HospContract",
                maxLength: 8,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(8)",
                oldUnicode: false,
                oldFixedLength: true,
                oldMaxLength: 8,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "HospEndDate",
                table: "HospContract",
                maxLength: 8,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldType: "char(8)",
                oldUnicode: false,
                oldFixedLength: true,
                oldMaxLength: 8,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "EndReasonNo",
                table: "HospContract",
                maxLength: 2,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldType: "char(2)",
                oldUnicode: false,
                oldFixedLength: true,
                oldMaxLength: 2,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "CreateDate",
                table: "HospContract",
                maxLength: 8,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldType: "char(8)",
                oldUnicode: false,
                oldFixedLength: true,
                oldMaxLength: 8,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "HospType",
                table: "HospBscAll",
                maxLength: 5,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldType: "char(5)",
                oldUnicode: false,
                oldFixedLength: true,
                oldMaxLength: 5,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "HospTelArea",
                table: "HospBscAll",
                maxLength: 5,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldType: "char(5)",
                oldUnicode: false,
                oldFixedLength: true,
                oldMaxLength: 5,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "HospTel",
                table: "HospBscAll",
                maxLength: 20,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldType: "char(20)",
                oldUnicode: false,
                oldFixedLength: true,
                oldMaxLength: 20,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "HospKind",
                table: "HospBscAll",
                maxLength: 5,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldType: "char(5)",
                oldUnicode: false,
                oldFixedLength: true,
                oldMaxLength: 5,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "HospEndDate",
                table: "HospBscAll",
                maxLength: 8,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldType: "char(8)",
                oldUnicode: false,
                oldFixedLength: true,
                oldMaxLength: 8,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "HospAddress",
                table: "HospBscAll",
                maxLength: 80,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldType: "varchar(80)",
                oldUnicode: false,
                oldMaxLength: 80,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "ContType",
                table: "HospBscAll",
                maxLength: 5,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldType: "char(5)",
                oldUnicode: false,
                oldFixedLength: true,
                oldMaxLength: 5,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "BranchNo",
                table: "HospBscAll",
                maxLength: 1,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldType: "char(1)",
                oldUnicode: false,
                oldFixedLength: true,
                oldMaxLength: 1,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "ZIP",
                table: "HospBasic",
                maxLength: 5,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldType: "char(5)",
                oldUnicode: false,
                oldFixedLength: true,
                oldMaxLength: 5,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "SubDivisionNo",
                table: "HospBasic",
                maxLength: 10,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldType: "char(10)",
                oldUnicode: false,
                oldFixedLength: true,
                oldMaxLength: 10,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "Remark",
                table: "HospBasic",
                maxLength: 200,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldType: "varchar(200)",
                oldUnicode: false,
                oldMaxLength: 200,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "PrevHospSeqNo",
                table: "HospBasic",
                maxLength: 2,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldType: "char(2)",
                oldUnicode: false,
                oldFixedLength: true,
                oldMaxLength: 2,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "PrevHospID",
                table: "HospBasic",
                maxLength: 10,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldType: "char(10)",
                oldUnicode: false,
                oldFixedLength: true,
                oldMaxLength: 10,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "ModifyDate",
                table: "HospBasic",
                maxLength: 8,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldType: "char(8)",
                oldUnicode: false,
                oldFixedLength: true,
                oldMaxLength: 8,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "LastHospSeqNo",
                table: "HospBasic",
                maxLength: 2,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldType: "char(2)",
                oldUnicode: false,
                oldFixedLength: true,
                oldMaxLength: 2,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "LastHospID",
                table: "HospBasic",
                maxLength: 10,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldType: "char(10)",
                oldUnicode: false,
                oldFixedLength: true,
                oldMaxLength: 10,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "LastContType",
                table: "HospBasic",
                maxLength: 1,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldType: "char(1)",
                oldUnicode: false,
                oldFixedLength: true,
                oldMaxLength: 1,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "HospTel",
                table: "HospBasic",
                maxLength: 20,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldType: "char(20)",
                oldUnicode: false,
                oldFixedLength: true,
                oldMaxLength: 20,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "HospStatus",
                table: "HospBasic",
                maxLength: 1,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldType: "char(1)",
                oldUnicode: false,
                oldFixedLength: true,
                oldMaxLength: 1,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "HospOwnName",
                table: "HospBasic",
                maxLength: 20,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldType: "nchar(20)",
                oldFixedLength: true,
                oldMaxLength: 20,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "HospOwnID",
                table: "HospBasic",
                maxLength: 10,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldType: "char(10)",
                oldUnicode: false,
                oldFixedLength: true,
                oldMaxLength: 10,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "HospFax",
                table: "HospBasic",
                maxLength: 20,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldType: "char(20)",
                oldUnicode: false,
                oldFixedLength: true,
                oldMaxLength: 20,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "HospEmail",
                table: "HospBasic",
                maxLength: 60,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldType: "varchar(60)",
                oldUnicode: false,
                oldMaxLength: 60,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "HospAddress",
                table: "HospBasic",
                maxLength: 80,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldType: "varchar(80)",
                oldUnicode: false,
                oldMaxLength: 80,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "FirstHospSeqNo",
                table: "HospBasic",
                maxLength: 2,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldType: "char(2)",
                oldUnicode: false,
                oldFixedLength: true,
                oldMaxLength: 2,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "FirstHospID",
                table: "HospBasic",
                maxLength: 10,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldType: "char(10)",
                oldUnicode: false,
                oldFixedLength: true,
                oldMaxLength: 10,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "DivisionNo",
                table: "HospBasic",
                maxLength: 2,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldType: "char(2)",
                oldUnicode: false,
                oldFixedLength: true,
                oldMaxLength: 2,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "CreateDate",
                table: "HospBasic",
                maxLength: 8,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldType: "char(8)",
                oldUnicode: false,
                oldFixedLength: true,
                oldMaxLength: 8,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "ContactTel2",
                table: "HospBasic",
                maxLength: 20,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldType: "char(20)",
                oldUnicode: false,
                oldFixedLength: true,
                oldMaxLength: 20,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "ContactTel1",
                table: "HospBasic",
                maxLength: 20,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldType: "char(20)",
                oldUnicode: false,
                oldFixedLength: true,
                oldMaxLength: 20,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "ContactFax2",
                table: "HospBasic",
                maxLength: 20,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldType: "char(20)",
                oldUnicode: false,
                oldFixedLength: true,
                oldMaxLength: 20,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "ContactFax1",
                table: "HospBasic",
                maxLength: 20,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldType: "char(20)",
                oldUnicode: false,
                oldFixedLength: true,
                oldMaxLength: 20,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "ContactEmail2",
                table: "HospBasic",
                maxLength: 60,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldType: "varchar(60)",
                oldUnicode: false,
                oldMaxLength: 60,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "ContactEmail1",
                table: "HospBasic",
                maxLength: 60,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldType: "varchar(60)",
                oldUnicode: false,
                oldMaxLength: 60,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "Contact2",
                table: "HospBasic",
                maxLength: 20,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldType: "nchar(20)",
                oldFixedLength: true,
                oldMaxLength: 20,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "Contact1",
                table: "HospBasic",
                maxLength: 20,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldType: "nchar(20)",
                oldFixedLength: true,
                oldMaxLength: 20,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "chFlg3",
                table: "HospBasic",
                unicode: false,
                maxLength: 1,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldType: "char(1)",
                oldUnicode: false,
                oldFixedLength: true,
                oldMaxLength: 1,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "chFlg2",
                table: "HospBasic",
                unicode: false,
                maxLength: 1,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldType: "char(1)",
                oldUnicode: false,
                oldFixedLength: true,
                oldMaxLength: 1,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "chFlg1",
                table: "HospBasic",
                unicode: false,
                maxLength: 1,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldType: "char(1)",
                oldUnicode: false,
                oldFixedLength: true,
                oldMaxLength: 1,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "BranchNo",
                table: "HospBasic",
                unicode: false,
                maxLength: 1,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldType: "char(1)",
                oldUnicode: false,
                oldFixedLength: true,
                oldMaxLength: 1,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "SpecialistName",
                table: "GenSpecial",
                maxLength: 20,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldType: "varchar(20)",
                oldUnicode: false,
                oldMaxLength: 20,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "SMKContractName",
                table: "GenSMKContract",
                maxLength: 20,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldType: "varchar(20)",
                oldUnicode: false,
                oldMaxLength: 20,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "PrsnTypeName",
                table: "GenPrsnType",
                maxLength: 20,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldType: "varchar(20)",
                oldUnicode: false,
                oldMaxLength: 20,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "LicenceName",
                table: "GenLicenceType",
                maxLength: 20,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldType: "varchar(20)",
                oldUnicode: false,
                oldMaxLength: 20,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "HospContName",
                table: "GenHospCont",
                maxLength: 20,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldType: "varchar(20)",
                oldUnicode: false,
                oldMaxLength: 20,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "EndReasonName",
                table: "GenEndReason",
                maxLength: 20,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldType: "varchar(20)",
                oldUnicode: false,
                oldMaxLength: 20,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "BranchName",
                table: "GenBranch",
                maxLength: 20,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldType: "varchar(20)",
                oldUnicode: false,
                oldMaxLength: 20,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.UpdateData(
                table: "GenEmpData",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2020, 5, 30, 1, 41, 40, 501, DateTimeKind.Local).AddTicks(2920));

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2020, 5, 30, 1, 41, 40, 503, DateTimeKind.Local).AddTicks(2247));

            migrationBuilder.UpdateData(
                table: "RoleEmpMapping",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2020, 5, 30, 1, 41, 40, 503, DateTimeKind.Local).AddTicks(3837));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Remark",
                table: "PrsnLicence",
                type: "char(200)",
                unicode: false,
                fixedLength: true,
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PrsnID",
                table: "PrsnLicence",
                type: "char(10)",
                unicode: false,
                fixedLength: true,
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LicenceType",
                table: "PrsnLicence",
                type: "char(2)",
                unicode: false,
                fixedLength: true,
                maxLength: 2,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 2,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LicenceNo",
                table: "PrsnLicence",
                type: "char(20)",
                unicode: false,
                fixedLength: true,
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LicenceEndDate",
                table: "PrsnLicence",
                type: "char(8)",
                unicode: false,
                fixedLength: true,
                maxLength: 8,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldMaxLength: 8,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "SMKContractType",
                table: "PrsnContract",
                type: "char(10)",
                unicode: false,
                fixedLength: true,
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Remark",
                table: "PrsnContract",
                type: "varchar(200)",
                unicode: false,
                maxLength: 200,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldMaxLength: 200,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "PrsnStartDate",
                table: "PrsnContract",
                type: "char(8)",
                unicode: false,
                fixedLength: true,
                maxLength: 8,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldMaxLength: 8,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "PrsnEndDate",
                table: "PrsnContract",
                type: "char(8)",
                unicode: false,
                fixedLength: true,
                maxLength: 8,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldMaxLength: 8,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "ModifyDate",
                table: "PrsnContract",
                type: "char(8)",
                unicode: false,
                fixedLength: true,
                maxLength: 8,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldMaxLength: 8,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "CreateDate",
                table: "PrsnContract",
                type: "char(8)",
                unicode: false,
                fixedLength: true,
                maxLength: 8,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldMaxLength: 8,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "CouldTreat",
                table: "PrsnContract",
                type: "char(1)",
                unicode: false,
                fixedLength: true,
                maxLength: 1,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldMaxLength: 1,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "CouldInstruct",
                table: "PrsnContract",
                type: "char(1)",
                unicode: false,
                fixedLength: true,
                maxLength: 1,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldMaxLength: 1,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "SubSpecialistNo",
                table: "PrsnBasic",
                type: "char(5)",
                unicode: false,
                fixedLength: true,
                maxLength: 5,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldMaxLength: 5,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "Remark",
                table: "PrsnBasic",
                type: "varchar(200)",
                unicode: false,
                maxLength: 200,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldMaxLength: 200,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "PrsnType",
                table: "PrsnBasic",
                type: "char(1)",
                unicode: false,
                fixedLength: true,
                maxLength: 1,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldMaxLength: 1,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "PrsnName",
                table: "PrsnBasic",
                type: "nchar(20)",
                fixedLength: true,
                maxLength: 20,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldMaxLength: 20,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "PrsnBirthday",
                table: "PrsnBasic",
                type: "char(8)",
                unicode: false,
                fixedLength: true,
                maxLength: 8,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldMaxLength: 8,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "PEmail",
                table: "PrsnBasic",
                type: "varchar(80)",
                unicode: false,
                maxLength: 80,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 80,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MajorSpecialistNo",
                table: "PrsnBasic",
                type: "char(5)",
                unicode: false,
                fixedLength: true,
                maxLength: 5,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldMaxLength: 5,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "SMKContractType",
                table: "HospContract",
                type: "char(2)",
                unicode: false,
                fixedLength: true,
                maxLength: 2,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 2,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Remark",
                table: "HospContract",
                type: "varchar(200)",
                unicode: false,
                maxLength: 200,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldMaxLength: 200,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "ModifyDate",
                table: "HospContract",
                type: "char(8)",
                unicode: false,
                fixedLength: true,
                maxLength: 8,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldMaxLength: 8,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "HospStartDate",
                table: "HospContract",
                type: "char(8)",
                unicode: false,
                fixedLength: true,
                maxLength: 8,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 8,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "HospEndDate",
                table: "HospContract",
                type: "char(8)",
                unicode: false,
                fixedLength: true,
                maxLength: 8,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldMaxLength: 8,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "EndReasonNo",
                table: "HospContract",
                type: "char(2)",
                unicode: false,
                fixedLength: true,
                maxLength: 2,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldMaxLength: 2,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "CreateDate",
                table: "HospContract",
                type: "char(8)",
                unicode: false,
                fixedLength: true,
                maxLength: 8,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldMaxLength: 8,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "HospType",
                table: "HospBscAll",
                type: "char(5)",
                unicode: false,
                fixedLength: true,
                maxLength: 5,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldMaxLength: 5,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "HospTelArea",
                table: "HospBscAll",
                type: "char(5)",
                unicode: false,
                fixedLength: true,
                maxLength: 5,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldMaxLength: 5,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "HospTel",
                table: "HospBscAll",
                type: "char(20)",
                unicode: false,
                fixedLength: true,
                maxLength: 20,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldMaxLength: 20,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "HospKind",
                table: "HospBscAll",
                type: "char(5)",
                unicode: false,
                fixedLength: true,
                maxLength: 5,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldMaxLength: 5,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "HospEndDate",
                table: "HospBscAll",
                type: "char(8)",
                unicode: false,
                fixedLength: true,
                maxLength: 8,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldMaxLength: 8,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "HospAddress",
                table: "HospBscAll",
                type: "varchar(80)",
                unicode: false,
                maxLength: 80,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldMaxLength: 80,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "ContType",
                table: "HospBscAll",
                type: "char(5)",
                unicode: false,
                fixedLength: true,
                maxLength: 5,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldMaxLength: 5,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "BranchNo",
                table: "HospBscAll",
                type: "char(1)",
                unicode: false,
                fixedLength: true,
                maxLength: 1,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldMaxLength: 1,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "ZIP",
                table: "HospBasic",
                type: "char(5)",
                unicode: false,
                fixedLength: true,
                maxLength: 5,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldMaxLength: 5,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "SubDivisionNo",
                table: "HospBasic",
                type: "char(10)",
                unicode: false,
                fixedLength: true,
                maxLength: 10,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldMaxLength: 10,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "Remark",
                table: "HospBasic",
                type: "varchar(200)",
                unicode: false,
                maxLength: 200,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldMaxLength: 200,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "PrevHospSeqNo",
                table: "HospBasic",
                type: "char(2)",
                unicode: false,
                fixedLength: true,
                maxLength: 2,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldMaxLength: 2,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "PrevHospID",
                table: "HospBasic",
                type: "char(10)",
                unicode: false,
                fixedLength: true,
                maxLength: 10,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldMaxLength: 10,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "ModifyDate",
                table: "HospBasic",
                type: "char(8)",
                unicode: false,
                fixedLength: true,
                maxLength: 8,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldMaxLength: 8,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "LastHospSeqNo",
                table: "HospBasic",
                type: "char(2)",
                unicode: false,
                fixedLength: true,
                maxLength: 2,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldMaxLength: 2,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "LastHospID",
                table: "HospBasic",
                type: "char(10)",
                unicode: false,
                fixedLength: true,
                maxLength: 10,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldMaxLength: 10,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "LastContType",
                table: "HospBasic",
                type: "char(1)",
                unicode: false,
                fixedLength: true,
                maxLength: 1,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldMaxLength: 1,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "HospTel",
                table: "HospBasic",
                type: "char(20)",
                unicode: false,
                fixedLength: true,
                maxLength: 20,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldMaxLength: 20,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "HospStatus",
                table: "HospBasic",
                type: "char(1)",
                unicode: false,
                fixedLength: true,
                maxLength: 1,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldMaxLength: 1,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "HospOwnName",
                table: "HospBasic",
                type: "nchar(20)",
                fixedLength: true,
                maxLength: 20,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldMaxLength: 20,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "HospOwnID",
                table: "HospBasic",
                type: "char(10)",
                unicode: false,
                fixedLength: true,
                maxLength: 10,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldMaxLength: 10,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "HospFax",
                table: "HospBasic",
                type: "char(20)",
                unicode: false,
                fixedLength: true,
                maxLength: 20,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldMaxLength: 20,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "HospEmail",
                table: "HospBasic",
                type: "varchar(60)",
                unicode: false,
                maxLength: 60,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldMaxLength: 60,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "HospAddress",
                table: "HospBasic",
                type: "varchar(80)",
                unicode: false,
                maxLength: 80,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldMaxLength: 80,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "FirstHospSeqNo",
                table: "HospBasic",
                type: "char(2)",
                unicode: false,
                fixedLength: true,
                maxLength: 2,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldMaxLength: 2,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "FirstHospID",
                table: "HospBasic",
                type: "char(10)",
                unicode: false,
                fixedLength: true,
                maxLength: 10,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldMaxLength: 10,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "DivisionNo",
                table: "HospBasic",
                type: "char(2)",
                unicode: false,
                fixedLength: true,
                maxLength: 2,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldMaxLength: 2,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "CreateDate",
                table: "HospBasic",
                type: "char(8)",
                unicode: false,
                fixedLength: true,
                maxLength: 8,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldMaxLength: 8,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "ContactTel2",
                table: "HospBasic",
                type: "char(20)",
                unicode: false,
                fixedLength: true,
                maxLength: 20,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldMaxLength: 20,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "ContactTel1",
                table: "HospBasic",
                type: "char(20)",
                unicode: false,
                fixedLength: true,
                maxLength: 20,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldMaxLength: 20,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "ContactFax2",
                table: "HospBasic",
                type: "char(20)",
                unicode: false,
                fixedLength: true,
                maxLength: 20,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldMaxLength: 20,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "ContactFax1",
                table: "HospBasic",
                type: "char(20)",
                unicode: false,
                fixedLength: true,
                maxLength: 20,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldMaxLength: 20,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "ContactEmail2",
                table: "HospBasic",
                type: "varchar(60)",
                unicode: false,
                maxLength: 60,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldMaxLength: 60,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "ContactEmail1",
                table: "HospBasic",
                type: "varchar(60)",
                unicode: false,
                maxLength: 60,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldMaxLength: 60,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "Contact2",
                table: "HospBasic",
                type: "nchar(20)",
                fixedLength: true,
                maxLength: 20,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldMaxLength: 20,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "Contact1",
                table: "HospBasic",
                type: "nchar(20)",
                fixedLength: true,
                maxLength: 20,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldMaxLength: 20,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "chFlg3",
                table: "HospBasic",
                type: "char(1)",
                unicode: false,
                fixedLength: true,
                maxLength: 1,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldUnicode: false,
                oldMaxLength: 1,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "chFlg2",
                table: "HospBasic",
                type: "char(1)",
                unicode: false,
                fixedLength: true,
                maxLength: 1,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldUnicode: false,
                oldMaxLength: 1,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "chFlg1",
                table: "HospBasic",
                type: "char(1)",
                unicode: false,
                fixedLength: true,
                maxLength: 1,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldUnicode: false,
                oldMaxLength: 1,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "BranchNo",
                table: "HospBasic",
                type: "char(1)",
                unicode: false,
                fixedLength: true,
                maxLength: 1,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldUnicode: false,
                oldMaxLength: 1,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "SpecialistName",
                table: "GenSpecial",
                type: "varchar(20)",
                unicode: false,
                maxLength: 20,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldMaxLength: 20,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "SMKContractName",
                table: "GenSMKContract",
                type: "varchar(20)",
                unicode: false,
                maxLength: 20,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldMaxLength: 20,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "PrsnTypeName",
                table: "GenPrsnType",
                type: "varchar(20)",
                unicode: false,
                maxLength: 20,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldMaxLength: 20,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "LicenceName",
                table: "GenLicenceType",
                type: "varchar(20)",
                unicode: false,
                maxLength: 20,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldMaxLength: 20,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "HospContName",
                table: "GenHospCont",
                type: "varchar(20)",
                unicode: false,
                maxLength: 20,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldMaxLength: 20,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "EndReasonName",
                table: "GenEndReason",
                type: "varchar(20)",
                unicode: false,
                maxLength: 20,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldMaxLength: 20,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.AlterColumn<string>(
                name: "BranchName",
                table: "GenBranch",
                type: "varchar(20)",
                unicode: false,
                maxLength: 20,
                nullable: true,
                defaultValueSql: "('')",
                oldClrType: typeof(string),
                oldMaxLength: 20,
                oldNullable: true,
                oldDefaultValueSql: "('')");

            migrationBuilder.UpdateData(
                table: "GenEmpData",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2020, 5, 28, 1, 41, 50, 910, DateTimeKind.Local).AddTicks(8449));

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2020, 5, 28, 1, 41, 50, 912, DateTimeKind.Local).AddTicks(3740));

            migrationBuilder.UpdateData(
                table: "RoleEmpMapping",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                column: "CreatedAt",
                value: new DateTime(2020, 5, 28, 1, 41, 50, 912, DateTimeKind.Local).AddTicks(5374));
        }
    }
}
