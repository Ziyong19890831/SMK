CREATE TABLE [dbo].[{0}]  ( 
	[CaseNo]    	char(10) NOT NULL,
	[FirstMonth]	char(6) NOT NULL,
	[TimeSpan]  	int NOT NULL,
	[Birthday]  	char(8) NULL,
	[Edition]   	varchar(5) NULL,
	[Edu]       	varchar(3) NULL,
	[HospID]    	char(10) NULL,
	[HospSeqNo] 	char(2) NULL,
	[ID]        	char(10) NULL,
	[Job]       	varchar(3) NULL,
	[QuitPnt]   	char(1) NULL,
	[QuitCtn]   	char(1) NULL,
	[Result]    	char(3) NULL,
	[VisitDate] 	char(8) NULL,
	PRIMARY KEY CLUSTERED([CaseNo],[FirstMonth],[TimeSpan])
 ON [PRIMARY])
ON [PRIMARY]
	WITH (
		DATA_COMPRESSION = NONE
	);
