﻿ALTER TABLE [FileUploadLog] ADD [FileStatus] nvarchar(max) NULL;

GO

UPDATE [GenEmpData] SET [CreatedAt] = '2020-07-20T23:59:12.0437807+08:00'
WHERE [Id] = N'00000000-0000-0000-0000-000000000000';
SELECT @@ROWCOUNT;


GO

UPDATE [Role] SET [CreatedAt] = '2020-07-20T23:59:12.0456502+08:00'
WHERE [Id] = N'00000000-0000-0000-0000-000000000000';
SELECT @@ROWCOUNT;


GO

UPDATE [RoleEmpMapping] SET [CreatedAt] = '2020-07-20T23:59:12.0457995+08:00'
WHERE [Id] = N'00000000-0000-0000-0000-000000000000';
SELECT @@ROWCOUNT;


GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200720155912_FileUploadLog_AddFileStatus', N'3.1.3');

GO

