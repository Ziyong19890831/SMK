CREATE TABLE [dbo].[{0}](
	[HospID] [char](10) NOT NULL,
	[ID] [char](10) NOT NULL,
	[Birthday] [char](8) NOT NULL,
	[FuncDate] [char](8) NOT NULL,
	[CureStage] [char](1) NOT NULL,
	[ExamYear] [char](4) NOT NULL,
	[CurtState] [varchar](20) NOT NULL,
	[Cure_Type] [char](1) NOT NULL,
	[HospSeqNo] [char](2) NOT NULL,
	[AdjustUserID] [char](30) NULL,
	[BaseWeight] [decimal](18, 2) NULL,
	[BranchCode] [char](1) NULL,
	[Case_Kind] [char](1) NULL,
	[Case_Source] [char](1) NULL,
	[ChType] [char](1) NULL,
	[CoCheck] [decimal](10, 0) NULL,
	[CureAgree] [char](1) NULL,
	[Cure_State2] [char](2) NULL,
	[CureStateOther] [varchar](500) NULL,
	[CureWeek] [int] NULL,
	[FeeMark] [char](1) NULL,
	[FirstTreatDate] [char](8) NULL,
	[PrsnID] [char](10) NULL,
	[SideEffect] [varchar](100) NOT NULL,
	[SmokeBed] [char](1) NULL,
	[SmokeDayNum] [int] NULL,
	[SmokeFirst] [char](1) NULL,
	[SmokeLung] [char](1) NULL,
	[SmokeMon] [int] NULL,
	[SmokeMuch] [char](1) NULL,
	[SmokeNico] [char](1) NULL,
	[SmokeNoGp] [char](1) NULL,
	[SmokeScore] [char](6) NULL,
	[SmokeSick] [char](1) NULL,
	[SmokeStop] [char](1) NULL,
	[SmokeYear] [int] NULL,
	[TraceCoCheck] [decimal](10, 0) NULL,
	[Trace_Co_Check2] [char](2) NULL,
	[TraceDate] [varchar](10) NULL,
	[Trace_Date2] [char](8) NULL,
	[TraceState] [varchar](30) NULL,
	[Trace_State2] [char](3) NULL,
	[TxtDate] [char](8) NULL,
	[WeekTot] [int] NULL,
 PRIMARY KEY CLUSTERED 
(
	[HospID] ASC,
	[ID] ASC,
	[Birthday] ASC,
	[FuncDate] ASC,
	[CureStage] ASC,
	[ExamYear] ASC,
	[CurtState] ASC,
	[Cure_Type] ASC,
	[HospSeqNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY];
ALTER TABLE [dbo].[{0}] ADD  DEFAULT (('') collate Chinese_Taiwan_Stroke_CI_AS) FOR [FirstTreatDate];
ALTER TABLE [dbo].[{0}] ADD  DEFAULT (('') collate Chinese_Taiwan_Stroke_CI_AS) FOR [PrsnID];
ALTER TABLE [dbo].[{0}] ADD  DEFAULT (('') collate Chinese_Taiwan_Stroke_CI_AS) FOR [SmokeFirst];
ALTER TABLE [dbo].[{0}] ADD  DEFAULT (('') collate Chinese_Taiwan_Stroke_CI_AS) FOR [SmokeStop];
ALTER TABLE [dbo].[{0}] ADD  DEFAULT (('') collate Chinese_Taiwan_Stroke_CI_AS) FOR [SmokeNoGp];
ALTER TABLE [dbo].[{0}] ADD  DEFAULT (('') collate Chinese_Taiwan_Stroke_CI_AS) FOR [SmokeMuch];
ALTER TABLE [dbo].[{0}] ADD  DEFAULT (('') collate Chinese_Taiwan_Stroke_CI_AS) FOR [SmokeBed];
ALTER TABLE [dbo].[{0}] ADD  DEFAULT (('') collate Chinese_Taiwan_Stroke_CI_AS) FOR [SmokeSick];
ALTER TABLE [dbo].[{0}] ADD  DEFAULT (('') collate Chinese_Taiwan_Stroke_CI_AS) FOR [SmokeNico];
ALTER TABLE [dbo].[{0}] ADD  DEFAULT (('') collate Chinese_Taiwan_Stroke_CI_AS) FOR [SmokeLung];
ALTER TABLE [dbo].[{0}] ADD  DEFAULT (('') collate Chinese_Taiwan_Stroke_CI_AS) FOR [SmokeScore];
ALTER TABLE [dbo].[{0}] ADD  DEFAULT (('') collate Chinese_Taiwan_Stroke_CI_AS) FOR [CureAgree];
ALTER TABLE [dbo].[{0}] ADD  DEFAULT (('') collate Chinese_Taiwan_Stroke_CI_AS) FOR [BranchCode];
ALTER TABLE [dbo].[{0}] ADD  DEFAULT (('') collate Chinese_Taiwan_Stroke_CI_AS) FOR [TxtDate];
ALTER TABLE [dbo].[{0}] ADD  DEFAULT (('') collate Chinese_Taiwan_Stroke_CI_AS) FOR [AdjustUserID];
ALTER TABLE [dbo].[{0}] ADD  DEFAULT (('') collate Chinese_Taiwan_Stroke_CI_AS) FOR [FeeMark];
ALTER TABLE [dbo].[{0}] ADD  DEFAULT (('') collate Chinese_Taiwan_Stroke_CI_AS) FOR [TraceDate];
ALTER TABLE [dbo].[{0}] ADD  DEFAULT (('') collate Chinese_Taiwan_Stroke_CI_AS) FOR [TraceState];
ALTER TABLE [dbo].[{0}] ADD  DEFAULT (('') collate Chinese_Taiwan_Stroke_CI_AS) FOR [SideEffect];
ALTER TABLE [dbo].[{0}] ADD  DEFAULT (('') collate Chinese_Taiwan_Stroke_CI_AS) FOR [CureStateOther];
ALTER TABLE [dbo].[{0}] ADD  DEFAULT (('') collate Chinese_Taiwan_Stroke_CI_AS) FOR [ChType];
ALTER TABLE [dbo].[{0}] ADD  DEFAULT (('') collate Chinese_Taiwan_Stroke_CI_AS) FOR [Trace_Date2];
ALTER TABLE [dbo].[{0}] ADD  DEFAULT (('') collate Chinese_Taiwan_Stroke_CI_AS) FOR [Trace_State2];
ALTER TABLE [dbo].[{0}] ADD  DEFAULT (('') collate Chinese_Taiwan_Stroke_CI_AS) FOR [Cure_State2];
ALTER TABLE [dbo].[{0}] ADD  DEFAULT (('') collate Chinese_Taiwan_Stroke_CI_AS) FOR [Trace_Co_Check2];
ALTER TABLE [dbo].[{0}] ADD  DEFAULT (('') collate Chinese_Taiwan_Stroke_CI_AS) FOR [Case_Source];
ALTER TABLE [dbo].[{0}] ADD  DEFAULT (('') collate Chinese_Taiwan_Stroke_CI_AS) FOR [Case_Kind];
