DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[PrsnLicence]') AND [c].[name] = N'LicenceEndDate');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [PrsnLicence] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [PrsnLicence] DROP COLUMN [LicenceEndDate];

GO

DROP INDEX [IX_PrsnLicence_PrsnID_LicenceType_LicenceNo] ON [PrsnLicence];
DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[PrsnLicence]') AND [c].[name] = N'LicenceNo');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [PrsnLicence] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [PrsnLicence] ALTER COLUMN [LicenceNo] nvarchar(30) NULL;
CREATE UNIQUE INDEX [IX_PrsnLicence_PrsnID_LicenceType_LicenceNo] ON [PrsnLicence] ([PrsnID], [LicenceType], [LicenceNo]) WHERE [PrsnID] IS NOT NULL AND [LicenceType] IS NOT NULL AND [LicenceNo] IS NOT NULL;

GO

ALTER TABLE [PrsnLicence] ADD [CertEndDate] nvarchar(8) NULL DEFAULT ((''));

GO

ALTER TABLE [PrsnLicence] ADD [CertPublicDate] nvarchar(8) NULL DEFAULT ((''));

GO

ALTER TABLE [PrsnLicence] ADD [CertStartDate] nvarchar(8) NULL DEFAULT ((''));

GO

ALTER TABLE [PrsnLicence] ADD [CreatedAt] datetime2 NOT NULL DEFAULT '0001-01-01T00:00:00.0000000';

GO

ALTER TABLE [PrsnLicence] ADD [UpdatedAt] datetime2 NULL;

GO

ALTER TABLE [PrsnLicence] ADD [UpdatedBy] nvarchar(127) NULL;

GO

CREATE TABLE [QsLicenceMap] (
    [Id] int NOT NULL IDENTITY,
    [LicenceType] nvarchar(2) NULL,
    [CTypeSNO] int NOT NULL,
    [CTypeName] nvarchar(50) NULL,
    CONSTRAINT [PK_QsLicenceMap] PRIMARY KEY ([Id])
);

GO

UPDATE [GenEmpData] SET [CreatedAt] = '2020-06-04T00:56:03.9547114+08:00'
WHERE [Id] = N'00000000-0000-0000-0000-000000000000';
SELECT @@ROWCOUNT;


GO

UPDATE [Role] SET [CreatedAt] = '2020-06-04T00:56:03.9566231+08:00'
WHERE [Id] = N'00000000-0000-0000-0000-000000000000';
SELECT @@ROWCOUNT;


GO

UPDATE [RoleEmpMapping] SET [CreatedAt] = '2020-06-04T00:56:03.9567728+08:00'
WHERE [Id] = N'00000000-0000-0000-0000-000000000000';
SELECT @@ROWCOUNT;


GO

CREATE UNIQUE INDEX [IX_QsLicenceMap_LicenceType_CTypeSNO] ON [QsLicenceMap] ([LicenceType], [CTypeSNO]) WHERE [LicenceType] IS NOT NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200603165604_QsLicenceMap', N'3.1.3');

GO

