ALTER TABLE [HospContract] DROP CONSTRAINT [PK_HospContract];

GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[HospContract]') AND [c].[name] = N'HospStartDate');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [HospContract] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [HospContract] ALTER COLUMN [HospStartDate] char(8) NULL;

GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[HospContract]') AND [c].[name] = N'SMKContractType');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [HospContract] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [HospContract] ALTER COLUMN [SMKContractType] char(2) NULL;

GO

DECLARE @var2 sysname;
SELECT @var2 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[HospContract]') AND [c].[name] = N'HospSeqNo');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [HospContract] DROP CONSTRAINT [' + @var2 + '];');
ALTER TABLE [HospContract] ALTER COLUMN [HospSeqNo] char(2) NULL;

GO

DECLARE @var3 sysname;
SELECT @var3 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[HospContract]') AND [c].[name] = N'HospID');
IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [HospContract] DROP CONSTRAINT [' + @var3 + '];');
ALTER TABLE [HospContract] ALTER COLUMN [HospID] char(10) NULL;

GO

ALTER TABLE [HospContract] ADD [Id] int NOT NULL IDENTITY;

GO

ALTER TABLE [HospContract] ADD CONSTRAINT [PK_HospContract] PRIMARY KEY ([Id]);

GO

UPDATE [GenEmpData] SET [CreatedAt] = '2020-05-25T01:22:16.6114800+08:00'
WHERE [Id] = N'00000000-0000-0000-0000-000000000000';
SELECT @@ROWCOUNT;


GO

UPDATE [Role] SET [CreatedAt] = '2020-05-25T01:22:16.6133583+08:00'
WHERE [Id] = N'00000000-0000-0000-0000-000000000000';
SELECT @@ROWCOUNT;


GO

UPDATE [RoleEmpMapping] SET [CreatedAt] = '2020-05-25T01:22:16.6135103+08:00'
WHERE [Id] = N'00000000-0000-0000-0000-000000000000';
SELECT @@ROWCOUNT;


GO

CREATE UNIQUE INDEX [IX_HospContract_HospID_HospSeqNo_SMKContractType_HospStartDate] ON [HospContract] ([HospID], [HospSeqNo], [SMKContractType], [HospStartDate]) WHERE [HospID] IS NOT NULL AND [HospSeqNo] IS NOT NULL AND [SMKContractType] IS NOT NULL AND [HospStartDate] IS NOT NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200524172216_HospContract', N'3.1.3');

GO

