ALTER TABLE [PrsnContract] DROP CONSTRAINT [PK_PrsnContract];

GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[PrsnContract]') AND [c].[name] = N'PrsnStartDate');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [PrsnContract] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [PrsnContract] ALTER COLUMN [PrsnStartDate] char(8) NULL;
ALTER TABLE [PrsnContract] ADD DEFAULT (('')) FOR [PrsnStartDate];

GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[PrsnContract]') AND [c].[name] = N'SMKContractType');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [PrsnContract] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [PrsnContract] ALTER COLUMN [SMKContractType] char(10) NULL;

GO

DECLARE @var2 sysname;
SELECT @var2 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[PrsnContract]') AND [c].[name] = N'PrsnID');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [PrsnContract] DROP CONSTRAINT [' + @var2 + '];');
ALTER TABLE [PrsnContract] ALTER COLUMN [PrsnID] char(10) NULL;

GO

DECLARE @var3 sysname;
SELECT @var3 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[PrsnContract]') AND [c].[name] = N'HospSeqNo');
IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [PrsnContract] DROP CONSTRAINT [' + @var3 + '];');
ALTER TABLE [PrsnContract] ALTER COLUMN [HospSeqNo] char(2) NULL;

GO

DECLARE @var4 sysname;
SELECT @var4 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[PrsnContract]') AND [c].[name] = N'HospID');
IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [PrsnContract] DROP CONSTRAINT [' + @var4 + '];');
ALTER TABLE [PrsnContract] ALTER COLUMN [HospID] char(10) NULL;

GO

ALTER TABLE [PrsnContract] ADD [Id] int NOT NULL IDENTITY;

GO

ALTER TABLE [PrsnBasic] ADD [PEmail] varchar(80) NULL;

GO

ALTER TABLE [PrsnContract] ADD CONSTRAINT [PK_PrsnContract] PRIMARY KEY ([Id]);

GO

CREATE TABLE [PrsnEmail] (
    [PrsnID] char(10) NOT NULL,
    [PEmail] varchar(80) NOT NULL,
    CONSTRAINT [PK_PrsnEmail] PRIMARY KEY ([PrsnID], [PEmail])
);

GO

UPDATE [GenEmpData] SET [CreatedAt] = '2020-05-27T21:03:03.9757159+08:00'
WHERE [Id] = N'00000000-0000-0000-0000-000000000000';
SELECT @@ROWCOUNT;


GO

UPDATE [Role] SET [CreatedAt] = '2020-05-27T21:03:03.9775874+08:00'
WHERE [Id] = N'00000000-0000-0000-0000-000000000000';
SELECT @@ROWCOUNT;


GO

UPDATE [RoleEmpMapping] SET [CreatedAt] = '2020-05-27T21:03:03.9777372+08:00'
WHERE [Id] = N'00000000-0000-0000-0000-000000000000';
SELECT @@ROWCOUNT;


GO

CREATE UNIQUE INDEX [IX_PrsnContract_HospID_HospSeqNo_PrsnID_SMKContractType_PrsnStartDate] ON [PrsnContract] ([HospID], [HospSeqNo], [PrsnID], [SMKContractType], [PrsnStartDate]) WHERE [HospID] IS NOT NULL AND [HospSeqNo] IS NOT NULL AND [PrsnID] IS NOT NULL AND [SMKContractType] IS NOT NULL AND [PrsnStartDate] IS NOT NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200527130304_prsnFix', N'3.1.3');

GO

