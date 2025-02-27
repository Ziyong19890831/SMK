DROP INDEX [IX_HospContract_HospID_HospSeqNo_SMKContractType_HospStartDate] ON [HospContract];

GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[HospContract]') AND [c].[name] = N'HospSeqNo');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [HospContract] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [HospContract] ALTER COLUMN [HospSeqNo] char(2) NOT NULL;

GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[HospContract]') AND [c].[name] = N'HospID');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [HospContract] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [HospContract] ALTER COLUMN [HospID] char(10) NOT NULL;

GO

UPDATE [GenEmpData] SET [CreatedAt] = '2020-06-01T00:16:29.4172510+08:00'
WHERE [Id] = N'00000000-0000-0000-0000-000000000000';
SELECT @@ROWCOUNT;


GO

UPDATE [Role] SET [CreatedAt] = '2020-06-01T00:16:29.4192261+08:00'
WHERE [Id] = N'00000000-0000-0000-0000-000000000000';
SELECT @@ROWCOUNT;


GO

UPDATE [RoleEmpMapping] SET [CreatedAt] = '2020-06-01T00:16:29.4193975+08:00'
WHERE [Id] = N'00000000-0000-0000-0000-000000000000';
SELECT @@ROWCOUNT;


GO

CREATE UNIQUE INDEX [IX_HospContract_HospID_HospSeqNo_SMKContractType_HospStartDate] ON [HospContract] ([HospID], [HospSeqNo], [SMKContractType], [HospStartDate]) WHERE [SMKContractType] IS NOT NULL AND [HospStartDate] IS NOT NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200531161629_fixHospContract_HospId_HospSeqno', N'3.1.3');

GO

