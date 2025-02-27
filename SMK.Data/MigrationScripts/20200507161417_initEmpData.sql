IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Account', N'CreatedAt', N'Enable', N'LastLoginDate', N'Name', N'Pwd', N'UpdatedAt', N'UpdatedBy') AND [object_id] = OBJECT_ID(N'[GenEmpData]'))
    SET IDENTITY_INSERT [GenEmpData] ON;
INSERT INTO [GenEmpData] ([Id], [Account], [CreatedAt], [Enable], [LastLoginDate], [Name], [Pwd], [UpdatedAt], [UpdatedBy])
VALUES (N'00000000-0000-0000-0000-000000000000', N'Admin', '2020-05-08T00:14:17.6763004+08:00', CAST(1 AS bit), '0001-01-01T00:00:00.0000000', N'系統管理員', N'ZHACEBb7ESmBmYj7XqLotw==', NULL, NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Account', N'CreatedAt', N'Enable', N'LastLoginDate', N'Name', N'Pwd', N'UpdatedAt', N'UpdatedBy') AND [object_id] = OBJECT_ID(N'[GenEmpData]'))
    SET IDENTITY_INSERT [GenEmpData] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CreatedAt', N'Name', N'UpdatedAt', N'UpdatedBy') AND [object_id] = OBJECT_ID(N'[Role]'))
    SET IDENTITY_INSERT [Role] ON;
INSERT INTO [Role] ([Id], [CreatedAt], [Name], [UpdatedAt], [UpdatedBy])
VALUES (N'00000000-0000-0000-0000-000000000000', '2020-05-08T00:14:17.6781398+08:00', N'SuperAdmin', NULL, NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CreatedAt', N'Name', N'UpdatedAt', N'UpdatedBy') AND [object_id] = OBJECT_ID(N'[Role]'))
    SET IDENTITY_INSERT [Role] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CreatedAt', N'EmpId', N'RoleId', N'UpdatedAt', N'UpdatedBy') AND [object_id] = OBJECT_ID(N'[RoleEmpMapping]'))
    SET IDENTITY_INSERT [RoleEmpMapping] ON;
INSERT INTO [RoleEmpMapping] ([Id], [CreatedAt], [EmpId], [RoleId], [UpdatedAt], [UpdatedBy])
VALUES (N'00000000-0000-0000-0000-000000000000', '2020-05-08T00:14:17.6782888+08:00', N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', NULL, NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CreatedAt', N'EmpId', N'RoleId', N'UpdatedAt', N'UpdatedBy') AND [object_id] = OBJECT_ID(N'[RoleEmpMapping]'))
    SET IDENTITY_INSERT [RoleEmpMapping] OFF;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200507161417_initEmpData', N'3.1.3');

GO

