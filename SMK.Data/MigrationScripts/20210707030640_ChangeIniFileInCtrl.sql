BEGIN TRANSACTION;
GO

CREATE INDEX [IX_iniDrDtl_tran_date] ON [iniDrDtl] ([tran_date]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20210706172200_AddIniDrDtlIndex', N'5.0.7');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[IniFileInCtrl]') AND [c].[name] = N'IniDrDtlStatus');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [IniFileInCtrl] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [IniFileInCtrl] DROP COLUMN [IniDrDtlStatus];
GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[IniFileInCtrl]') AND [c].[name] = N'IniDrDtlStatusUpdatedAt');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [IniFileInCtrl] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [IniFileInCtrl] DROP COLUMN [IniDrDtlStatusUpdatedAt];
GO

DECLARE @var2 sysname;
SELECT @var2 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[IniFileInCtrl]') AND [c].[name] = N'IniDrOrdStatus');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [IniFileInCtrl] DROP CONSTRAINT [' + @var2 + '];');
ALTER TABLE [IniFileInCtrl] DROP COLUMN [IniDrOrdStatus];
GO

DECLARE @var3 sysname;
SELECT @var3 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[IniFileInCtrl]') AND [c].[name] = N'IniDrOrdStatusUpdatedAt');
IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [IniFileInCtrl] DROP CONSTRAINT [' + @var3 + '];');
ALTER TABLE [IniFileInCtrl] DROP COLUMN [IniDrOrdStatusUpdatedAt];
GO

DECLARE @var4 sysname;
SELECT @var4 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[IniFileInCtrl]') AND [c].[name] = N'IniOpDtlStatus');
IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [IniFileInCtrl] DROP CONSTRAINT [' + @var4 + '];');
ALTER TABLE [IniFileInCtrl] DROP COLUMN [IniOpDtlStatus];
GO

DECLARE @var5 sysname;
SELECT @var5 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[IniFileInCtrl]') AND [c].[name] = N'IniOpDtlStatusUpdatedAt');
IF @var5 IS NOT NULL EXEC(N'ALTER TABLE [IniFileInCtrl] DROP CONSTRAINT [' + @var5 + '];');
ALTER TABLE [IniFileInCtrl] DROP COLUMN [IniOpDtlStatusUpdatedAt];
GO

EXEC sp_rename N'[IniFileInCtrl].[IniOpOrdStatusUpdatedAt]', N'StatusUpdatedAt', N'COLUMN';
GO

EXEC sp_rename N'[IniFileInCtrl].[IniOpOrdStatus]', N'Status', N'COLUMN';
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20210707030640_ChangeIniFileInCtrl', N'5.0.7');
GO

COMMIT;
GO
