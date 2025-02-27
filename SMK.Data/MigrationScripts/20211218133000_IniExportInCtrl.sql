BEGIN TRANSACTION;
GO

CREATE TABLE [IniExportInCtrl] (
    [Id] int NOT NULL IDENTITY,
    [fee_ym] varchar(6) NULL,
    [StartedAt] datetime2 NOT NULL,
    [Status] varchar(20) NOT NULL,
    [StatusUpdatedAt] datetime2 NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    CONSTRAINT [PK_IniExportInCtrl] PRIMARY KEY ([Id])
);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20211218133000_IniExportInCtrl', N'5.0.7');
GO

COMMIT;
GO

