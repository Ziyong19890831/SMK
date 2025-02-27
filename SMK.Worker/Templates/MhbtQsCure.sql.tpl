CREATE TABLE [dbo].[{0}](
	[HospID] [char](10) NOT NULL,
	[ID] [char](10) NOT NULL,
	[Birthday] [char](8) NOT NULL,
	[FuncDate] [char](8) NOT NULL,
	[CureItem] [char](10) NOT NULL,
	[HospSeqNo] [char](2) NOT NULL,
	[AdjustUserID] [char](30) NULL,
	[CureNum] [int] NULL,
	[TxtDate] [char](8) NULL,
 PRIMARY KEY CLUSTERED 
(
	[HospID] ASC,
	[ID] ASC,
	[Birthday] ASC,
	[FuncDate] ASC,
	[CureItem] ASC,
	[HospSeqNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY];
ALTER TABLE [dbo].[{0}] ADD  DEFAULT (('') collate Chinese_Taiwan_Stroke_CI_AS) FOR [TxtDate];
ALTER TABLE [dbo].[{0}] ADD  DEFAULT (('') collate Chinese_Taiwan_Stroke_CI_AS) FOR [AdjustUserID];
