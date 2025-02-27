CREATE TABLE [dbo].[{0}](
	[HospID] [char](10) NOT NULL,
	[HospAgentCode] [char](5) NULL,
	[ID] [char](10) NOT NULL,
	[Birthday] [char](8) NOT NULL,
	[BranchCode] [char](1) NULL,
	[TxtDate] [char](8) NULL,
	[FuncMark] [char](1) NULL,
	[InformADDR] [nvarchar](120) NULL,
	[Name] [nchar](30) NULL,
	[SeqNo] [char](20) NULL,
	[Sex] [char](1) NULL,
	[TelD] [char](30) NULL,
	[TelM] [char](30) NULL,
	[TelN] [char](30) NULL,
	[TownCode] [char](4) NULL,
	[TownName] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[HospID] ASC,
	[ID] ASC,
	[Birthday] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY];

ALTER TABLE [dbo].[{0}] ADD  DEFAULT (('') collate Chinese_Taiwan_Stroke_CI_AS) FOR [HospAgentCode];
ALTER TABLE [dbo].[{0}] ADD  DEFAULT (('') collate Chinese_Taiwan_Stroke_CI_AS) FOR [Name];
ALTER TABLE [dbo].[{0}] ADD  DEFAULT (('') collate Chinese_Taiwan_Stroke_CI_AS) FOR [Sex];
ALTER TABLE [dbo].[{0}] ADD  DEFAULT (('') collate Chinese_Taiwan_Stroke_CI_AS) FOR [InformADDR];
ALTER TABLE [dbo].[{0}] ADD  DEFAULT (('') collate Chinese_Taiwan_Stroke_CI_AS) FOR [TelD];
ALTER TABLE [dbo].[{0}] ADD  DEFAULT (('') collate Chinese_Taiwan_Stroke_CI_AS) FOR [TelN];
ALTER TABLE [dbo].[{0}] ADD  DEFAULT (('') collate Chinese_Taiwan_Stroke_CI_AS) FOR [TelM];
ALTER TABLE [dbo].[{0}] ADD  DEFAULT (('') collate Chinese_Taiwan_Stroke_CI_AS) FOR [SeqNo];
ALTER TABLE [dbo].[{0}] ADD  DEFAULT (('') collate Chinese_Taiwan_Stroke_CI_AS) FOR [BranchCode];
ALTER TABLE [dbo].[{0}] ADD  DEFAULT (('') collate Chinese_Taiwan_Stroke_CI_AS) FOR [FuncMark];
ALTER TABLE [dbo].[{0}] ADD  DEFAULT (('') collate Chinese_Taiwan_Stroke_CI_AS) FOR [TxtDate];
ALTER TABLE [dbo].[{0}] ADD  DEFAULT (('') collate Chinese_Taiwan_Stroke_CI_AS) FOR [TownCode];
ALTER TABLE [dbo].[{0}] ADD  DEFAULT (('') collate Chinese_Taiwan_Stroke_CI_AS) FOR [TownName];
