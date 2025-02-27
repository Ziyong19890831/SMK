CREATE TABLE [DtlWithSet] (
    [Data_ID] char(28) NOT NULL,
    [Fee_YM] char(6) NOT NULL,
    [ID] char(10) NULL,
    [Func_Date] char(8) NULL,
    [LastHospID] char(10) NULL,
    [LastHospSeqNo] char(2) NULL,
    [HospID] char(10) NULL,
    [HospSeqNo] char(2) NULL,
    [ID_Sex] char(1) NULL,
    [Birthday] char(8) NULL,
    [Prsn_ID] char(10) NULL,
    [ExePrsnID] char(10) NULL,
    [HospContType] char(1) NULL,
    [MedQlty] char(1) NOT NULL,
    [InsQlty] char(1) NOT NULL,
    [Part_Code] char(3) NULL,
    [Func_Type] char(2) NULL,
    [Rel_Mode] char(1) NULL,
    [Drug_Days] int NULL,
    [MedApply] char(1) NULL,
    [InstructApply] char(1) NULL,
    [TraceApply] char(1) NULL,
    [ReleaseApply] char(1) NULL,
    [Referral] char(1) NULL,
    [LowIncome] char(1) NOT NULL,
    [Correction] char(1) NOT NULL,
    [Drug] varchar(20) NULL,
    [Exp_Dot] int NULL,
    [Part_Amt] int NULL,
    [Appl_Dot] int NULL,
    [ServiceFee] int NULL,
    [DrugFee] int NULL,
    [DispensingFee] int NULL,
    [InstructionFee] int NULL,
    [TracingFee] int NULL,
    [ReferralFee] int NULL,
    [ExamYear] char(4) NULL,
    [ExamTime] int NULL,
    [FirstTreatDate] char(8) NULL,
    [WeekCount] int NULL,
    [InstructExamYear] char(4) NULL,
    [InstructExamTime] int NULL,
    [FirstInstructDate] char(8) NULL,
    [InctructSerial] int NULL,
    [Orig_Hosp_ID] char(10) NULL,
    [Patch_N] char(1) NOT NULL,
    [Patch_S] char(1) NOT NULL,
    [Gum_Lozenge] char(1) NOT NULL,
    [Inhaler] char(1) NOT NULL,
    [Bupropion] char(1) NOT NULL,
    [Varenicline] char(1) NOT NULL,
    [PrsnType] varchar(20) NULL,
    [Visits] int NULL,
    [DrTreat] char(1) NOT NULL,
    [DentistTreat] char(1) NOT NULL,
    [PharTreat] char(1) NOT NULL,
    [EduTreat] char(1) NOT NULL,
    [Remark] varchar(50) NOT NULL,
    [UnCount] varchar(10) NOT NULL,
    [Func_Month] char(6) NOT NULL
);

GO

CREATE TABLE [MhbtAgentPatient] (
    [HospID] varchar(10) NOT NULL,
    [HospAgentCode] varchar(5) NOT NULL,
    [ID] varchar(10) NOT NULL,
    [BIRTHDAY] date NOT NULL DEFAULT ((('') collate Chinese_Taiwan_Stroke_CI_AS)),
    [Name] nvarchar(12) NOT NULL DEFAULT ((('') collate Chinese_Taiwan_Stroke_CI_AS)),
    [Sex] varchar(1) NOT NULL DEFAULT ((('') collate Chinese_Taiwan_Stroke_CI_AS)),
    [Inform_ADDR] nvarchar(120) NULL DEFAULT ((('') collate Chinese_Taiwan_Stroke_CI_AS)),
    [Tel_D_AC] varchar(4) NULL DEFAULT ((('') collate Chinese_Taiwan_Stroke_CI_AS)),
    [TEL_D] varchar(10) NULL,
    [TEL_D_ENTX] varchar(5) NULL,
    [Tel_N_AC] varchar(4) NULL DEFAULT ((('') collate Chinese_Taiwan_Stroke_CI_AS)),
    [TEL_N] varchar(10) NULL,
    [TEL_N_ENTX] varchar(5) NULL,
    [Tel_M] varchar(15) NULL DEFAULT ((('') collate Chinese_Taiwan_Stroke_CI_AS)),
    [Seq_No] varchar(20) NULL DEFAULT ((('') collate Chinese_Taiwan_Stroke_CI_AS)),
    [Branch_Code] varchar(1) NOT NULL DEFAULT ((('') collate Chinese_Taiwan_Stroke_CI_AS)),
    [TXT_Date] datetime NOT NULL DEFAULT ((('') collate Chinese_Taiwan_Stroke_CI_AS)),
    [Func_Mark] varchar(1) NULL DEFAULT ((('') collate Chinese_Taiwan_Stroke_CI_AS)),
    [Town_Code] varchar(4) NULL DEFAULT ((('') collate Chinese_Taiwan_Stroke_CI_AS)),
    [Town_Name] nvarchar(10) NULL DEFAULT ((('') collate Chinese_Taiwan_Stroke_CI_AS)),
    CONSTRAINT [PK_MhbtAgentPatient_1] PRIMARY KEY ([HospID], [HospAgentCode], [ID], [BIRTHDAY])
);

GO

CREATE TABLE [MhbtQsCure] (
    [Hosp_ID] varchar(10) NOT NULL,
    [ID] varchar(10) NOT NULL,
    [Birthday] date NOT NULL,
    [Func_Date] date NOT NULL,
    [Cure_Item] varchar(10) NOT NULL,
    [Hosp_Seq_No] varchar(2) NOT NULL,
    [Cure_Num] numeric(5, 1) NOT NULL,
    [Txt_Date] datetime NOT NULL DEFAULT ((('') collate Chinese_Taiwan_Stroke_CI_AS)),
    [Adjust_User_ID] varchar(30) NOT NULL DEFAULT ((('') collate Chinese_Taiwan_Stroke_CI_AS)),
    CONSTRAINT [PK_MhbtQsCure] PRIMARY KEY ([Hosp_ID], [ID], [Birthday], [Func_Date], [Cure_Item], [Hosp_Seq_No])
);

GO

CREATE TABLE [MhbtQsData] (
    [Hosp_ID] varchar(10) NOT NULL,
    [ID] varchar(10) NOT NULL,
    [Birthday] date NOT NULL,
    [FuncDate] date NOT NULL,
    [Cure_Type] varchar(1) NOT NULL,
    [HospSeqNo] varchar(2) NOT NULL,
    [PRSN_ID] varchar(10) NULL DEFAULT ((('') collate Chinese_Taiwan_Stroke_CI_AS)),
    [Cure_Stage] varchar(1) NULL,
    [ExamYear] varchar(3) NULL,
    [Smoke_Year] numeric(3, 0) NULL,
    [Smoke_Mon] numeric(2, 0) NULL,
    [Smoke_Day_Num] numeric(4, 0) NULL,
    [Base_Weight] numeric(4, 1) NULL,
    [Cure_Week] numeric(1, 0) NULL,
    [Week_Tot] numeric(2, 0) NULL,
    [Smoke_First] varchar(1) NULL DEFAULT ((('') collate Chinese_Taiwan_Stroke_CI_AS)),
    [Smoke_Stop] varchar(1) NULL DEFAULT ((('') collate Chinese_Taiwan_Stroke_CI_AS)),
    [Smoke_No_Gp] varchar(1) NULL DEFAULT ((('') collate Chinese_Taiwan_Stroke_CI_AS)),
    [Smoke_Much] varchar(1) NULL DEFAULT ((('') collate Chinese_Taiwan_Stroke_CI_AS)),
    [Smoke_Bed] varchar(1) NULL DEFAULT ((('') collate Chinese_Taiwan_Stroke_CI_AS)),
    [Smoke_Sick] varchar(1) NULL DEFAULT ((('') collate Chinese_Taiwan_Stroke_CI_AS)),
    [Smoke_Score] numeric(2, 0) NULL DEFAULT ((('') collate Chinese_Taiwan_Stroke_CI_AS)),
    [Cure_Agree] varchar(1) NULL DEFAULT ((('') collate Chinese_Taiwan_Stroke_CI_AS)),
    [Branch_Code] varchar(1) NOT NULL DEFAULT ((('') collate Chinese_Taiwan_Stroke_CI_AS)),
    [TXT_DATE] datetime NOT NULL DEFAULT ((('') collate Chinese_Taiwan_Stroke_CI_AS)),
    [ADJUST_USER_ID] varchar(30) NOT NULL DEFAULT ((('') collate Chinese_Taiwan_Stroke_CI_AS)),
    [FEE_MARK] varchar(1) NULL DEFAULT ((('') collate Chinese_Taiwan_Stroke_CI_AS)),
    [CO_CHECK] numeric(2, 0) NULL DEFAULT ((('') collate Chinese_Taiwan_Stroke_CI_AS)),
    [TRACE_DATE] date NULL DEFAULT ((('') collate Chinese_Taiwan_Stroke_CI_AS)),
    [TRACE_STATE] varchar(3) NULL,
    [CURE_STATE] varchar(2) NULL,
    [TRACE_CO_CHECK] numeric(2, 0) NULL DEFAULT ((('') collate Chinese_Taiwan_Stroke_CI_AS)),
    [Trace_Date2] date NULL DEFAULT ((('') collate Chinese_Taiwan_Stroke_CI_AS)),
    [Trace_State2] varchar(3) NULL DEFAULT ((('') collate Chinese_Taiwan_Stroke_CI_AS)),
    [Cure_State2] varchar(2) NULL DEFAULT ((('') collate Chinese_Taiwan_Stroke_CI_AS)),
    [Trace_Co_Check2] numeric(2, 0) NULL DEFAULT ((('') collate Chinese_Taiwan_Stroke_CI_AS)),
    [Case_Source] varchar(1) NULL DEFAULT ((('') collate Chinese_Taiwan_Stroke_CI_AS)),
    [Case_Kind] varchar(1) NULL DEFAULT ((('') collate Chinese_Taiwan_Stroke_CI_AS)),
    CONSTRAINT [PK_MhbtQsData] PRIMARY KEY ([Hosp_ID], [ID], [Birthday], [FuncDate], [Cure_Type], [HospSeqNo])
);

GO

CREATE TABLE [MhbtQsState] (
    [Hosp_ID] varchar(10) NOT NULL,
    [ID] varchar(10) NOT NULL,
    [Birthday] date NOT NULL,
    [Func_Date] date NOT NULL,
    [Cure_State] varchar(1) NOT NULL,
    [Cure_Type] varchar(1) NOT NULL,
    [Hosp_Seq_No] varchar(2) NOT NULL,
    [Cure_State_Other] nvarchar(60) NULL DEFAULT ((('') collate Chinese_Taiwan_Stroke_CI_AS)),
    [Txt_Date] datetime NOT NULL DEFAULT ((('') collate Chinese_Taiwan_Stroke_CI_AS)),
    [Adjust_User_ID] varchar(30) NOT NULL DEFAULT ((('') collate Chinese_Taiwan_Stroke_CI_AS)),
    CONSTRAINT [PK_MhbtQsState] PRIMARY KEY ([Hosp_ID], [ID], [Birthday], [Func_Date], [Cure_State], [Cure_Type], [Hosp_Seq_No])
);

GO

CREATE TABLE [SamplingData] (
    [fee_ym] char(6) NOT NULL,
    [data_id] varchar(28) NOT NULL,
    [order_seq_no] int NOT NULL,
    [review] varchar(20) NULL,
    [reviewdate] varchar(8) NULL,
    [reviewremark] varchar(50) NULL,
    [appeals] varchar(20) NULL,
    [appealsdate] varchar(8) NULL,
    [appealsremark] varchar(50) NULL,
    [reviewamt] int NULL,
    [appealsamt] int NULL,
    CONSTRAINT [PK_SamplingData] PRIMARY KEY ([fee_ym], [data_id], [order_seq_no])
);

GO

CREATE TABLE [SamplingList] (
    [fee_ym] char(6) NOT NULL,
    [data_id] varchar(28) NOT NULL,
    [SamplingNo] char(7) NULL,
    [ChkFlg] char(1) NULL,
    [accessdate] varchar(7) NULL,
    [accessno] varchar(6) NULL,
    [replydate] varchar(8) NULL,
    [replyno] varchar(6) NULL,
    CONSTRAINT [PK_SamplingList] PRIMARY KEY ([fee_ym], [data_id])
);

GO

UPDATE [GenEmpData] SET [CreatedAt] = '2020-10-07T01:56:19.7149551+08:00'
WHERE [Id] = N'00000000-0000-0000-0000-000000000000';
SELECT @@ROWCOUNT;


GO

UPDATE [Role] SET [CreatedAt] = '2020-10-07T01:56:19.7169808+08:00'
WHERE [Id] = N'00000000-0000-0000-0000-000000000000';
SELECT @@ROWCOUNT;


GO

UPDATE [RoleEmpMapping] SET [CreatedAt] = '2020-10-07T01:56:19.7171940+08:00'
WHERE [Id] = N'00000000-0000-0000-0000-000000000000';
SELECT @@ROWCOUNT;


GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20201006175620_Sampling', N'3.1.7');

GO

