BEGIN TRANSACTION;
GO

EXEC sp_rename N'[IniFileInCtrl].[FileDate]', N'StartedAt', N'COLUMN';
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20210707132709_AlterIniFileInCtrl', N'5.0.7');
GO

COMMIT;
GO

