BEGIN TRANSACTION;
GO

CREATE TABLE [IniMonthDetail] (
    [Id] int NOT NULL IDENTITY,
    [ContractYM] nvarchar(6) NULL,
    [ContractTotal] int NOT NULL,
    [ContractAllTotal] int NOT NULL,
    [RunTimeContractAllTotal] int NOT NULL,
    [ContractPersonTotal] int NOT NULL,
    [ContractPersonAllTotal] int NOT NULL,
    [RunTimeContractPersonAllTotal] int NOT NULL,
    [NhiYM] nvarchar(6) NULL,
    [TreatInstructCnt] int NOT NULL,
    [TreatCnt] int NOT NULL,
    [InstructCnt] int NOT NULL,
    [TreatInstructSum] int NOT NULL,
    [TreatSum] int NOT NULL,
    [InstructSum] int NOT NULL,
    [TreatAddInstruct] int NOT NULL,
    [TreatWeek] int NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_IniMonthDetail] PRIMARY KEY ([Id])
);
GO

CREATE UNIQUE INDEX [IX_IniMonthDetail_ContractYM] ON [IniMonthDetail] ([ContractYM]) WHERE [ContractYM] IS NOT NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20220115173832_IniMonthDetail', N'5.0.7');
GO

COMMIT;
GO

