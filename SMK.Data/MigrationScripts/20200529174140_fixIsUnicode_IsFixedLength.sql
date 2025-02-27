DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[PrsnLicence]') AND [c].[name] = N'Remark');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [PrsnLicence] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [PrsnLicence] ALTER COLUMN [Remark] nvarchar(200) NULL;

GO

DROP INDEX [IX_PrsnLicence_PrsnID_LicenceType_LicenceNo] ON [PrsnLicence];
DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[PrsnLicence]') AND [c].[name] = N'PrsnID');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [PrsnLicence] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [PrsnLicence] ALTER COLUMN [PrsnID] nvarchar(10) NULL;
CREATE UNIQUE INDEX [IX_PrsnLicence_PrsnID_LicenceType_LicenceNo] ON [PrsnLicence] ([PrsnID], [LicenceType], [LicenceNo]) WHERE [PrsnID] IS NOT NULL AND [LicenceType] IS NOT NULL AND [LicenceNo] IS NOT NULL;

GO

DROP INDEX [IX_PrsnLicence_PrsnID_LicenceType_LicenceNo] ON [PrsnLicence];
DECLARE @var2 sysname;
SELECT @var2 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[PrsnLicence]') AND [c].[name] = N'LicenceType');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [PrsnLicence] DROP CONSTRAINT [' + @var2 + '];');
ALTER TABLE [PrsnLicence] ALTER COLUMN [LicenceType] nvarchar(2) NULL;
CREATE UNIQUE INDEX [IX_PrsnLicence_PrsnID_LicenceType_LicenceNo] ON [PrsnLicence] ([PrsnID], [LicenceType], [LicenceNo]) WHERE [PrsnID] IS NOT NULL AND [LicenceType] IS NOT NULL AND [LicenceNo] IS NOT NULL;

GO

DROP INDEX [IX_PrsnLicence_PrsnID_LicenceType_LicenceNo] ON [PrsnLicence];
DECLARE @var3 sysname;
SELECT @var3 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[PrsnLicence]') AND [c].[name] = N'LicenceNo');
IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [PrsnLicence] DROP CONSTRAINT [' + @var3 + '];');
ALTER TABLE [PrsnLicence] ALTER COLUMN [LicenceNo] nvarchar(20) NULL;
CREATE UNIQUE INDEX [IX_PrsnLicence_PrsnID_LicenceType_LicenceNo] ON [PrsnLicence] ([PrsnID], [LicenceType], [LicenceNo]) WHERE [PrsnID] IS NOT NULL AND [LicenceType] IS NOT NULL AND [LicenceNo] IS NOT NULL;

GO

DECLARE @var4 sysname;
SELECT @var4 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[PrsnLicence]') AND [c].[name] = N'LicenceEndDate');
IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [PrsnLicence] DROP CONSTRAINT [' + @var4 + '];');
ALTER TABLE [PrsnLicence] ALTER COLUMN [LicenceEndDate] nvarchar(8) NULL;
ALTER TABLE [PrsnLicence] ADD DEFAULT (('')) FOR [LicenceEndDate];

GO

DROP INDEX [IX_PrsnContract_HospID_HospSeqNo_PrsnID_SMKContractType_PrsnStartDate] ON [PrsnContract];
DECLARE @var5 sysname;
SELECT @var5 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[PrsnContract]') AND [c].[name] = N'SMKContractType');
IF @var5 IS NOT NULL EXEC(N'ALTER TABLE [PrsnContract] DROP CONSTRAINT [' + @var5 + '];');
ALTER TABLE [PrsnContract] ALTER COLUMN [SMKContractType] nvarchar(10) NULL;
CREATE UNIQUE INDEX [IX_PrsnContract_HospID_HospSeqNo_PrsnID_SMKContractType_PrsnStartDate] ON [PrsnContract] ([HospID], [HospSeqNo], [PrsnID], [SMKContractType], [PrsnStartDate]) WHERE [HospID] IS NOT NULL AND [HospSeqNo] IS NOT NULL AND [PrsnID] IS NOT NULL AND [SMKContractType] IS NOT NULL AND [PrsnStartDate] IS NOT NULL;

GO

DECLARE @var6 sysname;
SELECT @var6 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[PrsnContract]') AND [c].[name] = N'Remark');
IF @var6 IS NOT NULL EXEC(N'ALTER TABLE [PrsnContract] DROP CONSTRAINT [' + @var6 + '];');
ALTER TABLE [PrsnContract] ALTER COLUMN [Remark] nvarchar(200) NULL;
ALTER TABLE [PrsnContract] ADD DEFAULT (('')) FOR [Remark];

GO

DROP INDEX [IX_PrsnContract_HospID_HospSeqNo_PrsnID_SMKContractType_PrsnStartDate] ON [PrsnContract];
DECLARE @var7 sysname;
SELECT @var7 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[PrsnContract]') AND [c].[name] = N'PrsnStartDate');
IF @var7 IS NOT NULL EXEC(N'ALTER TABLE [PrsnContract] DROP CONSTRAINT [' + @var7 + '];');
ALTER TABLE [PrsnContract] ALTER COLUMN [PrsnStartDate] nvarchar(8) NULL;
ALTER TABLE [PrsnContract] ADD DEFAULT (('')) FOR [PrsnStartDate];
CREATE UNIQUE INDEX [IX_PrsnContract_HospID_HospSeqNo_PrsnID_SMKContractType_PrsnStartDate] ON [PrsnContract] ([HospID], [HospSeqNo], [PrsnID], [SMKContractType], [PrsnStartDate]) WHERE [HospID] IS NOT NULL AND [HospSeqNo] IS NOT NULL AND [PrsnID] IS NOT NULL AND [SMKContractType] IS NOT NULL AND [PrsnStartDate] IS NOT NULL;

GO

DECLARE @var8 sysname;
SELECT @var8 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[PrsnContract]') AND [c].[name] = N'PrsnEndDate');
IF @var8 IS NOT NULL EXEC(N'ALTER TABLE [PrsnContract] DROP CONSTRAINT [' + @var8 + '];');
ALTER TABLE [PrsnContract] ALTER COLUMN [PrsnEndDate] nvarchar(8) NULL;
ALTER TABLE [PrsnContract] ADD DEFAULT (('')) FOR [PrsnEndDate];

GO

DECLARE @var9 sysname;
SELECT @var9 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[PrsnContract]') AND [c].[name] = N'ModifyDate');
IF @var9 IS NOT NULL EXEC(N'ALTER TABLE [PrsnContract] DROP CONSTRAINT [' + @var9 + '];');
ALTER TABLE [PrsnContract] ALTER COLUMN [ModifyDate] nvarchar(8) NULL;
ALTER TABLE [PrsnContract] ADD DEFAULT (('')) FOR [ModifyDate];

GO

DECLARE @var10 sysname;
SELECT @var10 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[PrsnContract]') AND [c].[name] = N'CreateDate');
IF @var10 IS NOT NULL EXEC(N'ALTER TABLE [PrsnContract] DROP CONSTRAINT [' + @var10 + '];');
ALTER TABLE [PrsnContract] ALTER COLUMN [CreateDate] nvarchar(8) NULL;
ALTER TABLE [PrsnContract] ADD DEFAULT (('')) FOR [CreateDate];

GO

DECLARE @var11 sysname;
SELECT @var11 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[PrsnContract]') AND [c].[name] = N'CouldTreat');
IF @var11 IS NOT NULL EXEC(N'ALTER TABLE [PrsnContract] DROP CONSTRAINT [' + @var11 + '];');
ALTER TABLE [PrsnContract] ALTER COLUMN [CouldTreat] nvarchar(1) NULL;
ALTER TABLE [PrsnContract] ADD DEFAULT (('')) FOR [CouldTreat];

GO

DECLARE @var12 sysname;
SELECT @var12 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[PrsnContract]') AND [c].[name] = N'CouldInstruct');
IF @var12 IS NOT NULL EXEC(N'ALTER TABLE [PrsnContract] DROP CONSTRAINT [' + @var12 + '];');
ALTER TABLE [PrsnContract] ALTER COLUMN [CouldInstruct] nvarchar(1) NULL;
ALTER TABLE [PrsnContract] ADD DEFAULT (('')) FOR [CouldInstruct];

GO

DECLARE @var13 sysname;
SELECT @var13 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[PrsnBasic]') AND [c].[name] = N'SubSpecialistNo');
IF @var13 IS NOT NULL EXEC(N'ALTER TABLE [PrsnBasic] DROP CONSTRAINT [' + @var13 + '];');
ALTER TABLE [PrsnBasic] ALTER COLUMN [SubSpecialistNo] nvarchar(5) NULL;
ALTER TABLE [PrsnBasic] ADD DEFAULT (('')) FOR [SubSpecialistNo];

GO

DECLARE @var14 sysname;
SELECT @var14 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[PrsnBasic]') AND [c].[name] = N'Remark');
IF @var14 IS NOT NULL EXEC(N'ALTER TABLE [PrsnBasic] DROP CONSTRAINT [' + @var14 + '];');
ALTER TABLE [PrsnBasic] ALTER COLUMN [Remark] nvarchar(200) NULL;
ALTER TABLE [PrsnBasic] ADD DEFAULT (('')) FOR [Remark];

GO

DECLARE @var15 sysname;
SELECT @var15 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[PrsnBasic]') AND [c].[name] = N'PrsnType');
IF @var15 IS NOT NULL EXEC(N'ALTER TABLE [PrsnBasic] DROP CONSTRAINT [' + @var15 + '];');
ALTER TABLE [PrsnBasic] ALTER COLUMN [PrsnType] nvarchar(1) NULL;
ALTER TABLE [PrsnBasic] ADD DEFAULT (('')) FOR [PrsnType];

GO

DECLARE @var16 sysname;
SELECT @var16 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[PrsnBasic]') AND [c].[name] = N'PrsnName');
IF @var16 IS NOT NULL EXEC(N'ALTER TABLE [PrsnBasic] DROP CONSTRAINT [' + @var16 + '];');
ALTER TABLE [PrsnBasic] ALTER COLUMN [PrsnName] nvarchar(20) NULL;
ALTER TABLE [PrsnBasic] ADD DEFAULT (('')) FOR [PrsnName];

GO

DECLARE @var17 sysname;
SELECT @var17 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[PrsnBasic]') AND [c].[name] = N'PrsnBirthday');
IF @var17 IS NOT NULL EXEC(N'ALTER TABLE [PrsnBasic] DROP CONSTRAINT [' + @var17 + '];');
ALTER TABLE [PrsnBasic] ALTER COLUMN [PrsnBirthday] nvarchar(8) NULL;
ALTER TABLE [PrsnBasic] ADD DEFAULT (('')) FOR [PrsnBirthday];

GO

DECLARE @var18 sysname;
SELECT @var18 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[PrsnBasic]') AND [c].[name] = N'PEmail');
IF @var18 IS NOT NULL EXEC(N'ALTER TABLE [PrsnBasic] DROP CONSTRAINT [' + @var18 + '];');
ALTER TABLE [PrsnBasic] ALTER COLUMN [PEmail] nvarchar(80) NULL;

GO

DECLARE @var19 sysname;
SELECT @var19 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[PrsnBasic]') AND [c].[name] = N'MajorSpecialistNo');
IF @var19 IS NOT NULL EXEC(N'ALTER TABLE [PrsnBasic] DROP CONSTRAINT [' + @var19 + '];');
ALTER TABLE [PrsnBasic] ALTER COLUMN [MajorSpecialistNo] nvarchar(5) NULL;
ALTER TABLE [PrsnBasic] ADD DEFAULT (('')) FOR [MajorSpecialistNo];

GO

DROP INDEX [IX_HospContract_HospID_HospSeqNo_SMKContractType_HospStartDate] ON [HospContract];
DECLARE @var20 sysname;
SELECT @var20 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[HospContract]') AND [c].[name] = N'SMKContractType');
IF @var20 IS NOT NULL EXEC(N'ALTER TABLE [HospContract] DROP CONSTRAINT [' + @var20 + '];');
ALTER TABLE [HospContract] ALTER COLUMN [SMKContractType] nvarchar(2) NULL;
CREATE UNIQUE INDEX [IX_HospContract_HospID_HospSeqNo_SMKContractType_HospStartDate] ON [HospContract] ([HospID], [HospSeqNo], [SMKContractType], [HospStartDate]) WHERE [HospID] IS NOT NULL AND [HospSeqNo] IS NOT NULL AND [SMKContractType] IS NOT NULL AND [HospStartDate] IS NOT NULL;

GO

DECLARE @var21 sysname;
SELECT @var21 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[HospContract]') AND [c].[name] = N'Remark');
IF @var21 IS NOT NULL EXEC(N'ALTER TABLE [HospContract] DROP CONSTRAINT [' + @var21 + '];');
ALTER TABLE [HospContract] ALTER COLUMN [Remark] nvarchar(200) NULL;
ALTER TABLE [HospContract] ADD DEFAULT (('')) FOR [Remark];

GO

DECLARE @var22 sysname;
SELECT @var22 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[HospContract]') AND [c].[name] = N'ModifyDate');
IF @var22 IS NOT NULL EXEC(N'ALTER TABLE [HospContract] DROP CONSTRAINT [' + @var22 + '];');
ALTER TABLE [HospContract] ALTER COLUMN [ModifyDate] nvarchar(8) NULL;
ALTER TABLE [HospContract] ADD DEFAULT (('')) FOR [ModifyDate];

GO

DROP INDEX [IX_HospContract_HospID_HospSeqNo_SMKContractType_HospStartDate] ON [HospContract];
DECLARE @var23 sysname;
SELECT @var23 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[HospContract]') AND [c].[name] = N'HospStartDate');
IF @var23 IS NOT NULL EXEC(N'ALTER TABLE [HospContract] DROP CONSTRAINT [' + @var23 + '];');
ALTER TABLE [HospContract] ALTER COLUMN [HospStartDate] nvarchar(8) NULL;
CREATE UNIQUE INDEX [IX_HospContract_HospID_HospSeqNo_SMKContractType_HospStartDate] ON [HospContract] ([HospID], [HospSeqNo], [SMKContractType], [HospStartDate]) WHERE [HospID] IS NOT NULL AND [HospSeqNo] IS NOT NULL AND [SMKContractType] IS NOT NULL AND [HospStartDate] IS NOT NULL;

GO

DECLARE @var24 sysname;
SELECT @var24 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[HospContract]') AND [c].[name] = N'HospEndDate');
IF @var24 IS NOT NULL EXEC(N'ALTER TABLE [HospContract] DROP CONSTRAINT [' + @var24 + '];');
ALTER TABLE [HospContract] ALTER COLUMN [HospEndDate] nvarchar(8) NULL;
ALTER TABLE [HospContract] ADD DEFAULT (('')) FOR [HospEndDate];

GO

DECLARE @var25 sysname;
SELECT @var25 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[HospContract]') AND [c].[name] = N'EndReasonNo');
IF @var25 IS NOT NULL EXEC(N'ALTER TABLE [HospContract] DROP CONSTRAINT [' + @var25 + '];');
ALTER TABLE [HospContract] ALTER COLUMN [EndReasonNo] nvarchar(2) NULL;
ALTER TABLE [HospContract] ADD DEFAULT (('')) FOR [EndReasonNo];

GO

DECLARE @var26 sysname;
SELECT @var26 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[HospContract]') AND [c].[name] = N'CreateDate');
IF @var26 IS NOT NULL EXEC(N'ALTER TABLE [HospContract] DROP CONSTRAINT [' + @var26 + '];');
ALTER TABLE [HospContract] ALTER COLUMN [CreateDate] nvarchar(8) NULL;
ALTER TABLE [HospContract] ADD DEFAULT (('')) FOR [CreateDate];

GO

DECLARE @var27 sysname;
SELECT @var27 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[HospBscAll]') AND [c].[name] = N'HospType');
IF @var27 IS NOT NULL EXEC(N'ALTER TABLE [HospBscAll] DROP CONSTRAINT [' + @var27 + '];');
ALTER TABLE [HospBscAll] ALTER COLUMN [HospType] nvarchar(5) NULL;
ALTER TABLE [HospBscAll] ADD DEFAULT (('')) FOR [HospType];

GO

DECLARE @var28 sysname;
SELECT @var28 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[HospBscAll]') AND [c].[name] = N'HospTelArea');
IF @var28 IS NOT NULL EXEC(N'ALTER TABLE [HospBscAll] DROP CONSTRAINT [' + @var28 + '];');
ALTER TABLE [HospBscAll] ALTER COLUMN [HospTelArea] nvarchar(5) NULL;
ALTER TABLE [HospBscAll] ADD DEFAULT (('')) FOR [HospTelArea];

GO

DECLARE @var29 sysname;
SELECT @var29 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[HospBscAll]') AND [c].[name] = N'HospTel');
IF @var29 IS NOT NULL EXEC(N'ALTER TABLE [HospBscAll] DROP CONSTRAINT [' + @var29 + '];');
ALTER TABLE [HospBscAll] ALTER COLUMN [HospTel] nvarchar(20) NULL;
ALTER TABLE [HospBscAll] ADD DEFAULT (('')) FOR [HospTel];

GO

DECLARE @var30 sysname;
SELECT @var30 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[HospBscAll]') AND [c].[name] = N'HospKind');
IF @var30 IS NOT NULL EXEC(N'ALTER TABLE [HospBscAll] DROP CONSTRAINT [' + @var30 + '];');
ALTER TABLE [HospBscAll] ALTER COLUMN [HospKind] nvarchar(5) NULL;
ALTER TABLE [HospBscAll] ADD DEFAULT (('')) FOR [HospKind];

GO

DECLARE @var31 sysname;
SELECT @var31 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[HospBscAll]') AND [c].[name] = N'HospEndDate');
IF @var31 IS NOT NULL EXEC(N'ALTER TABLE [HospBscAll] DROP CONSTRAINT [' + @var31 + '];');
ALTER TABLE [HospBscAll] ALTER COLUMN [HospEndDate] nvarchar(8) NULL;
ALTER TABLE [HospBscAll] ADD DEFAULT (('')) FOR [HospEndDate];

GO

DECLARE @var32 sysname;
SELECT @var32 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[HospBscAll]') AND [c].[name] = N'HospAddress');
IF @var32 IS NOT NULL EXEC(N'ALTER TABLE [HospBscAll] DROP CONSTRAINT [' + @var32 + '];');
ALTER TABLE [HospBscAll] ALTER COLUMN [HospAddress] nvarchar(80) NULL;
ALTER TABLE [HospBscAll] ADD DEFAULT (('')) FOR [HospAddress];

GO

DECLARE @var33 sysname;
SELECT @var33 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[HospBscAll]') AND [c].[name] = N'ContType');
IF @var33 IS NOT NULL EXEC(N'ALTER TABLE [HospBscAll] DROP CONSTRAINT [' + @var33 + '];');
ALTER TABLE [HospBscAll] ALTER COLUMN [ContType] nvarchar(5) NULL;
ALTER TABLE [HospBscAll] ADD DEFAULT (('')) FOR [ContType];

GO

DECLARE @var34 sysname;
SELECT @var34 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[HospBscAll]') AND [c].[name] = N'BranchNo');
IF @var34 IS NOT NULL EXEC(N'ALTER TABLE [HospBscAll] DROP CONSTRAINT [' + @var34 + '];');
ALTER TABLE [HospBscAll] ALTER COLUMN [BranchNo] nvarchar(1) NULL;
ALTER TABLE [HospBscAll] ADD DEFAULT (('')) FOR [BranchNo];

GO

DECLARE @var35 sysname;
SELECT @var35 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[HospBasic]') AND [c].[name] = N'ZIP');
IF @var35 IS NOT NULL EXEC(N'ALTER TABLE [HospBasic] DROP CONSTRAINT [' + @var35 + '];');
ALTER TABLE [HospBasic] ALTER COLUMN [ZIP] nvarchar(5) NULL;
ALTER TABLE [HospBasic] ADD DEFAULT (('')) FOR [ZIP];

GO

DECLARE @var36 sysname;
SELECT @var36 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[HospBasic]') AND [c].[name] = N'SubDivisionNo');
IF @var36 IS NOT NULL EXEC(N'ALTER TABLE [HospBasic] DROP CONSTRAINT [' + @var36 + '];');
ALTER TABLE [HospBasic] ALTER COLUMN [SubDivisionNo] nvarchar(10) NULL;
ALTER TABLE [HospBasic] ADD DEFAULT (('')) FOR [SubDivisionNo];

GO

DECLARE @var37 sysname;
SELECT @var37 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[HospBasic]') AND [c].[name] = N'Remark');
IF @var37 IS NOT NULL EXEC(N'ALTER TABLE [HospBasic] DROP CONSTRAINT [' + @var37 + '];');
ALTER TABLE [HospBasic] ALTER COLUMN [Remark] nvarchar(200) NULL;
ALTER TABLE [HospBasic] ADD DEFAULT (('')) FOR [Remark];

GO

DECLARE @var38 sysname;
SELECT @var38 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[HospBasic]') AND [c].[name] = N'PrevHospSeqNo');
IF @var38 IS NOT NULL EXEC(N'ALTER TABLE [HospBasic] DROP CONSTRAINT [' + @var38 + '];');
ALTER TABLE [HospBasic] ALTER COLUMN [PrevHospSeqNo] nvarchar(2) NULL;
ALTER TABLE [HospBasic] ADD DEFAULT (('')) FOR [PrevHospSeqNo];

GO

DECLARE @var39 sysname;
SELECT @var39 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[HospBasic]') AND [c].[name] = N'PrevHospID');
IF @var39 IS NOT NULL EXEC(N'ALTER TABLE [HospBasic] DROP CONSTRAINT [' + @var39 + '];');
ALTER TABLE [HospBasic] ALTER COLUMN [PrevHospID] nvarchar(10) NULL;
ALTER TABLE [HospBasic] ADD DEFAULT (('')) FOR [PrevHospID];

GO

DECLARE @var40 sysname;
SELECT @var40 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[HospBasic]') AND [c].[name] = N'ModifyDate');
IF @var40 IS NOT NULL EXEC(N'ALTER TABLE [HospBasic] DROP CONSTRAINT [' + @var40 + '];');
ALTER TABLE [HospBasic] ALTER COLUMN [ModifyDate] nvarchar(8) NULL;
ALTER TABLE [HospBasic] ADD DEFAULT (('')) FOR [ModifyDate];

GO

DECLARE @var41 sysname;
SELECT @var41 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[HospBasic]') AND [c].[name] = N'LastHospSeqNo');
IF @var41 IS NOT NULL EXEC(N'ALTER TABLE [HospBasic] DROP CONSTRAINT [' + @var41 + '];');
ALTER TABLE [HospBasic] ALTER COLUMN [LastHospSeqNo] nvarchar(2) NULL;
ALTER TABLE [HospBasic] ADD DEFAULT (('')) FOR [LastHospSeqNo];

GO

DECLARE @var42 sysname;
SELECT @var42 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[HospBasic]') AND [c].[name] = N'LastHospID');
IF @var42 IS NOT NULL EXEC(N'ALTER TABLE [HospBasic] DROP CONSTRAINT [' + @var42 + '];');
ALTER TABLE [HospBasic] ALTER COLUMN [LastHospID] nvarchar(10) NULL;
ALTER TABLE [HospBasic] ADD DEFAULT (('')) FOR [LastHospID];

GO

DECLARE @var43 sysname;
SELECT @var43 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[HospBasic]') AND [c].[name] = N'LastContType');
IF @var43 IS NOT NULL EXEC(N'ALTER TABLE [HospBasic] DROP CONSTRAINT [' + @var43 + '];');
ALTER TABLE [HospBasic] ALTER COLUMN [LastContType] nvarchar(1) NULL;
ALTER TABLE [HospBasic] ADD DEFAULT (('')) FOR [LastContType];

GO

DECLARE @var44 sysname;
SELECT @var44 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[HospBasic]') AND [c].[name] = N'HospTel');
IF @var44 IS NOT NULL EXEC(N'ALTER TABLE [HospBasic] DROP CONSTRAINT [' + @var44 + '];');
ALTER TABLE [HospBasic] ALTER COLUMN [HospTel] nvarchar(20) NULL;
ALTER TABLE [HospBasic] ADD DEFAULT (('')) FOR [HospTel];

GO

DECLARE @var45 sysname;
SELECT @var45 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[HospBasic]') AND [c].[name] = N'HospStatus');
IF @var45 IS NOT NULL EXEC(N'ALTER TABLE [HospBasic] DROP CONSTRAINT [' + @var45 + '];');
ALTER TABLE [HospBasic] ALTER COLUMN [HospStatus] nvarchar(1) NULL;
ALTER TABLE [HospBasic] ADD DEFAULT (('')) FOR [HospStatus];

GO

DECLARE @var46 sysname;
SELECT @var46 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[HospBasic]') AND [c].[name] = N'HospOwnName');
IF @var46 IS NOT NULL EXEC(N'ALTER TABLE [HospBasic] DROP CONSTRAINT [' + @var46 + '];');
ALTER TABLE [HospBasic] ALTER COLUMN [HospOwnName] nvarchar(20) NULL;
ALTER TABLE [HospBasic] ADD DEFAULT (('')) FOR [HospOwnName];

GO

DECLARE @var47 sysname;
SELECT @var47 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[HospBasic]') AND [c].[name] = N'HospOwnID');
IF @var47 IS NOT NULL EXEC(N'ALTER TABLE [HospBasic] DROP CONSTRAINT [' + @var47 + '];');
ALTER TABLE [HospBasic] ALTER COLUMN [HospOwnID] nvarchar(10) NULL;
ALTER TABLE [HospBasic] ADD DEFAULT (('')) FOR [HospOwnID];

GO

DECLARE @var48 sysname;
SELECT @var48 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[HospBasic]') AND [c].[name] = N'HospFax');
IF @var48 IS NOT NULL EXEC(N'ALTER TABLE [HospBasic] DROP CONSTRAINT [' + @var48 + '];');
ALTER TABLE [HospBasic] ALTER COLUMN [HospFax] nvarchar(20) NULL;
ALTER TABLE [HospBasic] ADD DEFAULT (('')) FOR [HospFax];

GO

DECLARE @var49 sysname;
SELECT @var49 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[HospBasic]') AND [c].[name] = N'HospEmail');
IF @var49 IS NOT NULL EXEC(N'ALTER TABLE [HospBasic] DROP CONSTRAINT [' + @var49 + '];');
ALTER TABLE [HospBasic] ALTER COLUMN [HospEmail] nvarchar(60) NULL;
ALTER TABLE [HospBasic] ADD DEFAULT (('')) FOR [HospEmail];

GO

DECLARE @var50 sysname;
SELECT @var50 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[HospBasic]') AND [c].[name] = N'HospAddress');
IF @var50 IS NOT NULL EXEC(N'ALTER TABLE [HospBasic] DROP CONSTRAINT [' + @var50 + '];');
ALTER TABLE [HospBasic] ALTER COLUMN [HospAddress] nvarchar(80) NULL;
ALTER TABLE [HospBasic] ADD DEFAULT (('')) FOR [HospAddress];

GO

DECLARE @var51 sysname;
SELECT @var51 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[HospBasic]') AND [c].[name] = N'FirstHospSeqNo');
IF @var51 IS NOT NULL EXEC(N'ALTER TABLE [HospBasic] DROP CONSTRAINT [' + @var51 + '];');
ALTER TABLE [HospBasic] ALTER COLUMN [FirstHospSeqNo] nvarchar(2) NULL;
ALTER TABLE [HospBasic] ADD DEFAULT (('')) FOR [FirstHospSeqNo];

GO

DECLARE @var52 sysname;
SELECT @var52 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[HospBasic]') AND [c].[name] = N'FirstHospID');
IF @var52 IS NOT NULL EXEC(N'ALTER TABLE [HospBasic] DROP CONSTRAINT [' + @var52 + '];');
ALTER TABLE [HospBasic] ALTER COLUMN [FirstHospID] nvarchar(10) NULL;
ALTER TABLE [HospBasic] ADD DEFAULT (('')) FOR [FirstHospID];

GO

DECLARE @var53 sysname;
SELECT @var53 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[HospBasic]') AND [c].[name] = N'DivisionNo');
IF @var53 IS NOT NULL EXEC(N'ALTER TABLE [HospBasic] DROP CONSTRAINT [' + @var53 + '];');
ALTER TABLE [HospBasic] ALTER COLUMN [DivisionNo] nvarchar(2) NULL;
ALTER TABLE [HospBasic] ADD DEFAULT (('')) FOR [DivisionNo];

GO

DECLARE @var54 sysname;
SELECT @var54 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[HospBasic]') AND [c].[name] = N'CreateDate');
IF @var54 IS NOT NULL EXEC(N'ALTER TABLE [HospBasic] DROP CONSTRAINT [' + @var54 + '];');
ALTER TABLE [HospBasic] ALTER COLUMN [CreateDate] nvarchar(8) NULL;
ALTER TABLE [HospBasic] ADD DEFAULT (('')) FOR [CreateDate];

GO

DECLARE @var55 sysname;
SELECT @var55 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[HospBasic]') AND [c].[name] = N'ContactTel2');
IF @var55 IS NOT NULL EXEC(N'ALTER TABLE [HospBasic] DROP CONSTRAINT [' + @var55 + '];');
ALTER TABLE [HospBasic] ALTER COLUMN [ContactTel2] nvarchar(20) NULL;
ALTER TABLE [HospBasic] ADD DEFAULT (('')) FOR [ContactTel2];

GO

DECLARE @var56 sysname;
SELECT @var56 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[HospBasic]') AND [c].[name] = N'ContactTel1');
IF @var56 IS NOT NULL EXEC(N'ALTER TABLE [HospBasic] DROP CONSTRAINT [' + @var56 + '];');
ALTER TABLE [HospBasic] ALTER COLUMN [ContactTel1] nvarchar(20) NULL;
ALTER TABLE [HospBasic] ADD DEFAULT (('')) FOR [ContactTel1];

GO

DECLARE @var57 sysname;
SELECT @var57 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[HospBasic]') AND [c].[name] = N'ContactFax2');
IF @var57 IS NOT NULL EXEC(N'ALTER TABLE [HospBasic] DROP CONSTRAINT [' + @var57 + '];');
ALTER TABLE [HospBasic] ALTER COLUMN [ContactFax2] nvarchar(20) NULL;
ALTER TABLE [HospBasic] ADD DEFAULT (('')) FOR [ContactFax2];

GO

DECLARE @var58 sysname;
SELECT @var58 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[HospBasic]') AND [c].[name] = N'ContactFax1');
IF @var58 IS NOT NULL EXEC(N'ALTER TABLE [HospBasic] DROP CONSTRAINT [' + @var58 + '];');
ALTER TABLE [HospBasic] ALTER COLUMN [ContactFax1] nvarchar(20) NULL;
ALTER TABLE [HospBasic] ADD DEFAULT (('')) FOR [ContactFax1];

GO

DECLARE @var59 sysname;
SELECT @var59 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[HospBasic]') AND [c].[name] = N'ContactEmail2');
IF @var59 IS NOT NULL EXEC(N'ALTER TABLE [HospBasic] DROP CONSTRAINT [' + @var59 + '];');
ALTER TABLE [HospBasic] ALTER COLUMN [ContactEmail2] nvarchar(60) NULL;
ALTER TABLE [HospBasic] ADD DEFAULT (('')) FOR [ContactEmail2];

GO

DECLARE @var60 sysname;
SELECT @var60 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[HospBasic]') AND [c].[name] = N'ContactEmail1');
IF @var60 IS NOT NULL EXEC(N'ALTER TABLE [HospBasic] DROP CONSTRAINT [' + @var60 + '];');
ALTER TABLE [HospBasic] ALTER COLUMN [ContactEmail1] nvarchar(60) NULL;
ALTER TABLE [HospBasic] ADD DEFAULT (('')) FOR [ContactEmail1];

GO

DECLARE @var61 sysname;
SELECT @var61 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[HospBasic]') AND [c].[name] = N'Contact2');
IF @var61 IS NOT NULL EXEC(N'ALTER TABLE [HospBasic] DROP CONSTRAINT [' + @var61 + '];');
ALTER TABLE [HospBasic] ALTER COLUMN [Contact2] nvarchar(20) NULL;
ALTER TABLE [HospBasic] ADD DEFAULT (('')) FOR [Contact2];

GO

DECLARE @var62 sysname;
SELECT @var62 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[HospBasic]') AND [c].[name] = N'Contact1');
IF @var62 IS NOT NULL EXEC(N'ALTER TABLE [HospBasic] DROP CONSTRAINT [' + @var62 + '];');
ALTER TABLE [HospBasic] ALTER COLUMN [Contact1] nvarchar(20) NULL;
ALTER TABLE [HospBasic] ADD DEFAULT (('')) FOR [Contact1];

GO

DECLARE @var63 sysname;
SELECT @var63 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[HospBasic]') AND [c].[name] = N'chFlg3');
IF @var63 IS NOT NULL EXEC(N'ALTER TABLE [HospBasic] DROP CONSTRAINT [' + @var63 + '];');
ALTER TABLE [HospBasic] ALTER COLUMN [chFlg3] varchar(1) NULL;
ALTER TABLE [HospBasic] ADD DEFAULT (('')) FOR [chFlg3];

GO

DECLARE @var64 sysname;
SELECT @var64 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[HospBasic]') AND [c].[name] = N'chFlg2');
IF @var64 IS NOT NULL EXEC(N'ALTER TABLE [HospBasic] DROP CONSTRAINT [' + @var64 + '];');
ALTER TABLE [HospBasic] ALTER COLUMN [chFlg2] varchar(1) NULL;
ALTER TABLE [HospBasic] ADD DEFAULT (('')) FOR [chFlg2];

GO

DECLARE @var65 sysname;
SELECT @var65 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[HospBasic]') AND [c].[name] = N'chFlg1');
IF @var65 IS NOT NULL EXEC(N'ALTER TABLE [HospBasic] DROP CONSTRAINT [' + @var65 + '];');
ALTER TABLE [HospBasic] ALTER COLUMN [chFlg1] varchar(1) NULL;
ALTER TABLE [HospBasic] ADD DEFAULT (('')) FOR [chFlg1];

GO

DECLARE @var66 sysname;
SELECT @var66 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[HospBasic]') AND [c].[name] = N'BranchNo');
IF @var66 IS NOT NULL EXEC(N'ALTER TABLE [HospBasic] DROP CONSTRAINT [' + @var66 + '];');
ALTER TABLE [HospBasic] ALTER COLUMN [BranchNo] varchar(1) NULL;
ALTER TABLE [HospBasic] ADD DEFAULT (('')) FOR [BranchNo];

GO

DECLARE @var67 sysname;
SELECT @var67 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[GenSpecial]') AND [c].[name] = N'SpecialistName');
IF @var67 IS NOT NULL EXEC(N'ALTER TABLE [GenSpecial] DROP CONSTRAINT [' + @var67 + '];');
ALTER TABLE [GenSpecial] ALTER COLUMN [SpecialistName] nvarchar(20) NULL;
ALTER TABLE [GenSpecial] ADD DEFAULT (('')) FOR [SpecialistName];

GO

DECLARE @var68 sysname;
SELECT @var68 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[GenSMKContract]') AND [c].[name] = N'SMKContractName');
IF @var68 IS NOT NULL EXEC(N'ALTER TABLE [GenSMKContract] DROP CONSTRAINT [' + @var68 + '];');
ALTER TABLE [GenSMKContract] ALTER COLUMN [SMKContractName] nvarchar(20) NULL;
ALTER TABLE [GenSMKContract] ADD DEFAULT (('')) FOR [SMKContractName];

GO

DECLARE @var69 sysname;
SELECT @var69 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[GenPrsnType]') AND [c].[name] = N'PrsnTypeName');
IF @var69 IS NOT NULL EXEC(N'ALTER TABLE [GenPrsnType] DROP CONSTRAINT [' + @var69 + '];');
ALTER TABLE [GenPrsnType] ALTER COLUMN [PrsnTypeName] nvarchar(20) NULL;
ALTER TABLE [GenPrsnType] ADD DEFAULT (('')) FOR [PrsnTypeName];

GO

DECLARE @var70 sysname;
SELECT @var70 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[GenLicenceType]') AND [c].[name] = N'LicenceName');
IF @var70 IS NOT NULL EXEC(N'ALTER TABLE [GenLicenceType] DROP CONSTRAINT [' + @var70 + '];');
ALTER TABLE [GenLicenceType] ALTER COLUMN [LicenceName] nvarchar(20) NULL;
ALTER TABLE [GenLicenceType] ADD DEFAULT (('')) FOR [LicenceName];

GO

DECLARE @var71 sysname;
SELECT @var71 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[GenHospCont]') AND [c].[name] = N'HospContName');
IF @var71 IS NOT NULL EXEC(N'ALTER TABLE [GenHospCont] DROP CONSTRAINT [' + @var71 + '];');
ALTER TABLE [GenHospCont] ALTER COLUMN [HospContName] nvarchar(20) NULL;
ALTER TABLE [GenHospCont] ADD DEFAULT (('')) FOR [HospContName];

GO

DECLARE @var72 sysname;
SELECT @var72 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[GenEndReason]') AND [c].[name] = N'EndReasonName');
IF @var72 IS NOT NULL EXEC(N'ALTER TABLE [GenEndReason] DROP CONSTRAINT [' + @var72 + '];');
ALTER TABLE [GenEndReason] ALTER COLUMN [EndReasonName] nvarchar(20) NULL;
ALTER TABLE [GenEndReason] ADD DEFAULT (('')) FOR [EndReasonName];

GO

DECLARE @var73 sysname;
SELECT @var73 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[GenBranch]') AND [c].[name] = N'BranchName');
IF @var73 IS NOT NULL EXEC(N'ALTER TABLE [GenBranch] DROP CONSTRAINT [' + @var73 + '];');
ALTER TABLE [GenBranch] ALTER COLUMN [BranchName] nvarchar(20) NULL;
ALTER TABLE [GenBranch] ADD DEFAULT (('')) FOR [BranchName];

GO

UPDATE [GenEmpData] SET [CreatedAt] = '2020-05-30T01:41:40.5012920+08:00'
WHERE [Id] = N'00000000-0000-0000-0000-000000000000';
SELECT @@ROWCOUNT;


GO

UPDATE [Role] SET [CreatedAt] = '2020-05-30T01:41:40.5032247+08:00'
WHERE [Id] = N'00000000-0000-0000-0000-000000000000';
SELECT @@ROWCOUNT;


GO

UPDATE [RoleEmpMapping] SET [CreatedAt] = '2020-05-30T01:41:40.5033837+08:00'
WHERE [Id] = N'00000000-0000-0000-0000-000000000000';
SELECT @@ROWCOUNT;


GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200529174140_fixIsUnicode_IsFixedLength', N'3.1.3');

GO

