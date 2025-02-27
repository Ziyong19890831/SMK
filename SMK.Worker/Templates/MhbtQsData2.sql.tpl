CREATE TABLE [dbo].[{0}](
	[HospId] [nvarchar](10) NOT NULL,
	[ID] [nvarchar](10) NOT NULL,
	[Birthday] [nvarchar](8) NOT NULL,
	[FuncDate] [nvarchar](8) NOT NULL,
	[CureStage] [nvarchar](1) NOT NULL,
	[ExamYear] [nvarchar](4) NOT NULL,
	[Cure_Type] [nvarchar](1) NOT NULL,
	[HospSeqNo] [nvarchar](2) NOT NULL,
	[BaseWeight] [decimal](18, 2) NULL,
	[Cure_State3] [nvarchar](2) NULL,
	[Height] [decimal](18, 2) NULL,
	[PrsnID] [nvarchar](10) NULL,
	[Trace_Co_Check3] [nvarchar](2) NULL,
	[Trace_Date3] [nvarchar](8) NULL,
	[Trace_State3] [nvarchar](3) NULL,
 PRIMARY KEY CLUSTERED
(
	[HospId] ASC,
	[ID] ASC,
	[Birthday] ASC,
	[FuncDate] ASC,
	[CureStage] ASC,
	[ExamYear] ASC,
	[Cure_Type] ASC,
	[HospSeqNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY];

ALTER TABLE [dbo].[{0}] ADD  DEFAULT (('') collate Chinese_Taiwan_Stroke_CI_AS) FOR [PrsnID];
ALTER TABLE [dbo].[{0}] ADD  DEFAULT (('') collate Chinese_Taiwan_Stroke_CI_AS) FOR [Trace_Co_Check3];
ALTER TABLE [dbo].[{0}] ADD  DEFAULT (('') collate Chinese_Taiwan_Stroke_CI_AS) FOR [Trace_Date3];
ALTER TABLE [dbo].[{0}] ADD  DEFAULT (('') collate Chinese_Taiwan_Stroke_CI_AS) FOR [Cure_State3];
ALTER TABLE [dbo].[{0}] ADD  DEFAULT (('') collate Chinese_Taiwan_Stroke_CI_AS) FOR [Trace_State3];

