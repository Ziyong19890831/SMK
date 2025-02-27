CREATE TABLE [AuditLog] (
    [Id] nvarchar(36) NOT NULL,
    [Account] nvarchar(128) NULL,
    [SourceTable] nvarchar(64) NULL,
    [ActionType] nvarchar(max) NULL,
    [RecordId] nvarchar(36) NULL,
    [OriginalRecord] text NULL,
    [CurrentRecord] text NULL,
    [InvolvedColumns] nvarchar(1024) NULL,
    [ActionRemark] nvarchar(1024) NULL,
    [Description] nvarchar(511) NULL,
    CONSTRAINT [PK_AuditLog] PRIMARY KEY ([Id])
);

GO

CREATE INDEX [IX_AuditLog_Account] ON [AuditLog] ([Account]);

GO

CREATE INDEX [IX_AuditLog_RecordId] ON [AuditLog] ([RecordId]);

GO

CREATE INDEX [IX_AuditLog_SourceTable] ON [AuditLog] ([SourceTable]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200506154638_AuditLog', N'3.1.3');

GO

