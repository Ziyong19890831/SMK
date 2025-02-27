BEGIN TRANSACTION;
GO

EXEC sp_rename N'[IniFileInCtl]', N'IniFileInCtrl';
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20210706083131_UpdateIniFileInCtrl', N'5.0.7');
GO

COMMIT;
GO

