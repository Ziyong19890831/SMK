﻿ALTER TABLE [HospBasic] ADD [CreateDate] char(8) NULL DEFAULT ((''));

GO

ALTER TABLE [HospBasic] ADD [ModifyDate] char(8) NULL DEFAULT ((''));

GO

UPDATE [GenEmpData] SET [CreatedAt] = '2020-05-19T01:53:08.4979901+08:00'
WHERE [Id] = N'00000000-0000-0000-0000-000000000000';
SELECT @@ROWCOUNT;


GO

UPDATE [Role] SET [CreatedAt] = '2020-05-19T01:53:08.4998076+08:00'
WHERE [Id] = N'00000000-0000-0000-0000-000000000000';
SELECT @@ROWCOUNT;


GO

UPDATE [RoleEmpMapping] SET [CreatedAt] = '2020-05-19T01:53:08.4999557+08:00'
WHERE [Id] = N'00000000-0000-0000-0000-000000000000';
SELECT @@ROWCOUNT;


GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200518175308_HospBasicFix', N'3.1.3');

GO

