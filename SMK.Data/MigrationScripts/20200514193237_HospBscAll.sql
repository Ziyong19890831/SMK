CREATE TABLE [HospBscAll] (
    [HospID] char(10) NOT NULL,
    [HospName] nvarchar(80) NULL DEFAULT (('')),
    [HospTelArea] char(5) NULL DEFAULT (('')),
    [HospTel] char(20) NULL DEFAULT (('')),
    [BranchNo] char(1) NULL DEFAULT (('')),
    [HospAddress] varchar(80) NULL DEFAULT (('')),
    [ContType] char(5) NULL DEFAULT (('')),
    [HospType] char(5) NULL DEFAULT (('')),
    [HospKind] char(5) NULL DEFAULT (('')),
    [HospEndDate] char(8) NULL DEFAULT (('')),
    CONSTRAINT [PK_HospBscAll] PRIMARY KEY ([HospID])
);

GO

UPDATE [GenEmpData] SET [CreatedAt] = '2020-05-15T03:32:37.5155309+08:00'
WHERE [Id] = N'00000000-0000-0000-0000-000000000000';
SELECT @@ROWCOUNT;


GO

UPDATE [Role] SET [CreatedAt] = '2020-05-15T03:32:37.5174130+08:00'
WHERE [Id] = N'00000000-0000-0000-0000-000000000000';
SELECT @@ROWCOUNT;


GO

UPDATE [RoleEmpMapping] SET [CreatedAt] = '2020-05-15T03:32:37.5175667+08:00'
WHERE [Id] = N'00000000-0000-0000-0000-000000000000';
SELECT @@ROWCOUNT;


GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200514193237_HospBscAll', N'3.1.3');

GO

