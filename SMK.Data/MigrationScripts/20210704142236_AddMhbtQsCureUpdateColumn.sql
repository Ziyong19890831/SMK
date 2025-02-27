-- noinspection SqlDialectInspectionForFile

BEGIN TRANSACTION;
GO

EXEC sp_rename N'[MhbtQsCure].[ID]', N'Id', N'COLUMN';
GO

EXEC sp_rename N'[MhbtQsCure].[Txt_Date]', N'TxtDate', N'COLUMN';
GO

EXEC sp_rename N'[MhbtQsCure].[Cure_Num]', N'CureNum', N'COLUMN';
GO

EXEC sp_rename N'[MhbtQsCure].[Adjust_User_ID]', N'AdjustUserId', N'COLUMN';
GO

EXEC sp_rename N'[MhbtQsCure].[Hosp_Seq_No]', N'HospSeqNo', N'COLUMN';
GO

EXEC sp_rename N'[MhbtQsCure].[Cure_Item]', N'CureItem', N'COLUMN';
GO

EXEC sp_rename N'[MhbtQsCure].[Func_Date]', N'FuncDate', N'COLUMN';
GO

EXEC sp_rename N'[MhbtQsCure].[Hosp_ID]', N'HospId', N'COLUMN';
GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[MhbtQsCure]') AND [c].[name] = N'Birthday');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [MhbtQsCure] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [MhbtQsCure] ALTER COLUMN [Birthday] nvarchar(450) NOT NULL;
GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[MhbtQsCure]') AND [c].[name] = N'TxtDate');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [MhbtQsCure] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [MhbtQsCure] ALTER COLUMN [TxtDate] nvarchar(max) NULL;
ALTER TABLE [MhbtQsCure] ADD DEFAULT ((('') collate Chinese_Taiwan_Stroke_CI_AS)) FOR [TxtDate];
GO

DECLARE @var2 sysname;
SELECT @var2 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[MhbtQsCure]') AND [c].[name] = N'FuncDate');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [MhbtQsCure] DROP CONSTRAINT [' + @var2 + '];');
ALTER TABLE [MhbtQsCure] ALTER COLUMN [FuncDate] nvarchar(450) NOT NULL;
GO

ALTER TABLE [MhbtQsCure] ADD [CreatedAt] datetime2 NULL;
GO

ALTER TABLE [MhbtQsCure] ADD [UpdatedAt] datetime2 NOT NULL DEFAULT '0001-01-01T00:00:00.0000000';
GO

UPDATE [GenEmpData] SET [CreatedAt] = '2021-07-04T22:22:33.6600112+08:00'
WHERE [Id] = N'00000000-0000-0000-0000-000000000000';
SELECT @@ROWCOUNT;

GO

UPDATE [Role] SET [CreatedAt] = '2021-07-04T22:22:33.6643252+08:00'
WHERE [Id] = N'00000000-0000-0000-0000-000000000000';
SELECT @@ROWCOUNT;

GO

UPDATE [RoleEmpMapping] SET [CreatedAt] = '2021-07-04T22:22:33.6645368+08:00'
WHERE [Id] = N'00000000-0000-0000-0000-000000000000';
SELECT @@ROWCOUNT;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20210704142236_AddMhbtQsCureUpdateColumn', N'5.0.7');
GO

COMMIT;
GO

