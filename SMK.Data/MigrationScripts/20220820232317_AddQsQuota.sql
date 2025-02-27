BEGIN TRANSACTION;
GO

ALTER TABLE [GenDrugBasic] ADD [HealthCarePrice] bit NOT NULL DEFAULT CAST(0 AS bit);
GO

CREATE TABLE [CST_QS_QUOTA] (
    [YEARS] nvarchar(450) NOT NULL,
    [HOSP_ID] nvarchar(450) NOT NULL,
    [HOSP_SEQ_NO] nvarchar(450) NOT NULL,
    [CURE_TYPE] int NOT NULL,
    [QUOTA] int NULL,
    [TXT_DATE] datetime2 NULL,
    [ADJUST_USER_ID] nvarchar(max) NULL,
    [VALID_S_DATE] datetime2 NOT NULL,
    [VALID_E_DATE] datetime2 NOT NULL,
    [REMARK] nvarchar(max) NULL,
    [CREAT_USER_ID] nvarchar(max) NULL,
    [CREAT_DATE] datetime2 NOT NULL,
    CONSTRAINT [PK_CST_QS_QUOTA] PRIMARY KEY ([YEARS], [HOSP_ID], [HOSP_SEQ_NO], [CURE_TYPE])
);
GO

CREATE TABLE [GenOrderCode] (
    [OrderCode] nvarchar(450) NOT NULL,
    [OrderChiName] nvarchar(max) NULL,
    [OrderEngName] nvarchar(max) NULL,
    [OrderCate] nvarchar(max) NULL,
    [Remark] nvarchar(max) NULL,
    CONSTRAINT [PK_GenOrderCode] PRIMARY KEY ([OrderCode])
);
GO

CREATE TABLE [OrdOfB7] (
    [Data_ID] nvarchar(450) NOT NULL,
    [Fee_YM] nvarchar(max) NULL,
    [Order_Seq_No] int NOT NULL,
    [Order_Type] nvarchar(max) NULL,
    [Order_Code] nvarchar(max) NULL,
    [Rel_Mode] nvarchar(max) NULL,
    [Drug_Num] nvarchar(max) NULL,
    [Drug_Fre] nvarchar(max) NULL,
    [Drug_Path] nvarchar(max) NULL,
    [Order_Uprice] decimal(18,2) NULL,
    [Order_Qty] decimal(18,2) NULL,
    [Order_Dot] int NULL,
    [Exe_Prsn_ID] nvarchar(max) NULL,
    [Remark] nvarchar(max) NULL,
    CONSTRAINT [PK_OrdOfB7] PRIMARY KEY ([Data_ID])
);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20220820150731_AddQsQuota', N'5.0.7');
GO

COMMIT;
GO

