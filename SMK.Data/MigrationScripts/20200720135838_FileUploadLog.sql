CREATE TABLE [FileUploadLog] (
    [Id] nvarchar(36) NOT NULL,
    [FileType] nvarchar(max) NULL,
    [FileName] nvarchar(128) NULL,
    [CreatedAt] datetime2 NOT NULL,
    [CreatedBy] nvarchar(127) NULL,
    [UpdatedAt] datetime2 NULL,
    [UpdatedBy] nvarchar(127) NULL,
    CONSTRAINT [PK_FileUploadLog] PRIMARY KEY ([Id])
);

GO

UPDATE [GenEmpData] SET [CreatedAt] = '2020-07-20T21:58:38.3224190+08:00'
WHERE [Id] = N'00000000-0000-0000-0000-000000000000';
SELECT @@ROWCOUNT;


GO

UPDATE [Role] SET [CreatedAt] = '2020-07-20T21:58:38.3243414+08:00'
WHERE [Id] = N'00000000-0000-0000-0000-000000000000';
SELECT @@ROWCOUNT;


GO

UPDATE [RoleEmpMapping] SET [CreatedAt] = '2020-07-20T21:58:38.3244981+08:00'
WHERE [Id] = N'00000000-0000-0000-0000-000000000000';
SELECT @@ROWCOUNT;


GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200720135838_FileUploadLog', N'3.1.3');

GO

