BEGIN TRANSACTION;
GO

CREATE TABLE [CorrectionSlip] (
    [CaseNo] nvarchar(9) NOT NULL,
    [ReceiveDate] datetime2 NOT NULL,
    [HospId] nvarchar(max) NULL,
    [HospName] nvarchar(max) NULL,
    [Name] nvarchar(max) NULL,
    [ID] nvarchar(10) NULL,
    [Birthday] datetime2 NOT NULL,
    [CorrectBasic] nvarchar(max) NULL,
    [CorrectHosp] nvarchar(max) NULL,
    [CorrectHealth] nvarchar(max) NULL,
    [CorrectOther] nvarchar(max) NULL,
    [CorrectItems] nvarchar(max) NULL,
    [CorrectItems2] nvarchar(max) NULL,
    [source] nvarchar(max) NULL,
    [Memo] nvarchar(max) NULL,
    [UpdateAt] datetime2 NOT NULL,
    [UpdatedBy] nvarchar(127) NULL,
    CONSTRAINT [PK_CorrectionSlip] PRIMARY KEY ([CaseNo])
);
GO

CREATE INDEX [IX_CorrectionSlip_CaseNo] ON [CorrectionSlip] ([CaseNo]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20220407020432_AddCorrectionSlip', N'5.0.7');
GO

COMMIT;
GO

