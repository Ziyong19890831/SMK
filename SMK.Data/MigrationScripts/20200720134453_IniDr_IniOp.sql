CREATE TABLE [iniDrDtl] (
    [data_id] varchar(28) NOT NULL,
    [fee_ym] char(6) NOT NULL,
    [HospID] char(10) NULL DEFAULT (('')),
    [ExamYear] char(4) NULL DEFAULT (('')),
    [ExamTime] int NULL,
    [FirstTreatDate] char(8) NULL DEFAULT (('')),
    [WeekCount] int NULL,
    [InstructExamYear] char(4) NULL DEFAULT (('')),
    [InstructExamTime] int NULL,
    [FirstInstructDate] char(8) NULL DEFAULT (('')),
    [InctructSerial] int NULL,
    [MedApply] char(1) NULL DEFAULT (('')),
    [InstructApply] char(1) NULL DEFAULT (('')),
    [TraceApply] char(1) NULL DEFAULT (('')),
    [ReleaseApply] char(1) NULL DEFAULT (('')),
    [appl_type] char(1) NULL DEFAULT (('')),
    [appl_date] char(8) NULL DEFAULT (('')),
    [case_type] char(1) NULL DEFAULT (('')),
    [seq_no] int NULL,
    [func_type] char(2) NULL DEFAULT (('')),
    [func_date] char(8) NULL DEFAULT (('')),
    [rel_date] char(8) NULL DEFAULT (('')),
    [birthday] char(8) NULL DEFAULT (('')),
    [id] char(10) NULL DEFAULT (('')),
    [func_seq_no] char(10) NULL DEFAULT (('')),
    [pay_type] char(1) NULL DEFAULT (('')),
    [part_code] char(3) NULL DEFAULT (('')),
    [icd9cm_code] varchar(9) NULL DEFAULT (('')),
    [icd9cm_code1] varchar(9) NULL DEFAULT (('')),
    [icd9cm_code2] varchar(9) NULL DEFAULT (('')),
    [drug_days] int NULL,
    [prsn_id] char(10) NULL DEFAULT (('')),
    [drug_prsn_id] char(10) NULL DEFAULT (('')),
    [drug_dot] int NULL,
    [cure_dot] int NULL,
    [dsvc_code] char(12) NULL DEFAULT (('')),
    [dsvc_dot] int NULL,
    [exp_dot] int NULL,
    [part_amt] int NULL,
    [appl_dot] int NULL,
    [orig_hosp_id] char(10) NULL DEFAULT (('')),
    [Id_Sex] char(1) NULL DEFAULT (('')),
    [cure_item1] char(2) NULL DEFAULT (('')),
    [cure_item2] char(2) NULL DEFAULT (('')),
    [cure_item3] char(2) NULL DEFAULT (('')),
    [cure_item4] char(2) NULL DEFAULT (('')),
    [orig_case_type] char(2) NULL DEFAULT (('')),
    [other_part_amt] int NULL,
    [appl_cause_mark] char(1) NULL DEFAULT (('')),
    [icd10cm_code2] varchar(9) NULL DEFAULT (('')),
    [icd10cm_code3] varchar(9) NULL DEFAULT (('')),
    [icd10cm_code4] varchar(9) NULL DEFAULT (('')),
    [corr_hosp_id] char(10) NULL DEFAULT (('')),
    [area_service] char(2) NULL DEFAULT (('')),
    [tran_date] char(8) NULL,
    [name] nchar(20) NULL,
    CONSTRAINT [PK_iniDrDtl] PRIMARY KEY ([data_id], [fee_ym])
);

GO

CREATE TABLE [iniDrOrd] (
    [data_id] varchar(28) NOT NULL,
    [order_seq_no] int NOT NULL,
    [fee_ym] char(6) NOT NULL,
    [order_type] char(1) NULL DEFAULT (('')),
    [order_code] char(12) NULL DEFAULT (('')),
    [drug_num] char(6) NULL DEFAULT (('')),
    [drug_fre] char(18) NULL DEFAULT (('')),
    [drug_path] char(15) NULL DEFAULT (('')),
    [order_uprice] decimal(9, 2) NULL,
    [order_qty] decimal(7, 2) NULL,
    [order_dot] int NULL,
    [order_drug_day] int NULL,
    [exe_prsn_id] char(10) NULL DEFAULT (('')),
    [tran_date] varchar(8) NULL,
    CONSTRAINT [PK_iniDrOrd_1] PRIMARY KEY ([data_id], [order_seq_no], [fee_ym])
);

GO

CREATE TABLE [iniOpDtl] (
    [data_id] varchar(28) NOT NULL,
    [fee_ym] char(6) NOT NULL,
    [ExamYear] char(4) NULL DEFAULT (('')),
    [ExamTime] int NULL,
    [FirstTreatDate] char(8) NULL DEFAULT (('')),
    [WeekCount] int NULL,
    [InstructExamYear] char(4) NULL DEFAULT (('')),
    [InstructExamTime] int NULL,
    [FirstInstructDate] char(8) NULL DEFAULT (('')),
    [InctructSerial] int NULL,
    [MedApply] char(1) NULL DEFAULT (('')),
    [InstructApply] char(1) NULL DEFAULT (('')),
    [TraceApply] char(1) NULL DEFAULT (('')),
    [ReleaseApply] char(1) NULL DEFAULT (('')),
    [appl_type] char(1) NULL DEFAULT (('')),
    [HospID] char(10) NULL DEFAULT (('')),
    [appl_date] char(8) NULL DEFAULT (('')),
    [case_type] char(2) NULL DEFAULT (('')),
    [seq_no] int NULL,
    [func_type] char(2) NULL DEFAULT (('')),
    [func_date] char(8) NULL DEFAULT (('')),
    [cure_e_date] char(8) NULL DEFAULT (('')),
    [birthday] char(8) NULL DEFAULT (('')),
    [id] char(10) NULL DEFAULT (('')),
    [func_seq_no] char(10) NULL DEFAULT (('')),
    [pay_type] char(1) NULL DEFAULT (('')),
    [part_code] char(3) NULL DEFAULT (('')),
    [icd9cm_code] varchar(9) NULL DEFAULT (('')),
    [icd9cm_code1] varchar(9) NULL DEFAULT (('')),
    [icd9cm_code2] varchar(9) NULL DEFAULT (('')),
    [drug_days] int NULL,
    [rel_mode] char(1) NULL DEFAULT (('')),
    [prsn_id] char(10) NULL DEFAULT (('')),
    [drug_prsn_id] char(10) NULL DEFAULT (('')),
    [drug_dot] int NULL,
    [cure_dot] int NULL,
    [diag_code] char(12) NULL DEFAULT (('')),
    [diag_dot] int NULL,
    [dsvc_code] char(12) NULL DEFAULT (('')),
    [dsvc_dot] int NULL,
    [exp_dot] int NULL,
    [part_amt] int NULL,
    [appl_dot] int NULL,
    [Id_Sex] char(1) NULL DEFAULT (('')),
    [cure_item1] char(2) NULL DEFAULT (('')),
    [cure_item2] char(2) NULL DEFAULT (('')),
    [cure_item3] char(2) NULL DEFAULT (('')),
    [cure_item4] char(2) NULL DEFAULT (('')),
    [area_service] char(2) NULL DEFAULT (('')),
    [supp_area] char(4) NULL DEFAULT (('')),
    [real_hosp_id] char(10) NULL DEFAULT (('')),
    [hosp_data_type] char(2) NULL DEFAULT (('')),
    [agency_part_amt] decimal(9, 2) NULL,
    [name] nchar(20) NULL DEFAULT (('')),
    [appl_cause_mark] char(1) NULL DEFAULT (('')),
    [icd10cm_code3] varchar(9) NULL DEFAULT (('')),
    [icd10cm_code4] varchar(9) NULL DEFAULT (('')),
    [met_dot] int NULL,
    [corr_hosp_id] char(10) NULL DEFAULT (('')),
    [tran_date] char(8) NULL,
    CONSTRAINT [PK_iniOpDtl] PRIMARY KEY ([data_id], [fee_ym])
);

GO

CREATE TABLE [iniOpOrd] (
    [data_id] varchar(28) NOT NULL,
    [order_seq_no] int NOT NULL,
    [fee_ym] char(6) NOT NULL,
    [order_type] char(1) NULL DEFAULT (('')),
    [order_code] char(12) NULL DEFAULT (('')),
    [rel_mode] char(1) NULL DEFAULT (('')),
    [chr_mark] char(1) NULL DEFAULT (('')),
    [drug_num] char(6) NULL DEFAULT (('')),
    [drug_fre] char(18) NULL DEFAULT (('')),
    [drug_path] char(15) NULL DEFAULT (('')),
    [order_uprice] decimal(9, 2) NULL,
    [order_qty] decimal(7, 2) NULL,
    [order_dot] int NULL,
    [exe_prsn_id] char(10) NULL DEFAULT (('')),
    [cure_path] char(20) NULL DEFAULT (('')),
    [order_drug_day] int NULL,
    [tran_date] varchar(8) NULL,
    CONSTRAINT [PK_iniOpOrd] PRIMARY KEY ([data_id], [order_seq_no], [fee_ym])
);

GO

UPDATE [GenEmpData] SET [CreatedAt] = '2020-07-20T21:44:53.4044864+08:00'
WHERE [Id] = N'00000000-0000-0000-0000-000000000000';
SELECT @@ROWCOUNT;


GO

UPDATE [Role] SET [CreatedAt] = '2020-07-20T21:44:53.4064176+08:00'
WHERE [Id] = N'00000000-0000-0000-0000-000000000000';
SELECT @@ROWCOUNT;


GO

UPDATE [RoleEmpMapping] SET [CreatedAt] = '2020-07-20T21:44:53.4065750+08:00'
WHERE [Id] = N'00000000-0000-0000-0000-000000000000';
SELECT @@ROWCOUNT;


GO

CREATE INDEX [INX_DrDtl] ON [iniDrDtl] ([id], [fee_ym], [HospID], [func_date], [birthday], [MedApply], [WeekCount], [drug_days], [orig_hosp_id], [tran_date]);

GO

CREATE INDEX [INX_DrOrd] ON [iniDrOrd] ([fee_ym], [order_code]);

GO

CREATE INDEX [INX_OpDtl] ON [iniOpDtl] ([id], [fee_ym], [HospID], [func_date], [birthday], [MedApply], [WeekCount], [drug_days], [tran_date]);

GO

CREATE INDEX [INX_OpOrd] ON [iniOpOrd] ([fee_ym], [order_code]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200720134453_IniDr_IniOp', N'3.1.3');

GO

