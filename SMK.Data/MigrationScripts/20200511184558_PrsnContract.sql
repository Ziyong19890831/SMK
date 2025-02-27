CREATE TABLE [PrsnContract] (
    [HospID] char(10) NOT NULL,
    [PrsnID] char(10) NOT NULL,
    [SMKContractType] char(10) NOT NULL,
    [PrsnStartDate] char(8) NOT NULL DEFAULT (('')),
    [HospSeqNo] char(2) NOT NULL,
    [PrsnEndDate] char(8) NULL DEFAULT (('')),
    [CreateDate] char(8) NULL DEFAULT (('')),
    [ModifyDate] char(8) NULL DEFAULT (('')),
    [ModifyPersonNo] int NULL,
    [Remark] varchar(200) NULL DEFAULT (('')),
    [CouldTreat] char(1) NULL DEFAULT (('')),
    [CouldInstruct] char(1) NULL DEFAULT (('')),
    CONSTRAINT [PK_PrsnContract] PRIMARY KEY ([HospID], [HospSeqNo], [PrsnID], [SMKContractType], [PrsnStartDate])
);

GO

UPDATE [GenEmpData] SET [CreatedAt] = '2020-05-12T02:45:57.6812942+08:00'
WHERE [Id] = N'00000000-0000-0000-0000-000000000000';
SELECT @@ROWCOUNT;


GO

UPDATE [Role] SET [CreatedAt] = '2020-05-12T02:45:57.6831565+08:00'
WHERE [Id] = N'00000000-0000-0000-0000-000000000000';
SELECT @@ROWCOUNT;


GO

UPDATE [RoleEmpMapping] SET [CreatedAt] = '2020-05-12T02:45:57.6833137+08:00'
WHERE [Id] = N'00000000-0000-0000-0000-000000000000';
SELECT @@ROWCOUNT;


GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200511184558_PrsnContract', N'3.1.3');

GO

