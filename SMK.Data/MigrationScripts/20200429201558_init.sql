IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

CREATE TABLE [GenBranch] (
    [BranchNo] char(1) NOT NULL,
    [BranchName] varchar(20) NULL DEFAULT (('')),
    CONSTRAINT [PK_GenBranch_1] PRIMARY KEY ([BranchNo])
);

GO

CREATE TABLE [GenEmpData] (
    [EmpNo] nvarchar(10) NOT NULL,
    [EmpName] nvarchar(100) NULL,
    [LoginID] nvarchar(20) NULL,
    [LoginPWD] nvarchar(50) NULL,
    [Enable] bit NOT NULL,
    [CreateAt] datetime2 NOT NULL,
    [UpdateAt] datetime2 NULL,
    [UpdateBy] nvarchar(100) NULL,
    CONSTRAINT [PK_GenEmpData] PRIMARY KEY ([EmpNo])
);

GO

CREATE TABLE [GenEndReason] (
    [EndReasonNo] char(2) NOT NULL,
    [EndReasonName] varchar(20) NULL DEFAULT (('')),
    CONSTRAINT [PK_GenEndReason_1] PRIMARY KEY ([EndReasonNo])
);

GO

CREATE TABLE [GenHospCont] (
    [HospContType] char(1) NOT NULL,
    [HospContName] varchar(20) NULL DEFAULT (('')),
    CONSTRAINT [PK_GenHospCont_1] PRIMARY KEY ([HospContType])
);

GO

CREATE TABLE [GenLicenceType] (
    [LicenceType] char(2) NOT NULL,
    [LicenceName] varchar(20) NULL DEFAULT (('')),
    CONSTRAINT [PK_GenLicenceType_1] PRIMARY KEY ([LicenceType])
);

GO

CREATE TABLE [GenProgramData] (
    [ProNo] nvarchar(30) NOT NULL,
    [ParentId] nvarchar(30) NULL,
    [ProName] nvarchar(30) NULL,
    [ControllerName] nvarchar(30) NULL,
    [ActionName] nvarchar(30) NULL,
    [QueryParams] nvarchar(30) NULL,
    [Sort] int NOT NULL,
    [CreateAt] datetime2 NOT NULL,
    [UpdateAt] datetime2 NULL,
    [UpdateBy] nvarchar(100) NULL,
    CONSTRAINT [PK_GenProgramData] PRIMARY KEY ([ProNo])
);

GO

CREATE TABLE [GenPrsnType] (
    [PrsnType] char(1) NOT NULL,
    [PrsnTypeName] varchar(20) NULL DEFAULT (('')),
    CONSTRAINT [PK_GenPrsnType_1] PRIMARY KEY ([PrsnType])
);

GO

CREATE TABLE [GenSMKContract] (
    [SMKContractType] char(2) NOT NULL,
    [SMKContractName] varchar(20) NULL DEFAULT (('')),
    CONSTRAINT [PK_GenSMKContract_1] PRIMARY KEY ([SMKContractType])
);

GO

CREATE TABLE [GenSpecial] (
    [SpecialistNo] varchar(5) NOT NULL,
    [SpecialistName] varchar(20) NULL DEFAULT (('')),
    CONSTRAINT [PK_GenSpecial_2] PRIMARY KEY ([SpecialistNo])
);

GO

CREATE TABLE [HospBasic] (
    [HospID] char(10) NOT NULL,
    [HospSeqNo] char(2) NOT NULL DEFAULT (('')),
    [HospName] nvarchar(80) NULL DEFAULT (('')),
    [HospTel] char(20) NULL DEFAULT (('')),
    [HospFax] char(20) NULL DEFAULT (('')),
    [HospEmail] varchar(60) NULL DEFAULT (('')),
    [Contact1] nchar(20) NULL DEFAULT (('')),
    [ContactTel1] char(20) NULL DEFAULT (('')),
    [ContactFax1] char(20) NULL DEFAULT (('')),
    [ContactEmail1] varchar(60) NULL DEFAULT (('')),
    [Contact2] nchar(20) NULL DEFAULT (('')),
    [ContactTel2] char(20) NULL DEFAULT (('')),
    [ContactFax2] char(20) NULL DEFAULT (('')),
    [ContactEmail2] varchar(60) NULL DEFAULT (('')),
    [HospOwnName] nchar(20) NULL DEFAULT (('')),
    [HospOwnID] char(10) NULL DEFAULT (('')),
    [BranchNo] char(1) NULL DEFAULT (('')),
    [ZIP] char(5) NULL DEFAULT (('')),
    [DivisionNo] char(2) NULL DEFAULT (('')),
    [SubDivisionNo] char(10) NULL DEFAULT (('')),
    [HospAddress] varchar(80) NULL DEFAULT (('')),
    [HospStatus] char(1) NULL DEFAULT (('')),
    [FirstHospID] char(10) NULL DEFAULT (('')),
    [PrevHospID] char(10) NULL DEFAULT (('')),
    [LastHospID] char(10) NULL DEFAULT (('')),
    [LastContType] char(1) NULL DEFAULT (('')),
    [Remark] varchar(200) NULL DEFAULT (('')),
    [chFlg1] char(1) NULL DEFAULT (('')),
    [chFlg2] char(1) NULL DEFAULT (('')),
    [chFlg3] char(1) NULL DEFAULT (('')),
    [PrevHospSeqNo] char(2) NULL DEFAULT (('')),
    [LastHospSeqNo] char(2) NULL DEFAULT (('')),
    [FirstHospSeqNo] char(2) NULL DEFAULT (('')),
    [HospAbbr] nvarchar(10) NULL DEFAULT (('')),
    CONSTRAINT [PK_SMKHospBasic] PRIMARY KEY ([HospID], [HospSeqNo])
);

GO

CREATE TABLE [HospContract] (
    [HospID] char(10) NOT NULL,
    [SMKContractType] char(2) NOT NULL,
    [HospStartDate] char(8) NOT NULL,
    [HospSeqNo] char(2) NOT NULL,
    [HospEndDate] char(8) NULL DEFAULT (('')),
    [EndReasonNo] char(2) NULL DEFAULT (('')),
    [CreateDate] char(8) NULL DEFAULT (('')),
    [ModifyDate] char(8) NULL DEFAULT (('')),
    [ModifyPersonNo] int NULL,
    [Remark] varchar(200) NULL DEFAULT (('')),
    CONSTRAINT [PK_HospContract] PRIMARY KEY ([HospID], [HospSeqNo], [SMKContractType], [HospStartDate])
);

GO

CREATE TABLE [PrsnBasic] (
    [PrsnID] char(10) NOT NULL,
    [PrsnName] nchar(20) NULL DEFAULT (('')),
    [PrsnBirthday] char(8) NULL DEFAULT (('')),
    [PrsnType] char(1) NULL DEFAULT (('')),
    [MajorSpecialistNo] char(5) NULL DEFAULT (('')),
    [SubSpecialistNo] char(5) NULL DEFAULT (('')),
    [Remark] varchar(200) NULL DEFAULT (('')),
    CONSTRAINT [PK_PrsnBasic] PRIMARY KEY ([PrsnID])
);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200429201558_init', N'3.1.3');

GO

