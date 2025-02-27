BEGIN TRANSACTION;
GO

CREATE TABLE [IniFileInCtl] (
    [Id] int NOT NULL IDENTITY,
    [Filename] nvarchar(255) NULL,
    [FileDate] datetime2 NOT NULL,
    [IniDrDtlStatus] varchar(20) NOT NULL,
    [IniDrOrdStatus] varchar(20) NOT NULL,
    [IniOpDtlStatus] varchar(20) NOT NULL,
    [IniOpOrdStatus] varchar(20) NOT NULL,
    [IniDrDtlStatusUpdatedAt] datetime2 NULL,
    [IniDrOrdStatusUpdatedAt] datetime2 NULL,
    [IniOpDtlStatusUpdatedAt] datetime2 NULL,
    [IniOpOrdStatusUpdatedAt] datetime2 NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    CONSTRAINT [PK_IniFileInCtrl] PRIMARY KEY ([Id])
);
GO

UPDATE [GenEmpData] SET [CreatedAt] = '2021-07-06T13:39:05.6278438+08:00'
WHERE [Id] = N'00000000-0000-0000-0000-000000000000';
SELECT @@ROWCOUNT;

GO

UPDATE [Role] SET [CreatedAt] = '2021-07-06T13:39:05.6321710+08:00'
WHERE [Id] = N'00000000-0000-0000-0000-000000000000';
SELECT @@ROWCOUNT;

GO

UPDATE [RoleEmpMapping] SET [CreatedAt] = '2021-07-06T13:39:05.6323958+08:00'
WHERE [Id] = N'00000000-0000-0000-0000-000000000000';
SELECT @@ROWCOUNT;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20210706082003_AddIniFileInCtrl', N'5.0.7');
GO

COMMIT;
GO
