CREATE TABLE [PrsnLicence] (
    [Id] int NOT NULL IDENTITY,
    [PrsnID] char(10) NULL,
    [LicenceType] char(2) NULL,
    [LicenceNo] char(20) NULL,
    [LicenceEndDate] char(8) NULL DEFAULT (('')),
    CONSTRAINT [PK_PrsnLicence] PRIMARY KEY ([Id])
);

GO

UPDATE [GenEmpData] SET [CreatedAt] = '2020-05-28T01:15:08.0906280+08:00'
WHERE [Id] = N'00000000-0000-0000-0000-000000000000';
SELECT @@ROWCOUNT;


GO

UPDATE [Role] SET [CreatedAt] = '2020-05-28T01:15:08.0925426+08:00'
WHERE [Id] = N'00000000-0000-0000-0000-000000000000';
SELECT @@ROWCOUNT;


GO

UPDATE [RoleEmpMapping] SET [CreatedAt] = '2020-05-28T01:15:08.0926976+08:00'
WHERE [Id] = N'00000000-0000-0000-0000-000000000000';
SELECT @@ROWCOUNT;


GO

CREATE UNIQUE INDEX [IX_PrsnLicence_PrsnID_LicenceType_LicenceNo] ON [PrsnLicence] ([PrsnID], [LicenceType], [LicenceNo]) WHERE [PrsnID] IS NOT NULL AND [LicenceType] IS NOT NULL AND [LicenceNo] IS NOT NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200527171508_prsnLicence', N'3.1.3');

GO

