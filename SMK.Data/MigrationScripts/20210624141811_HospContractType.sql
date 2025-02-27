BEGIN TRANSACTION;
GO

ALTER TABLE [MhbtQsData] DROP CONSTRAINT [PK_MhbtQsData_1];
GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[MhbtQsData]') AND [c].[name] = N'CURE_STATE');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [MhbtQsData] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [MhbtQsData] DROP COLUMN [CURE_STATE];
GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[MhbtAgentPatient]') AND [c].[name] = N'TEL_D_ENTX');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [MhbtAgentPatient] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [MhbtAgentPatient] DROP COLUMN [TEL_D_ENTX];
GO

DECLARE @var2 sysname;
SELECT @var2 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[MhbtAgentPatient]') AND [c].[name] = N'TEL_N_ENTX');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [MhbtAgentPatient] DROP CONSTRAINT [' + @var2 + '];');
ALTER TABLE [MhbtAgentPatient] DROP COLUMN [TEL_N_ENTX];
GO

DECLARE @var3 sysname;
SELECT @var3 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[MhbtAgentPatient]') AND [c].[name] = N'Tel_D_AC');
IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [MhbtAgentPatient] DROP CONSTRAINT [' + @var3 + '];');
ALTER TABLE [MhbtAgentPatient] DROP COLUMN [Tel_D_AC];
GO

DECLARE @var4 sysname;
SELECT @var4 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[MhbtAgentPatient]') AND [c].[name] = N'Tel_N_AC');
IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [MhbtAgentPatient] DROP CONSTRAINT [' + @var4 + '];');
ALTER TABLE [MhbtAgentPatient] DROP COLUMN [Tel_N_AC];
GO

EXEC sp_rename N'[MhbtQsData].[Week_Tot]', N'WeekTot', N'COLUMN';
GO

EXEC sp_rename N'[MhbtQsData].[TRACE_STATE]', N'TraceState', N'COLUMN';
GO

EXEC sp_rename N'[MhbtQsData].[TRACE_DATE]', N'TraceDate', N'COLUMN';
GO

EXEC sp_rename N'[MhbtQsData].[TRACE_CO_CHECK]', N'TraceCoCheck', N'COLUMN';
GO

EXEC sp_rename N'[MhbtQsData].[Smoke_Year]', N'SmokeYear', N'COLUMN';
GO

EXEC sp_rename N'[MhbtQsData].[Smoke_Stop]', N'SmokeStop', N'COLUMN';
GO

EXEC sp_rename N'[MhbtQsData].[Smoke_Sick]', N'SmokeSick', N'COLUMN';
GO

EXEC sp_rename N'[MhbtQsData].[Smoke_Score]', N'SmokeScore', N'COLUMN';
GO

EXEC sp_rename N'[MhbtQsData].[Smoke_No_Gp]', N'SmokeNoGp', N'COLUMN';
GO

EXEC sp_rename N'[MhbtQsData].[Smoke_Much]', N'SmokeMuch', N'COLUMN';
GO

EXEC sp_rename N'[MhbtQsData].[Smoke_Mon]', N'SmokeMon', N'COLUMN';
GO

EXEC sp_rename N'[MhbtQsData].[Smoke_First]', N'SmokeFirst', N'COLUMN';
GO

EXEC sp_rename N'[MhbtQsData].[Smoke_Day_Num]', N'SmokeDayNum', N'COLUMN';
GO

EXEC sp_rename N'[MhbtQsData].[Smoke_Bed]', N'SmokeBed', N'COLUMN';
GO

EXEC sp_rename N'[MhbtQsData].[PRSN_ID]', N'PrsnID', N'COLUMN';
GO

EXEC sp_rename N'[MhbtQsData].[FEE_MARK]', N'FeeMark', N'COLUMN';
GO

EXEC sp_rename N'[MhbtQsData].[Cure_Week]', N'CureWeek', N'COLUMN';
GO

EXEC sp_rename N'[MhbtQsData].[Cure_Stage]', N'CureStage', N'COLUMN';
GO

EXEC sp_rename N'[MhbtQsData].[Cure_Agree]', N'CureAgree', N'COLUMN';
GO

EXEC sp_rename N'[MhbtQsData].[CO_CHECK]', N'CoCheck', N'COLUMN';
GO

EXEC sp_rename N'[MhbtQsData].[Base_Weight]', N'BaseWeight', N'COLUMN';
GO

EXEC sp_rename N'[MhbtQsData].[ADJUST_USER_ID]', N'AdjustUserID', N'COLUMN';
GO

EXEC sp_rename N'[MhbtQsData].[TXT_DATE]', N'TxtDate', N'COLUMN';
GO

EXEC sp_rename N'[MhbtQsData].[Branch_Code]', N'BranchCode', N'COLUMN';
GO

EXEC sp_rename N'[MhbtQsData].[Hosp_ID]', N'HospId', N'COLUMN';
GO

EXEC sp_rename N'[MhbtAgentPatient].[BIRTHDAY]', N'Birthday', N'COLUMN';
GO

EXEC sp_rename N'[MhbtAgentPatient].[Town_Name]', N'TownName', N'COLUMN';
GO

EXEC sp_rename N'[MhbtAgentPatient].[Town_Code]', N'TownCode', N'COLUMN';
GO

EXEC sp_rename N'[MhbtAgentPatient].[Tel_M]', N'TelM', N'COLUMN';
GO

EXEC sp_rename N'[MhbtAgentPatient].[TEL_N]', N'TelN', N'COLUMN';
GO

EXEC sp_rename N'[MhbtAgentPatient].[TEL_D]', N'TelD', N'COLUMN';
GO

EXEC sp_rename N'[MhbtAgentPatient].[Seq_No]', N'SeqNo', N'COLUMN';
GO

EXEC sp_rename N'[MhbtAgentPatient].[Inform_ADDR]', N'InformAddr', N'COLUMN';
GO

EXEC sp_rename N'[MhbtAgentPatient].[Func_Mark]', N'FuncMark', N'COLUMN';
GO

EXEC sp_rename N'[MhbtAgentPatient].[TXT_Date]', N'TxtDate', N'COLUMN';
GO

EXEC sp_rename N'[MhbtAgentPatient].[Branch_Code]', N'BranchCode', N'COLUMN';
GO

DECLARE @var5 sysname;
SELECT @var5 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[MhbtQsData]') AND [c].[name] = N'Trace_Date2');
IF @var5 IS NOT NULL EXEC(N'ALTER TABLE [MhbtQsData] DROP CONSTRAINT [' + @var5 + '];');
ALTER TABLE [MhbtQsData] ALTER COLUMN [Trace_Date2] nvarchar(max) NULL;
ALTER TABLE [MhbtQsData] ADD DEFAULT ((('') collate Chinese_Taiwan_Stroke_CI_AS)) FOR [Trace_Date2];
GO

DECLARE @var6 sysname;
SELECT @var6 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[MhbtQsData]') AND [c].[name] = N'ExamYear');
IF @var6 IS NOT NULL EXEC(N'ALTER TABLE [MhbtQsData] DROP CONSTRAINT [' + @var6 + '];');
ALTER TABLE [MhbtQsData] ALTER COLUMN [ExamYear] varchar(900) NOT NULL;
ALTER TABLE [MhbtQsData] ADD DEFAULT '' FOR [ExamYear];
GO

DECLARE @var7 sysname;
SELECT @var7 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[MhbtQsData]') AND [c].[name] = N'FuncDate');
IF @var7 IS NOT NULL EXEC(N'ALTER TABLE [MhbtQsData] DROP CONSTRAINT [' + @var7 + '];');
ALTER TABLE [MhbtQsData] ALTER COLUMN [FuncDate] nvarchar(450) NOT NULL;
GO

DECLARE @var8 sysname;
SELECT @var8 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[MhbtQsData]') AND [c].[name] = N'Birthday');
IF @var8 IS NOT NULL EXEC(N'ALTER TABLE [MhbtQsData] DROP CONSTRAINT [' + @var8 + '];');
ALTER TABLE [MhbtQsData] ALTER COLUMN [Birthday] nvarchar(450) NOT NULL;
GO

DECLARE @var9 sysname;
SELECT @var9 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[MhbtQsData]') AND [c].[name] = N'TraceDate');
IF @var9 IS NOT NULL EXEC(N'ALTER TABLE [MhbtQsData] DROP CONSTRAINT [' + @var9 + '];');
ALTER TABLE [MhbtQsData] ALTER COLUMN [TraceDate] nvarchar(max) NULL;
ALTER TABLE [MhbtQsData] ADD DEFAULT ((('') collate Chinese_Taiwan_Stroke_CI_AS)) FOR [TraceDate];
GO

DECLARE @var10 sysname;
SELECT @var10 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[MhbtQsData]') AND [c].[name] = N'CureStage');
IF @var10 IS NOT NULL EXEC(N'ALTER TABLE [MhbtQsData] DROP CONSTRAINT [' + @var10 + '];');
ALTER TABLE [MhbtQsData] ALTER COLUMN [CureStage] varchar(1) NOT NULL;
ALTER TABLE [MhbtQsData] ADD DEFAULT '' FOR [CureStage];
GO

DECLARE @var11 sysname;
SELECT @var11 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[MhbtQsData]') AND [c].[name] = N'CoCheck');
IF @var11 IS NOT NULL EXEC(N'ALTER TABLE [MhbtQsData] DROP CONSTRAINT [' + @var11 + '];');
ALTER TABLE [MhbtQsData] ALTER COLUMN [CoCheck] numeric(10,0) NULL;
ALTER TABLE [MhbtQsData] ADD DEFAULT ((('') collate Chinese_Taiwan_Stroke_CI_AS)) FOR [CoCheck];
GO

DECLARE @var12 sysname;
SELECT @var12 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[MhbtQsData]') AND [c].[name] = N'BaseWeight');
IF @var12 IS NOT NULL EXEC(N'ALTER TABLE [MhbtQsData] DROP CONSTRAINT [' + @var12 + '];');
ALTER TABLE [MhbtQsData] ALTER COLUMN [BaseWeight] numeric(18,1) NULL;
GO

DECLARE @var13 sysname;
SELECT @var13 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[MhbtQsData]') AND [c].[name] = N'TxtDate');
IF @var13 IS NOT NULL EXEC(N'ALTER TABLE [MhbtQsData] DROP CONSTRAINT [' + @var13 + '];');
ALTER TABLE [MhbtQsData] ALTER COLUMN [TxtDate] nvarchar(max) NULL;
ALTER TABLE [MhbtQsData] ADD DEFAULT ((('') collate Chinese_Taiwan_Stroke_CI_AS)) FOR [TxtDate];
GO

DECLARE @var14 sysname;
SELECT @var14 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[MhbtQsData]') AND [c].[name] = N'BranchCode');
IF @var14 IS NOT NULL EXEC(N'ALTER TABLE [MhbtQsData] DROP CONSTRAINT [' + @var14 + '];');
ALTER TABLE [MhbtQsData] ALTER COLUMN [BranchCode] varchar(1) NULL;
ALTER TABLE [MhbtQsData] ADD DEFAULT ((('') collate Chinese_Taiwan_Stroke_CI_AS)) FOR [BranchCode];
GO

ALTER TABLE [MhbtQsData] ADD [CurtState] varchar(900) NOT NULL DEFAULT '';
GO

ALTER TABLE [MhbtQsData] ADD [ChType] nvarchar(max) NULL;
GO

ALTER TABLE [MhbtQsData] ADD [CureStateOther] nvarchar(max) NULL;
GO

ALTER TABLE [MhbtQsData] ADD [FirstTreatDate] nvarchar(max) NULL;
GO

ALTER TABLE [MhbtQsData] ADD [SideEffect] nvarchar(max) NULL;
GO

ALTER TABLE [MhbtQsData] ADD [SmokeLung] nvarchar(max) NULL;
GO

ALTER TABLE [MhbtQsData] ADD [SmokeNico] nvarchar(max) NULL;
GO

ALTER TABLE [MhbtQsData] ADD [UpdatedAt] datetime2 NULL;
GO

DECLARE @var15 sysname;
SELECT @var15 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[MhbtAgentPatient]') AND [c].[name] = N'Birthday');
IF @var15 IS NOT NULL EXEC(N'ALTER TABLE [MhbtAgentPatient] DROP CONSTRAINT [' + @var15 + '];');
ALTER TABLE [MhbtAgentPatient] ALTER COLUMN [Birthday] nvarchar(450) NOT NULL;
ALTER TABLE [MhbtAgentPatient] ADD DEFAULT ((('') collate Chinese_Taiwan_Stroke_CI_AS)) FOR [Birthday];
GO

DECLARE @var16 sysname;
SELECT @var16 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[MhbtAgentPatient]') AND [c].[name] = N'TxtDate');
IF @var16 IS NOT NULL EXEC(N'ALTER TABLE [MhbtAgentPatient] DROP CONSTRAINT [' + @var16 + '];');
ALTER TABLE [MhbtAgentPatient] ALTER COLUMN [TxtDate] nvarchar(450) NOT NULL;
ALTER TABLE [MhbtAgentPatient] ADD DEFAULT ((('') collate Chinese_Taiwan_Stroke_CI_AS)) FOR [TxtDate];
GO

ALTER TABLE [MhbtQsData] ADD CONSTRAINT [PK_MhbtQsData_1] PRIMARY KEY ([HospId], [ID], [Birthday], [FuncDate], [CureStage], [ExamYear], [CurtState], [Cure_Type], [HospSeqNo]);
GO

CREATE TABLE [QuitDataAll] (
    [ID] nvarchar(450) NOT NULL,
    [CaseNo] nvarchar(max) NULL,
    [FirstMonth] nvarchar(max) NULL,
    [TimeSpan] int NOT NULL,
    [HospID] nvarchar(max) NULL,
    [HospSeqNo] nvarchar(max) NULL,
    [Birthday] nvarchar(max) NULL,
    [Edition] nvarchar(max) NULL,
    [VisitDate] nvarchar(max) NULL,
    [Result] nvarchar(max) NULL,
    [QuitPnt] nvarchar(max) NULL,
    [QuitCtn] nvarchar(max) NULL,
    [Edu] nvarchar(max) NULL,
    [Job] nvarchar(max) NULL,
    CONSTRAINT [PK_QuitDataAll] PRIMARY KEY ([ID])
);
GO

UPDATE [GenEmpData] SET [CreatedAt] = '2021-07-04T21:10:45.4272672+08:00'
WHERE [Id] = N'00000000-0000-0000-0000-000000000000';
SELECT @@ROWCOUNT;

GO

UPDATE [Role] SET [CreatedAt] = '2021-07-04T21:10:45.4327026+08:00'
WHERE [Id] = N'00000000-0000-0000-0000-000000000000';
SELECT @@ROWCOUNT;

GO

UPDATE [RoleEmpMapping] SET [CreatedAt] = '2021-07-04T21:10:45.4329856+08:00'
WHERE [Id] = N'00000000-0000-0000-0000-000000000000';
SELECT @@ROWCOUNT;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20210704131049_AlterMhbtQsData', N'5.0.7');
GO

COMMIT;
GO

