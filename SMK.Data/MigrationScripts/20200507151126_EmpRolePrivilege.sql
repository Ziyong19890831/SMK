DROP TABLE [GenProgramData];

GO

ALTER TABLE [GenEmpData] DROP CONSTRAINT [PK_GenEmpData];

GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[GenEmpData]') AND [c].[name] = N'EmpNo');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [GenEmpData] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [GenEmpData] DROP COLUMN [EmpNo];

GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[GenEmpData]') AND [c].[name] = N'CreateAt');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [GenEmpData] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [GenEmpData] DROP COLUMN [CreateAt];

GO

DECLARE @var2 sysname;
SELECT @var2 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[GenEmpData]') AND [c].[name] = N'EmpName');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [GenEmpData] DROP CONSTRAINT [' + @var2 + '];');
ALTER TABLE [GenEmpData] DROP COLUMN [EmpName];

GO

DECLARE @var3 sysname;
SELECT @var3 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[GenEmpData]') AND [c].[name] = N'LoginID');
IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [GenEmpData] DROP CONSTRAINT [' + @var3 + '];');
ALTER TABLE [GenEmpData] DROP COLUMN [LoginID];

GO

DECLARE @var4 sysname;
SELECT @var4 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[GenEmpData]') AND [c].[name] = N'LoginPWD');
IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [GenEmpData] DROP CONSTRAINT [' + @var4 + '];');
ALTER TABLE [GenEmpData] DROP COLUMN [LoginPWD];

GO

DECLARE @var5 sysname;
SELECT @var5 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[GenEmpData]') AND [c].[name] = N'UpdateAt');
IF @var5 IS NOT NULL EXEC(N'ALTER TABLE [GenEmpData] DROP CONSTRAINT [' + @var5 + '];');
ALTER TABLE [GenEmpData] DROP COLUMN [UpdateAt];

GO

DECLARE @var6 sysname;
SELECT @var6 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[GenEmpData]') AND [c].[name] = N'UpdateBy');
IF @var6 IS NOT NULL EXEC(N'ALTER TABLE [GenEmpData] DROP CONSTRAINT [' + @var6 + '];');
ALTER TABLE [GenEmpData] DROP COLUMN [UpdateBy];

GO

ALTER TABLE [GenEmpData] ADD [Id] nvarchar(36) NOT NULL DEFAULT N'';

GO

ALTER TABLE [GenEmpData] ADD [Account] nvarchar(128) NULL;

GO

ALTER TABLE [GenEmpData] ADD [CreatedAt] datetime2 NOT NULL DEFAULT '0001-01-01T00:00:00.0000000';

GO

ALTER TABLE [GenEmpData] ADD [LastLoginDate] datetime2 NOT NULL DEFAULT '0001-01-01T00:00:00.0000000';

GO

ALTER TABLE [GenEmpData] ADD [Name] nvarchar(max) NULL;

GO

ALTER TABLE [GenEmpData] ADD [Pwd] nvarchar(256) NULL;

GO

ALTER TABLE [GenEmpData] ADD [UpdatedAt] datetime2 NULL;

GO

ALTER TABLE [GenEmpData] ADD [UpdatedBy] nvarchar(127) NULL;

GO

ALTER TABLE [GenEmpData] ADD CONSTRAINT [PK_GenEmpData] PRIMARY KEY ([Id]);

GO

CREATE TABLE [Privilege] (
    [Id] nvarchar(36) NOT NULL,
    [ParentId] nvarchar(36) NULL,
    [Name] nvarchar(max) NULL,
    [Sort] int NOT NULL,
    [Type] nvarchar(max) NULL,
    [ControllerName] nvarchar(max) NULL,
    [ActionName] nvarchar(max) NULL,
    [QueryParams] nvarchar(512) NULL,
    [Remark] nvarchar(512) NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    [UpdatedBy] nvarchar(127) NULL,
    CONSTRAINT [PK_Privilege] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [Role] (
    [Id] nvarchar(36) NOT NULL,
    [Name] nvarchar(450) NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    [UpdatedBy] nvarchar(127) NULL,
    CONSTRAINT [PK_Role] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [RoleEmpMapping] (
    [Id] nvarchar(36) NOT NULL,
    [EmpId] nvarchar(36) NULL,
    [RoleId] nvarchar(36) NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    [UpdatedBy] nvarchar(127) NULL,
    CONSTRAINT [PK_RoleEmpMapping] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [RolePrivilegeMapping] (
    [Id] nvarchar(36) NOT NULL,
    [RoleId] nvarchar(36) NULL,
    [PrivilegeId] nvarchar(36) NULL,
    [EnableEntry] bit NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    [UpdatedBy] nvarchar(127) NULL,
    CONSTRAINT [PK_RolePrivilegeMapping] PRIMARY KEY ([Id])
);

GO

CREATE UNIQUE INDEX [IX_GenEmpData_Account] ON [GenEmpData] ([Account]) WHERE [Account] IS NOT NULL;

GO

CREATE INDEX [IX_Privilege_ParentId] ON [Privilege] ([ParentId]);

GO

CREATE UNIQUE INDEX [IX_Role_Name] ON [Role] ([Name]) WHERE [Name] IS NOT NULL;

GO

CREATE UNIQUE INDEX [IX_RoleEmpMapping_EmpId_RoleId] ON [RoleEmpMapping] ([EmpId], [RoleId]) WHERE [EmpId] IS NOT NULL AND [RoleId] IS NOT NULL;

GO

CREATE UNIQUE INDEX [IX_RolePrivilegeMapping_PrivilegeId_RoleId] ON [RolePrivilegeMapping] ([PrivilegeId], [RoleId]) WHERE [PrivilegeId] IS NOT NULL AND [RoleId] IS NOT NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200507151126_EmpRolePrivilege', N'3.1.3');

GO

