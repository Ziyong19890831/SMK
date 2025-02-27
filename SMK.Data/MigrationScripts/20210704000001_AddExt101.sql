/****** Object:  StoredProcedure [dbo].[ExecHospQuotaProcess]    Script Date: 2021/7/16 下午 10:10:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--V2.0
CREATE PROCEDURE [dbo].[ExecHospQuotaProcess]
AS
Declare @strCurYear char(4)
Declare @strCCurYear char(3)
Declare @strHospID char(10)
Declare @strHospSeqNo char(2)
Declare @strHospStartDate char(10)
Declare @strHospEndDate char(10)
Declare @strSMKContractType char(2)
Declare @strHospStartDateTmp char(10)
Declare @strHospEndDateTmp char(10)
Declare @strSMKContractTypeTmp char(2)
Declare @strReHospStartDate char(10)

BEGIN
if object_id('tmpHospQuota') is not null
drop table tmpHospQuota

create table tmpHospQuota
(	CurYear char(3),
     HospID char(10),
     HospSeqNo char(2),
     ExamType char(1),
     Quota varchar(10),
     HospStartDate char(10),
     HospEndDate char(10),
     SMKContractType char(2),
     DelFlg char(1))

    set @strCurYear = year(getdate())
set @strCCurYear = year(getdate()) - 1911

Declare HospBasic_Cursor Cursor For select HospID,HospSeqNo from HospBasic where HospID = LastHospID and HospSeqNo = LastHospSeqNo and HospStatus in ('2','3') order by HospID,HospSeqNo
    Open HospBasic_Cursor Fetch Next From HospBasic_Cursor Into @strHospID,@strHospSeqNo
                                        While @@Fetch_Status = 0
Begin
		--SMKContractType = '02'
		Declare HospContract02_Cursor Cursor For select case when substring(ltrim(rtrim(HospStartDate)),1,4) = @strCurYear then HospStartDate else @strCurYear + '0101' end as HospStartDate,case when (substring(ltrim(rtrim(HospEndDate)),1,4) = @strCurYear) then HospEndDate when ltrim(rtrim(HospEndDate)) = '' then @strCurYear + '1231' else HospEndDate end as HospEndDate,SMKContractType from HospContract where HospID=@strHospID and HospSeqNo=@strHospSeqNo and ltrim(rtrim(HospStartDate)) <> '' and (substring(ltrim(rtrim(HospEndDate)), 1, 4) = @strCurYear or ltrim(rtrim(HospEndDate)) = '') and SMKContractType = '02'
    Open HospContract02_Cursor Fetch Next From HospContract02_Cursor Into @strHospStartDate,@strHospEndDate,@strSMKContractType
                                                     While @@Fetch_Status = 0
Begin
			--代碼變更
			set @strReHospStartDate = (select isnull(case
																	 when f.HospStartDate is not null then
																	  case
																		when substring(ltrim(rtrim(f.HospStartDate)), 1, 4) = @strCurYear and substring(ltrim(rtrim(f.HospEndDate)), 1, 4) = @strCurYear then
																		 f.HospStartDate
																		else
																			case when substring(ltrim(rtrim(f.HospEndDate)), 1, 4) = @strCurYear then
																				@strCurYear + '0101'
																			else
																				''
																			end
																	  end
																   end,'') as HospStartDate
															  from (select max(HospStartDate) as HospStartDate, max(HospEndDate) as HospEndDate
																	  from (select HospID, HospSeqNo, LastHospID, LastHospSeqNo
																			  from HospBasic
																			 where HospID <> LastHospID) a
																	  join (select HospID, HospSeqNo, HospStartDate, HospEndDate
																			 from HospContract
																			where substring(EndReasonNo, 1, 1) = '3'
																			   and SMKContractType = '02') b
																		on a.HospID = b.HospID
																	   and a.HospSeqNo = b.HospSeqNo
																	 where a.LastHospID = @strHospID
																	    and a.LastHospSeqNo = @strHospSeqNo) f)
			if @strReHospStartDate <> ''
Begin
					set @strHospStartDate = @strReHospStartDate
End
			if substring(@strHospStartDate,1,4) = @strCurYear and substring(@strHospEndDate,1,4) = @strCurYear
Begin
insert into tmpHospQuota(CurYear,HospID,HospSeqNo,ExamType,Quota,HospStartDate,HospEndDate,SMKContractType) values (@strCCurYear,@strHospID,@strHospSeqNo,'1','99999',@strHospStartDate,@strHospEndDate,@strSMKContractType)
End
Fetch Next From HospContract02_Cursor Into @strHospStartDate,@strHospEndDate,@strSMKContractType
End
Close HospContract02_Cursor
    Deallocate HospContract02_Cursor
--SMKContractType = '02'

--SMKContractType = '03'
Declare HospContract03_Cursor Cursor For select case when substring(ltrim(rtrim(HospStartDate)),1,4) = @strCurYear then HospStartDate else @strCurYear + '0101' end as HospStartDate,case when (substring(ltrim(rtrim(HospEndDate)),1,4) = @strCurYear) then HospEndDate when ltrim(rtrim(HospEndDate)) = '' then @strCurYear + '1231' else HospEndDate end as HospEndDate,SMKContractType from HospContract where HospID=@strHospID and ltrim(rtrim(HospStartDate)) <> '' and (substring(ltrim(rtrim(HospEndDate)), 1, 4) = @strCurYear or ltrim(rtrim(HospEndDate)) = '') and HospSeqNo=@strHospSeqNo and SMKContractType = '03'
    Open HospContract03_Cursor Fetch Next From HospContract03_Cursor Into @strHospStartDate,@strHospEndDate,@strSMKContractType
                                             While @@Fetch_Status = 0
Begin
			--代碼變更
			set @strReHospStartDate = (select isnull(case
																	 when f.HospStartDate is not null then
																	  case
																		when substring(ltrim(rtrim(f.HospStartDate)), 1, 4) = @strCurYear and substring(ltrim(rtrim(f.HospEndDate)), 1, 4) = @strCurYear then
																		 f.HospStartDate
																		else
																			case when substring(ltrim(rtrim(f.HospEndDate)), 1, 4) = @strCurYear then
																				@strCurYear + '0101'
																			else
																				''
																			end
																	  end
																   end,'') as HospStartDate
															  from (select max(HospStartDate) as HospStartDate, max(HospEndDate) as HospEndDate
																	  from (select HospID, HospSeqNo, LastHospID, LastHospSeqNo
																			  from HospBasic
																			 where HospID <> LastHospID) a
																	  join (select HospID, HospSeqNo, HospStartDate, HospEndDate
																			 from HospContract
																			where substring(EndReasonNo, 1, 1) = '3'
																			   and SMKContractType = '03') b
																		on a.HospID = b.HospID
																	   and a.HospSeqNo = b.HospSeqNo
																	 where a.LastHospID = @strHospID
																	    and a.LastHospSeqNo = @strHospSeqNo) f)
			if @strReHospStartDate <> ''
Begin
					set @strHospStartDate = @strReHospStartDate
End
			if substring(@strHospStartDate,1,4) = @strCurYear and substring(@strHospEndDate,1,4) = @strCurYear
Begin
insert into tmpHospQuota(CurYear,HospID,HospSeqNo,ExamType,Quota,HospStartDate,HospEndDate,SMKContractType) values (@strCCurYear,@strHospID,@strHospSeqNo,'2','99999',@strHospStartDate,@strHospEndDate,@strSMKContractType)
End
Fetch Next From HospContract03_Cursor Into @strHospStartDate,@strHospEndDate,@strSMKContractType
End
Close HospContract03_Cursor
    Deallocate HospContract03_Cursor
		--SMKContractType = '03'

		Fetch Next From HospBasic_Cursor Into @strHospID,@strHospSeqNo
End

Close HospBasic_Cursor
    Deallocate HospBasic_Cursor


Declare HospBasic_Cursor Cursor For select HospID,HospSeqNo from HospBasic where HospID = LastHospID and HospSeqNo = LastHospSeqNo and HospStatus in ('2','3') order by HospID,HospSeqNo
    Open HospBasic_Cursor Fetch Next From HospBasic_Cursor Into @strHospID,@strHospSeqNo
                                        While @@Fetch_Status = 0
Begin
		--SMKContractType = '01' (補前後日期，院所配額)
		Declare HospContract01_Cursor Cursor For select ff.HospStartDate, ff.HospEndDate, ff.SMKContractType
                                                 from (select row_number() over(partition by f.HospID, f.HospSeqNo order by f.HospStartDate desc) as rnk,
                                                               f.HospID,
                                                              f.HospSeqNo,
                                                              f.HospStartDate,
                                                              f.HospEndDate,
                                                              f.SMKContractType
                                                       from (select HospID,
                                                                    HospSeqNo,
                                                                    case
                                                                        when substring(ltrim(rtrim(HospStartDate)), 1, 4) =
                                                                             @strCurYear then
                                                                            HospStartDate
                                                                        else
                                                                                @strCurYear + '0101'
                                                                        end as HospStartDate,
                                                                    case
                                                                        when (substring(ltrim(rtrim(HospEndDate)), 1, 4) =
                                                                              @strCurYear) then
                                                                            HospEndDate
                                                                        when ltrim(rtrim(HospEndDate)) = '' then
                                                                                @strCurYear + '1231'
                                                                        else
                                                                            HospEndDate
                                                                        end as HospEndDate,
                                                                    SMKContractType
                                                             from HospContract
                                                             where HospID = @strHospID
                                                               and HospSeqNo = @strHospSeqNo
                                                               and (substring(ltrim(rtrim(HospEndDate)), 1, 4) = @strCurYear or
                                                                    ltrim(rtrim(HospEndDate)) = '')
                                                               and SMKContractType = '01'
                                                               and HospID + HospSeqNo in
                                                                   (select HospID + HospSeqNo
                                                                    from tmpHospQuota
                                                                    where (substring(HospStartDate, 5, 4) <> '0101' or
                                                                           substring(HospEndDate, 5, 4) <> '12/31'))) f) ff
                                                 where ff.rnk = 1
                                                     Open HospContract01_Cursor Fetch Next From HospContract01_Cursor Into @strHospStartDate,@strHospEndDate,@strSMKContractType
                                                     While @@Fetch_Status = 0
Begin
				Declare tmpHospQuota_Cursor Cursor For select f.HospStartDate, f.HospEndDate, f.SMKContractType
                                                       from (select row_number() over(partition by HospID, HospSeqNo, SMKContractType order by HospStartDate desc) as rnk,
                                                                     HospID,
                                                                    HospSeqNo,
                                                                    HospStartDate,
                                                                    HospEndDate,
                                                                    SMKContractType
                                                             from tmpHospQuota
                                                             where (substring(HospStartDate, 5, 4) <> '0101' or
                                                                    substring(HospEndDate, 5, 4) <> '12/31')
                                                               and HospID = @strHospID
                                                               and HospSeqNo = @strHospSeqNo) f
                                                       where rnk = 1
                                                           Open tmpHospQuota_Cursor Fetch Next From tmpHospQuota_Cursor Into @strHospStartDateTmp,@strHospEndDateTmp,@strSMKContractTypeTmp
                                                           While @@Fetch_Status = 0
Begin
					if @strSMKContractTypeTmp = '02'
Begin
							if substring(@strHospStartDateTmp,5,4) <> '0101' and @strHospStartDate < @strHospStartDateTmp
Begin
insert into tmpHospQuota(CurYear,HospID,HospSeqNo,ExamType,Quota,HospStartDate,HospEndDate,SMKContractType) values (@strCCurYear,@strHospID,@strHospSeqNo,'1','Q',@strHospStartDate,convert(char(8),dateadd(day,-1,convert(varchar,convert(datetime,@strHospStartDateTmp),111)),112),@strSMKContractType)
End
							if substring(@strHospEndDateTmp,5,4) <> '1231' and @strHospEndDate > @strHospEndDateTmp
Begin
								    if @strHospStartDate > convert(char(8),dateadd(day,1,convert(varchar,convert(datetime,@strHospEndDateTmp),111)),112)
Begin
insert into tmpHospQuota(CurYear,HospID,HospSeqNo,ExamType,Quota,HospStartDate,HospEndDate,SMKContractType) values (@strCCurYear,@strHospID,@strHospSeqNo,'1','Q',@strHospStartDate,@strHospEndDate,@strSMKContractType)
End
else
Begin
insert into tmpHospQuota(CurYear,HospID,HospSeqNo,ExamType,Quota,HospStartDate,HospEndDate,SMKContractType) values (@strCCurYear,@strHospID,@strHospSeqNo,'1','Q',convert(char(8),dateadd(day,1,convert(varchar,convert(datetime,@strHospEndDateTmp),111)),112),@strHospEndDate,@strSMKContractType)
End
End
End
					if @strSMKContractTypeTmp = '03'
Begin
							if substring(@strHospStartDateTmp,5,4) <> '0101' and @strHospStartDate < @strHospStartDateTmp
Begin
insert into tmpHospQuota(CurYear,HospID,HospSeqNo,ExamType,Quota,HospStartDate,HospEndDate,SMKContractType) values (@strCCurYear,@strHospID,@strHospSeqNo,'2','Q',@strHospStartDate,convert(char(8),dateadd(day,-1,convert(varchar,convert(datetime,@strHospStartDateTmp),111)),112),@strSMKContractType)
End
							if substring(@strHospEndDateTmp,5,4) <> '1231' and @strHospEndDate > @strHospEndDateTmp
Begin
								    if @strHospStartDate > convert(char(8),dateadd(day,1,convert(varchar,convert(datetime,@strHospEndDateTmp),111)),112)
Begin
insert into tmpHospQuota(CurYear,HospID,HospSeqNo,ExamType,Quota,HospStartDate,HospEndDate,SMKContractType) values (@strCCurYear,@strHospID,@strHospSeqNo,'2','Q',@strHospStartDate,@strHospEndDate,@strSMKContractType)
End
else
Begin
insert into tmpHospQuota(CurYear,HospID,HospSeqNo,ExamType,Quota,HospStartDate,HospEndDate,SMKContractType) values (@strCCurYear,@strHospID,@strHospSeqNo,'2','Q',convert(char(8),dateadd(day,1,convert(varchar,convert(datetime,@strHospEndDateTmp),111)),112),@strHospEndDate,@strSMKContractType)
End
End
End

Fetch Next From tmpHospQuota_Cursor Into @strHospStartDateTmp,@strHospEndDateTmp,@strSMKContractTypeTmp
End
Close tmpHospQuota_Cursor
    Deallocate tmpHospQuota_Cursor

			Fetch Next From HospContract01_Cursor Into @strHospStartDate,@strHospEndDate,@strSMKContractType
End
Close HospContract01_Cursor
    Deallocate HospContract01_Cursor
		--SMKContractType = '01' 補前後日期

		Fetch Next From HospBasic_Cursor Into @strHospID,@strHospSeqNo
End

Close HospBasic_Cursor
    Deallocate HospBasic_Cursor


Declare HospBasic_Cursor Cursor For select HospID,HospSeqNo from HospBasic where HospID = LastHospID and HospSeqNo = LastHospSeqNo and HospStatus in ('2','3') order by HospID,HospSeqNo
    Open HospBasic_Cursor Fetch Next From HospBasic_Cursor Into @strHospID,@strHospSeqNo
                                        While @@Fetch_Status = 0
Begin
		--SMKContractType = '01' (只有主約，判斷 PrsnContract，CouldTreat)
		Declare HospContract01_Cursor Cursor For select a.HospStartDate,
                                                        a.HospEndDate,
                                                        a.SMKContractType
                                                 from (select HospID,
                                                              HospSeqNo,
                                                              case
                                                                  when substring(ltrim(rtrim(HospStartDate)), 1, 4) = @strCurYear then
                                                                      HospStartDate
                                                                  else
                                                                          @strCurYear + '0101'
                                                                  end as HospStartDate,
                                                              case when (substring(ltrim(rtrim(HospEndDate)),1,4) = @strCurYear) then
                                                                       HospEndDate
                                                                   when ltrim(rtrim(HospEndDate)) = '' then
                                                                           @strCurYear + '1231'
                                                                   else
                                                                       HospEndDate
                                                                  end as HospEndDate,
                                                              SMKContractType
                                                       from HospContract
                                                       where (substring(ltrim(rtrim(HospEndDate)), 1, 4) = @strCurYear or
                                                              ltrim(rtrim(HospEndDate)) = '')
                                                         and SMKContractType = '01'
                                                         and HospID + HospSeqNo not in
                                                             (select HospID + HospSeqNo from tmpHospQuota where ExamType = '1')) a
                                                          join (select distinct HospID, HospSeqNo
                                                                from PrsnContract
                                                                where ltrim(rtrim(PrsnStartDate)) <> ''
                                                                  and (substring(ltrim(rtrim(PrsnEndDate)), 1, 4) = @strCurYear or
                                                                       ltrim(rtrim(PrsnEndDate)) = '')
                                                                  and CouldTreat = '1') b
                                                               on a.HospID = b.HospID
                                                                   and a.HospSeqNo = b.HospSeqNo
                                                 where a.HospID = @strHospID
                                                   and a.HospSeqNo = @strHospSeqNo
                                                     Open HospContract01_Cursor Fetch Next From HospContract01_Cursor Into @strHospStartDate,@strHospEndDate,@strSMKContractType
                                                     While @@Fetch_Status = 0
Begin
			--代碼變更
			set @strReHospStartDate = (select isnull(case
																	 when f.HospStartDate is not null then
																	  case
																		when substring(ltrim(rtrim(f.HospStartDate)), 1, 4) = @strCurYear then
																		 f.HospStartDate
																		else
																		 @strCurYear + '0101'
																	  end
																   end,'') as HospStartDate
															  from (select max(HospStartDate) as HospStartDate
																	  from (select HospID, HospSeqNo, LastHospID, LastHospSeqNo
																			  from HospBasic
																			 where HospID <> LastHospID) a
																	  join (select HospID, HospSeqNo, HospStartDate, HospEndDate
																			 from HospContract
																			where substring(EndReasonNo, 1, 1) = '3'
																			   and SMKContractType = '01') b
																		on a.HospID = b.HospID
																	   and a.HospSeqNo = b.HospSeqNo
																	 where a.LastHospID = @strHospID
																	    and a.LastHospSeqNo = @strHospSeqNo) f)
			if @strReHospStartDate <> ''
Begin
					set @strHospStartDate = @strReHospStartDate
End
insert into tmpHospQuota(CurYear,HospID,HospSeqNo,ExamType,Quota,HospStartDate,HospEndDate,SMKContractType) values (@strCCurYear,@strHospID,@strHospSeqNo,'1','Q',@strHospStartDate,@strHospEndDate,@strSMKContractType)
    Fetch Next From HospContract01_Cursor Into @strHospStartDate,@strHospEndDate,@strSMKContractType
End
Close HospContract01_Cursor
    Deallocate HospContract01_Cursor
		--SMKContractType = '01' (只有主約，判斷 PrsnContract，CouldTreat)

		Fetch Next From HospBasic_Cursor Into @strHospID,@strHospSeqNo
End

Close HospBasic_Cursor
    Deallocate HospBasic_Cursor

Declare HospBasic_Cursor Cursor For select HospID,HospSeqNo from HospBasic where HospID = LastHospID and HospSeqNo = LastHospSeqNo and HospStatus in ('2','3') order by HospID,HospSeqNo
    Open HospBasic_Cursor Fetch Next From HospBasic_Cursor Into @strHospID,@strHospSeqNo
                                        While @@Fetch_Status = 0
Begin
		--SMKContractType = '01' (只有主約，判斷 PrsnContract，CouldInstruct)
		Declare HospContract01_Cursor Cursor For select a.HospStartDate,
                                                        a.HospEndDate,
                                                        a.SMKContractType
                                                 from (select HospID,
                                                              HospSeqNo,
                                                              case
                                                                  when substring(ltrim(rtrim(HospStartDate)), 1, 4) = @strCurYear then
                                                                      HospStartDate
                                                                  else
                                                                          @strCurYear + '0101'
                                                                  end as HospStartDate,
                                                              case when (substring(ltrim(rtrim(HospEndDate)),1,4) = @strCurYear) then
                                                                       HospEndDate
                                                                   when ltrim(rtrim(HospEndDate)) = '' then
                                                                           @strCurYear + '1231'
                                                                   else
                                                                       HospEndDate
                                                                  end as HospEndDate,
                                                              SMKContractType
                                                       from HospContract
                                                       where (substring(ltrim(rtrim(HospEndDate)), 1, 4) = @strCurYear or
                                                              ltrim(rtrim(HospEndDate)) = '')
                                                         and SMKContractType = '01'
                                                         and HospID + HospSeqNo not in
                                                             (select HospID + HospSeqNo from tmpHospQuota where ExamType = '2')) a
                                                          join (select distinct HospID, HospSeqNo
                                                                from PrsnContract
                                                                where ltrim(rtrim(PrsnStartDate)) <> ''
                                                                  and (substring(ltrim(rtrim(PrsnEndDate)), 1, 4) = @strCurYear or
                                                                       ltrim(rtrim(PrsnEndDate)) = '')
                                                                  and CouldInstruct = '1') b
                                                               on a.HospID = b.HospID
                                                                   and a.HospSeqNo = b.HospSeqNo
                                                 where a.HospID = @strHospID
                                                   and a.HospSeqNo = @strHospSeqNo
                                                     Open HospContract01_Cursor Fetch Next From HospContract01_Cursor Into @strHospStartDate,@strHospEndDate,@strSMKContractType
                                                     While @@Fetch_Status = 0
Begin
			--代碼變更
			set @strReHospStartDate = (select isnull(case
																	 when f.HospStartDate is not null then
																	  case
																		when substring(ltrim(rtrim(f.HospStartDate)), 1, 4) = @strCurYear then
																		 f.HospStartDate
																		else
																		 @strCurYear + '0101'
																	  end
																   end,'') as HospStartDate
															  from (select max(HospStartDate) as HospStartDate
																	  from (select HospID, HospSeqNo, LastHospID, LastHospSeqNo
																			  from HospBasic
																			 where HospID <> LastHospID) a
																	  join (select HospID, HospSeqNo, HospStartDate, HospEndDate
																			 from HospContract
																			where substring(EndReasonNo, 1, 1) = '3'
																			   and SMKContractType = '01') b
																		on a.HospID = b.HospID
																	   and a.HospSeqNo = b.HospSeqNo
																	 where a.LastHospID = @strHospID
																	    and a.LastHospSeqNo = @strHospSeqNo) f)
			if @strReHospStartDate <> ''
Begin
					set @strHospStartDate = @strReHospStartDate
End
insert into tmpHospQuota(CurYear,HospID,HospSeqNo,ExamType,Quota,HospStartDate,HospEndDate,SMKContractType) values (@strCCurYear,@strHospID,@strHospSeqNo,'2','Q',@strHospStartDate,@strHospEndDate,@strSMKContractType)
    Fetch Next From HospContract01_Cursor Into @strHospStartDate,@strHospEndDate,@strSMKContractType
End
Close HospContract01_Cursor
    Deallocate HospContract01_Cursor
		--SMKContractType = '01' (只有主約，判斷 PrsnContract，CouldInstruct)

		Fetch Next From HospBasic_Cursor Into @strHospID,@strHospSeqNo
End

Close HospBasic_Cursor
    Deallocate HospBasic_Cursor


--update tmpHospQuota set HospStartDate = substring(HospStartDate,1,4) + '/' + substring(HospStartDate,5,2) + '/' + substring(HospStartDate,7,2) where charindex('/',HospStartDate) = 0
--update tmpHospQuota set HospEndDate = substring(HospEndDate,1,4) + '/' + substring(HospEndDate,5,2) + '/' + substring(HospEndDate,7,2) where charindex('/',HospEndDate) = 0

update tmpHospQuota
set tmpHospQuota.Quota = f.Quota
    from (select a.HospID,
					a.HospSeqNo,
					(case
					when b.LastContType = '1' then
					'300'
					when b.LastContType in
						('2', '5') then
					'180'
					when b.LastContType in
						('3', '4', '6', '7') then
					'120'
					end) as Quota
			from tmpHospQuota a
			join HospBasic b
				on a.HospID = b.LastHospID
				and a.HospSeqNo = b.LastHospSeqNo
			where a.Quota = 'Q') f
where tmpHospQuota.HospID = f.HospID
  and tmpHospQuota.HospSeqNo = f.HospSeqno
  and tmpHospQuota.Quota = 'Q'

--在年度中申請加入品質改善的院所(配額由少變多)，只出一筆
update tmpHospQuota
set tmpHospQuota.DelFlg = 'Y'
    from (select HospID, HospSeqNo, ExamType
  from tmpHospQuota
 where quota <> '99999'
   and HospID + HospSeqNo + ExamType in
       (select HospID + HospSeqNo + ExamType
          from tmpHospQuota
         where quota = '99999'
           and HospStartDate <> @strCurYear + '0101')) f
where tmpHospQuota.HospID = f.HospID
  and tmpHospQuota.HospSeqNo = f.HospSeqno
  and tmpHospQuota.ExamType = f.ExamType
  and tmpHospQuota.Quota <> '99999'

update tmpHospQuota
set tmpHospQuota.HospStartDate = f.HospStartDate
    from (select HospID, HospSeqNo, ExamType, HospStartDate
  from tmpHospQuota
 where quota <> '99999'
   and HospID + HospSeqNo + ExamType in
       (select HospID + HospSeqNo + ExamType
          from tmpHospQuota
         where quota = '99999'
           and HospStartDate <> @strCurYear + '0101')) f
where tmpHospQuota.HospID = f.HospID
  and tmpHospQuota.HospSeqNo = f.HospSeqno
  and tmpHospQuota.ExamType = f.ExamType
  and tmpHospQuota.Quota = '99999'

delete from tmpHospQuota where DelFlg = 'Y'

END
GO
/****** Object:  StoredProcedure [dbo].[ExecReviewComProcess]    Script Date: 2021/7/16 下午 10:10:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--V1.4
CREATE PROCEDURE [dbo].[ExecReviewComProcess]
@GetFeeYMS char(8),
@GetFeeYME char(8),
@GetHospID char(10)
AS
Declare @strFeeYMS char(8)
Declare @strFeeYME char(8)
Declare @strHospID char(10)

Declare @strfee_ym char(6)
Declare @strdata_id char(28)
Declare @intorder_seq_no int 
Declare @intorder_dot int
Declare @intdrug_dot int
Declare @intdsvc_dot int

BEGIN

set @strFeeYMS = @GetFeeYMS
set @strFeeYME = @GetFeeYME
set @strHospID = @GetHospID

if object_id('Dedu') is not null
drop table Dedu
    if object_id('DeduOrder') is not null
drop table DeduOrder

create table Dedu
(	data_id char(28),
     fee_ym char(6),
     HospID char(10),
     seq_no int,
     ID char(10),
     birthday char(8),
     func_date char(8),
     MedApply char(1),
     InstructApply char(1),
     TraceApply char(1),
     ReleaseApply char(1),
     appl_date char(8),
     prsn_id char(10),
     appl_dot int,
     part_code char(3),
     part_amt int,
     drug_days int,
     dsvc_code char(12),
     dsvc_dot int,
     drug_dot int,
     DeduDot int,
     DeduCode1 char(1),
     DeduCode2 char(1),
     DeduCode3 char(1),
     DeduCode4 char(1),
     DeduCode5 char(1),
     DeduCode6 char(1),
     DeduCode7 char(1),
     DeduCode8 char(1),
     DeduCode9 char(1),
     DeduCode10 char(1),
     DeduCode11 char(1),
     DeduCode12 char(1),
     DeduCode13 char(1),
     real_hosp_id char(10),
     orig_hosp_id char(10),
     area_service char(2),
     PartDeduDot int,
     C_Dedu_Dot int,
     c_dsvc_dot int,
     c_drug_dot int,
     c_appl_dot int,
     c_part_amt int,
     pay_amt int,
     icd9cm_code char(9),
     icd9cm_code1 char(9),
     icd9cm_code2 char(9),
     icd10cm_code3 char(10),
     icd10cm_code4 char(10),
     data_type char(1),
     rec_code char(60),
     drug_prsn_id char(10),
     CONSTRAINT [PK_Dedu] PRIMARY KEY CLUSTERED
         (
         [fee_ym] ASC,
         [data_id] ASC
         )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
CREATE INDEX INX_Dedu_1 ON Dedu (HospID)
CREATE INDEX INX_Dedu_2 ON Dedu (ID)
CREATE INDEX INX_Dedu_3 ON Dedu (birthday)
CREATE INDEX INX_Dedu_4 ON Dedu (func_date)
CREATE INDEX INX_Dedu_5 ON Dedu (data_type)
CREATE INDEX INX_Dedu_6 ON Dedu (prsn_id)
CREATE INDEX INX_Dedu_7 ON Dedu (drug_prsn_id)
CREATE INDEX INX_Dedu_8 ON Dedu (MedApply)
CREATE INDEX INX_Dedu_9 ON Dedu (InstructApply)
CREATE INDEX INX_Dedu_10 ON Dedu (TraceApply)

create table DeduOrder
(	data_id char(28),
     fee_ym char(6),
     hospid char(10),
     order_seq_no int,
     order_dot int,
     order_type char(1),
     order_code char(12),
     exe_prsn_id char(10),
     c_other_dot int,
     c_order_dot int,
     order_qty decimal(7,2),
     order_drug_day int
         CONSTRAINT [PK_DeduOrder] PRIMARY KEY CLUSTERED
         (
         [fee_ym] ASC,
         [data_id] ASC,
         [order_seq_no] ASC
         )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
CREATE INDEX INX_DeduOrder_1 ON DeduOrder (hospid)
CREATE INDEX INX_DeduOrder_2 ON DeduOrder (order_code)
CREATE INDEX INX_DeduOrder_3 ON DeduOrder (order_type)
CREATE INDEX INX_DeduOrder_4 ON DeduOrder (exe_prsn_id)

--Insert Dedu
insert dedu
select
    a.data_id,
    a.fee_ym,
    a.hospid,
    a.seq_no,
    a.id,
    a.birthday,
    a.func_date,
    a.MedApply,
    a.InstructApply,
    a.TraceApply,
    a.ReleaseApply,
    a.appl_date,
    a.prsn_id,
    a.appl_dot,
    a.part_code,
    a.part_amt,
    a.drug_days,
    a.dsvc_code,
    a.dsvc_dot,
    a.drug_dot,
    0 as dedudot,
    '' as deducode1,
    '' as deducode2,
    '' as deducode3,
    '' as deducode4,
    '' as deducode5,
    '' as deducode6,
    '' as deducode7,
    '' as deducode8,
    '' as deducode9,
    '' as deducode10,
    '' as deducode11,
    '' as deducode12,
    '' as deducode13,
    a.real_hosp_id,
    '' as orig_hosp_id,
    a.area_service,
    0 as partdedudot,
    dd.c_dedu_dot,
    0 as c_dsvc_dot,
    0 as c_drug_dot,
    0 as c_appl_dot,
    0 as c_part_amt,
    apm.pay_amt,
    a.icd9cm_code,
    a.icd9cm_code1,
    a.icd9cm_code2,
    a.icd10cm_code3,
    a.icd10cm_code4,
    '1' as data_type,
    apm.rec_code,
    a.drug_prsn_id
from
    iniOpDtl as a
        left join Apmas as apm on a.data_id = apm.data_id and a.fee_ym = apm.fee_ym
        left join DeduData as dd on a.data_id = dd.data_id and a.fee_ym = dd.fee_ym
where a.fee_ym between @strFeeYMS and @strFeeYME and a.hospid = (case when @strHospID = '' then a.hospid else @strHospID end)
  --and substring(a.func_date,1,6) between @strFeeYMS and @strFeeYME 

    insert dedu
select
    a.data_id,
    a.fee_ym,
    a.hospid,
    a.seq_no,
    a.id,
    a.birthday,
    a.func_date,
    a.MedApply,
    a.InstructApply,
    a.TraceApply,
    a.ReleaseApply,
    a.appl_date,
    a.prsn_id,
    a.appl_dot,
    a.part_code,
    a.part_amt,
    a.drug_days,
    a.dsvc_code,
    a.dsvc_dot,
    a.drug_dot,
    0 as dedudot,
    '' as deducode1,
    '' as deducode2,
    '' as deducode3,
    '' as deducode4,
    '' as deducode5,
    '' as deducode6,
    '' as deducode7,
    '' as deducode8,
    '' as deducode9,
    '' as deducode10,
    '' as deducode11,
    '' as deducode12,
    '' as deducode13,
    '' as real_hosp_id,
    a.orig_hosp_id,
    a.area_service,
    0 as partdedudot,
    dd.c_dedu_dot,
    0 as c_dsvc_dot,
    0 as c_drug_dot,
    0 as c_appl_dot,
    0 as c_part_amt,
    pbc.pay_amt,
    a.icd9cm_code,
    a.icd9cm_code1,
    a.icd9cm_code2,
    a.icd10cm_code3,
    a.icd10cm_code4,
    '2' as data_type,
    pbc.rec_code,
    a.drug_prsn_id
from
    iniDrDtl as a
        left join PbctDr as pbc on a.data_id = pbc.data_id and a.fee_ym = pbc.fee_ym
        left join DeduData as dd on a.data_id = dd.data_id and a.fee_ym = dd.fee_ym
where a.fee_ym between @strFeeYMS and @strFeeYME and a.hospid = (case when @strHospID = '' then a.hospid else @strHospID end)
  --and substring(a.func_date,1,6) between @strFeeYMS and @strFeeYME 

--Insert DeduOrder
    insert DeduOrder
select
    a.data_id,
    a.fee_ym,
    b.hospid,
    a.order_seq_no,
    a.order_dot,
    a.order_type,
    a.order_code,
    a.exe_prsn_id,
    0 as c_other_dot,
    0 as c_order_dot,
    a.order_qty,
    a.order_drug_day
from iniOpOrd a
         join iniOpDtl b on a.fee_ym = b.fee_ym and a.data_id = b.data_id
where a.fee_ym between @strFeeYMS and @strFeeYME and b.hospid = (case when @strHospID = '' then b.hospid else @strHospID end)

    insert DeduOrder
select
    a.data_id,
    a.fee_ym,
    b.hospid,
    a.order_seq_no,
    a.order_dot,
    a.order_type,
    a.order_code,
    a.exe_prsn_id,
    0 as c_other_dot,
    0 as c_order_dot,
    a.order_qty,
    a.order_drug_day
from iniDrOrd a
         join iniDrDtl b on a.fee_ym = b.fee_ym and a.data_id = b.data_id
where a.fee_ym between @strFeeYMS and @strFeeYME and b.hospid = (case when @strHospID = '' then b.hospid else @strHospID end)

--審查一：非合約機構 --appl_dot + part_amt
update Dedu set
                Dedu.deducode1='1',
                Dedu.c_appl_dot=f.appl_dot,
                Dedu.c_part_amt=f.part_amt
    from
	(select distinct a.fee_ym,a.data_id,a.appl_dot,a.part_amt
	   from (select * from Dedu where HospID <> '0101090517') a
	   join DeduOrder b on a.data_id = b.data_id and a.fee_ym = b.fee_ym
      where (b.order_code <> 'E1008C' and b.order_type <> '4' and (a.data_type = '1' or (a.data_type = '2' and a.orig_hosp_id = 'N')))
        and a.fee_ym + a.data_id not in 
			  (select a.fee_ym + a.data_id
				 from (select * from Dedu where hospid <> '0101090517') a
				 join (select hospid,hospstartdate,isnull((case when rtrim(hospenddate) = '' then null else rtrim(hospenddate) end),'20991231') as hospenddate 
				         from HospContract 
						where smkcontracttype = '01'
						  and HospID <> '0101090517') b on a.hospid = b.hospid
				where a.func_date between b.hospstartdate and b.hospenddate)
		union
		select distinct a.fee_ym,a.data_id,a.appl_dot,a.part_amt
				from (select * from Dedu where HospID = '0101090517') a
				join DeduOrder b on a.data_id = b.data_id and a.fee_ym = b.fee_ym
				where (b.order_code <> 'E1008C' and b.order_type <> '4' and (a.data_type = '1' or (a.data_type = '2' and a.orig_hosp_id = 'N')))
				and a.fee_ym + a.data_id not in 
						(select a.fee_ym + a.data_id
							from (select fee_ym,data_id,hospid,func_date,
											case when seq_no >= 100000 then '0' + substring(convert(varchar(8),seq_no),1,1) else '10' end as seq_no
									from Dedu where hospid = '0101090517') a
							join (select hospid,hospseqno,hospstartdate,isnull((case when rtrim(hospenddate) = '' then null else rtrim(hospenddate) end),'20991231') as hospenddate 
									from HospContract 
								where smkcontracttype = '01'
									and HospID = '0101090517') b on a.hospid = b.hospid and a.seq_no=b.hospseqno
						where a.func_date between b.hospstartdate and b.hospenddate)) f
where Dedu.fee_ym = f.fee_ym and Dedu.data_id = f.data_id


--審查二：非合約醫事人員
/*
院所
1.用藥申報MedApply=1，prsn_id不在該機構合約期間或CouldTreat<>1，就要扣[drug_dot]+[dsvc_dot]及E1006C,E1007C [order_dot]
2.衛教申報InstructApply=1，prsn_id,drug_prsn_id,exe_prsn_id皆不在該機構合約期間或三者皆CouldInstruct<>1，判斷時不看order_code，就要扣order_code=E1022C的order_dot
 
藥局orig_hosp_id=N
1.用藥申報MedApply=1，drug_prsn_id不在該機構合約期間或CouldTreat<>1，就要扣 [drug_dot]+[dsvc_dot]
2.衛教申報InstructApply=1，drug_prsn_id,exe_prsn_id皆不在該機構合約期間或CouldInstruct<>1，判斷時不看order_code，就要扣order_code=E1022C的order_dot
*/
--院所 用藥申報 (dsvc_dot + drug_dot)
update Dedu set
                Dedu.deducode2='1',
                Dedu.c_dsvc_dot=f.dsvc_dot,
                Dedu.c_drug_dot=f.drug_dot
    from (select distinct a.fee_ym,
								a.data_id,
								a.dsvc_dot,
								a.drug_dot,
								b.order_seq_no,
								b.order_dot
			  from Dedu a
			  join DeduOrder b
				on a.data_id = b.data_id
			   and a.fee_ym = b.fee_ym
			  left join (select hospid,
								prsnid,
								isnull((case
										 when rtrim(prsnstartdate) = '' then
										  null
										 else
										  rtrim(prsnstartdate)
									   end),
									   '20991231') as prsnstartdate,
								isnull((case
										 when rtrim(prsnenddate) = '' then
										  null
										 else
										  rtrim(prsnenddate)
									   end),
									   '20991231') as prsnenddate
						   from PrsnContract where couldtreat='1') c
				on a.prsn_id = c.prsnid
			   and a.hospid = (case
										 when c.hospid = '1131100010' then
										  '1101100011'
										 when c.hospid = '1101010012' then
										  '1132070011'
										 else
										  c.hospid
									   end)
			   and a.func_date between c.prsnstartdate and c.prsnenddate
			 where a.medapply = '1'
			   and a.data_type = '1'
			   and c.prsnid is null
) f where Dedu.fee_ym = f.fee_ym and Dedu.data_id = f.data_id

--院所 用藥申報 (order_dot)
update DeduOrder set
    DeduOrder.c_order_dot=f.order_dot
    from (select distinct a.fee_ym,
								a.data_id,
								a.dsvc_dot,
								a.drug_dot,
								b.order_seq_no,
								b.order_dot
			  from Dedu a
			  join DeduOrder b
				on a.data_id = b.data_id
			   and a.fee_ym = b.fee_ym
			  left join (select hospid,
								prsnid,
								isnull((case
										 when rtrim(prsnstartdate) = '' then
										  null
										 else
										  rtrim(prsnstartdate)
									   end),
									   '20991231') as prsnstartdate,
								isnull((case
										 when rtrim(prsnenddate) = '' then
										  null
										 else
										  rtrim(prsnenddate)
									   end),
									   '20991231') as prsnenddate
						   from PrsnContract where couldtreat='1') c
				on a.prsn_id = c.prsnid
			   and a.hospid = (case
										 when c.hospid = '1131100010' then
										  '1101100011'
										 when c.hospid = '1101010012' then
										  '1132070011'
										 else
										  c.hospid
									   end)
			   and a.func_date between c.prsnstartdate and c.prsnenddate
			 where a.medapply = '1'
			   and a.data_type = '1'
			   and b.order_code in ('E1006C','E1007C')
			   and c.prsnid is null
) f where DeduOrder.fee_ym = f.fee_ym and DeduOrder.data_id = f.data_id and DeduOrder.order_seq_no = f.order_seq_no

--院所 衛教申報
update Dedu set
    Dedu.deducode2='1'
    from (select distinct a.fee_ym,
								a.data_id,
								a.dsvc_dot,
								a.drug_dot,
								b.order_seq_no,
								b.order_dot
				  from Dedu a
				  join DeduOrder b
					on a.data_id = b.data_id
				   and a.fee_ym = b.fee_ym
				  left join (select hospid,
									prsnid,
									isnull((case
										 when rtrim(prsnstartdate) = '' then
										  null
										 else
										  rtrim(prsnstartdate)
									   end),
									   '20991231') as prsnstartdate,
									isnull((case
											 when rtrim(prsnenddate) = '' then
											  null
											 else
											  rtrim(prsnenddate)
										   end),
										   '20991231') as prsnenddate
							   from PrsnContract
							  where couldinstruct = '1') c
					on a.prsn_id = c.prsnid
				   and a.hospid = (case
						 when c.hospid = '1131100010' then
						  '1101100011'
						 when c.hospid = '1101010012' then
						  '1132070011'
						 else
						  c.hospid
					   end)
				   and a.func_date between c.prsnstartdate and c.prsnenddate
				  left join (select hospid,
									prsnid,
									isnull((case
										 when rtrim(prsnstartdate) = '' then
										  null
										 else
										  rtrim(prsnstartdate)
									   end),
									   '20991231') as prsnstartdate,
									isnull((case
											 when rtrim(prsnenddate) = '' then
											  null
											 else
											  rtrim(prsnenddate)
										   end),
										   '20991231') as prsnenddate
							   from PrsnContract
							  where couldinstruct = '1') e
					on a.drug_prsn_id = e.prsnid
				   and a.hospid = (case
						 when e.hospid = '1131100010' then
						  '1101100011'
						 when e.hospid = '1101010012' then
						  '1132070011'
						 else
						  e.hospid
					   end)
				   and a.func_date between e.prsnstartdate and e.prsnenddate
				  left join (select distinct a.fee_ym, a.data_id
							   from Dedu a
							   join DeduOrder b
								 on a.data_id = b.data_id
								and a.fee_ym = b.fee_ym
							   left join (select hospid,
												prsnid,
												isnull((case
													 when rtrim(prsnstartdate) = '' then
													  null
													 else
													  rtrim(prsnstartdate)
												   end),
												   '20991231') as prsnstartdate,
												isnull((case
														 when rtrim(prsnenddate) = '' then
														  null
														 else
														  rtrim(prsnenddate)
													   end),
													   '20991231') as prsnenddate
										   from PrsnContract
										  where couldinstruct = '1') c
								 on b.exe_prsn_id = c.prsnid
								and a.hospid = (case
									  when c.hospid = '1131100010' then
									   '1101100011'
									  when c.hospid = '1101010012' then
									   '1132070011'
									  else
									   c.hospid
									end)
								and a.func_date between c.prsnstartdate and c.prsnenddate
							  where c.prsnid is not null) d
					on a.data_id = d.data_id
				   and a.fee_ym = d.fee_ym
				 where a.InstructApply = '1'
				   and a.data_type = '1'
				   and b.order_code = 'E1022C'
				   and c.prsnid is null
				   and e.prsnid is null
				   and d.data_id is null
) f where Dedu.fee_ym = f.fee_ym and Dedu.data_id = f.data_id

--院所 衛教申報 (order_dot)
update DeduOrder set
    DeduOrder.c_order_dot=f.order_dot
    from (select distinct a.fee_ym,
								a.data_id,
								a.dsvc_dot,
								a.drug_dot,
								b.order_seq_no,
								b.order_dot
				  from Dedu a
				  join DeduOrder b
					on a.data_id = b.data_id
				   and a.fee_ym = b.fee_ym
				  left join (select hospid,
									prsnid,
									isnull((case
										 when rtrim(prsnstartdate) = '' then
										  null
										 else
										  rtrim(prsnstartdate)
									   end),
									   '20991231') as prsnstartdate,
									isnull((case
											 when rtrim(prsnenddate) = '' then
											  null
											 else
											  rtrim(prsnenddate)
										   end),
										   '20991231') as prsnenddate
							   from PrsnContract
							  where couldinstruct = '1') c
					on a.prsn_id = c.prsnid
				   and a.hospid = (case
						 when c.hospid = '1131100010' then
						  '1101100011'
						 when c.hospid = '1101010012' then
						  '1132070011'
						 else
						  c.hospid
					   end)
				   and a.func_date between c.PrsnStartDate and c.prsnenddate
				  left join (select hospid,
									prsnid,
									isnull((case
										 when rtrim(prsnstartdate) = '' then
										  null
										 else
										  rtrim(prsnstartdate)
									   end),
									   '20991231') as prsnstartdate,
									isnull((case
											 when rtrim(prsnenddate) = '' then
											  null
											 else
											  rtrim(prsnenddate)
										   end),
										   '20991231') as prsnenddate
							   from PrsnContract
							  where couldinstruct = '1') e
					on a.drug_prsn_id = e.prsnid
				   and a.hospid = (case
						 when e.hospid = '1131100010' then
						  '1101100011'
						 when e.hospid = '1101010012' then
						  '1132070011'
						 else
						  e.hospid
					   end)
				   and a.func_date between e.prsnstartdate and e.prsnenddate
				   left join (select distinct a.fee_ym, a.data_id
							   from Dedu a
							   join DeduOrder b
								 on a.data_id = b.data_id
								and a.fee_ym = b.fee_ym
							   left join (select hospid,
												prsnid,
												isnull((case
													 when rtrim(prsnstartdate) = '' then
													  null
													 else
													  rtrim(prsnstartdate)
												   end),
												   '20991231') as prsnstartdate,
												isnull((case
														 when rtrim(prsnenddate) = '' then
														  null
														 else
														  rtrim(prsnenddate)
													   end),
													   '20991231') as prsnenddate
										   from PrsnContract
										  where couldinstruct = '1') c
								 on b.exe_prsn_id = c.prsnid
								and a.hospid = (case
									  when c.hospid = '1131100010' then
									   '1101100011'
									  when c.hospid = '1101010012' then
									   '1132070011'
									  else
									   c.hospid
									end)
								and a.func_date between c.prsnstartdate and c.prsnenddate
							  where c.prsnid is not null) d
					on a.data_id = d.data_id
				   and a.fee_ym = d.fee_ym
				 where a.InstructApply = '1'
				   and a.data_type = '1'
				   and b.order_code = 'E1022C'
				   and c.prsnid is null
				   and e.prsnid is null
				   and d.data_id is null
) f where DeduOrder.fee_ym = f.fee_ym and DeduOrder.data_id = f.data_id and DeduOrder.order_seq_no = f.order_seq_no

--藥局 用藥申報 (dsvc_dot + drug_dot)
update Dedu set
                Dedu.deducode2='1',
                Dedu.c_dsvc_dot=f.dsvc_dot,
                Dedu.c_drug_dot=f.drug_dot
    from (select distinct a.fee_ym,
								a.data_id,
								a.dsvc_dot,
								a.drug_dot,
								b.order_seq_no,
								b.order_dot
			  from Dedu a
			  join DeduOrder b
				on a.data_id = b.data_id
			   and a.fee_ym = b.fee_ym
			  left join (select hospid,
								prsnid,
								isnull((case
										 when rtrim(prsnstartdate) = '' then
										  null
										 else
										  rtrim(prsnstartdate)
									   end),
									   '20991231') as prsnstartdate,
								isnull((case
										 when rtrim(prsnenddate) = '' then
										  null
										 else
										  rtrim(prsnenddate)
									   end),
									   '20991231') as prsnenddate
						   from PrsnContract where couldtreat='1') c
				on a.drug_prsn_id = c.prsnid
			   and a.hospid = (case
										 when c.hospid = '1131100010' then
										  '1101100011'
										 when c.hospid = '1101010012' then
										  '1132070011'
										 else
										  c.hospid
									   end)
			   and a.func_date between c.prsnstartdate and c.prsnenddate
			 where a.medapply = '1'
			   and a.data_type = '2' and a.orig_hosp_id = 'N'
			   and c.prsnid is null
) f where Dedu.fee_ym = f.fee_ym and Dedu.data_id = f.data_id

--藥局 衛教申報
update Dedu set
    Dedu.deducode2='1'
    from (select distinct a.fee_ym,
								a.data_id,
								a.dsvc_dot,
								a.drug_dot,
								b.order_seq_no,
								b.order_dot
				  from Dedu a
				  join DeduOrder b
					on a.data_id = b.data_id
				   and a.fee_ym = b.fee_ym
				  left join (select hospid,
									prsnid,
									isnull((case
										 when rtrim(prsnstartdate) = '' then
										  null
										 else
										  rtrim(prsnstartdate)
									   end),
									   '20991231') as prsnstartdate,
									isnull((case
											 when rtrim(prsnenddate) = '' then
											  null
											 else
											  rtrim(prsnenddate)
										   end),
										   '20991231') as prsnenddate
							   from PrsnContract where couldinstruct='1') e
					on a.drug_prsn_id = e.prsnid
				   and a.hospid = (case
										 when e.hospid = '1131100010' then
										  '1101100011'
										 when e.hospid = '1101010012' then
										  '1132070011'
										 else
										  e.hospid
									   end)
					and a.func_date between e.prsnstartdate and e.prsnenddate
					left join (select distinct a.fee_ym, a.data_id
							   from Dedu a
							   join DeduOrder b
								 on a.data_id = b.data_id
								and a.fee_ym = b.fee_ym
							   left join (select hospid,
												prsnid,
												isnull((case
													 when rtrim(prsnstartdate) = '' then
													  null
													 else
													  rtrim(prsnstartdate)
												   end),
												   '20991231') as prsnstartdate,
												isnull((case
														 when rtrim(prsnenddate) = '' then
														  null
														 else
														  rtrim(prsnenddate)
													   end),
													   '20991231') as prsnenddate
										   from PrsnContract
										  where couldinstruct = '1') c
								 on b.exe_prsn_id = c.prsnid
								and a.hospid = (case
									  when c.hospid = '1131100010' then
									   '1101100011'
									  when c.hospid = '1101010012' then
									   '1132070011'
									  else
									   c.hospid
									end)
								and a.func_date between c.prsnStartDate and c.prsnenddate
							  where c.prsnid is not null) d
					on a.data_id = d.data_id
				   and a.fee_ym = d.fee_ym
				 where a.InstructApply = '1'
				   and a.data_type = '2' and a.orig_hosp_id = 'N'
				   and b.order_code = 'E1022C'
				   and e.prsnid is null
				   and d.data_id is null
) f where Dedu.fee_ym = f.fee_ym and Dedu.data_id = f.data_id

--藥局 衛教申報 (order_dot)
update DeduOrder set
    DeduOrder.c_order_dot=f.order_dot
    from (select distinct a.fee_ym,
								a.data_id,
								a.dsvc_dot,
								a.drug_dot,
								b.order_seq_no,
								b.order_dot
				  from Dedu a
				  join DeduOrder b
					on a.data_id = b.data_id
				   and a.fee_ym = b.fee_ym
				  left join (select hospid,
									prsnid,
									isnull((case
										 when rtrim(prsnstartdate) = '' then
										  null
										 else
										  rtrim(prsnstartdate)
									   end),
									   '20991231') as prsnstartdate,
									isnull((case
											 when rtrim(prsnenddate) = '' then
											  null
											 else
											  rtrim(prsnenddate)
										   end),
										   '20991231') as prsnenddate
							   from PrsnContract where couldinstruct='1') e
					on a.drug_prsn_id = e.prsnid
				   and a.hospid = (case
										 when e.hospid = '1131100010' then
										  '1101100011'
										 when e.hospid = '1101010012' then
										  '1132070011'
										 else
										  e.hospid
									   end)
					and a.func_date between e.prsnstartdate and e.prsnenddate
					left join (select distinct a.fee_ym, a.data_id
							   from Dedu a
							   join DeduOrder b
								 on a.data_id = b.data_id
								and a.fee_ym = b.fee_ym
							   left join (select hospid,
												prsnid,
												isnull((case
													 when rtrim(prsnstartdate) = '' then
													  null
													 else
													  rtrim(prsnstartdate)
												   end),
												   '20991231') as prsnstartdate,
												isnull((case
														 when rtrim(prsnenddate) = '' then
														  null
														 else
														  rtrim(prsnenddate)
													   end),
													   '20991231') as prsnenddate
										   from PrsnContract
										  where couldinstruct = '1') c
								 on b.exe_prsn_id = c.prsnid
								and a.hospid = (case
									  when c.hospid = '1131100010' then
									   '1101100011'
									  when c.hospid = '1101010012' then
									   '1132070011'
									  else
									   c.hospid
									end)
								and a.func_date between c.prsnStartDate and c.prsnenddate
							  where c.prsnid is not null) d
					on a.data_id = d.data_id
				   and a.fee_ym = d.fee_ym
				 where a.InstructApply = '1'
				   and a.data_type = '2' and a.orig_hosp_id = 'N'
				   and b.order_code = 'E1022C'
				   and e.prsnid is null
				   and d.data_id is null
) f where DeduOrder.fee_ym = f.fee_ym and DeduOrder.data_id = f.data_id and DeduOrder.order_seq_no = f.order_seq_no


--審查四：主次診斷不符 --appl_dot + part_amt
update Dedu set
                Dedu.deducode4='1',
                Dedu.c_appl_dot=f.appl_dot,
                Dedu.c_part_amt=f.part_amt
    from
	(select distinct a.fee_ym, a.data_id, a.appl_dot, a.part_amt
		  from Dedu a
		 where ltrim(rtrim(a.icd9cm_code)) <> 'F17200'
		   and ltrim(rtrim(a.icd9cm_code1)) <> 'F17200'
		   and ltrim(rtrim(a.icd9cm_code2)) <> 'F17200'
		   and ltrim(rtrim(a.icd10cm_code3)) <> 'F17200'
		   and ltrim(rtrim(a.icd10cm_code4)) <> 'F17200') f
where Dedu.fee_ym = f.fee_ym and Dedu.data_id = f.data_id


--審查五：VPN沒登錄
/*
院所，藥局orig_hosp_id=N
1.用藥申報MedApply=1，HospID+id+birthday+func_date 對不到dbo_MhbtQsData!Cure_Type=1，HospID+ID+Birthday+FuncDate，就要扣 [drug_dot]+[dsvc_dot]及E1006C,E1007C [order_dot]
2.衛教申報InstructApply=1，HospID+id+birthday+func_date 對不到dbo_MhbtQsData!Cure_Type=2，HospID+ID+Birthday+FuncDate，就要扣order_code=E1022C的order_dot

3.追蹤申報TraceApply in (2,3,4,5)
          order_code=E1023C 對不到 MhbtQsData!Cure_Type=1，HospID+ID+Birthday+TraceDate，就要扣order_code=E1023C的order_dot
          order_code=E1024C 對不到 MhbtQsData!Cure_Type=1，HospID+ID+Birthday+Trace_Date2，就要扣order_code=E1024C的order_dot
          order_code=E1025C 對不到 MhbtQsData!Cure_Type=2，HospID+ID+Birthday+Trace_Date，就要扣order_code=E1025C的order_dot
          order_code=E1026C 對不到 MhbtQsData!Cure_Type=2，HospID+ID+Birthday+Trace_Date2，就要扣order_code=E1026C的order_dot
*/
--用藥申報 (dsvc_dot + drug_dot)
update Dedu set
                Dedu.deducode5='1',
                Dedu.c_dsvc_dot=f.dsvc_dot,
                Dedu.c_drug_dot=f.drug_dot
    from (select distinct a.fee_ym, a.data_id, a.dsvc_dot, a.drug_dot
			  from Dedu a
			  join DeduOrder b
				on a.fee_ym = b.fee_ym
			   and a.data_id = b.data_id
			  join (select * from HospBasic where hospseqno = '00') h
				on a.hospid = h.hospid
			  left join (select * from MhbtQsData where cure_type = '1') c
				on (a.hospid = (case
					 when c.hospid = '1131100010' then
					  '1101100011'
					 when c.hospid = '1101010012' then
					  '1132070011'
					 else
					  c.hospid
				   end) or a.orig_hosp_id = (case
					 when c.hospid = '1131100010' then
					  '1101100011'
					 when c.hospid = '1101010012' then
					  '1132070011'
					 else
					  c.hospid
				   end) or h.lasthospid = (case
					 when c.hospid = '1131100010' then
					  '1101100011'
					 when c.hospid = '1101010012' then
					  '1132070011'
					 else
					  c.hospid
				   end))
			   and a.id = c.id
			   and a.birthday = c.birthday
			   and a.func_date = c.funcdate
			 where a.medapply = '1'
			   and (a.data_type = '1' or (a.data_type = '2' and a.orig_hosp_id = 'N'))
			   and c.hospid is null
   ) f where Dedu.fee_ym = f.fee_ym and Dedu.data_id = f.data_id

--用藥申報 (order_dot)
update DeduOrder set
    DeduOrder.c_order_dot=f.order_dot
    from (select distinct a.fee_ym, a.data_id, b.order_seq_no, b.order_dot
			  from Dedu a
			  join DeduOrder b
				on a.fee_ym = b.fee_ym
			   and a.data_id = b.data_id
			  join (select * from HospBasic where hospseqno = '00') h
				on a.hospid = h.hospid
			  left join (select * from MhbtQsData where cure_type = '1') c
				on (a.hospid = (case
					 when c.hospid = '1131100010' then
					  '1101100011'
					 when c.hospid = '1101010012' then
					  '1132070011'
					 else
					  c.hospid
				   end) or a.orig_hosp_id = (case
					 when c.hospid = '1131100010' then
					  '1101100011'
					 when c.hospid = '1101010012' then
					  '1132070011'
					 else
					  c.hospid
				   end) or h.lasthospid = (case
					 when c.hospid = '1131100010' then
					  '1101100011'
					 when c.hospid = '1101010012' then
					  '1132070011'
					 else
					  c.hospid
				   end))
			   and a.id = c.id
			   and a.birthday = c.birthday
			   and a.func_date = c.funcdate
			 where a.medapply = '1'
			   and (a.data_type = '1' or (a.data_type = '2' and a.orig_hosp_id = 'N'))
			   and b.order_code in ('E1006C','E1007C')
			   and c.hospid is null
   ) f where DeduOrder.fee_ym = f.fee_ym and DeduOrder.data_id = f.data_id and DeduOrder.order_seq_no = f.order_seq_no

--衛教申報
update Dedu set
    Dedu.deducode5='1'
    from (select distinct a.fee_ym, a.data_id
			  from Dedu a
			  join DeduOrder b
				on a.fee_ym = b.fee_ym
			   and a.data_id = b.data_id
			  join (select * from HospBasic where hospseqno = '00') h
				on a.hospid = h.hospid
			  left join (select * from MhbtQsData where cure_type = '2') c
				on (a.hospid = (case
					 when c.hospid = '1131100010' then
					  '1101100011'
					 when c.hospid = '1101010012' then
					  '1132070011'
					 else
					  c.hospid
				   end) or a.orig_hosp_id = (case
					 when c.hospid = '1131100010' then
					  '1101100011'
					 when c.hospid = '1101010012' then
					  '1132070011'
					 else
					  c.hospid
				   end) or h.lasthospid = (case
					 when c.hospid = '1131100010' then
					  '1101100011'
					 when c.hospid = '1101010012' then
					  '1132070011'
					 else
					  c.hospid
				   end))
			   and a.id = c.id
			   and a.birthday = c.birthday
			   and a.func_date = c.funcdate
			 where a.InstructApply = '1'
			   and (a.data_type = '1' or (a.data_type = '2' and a.orig_hosp_id = 'N'))
			   and b.order_code in ('E1022C')
			   and c.hospid is null
   ) f where Dedu.fee_ym = f.fee_ym and Dedu.data_id = f.data_id

--衛教申報 (order_dot)
update DeduOrder set
    DeduOrder.c_order_dot=f.order_dot
    from (select distinct a.fee_ym, a.data_id, b.order_seq_no, b.order_dot
			  from Dedu a
			  join DeduOrder b
				on a.fee_ym = b.fee_ym
			   and a.data_id = b.data_id
			  join (select * from HospBasic where hospseqno = '00') h
				on a.hospid = h.hospid
			  left join (select * from MhbtQsData where cure_type = '2') c
				on (a.hospid = (case
					 when c.hospid = '1131100010' then
					  '1101100011'
					 when c.hospid = '1101010012' then
					  '1132070011'
					 else
					  c.hospid
				   end) or a.orig_hosp_id = (case
					 when c.hospid = '1131100010' then
					  '1101100011'
					 when c.hospid = '1101010012' then
					  '1132070011'
					 else
					  c.hospid
				   end) or h.lasthospid = (case
					 when c.hospid = '1131100010' then
					  '1101100011'
					 when c.hospid = '1101010012' then
					  '1132070011'
					 else
					  c.hospid
				   end))
			   and a.id = c.id
			   and a.birthday = c.birthday
			   and a.func_date = c.funcdate
			 where a.InstructApply = '1'
			   and (a.data_type = '1' or (a.data_type = '2' and a.orig_hosp_id = 'N'))
			   and b.order_code in ('E1022C')
			   and c.hospid is null
   ) f where DeduOrder.fee_ym = f.fee_ym and DeduOrder.data_id = f.data_id and DeduOrder.order_seq_no = f.order_seq_no

--追蹤申報 
--(cure_type = '1', order_code='E1023C', tracedate)
update Dedu set
    Dedu.deducode5='1'
    from (select distinct a.fee_ym, a.data_id, b.order_seq_no, b.order_dot
			  from Dedu a
			  join DeduOrder b
				on a.fee_ym = b.fee_ym
			   and a.data_id = b.data_id
			  join (select * from HospBasic where hospseqno = '00') h
				on a.hospid = h.hospid
			  left join (select * from MhbtQsData where cure_type = '1') c
				on (a.hospid = (case
					 when c.hospid = '1131100010' then
					  '1101100011'
					 when c.hospid = '1101010012' then
					  '1132070011'
					 else
					  c.hospid
				   end) or a.orig_hosp_id = (case
					 when c.hospid = '1131100010' then
					  '1101100011'
					 when c.hospid = '1101010012' then
					  '1132070011'
					 else
					  c.hospid
				   end) or h.lasthospid = (case
					 when c.hospid = '1131100010' then
					  '1101100011'
					 when c.hospid = '1101010012' then
					  '1132070011'
					 else
					  c.hospid
				   end))
			   and a.id = c.id
			   and a.birthday = c.birthday
			   and a.func_date = c.tracedate
			 where a.TraceApply in ('2','3','4','5')
			   and (a.data_type = '1' or (a.data_type = '2' and a.orig_hosp_id = 'N'))
			   and b.order_code in ('E1023C')
			   and c.hospid is null
   ) f where Dedu.fee_ym = f.fee_ym and Dedu.data_id = f.data_id

update DeduOrder set
    DeduOrder.c_order_dot=f.order_dot
    from (select distinct a.fee_ym, a.data_id, b.order_seq_no, b.order_dot
			  from Dedu a
			  join DeduOrder b
				on a.fee_ym = b.fee_ym
			   and a.data_id = b.data_id
			  join (select * from HospBasic where hospseqno = '00') h
				on a.hospid = h.hospid
			  left join (select * from MhbtQsData where cure_type = '1') c
				on (a.hospid = (case
					 when c.hospid = '1131100010' then
					  '1101100011'
					 when c.hospid = '1101010012' then
					  '1132070011'
					 else
					  c.hospid
				   end) or a.orig_hosp_id = (case
					 when c.hospid = '1131100010' then
					  '1101100011'
					 when c.hospid = '1101010012' then
					  '1132070011'
					 else
					  c.hospid
				   end) or h.lasthospid = (case
					 when c.hospid = '1131100010' then
					  '1101100011'
					 when c.hospid = '1101010012' then
					  '1132070011'
					 else
					  c.hospid
				   end))
			   and a.id = c.id
			   and a.birthday = c.birthday
			   and a.func_date = c.tracedate
			 where a.TraceApply in ('2','3','4','5')
			   and (a.data_type = '1' or (a.data_type = '2' and a.orig_hosp_id = 'N'))
			   and b.order_code in ('E1023C')
			   and c.hospid is null
   ) f where DeduOrder.fee_ym = f.fee_ym and DeduOrder.data_id = f.data_id and DeduOrder.order_seq_no = f.order_seq_no

--(cure_type = '1', order_code='E1024C', trace_date2)
update Dedu set
    Dedu.deducode5='1'
    from (select distinct a.fee_ym, a.data_id, b.order_seq_no, b.order_dot
			  from Dedu a
			  join DeduOrder b
				on a.fee_ym = b.fee_ym
			   and a.data_id = b.data_id
			  join (select * from HospBasic where hospseqno = '00') h
				on a.hospid = h.hospid
			  left join (select * from MhbtQsData where cure_type = '1') c
				on (a.hospid = (case
					 when c.hospid = '1131100010' then
					  '1101100011'
					 when c.hospid = '1101010012' then
					  '1132070011'
					 else
					  c.hospid
				   end) or a.orig_hosp_id = (case
					 when c.hospid = '1131100010' then
					  '1101100011'
					 when c.hospid = '1101010012' then
					  '1132070011'
					 else
					  c.hospid
				   end) or h.lasthospid = (case
					 when c.hospid = '1131100010' then
					  '1101100011'
					 when c.hospid = '1101010012' then
					  '1132070011'
					 else
					  c.hospid
				   end))
			   and a.id = c.id
			   and a.birthday = c.birthday
			   and a.func_date = c.trace_date2
			 where a.TraceApply in ('2','3','4','5')
			   and (a.data_type = '1' or (a.data_type = '2' and a.orig_hosp_id = 'N'))
			   and b.order_code in ('E1024C')
			   and c.hospid is null
   ) f where Dedu.fee_ym = f.fee_ym and Dedu.data_id = f.data_id

update DeduOrder set
    DeduOrder.c_order_dot=f.order_dot
    from (select distinct a.fee_ym, a.data_id, b.order_seq_no, b.order_dot
			  from Dedu a
			  join DeduOrder b
				on a.fee_ym = b.fee_ym
			   and a.data_id = b.data_id
			  join (select * from HospBasic where hospseqno = '00') h
				on a.hospid = h.hospid
			  left join (select * from MhbtQsData where cure_type = '1') c
				on (a.hospid = (case
					 when c.hospid = '1131100010' then
					  '1101100011'
					 when c.hospid = '1101010012' then
					  '1132070011'
					 else
					  c.hospid
				   end) or a.orig_hosp_id = (case
					 when c.hospid = '1131100010' then
					  '1101100011'
					 when c.hospid = '1101010012' then
					  '1132070011'
					 else
					  c.hospid
				   end) or h.lasthospid = (case
					 when c.hospid = '1131100010' then
					  '1101100011'
					 when c.hospid = '1101010012' then
					  '1132070011'
					 else
					  c.hospid
				   end))
			   and a.id = c.id
			   and a.birthday = c.birthday
			   and a.func_date = c.trace_date2
			 where a.TraceApply in ('2','3','4','5')
			   and (a.data_type = '1' or (a.data_type = '2' and a.orig_hosp_id = 'N'))
			   and b.order_code in ('E1024C')
			   and c.hospid is null
   ) f where DeduOrder.fee_ym = f.fee_ym and DeduOrder.data_id = f.data_id and DeduOrder.order_seq_no = f.order_seq_no

--(cure_type = '2', order_code='E1025C', tracedate)
update Dedu set
    Dedu.deducode5='1'
    from (select distinct a.fee_ym, a.data_id, b.order_seq_no, b.order_dot
			  from Dedu a
			  join DeduOrder b
				on a.fee_ym = b.fee_ym
			   and a.data_id = b.data_id
			  join (select * from HospBasic where hospseqno = '00') h
				on a.hospid = h.hospid
			  left join (select * from MhbtQsData where cure_type = '2') c
				on (a.hospid = (case
					 when c.hospid = '1131100010' then
					  '1101100011'
					 when c.hospid = '1101010012' then
					  '1132070011'
					 else
					  c.hospid
				   end) or a.orig_hosp_id = (case
					 when c.hospid = '1131100010' then
					  '1101100011'
					 when c.hospid = '1101010012' then
					  '1132070011'
					 else
					  c.hospid
				   end) or h.lasthospid = (case
					 when c.hospid = '1131100010' then
					  '1101100011'
					 when c.hospid = '1101010012' then
					  '1132070011'
					 else
					  c.hospid
				   end))
			   and a.id = c.id
			   and a.birthday = c.birthday
			   and a.func_date = c.tracedate
			 where a.TraceApply in ('2','3','4','5')
			   and (a.data_type = '1' or (a.data_type = '2' and a.orig_hosp_id = 'N'))
			   and b.order_code in ('E1025C')
			   and c.hospid is null
   ) f where Dedu.fee_ym = f.fee_ym and Dedu.data_id = f.data_id

update DeduOrder set
    DeduOrder.c_order_dot=f.order_dot
    from (select distinct a.fee_ym, a.data_id, b.order_seq_no, b.order_dot
			  from Dedu a
			  join DeduOrder b
				on a.fee_ym = b.fee_ym
			   and a.data_id = b.data_id
			  join (select * from HospBasic where hospseqno = '00') h
				on a.hospid = h.hospid
			  left join (select * from MhbtQsData where cure_type = '2') c
				on (a.hospid = (case
					 when c.hospid = '1131100010' then
					  '1101100011'
					 when c.hospid = '1101010012' then
					  '1132070011'
					 else
					  c.hospid
				   end) or a.orig_hosp_id = (case
					 when c.hospid = '1131100010' then
					  '1101100011'
					 when c.hospid = '1101010012' then
					  '1132070011'
					 else
					  c.hospid
				   end) or h.lasthospid = (case
					 when c.hospid = '1131100010' then
					  '1101100011'
					 when c.hospid = '1101010012' then
					  '1132070011'
					 else
					  c.hospid
				   end))
			   and a.id = c.id
			   and a.birthday = c.birthday
			   and a.func_date = c.tracedate
			 where a.TraceApply in ('2','3','4','5')
			   and (a.data_type = '1' or (a.data_type = '2' and a.orig_hosp_id = 'N'))
			   and b.order_code in ('E1025C')
			   and c.hospid is null
   ) f where DeduOrder.fee_ym = f.fee_ym and DeduOrder.data_id = f.data_id and DeduOrder.order_seq_no = f.order_seq_no

--(cure_type = '2', order_code='E1026C', trace_date2)
update Dedu set
    Dedu.deducode5='1'
    from (select distinct a.fee_ym, a.data_id, b.order_seq_no, b.order_dot
			  from Dedu a
			  join DeduOrder b
				on a.fee_ym = b.fee_ym
			   and a.data_id = b.data_id
			  join (select * from HospBasic where hospseqno = '00') h
				on a.hospid = h.hospid
			  left join (select * from MhbtQsData where cure_type = '2') c
				on (a.hospid = (case
					 when c.hospid = '1131100010' then
					  '1101100011'
					 when c.hospid = '1101010012' then
					  '1132070011'
					 else
					  c.hospid
				   end) or a.orig_hosp_id = (case
					 when c.hospid = '1131100010' then
					  '1101100011'
					 when c.hospid = '1101010012' then
					  '1132070011'
					 else
					  c.hospid
				   end) or h.lasthospid = (case
					 when c.hospid = '1131100010' then
					  '1101100011'
					 when c.hospid = '1101010012' then
					  '1132070011'
					 else
					  c.hospid
				   end))
			   and a.id = c.id
			   and a.birthday = c.birthday
			   and a.func_date = c.trace_date2
			 where a.TraceApply in ('2','3','4','5')
			   and (a.data_type = '1' or (a.data_type = '2' and a.orig_hosp_id = 'N'))
			   and b.order_code in ('E1026C')
			   and c.hospid is null
   ) f where Dedu.fee_ym = f.fee_ym and Dedu.data_id = f.data_id

update DeduOrder set
    DeduOrder.c_order_dot=f.order_dot
    from (select distinct a.fee_ym, a.data_id, b.order_seq_no, b.order_dot
			  from Dedu a
			  join DeduOrder b
				on a.fee_ym = b.fee_ym
			   and a.data_id = b.data_id
			  join (select * from HospBasic where hospseqno = '00') h
				on a.hospid = h.hospid
			  left join (select * from MhbtQsData where cure_type = '2') c
				on (a.hospid = (case
					 when c.hospid = '1131100010' then
					  '1101100011'
					 when c.hospid = '1101010012' then
					  '1132070011'
					 else
					  c.hospid
				   end) or a.orig_hosp_id = (case
					 when c.hospid = '1131100010' then
					  '1101100011'
					 when c.hospid = '1101010012' then
					  '1132070011'
					 else
					  c.hospid
				   end) or h.lasthospid = (case
					 when c.hospid = '1131100010' then
					  '1101100011'
					 when c.hospid = '1101010012' then
					  '1132070011'
					 else
					  c.hospid
				   end))
			   and a.id = c.id
			   and a.birthday = c.birthday
			   and a.func_date = c.trace_date2
			 where a.TraceApply in ('2','3','4','5')
			   and (a.data_type = '1' or (a.data_type = '2' and a.orig_hosp_id = 'N'))
			   and b.order_code in ('E1026C')
			   and c.hospid is null
   ) f where DeduOrder.fee_ym = f.fee_ym and DeduOrder.data_id = f.data_id and DeduOrder.order_seq_no = f.order_seq_no


--審查六：申報之藥服費代碼與給藥天數不符(含未開藥)
--藥服費不符 other_dot
update Dedu set
    Dedu.deducode6='1'
    from
	(select a1.fee_ym,a1.data_id
	  from (select distinct b.fee_ym, b.data_id
				from Dedu a
				join DeduOrder b
				on a.fee_ym = b.fee_ym
				and a.data_id = b.data_id
				left join (select b.fee_ym, b.data_id
							from Dedu a
							join DeduOrder b
								on a.fee_ym = b.fee_ym
							and a.data_id = b.data_id
							where (a.drug_days > 7 or b.order_drug_day > 7)) c
				on a.fee_ym = c.fee_ym
				and a.data_id = c.data_id
				where b.order_code in
					('E1010D', 'E1012C', 'E1016B', 'E1018A', 'E1020A', 'E1014B')
				and c.fee_ym is null) a1) f
where Dedu.fee_ym = f.fee_ym and Dedu.data_id = f.data_id

update DeduOrder set
    DeduOrder.c_other_dot=f.other_dot
    from
	(select a1.fee_ym, a1.data_id, a1.order_seq_no, a1.other_dot
	  from (select distinct b.fee_ym,
							b.data_id,
							b.order_seq_no,
							case
							  when b.order_code in ('E1012C', 'E1018A', 'E1020A') then
							   11
							  when b.order_code in ('E1010D', 'E1016B', 'E1014B') then
							   10
							end as other_dot
			  from Dedu a
			  join DeduOrder b
				on a.fee_ym = b.fee_ym
			   and a.data_id = b.data_id
			  left join (select b.fee_ym, b.data_id
						  from Dedu a
						  join DeduOrder b
							on a.fee_ym = b.fee_ym
						   and a.data_id = b.data_id
						 where (a.drug_days > 7 or b.order_drug_day > 7)) c
				on a.fee_ym = c.fee_ym
			   and a.data_id = c.data_id
			 where b.order_code in
				   ('E1010D', 'E1012C', 'E1016B', 'E1018A', 'E1020A', 'E1014B')
			   and c.fee_ym is null) a1) f
where DeduOrder.fee_ym = f.fee_ym and DeduOrder.data_id = f.data_id and DeduOrder.order_seq_no = f.order_seq_no

--未開藥
/*
1.有報藥服費沒報藥費==>有order_type=9的醫令時，沒有order_typ=1的醫令  亦可看成是[dsvc_dot]>0且[drug_dot]=0  扣E1006C+E1007C+[dsvc_dot] 
2.有E1006C沒有ordertype=1的藥費醫令(排除有E1006C有ordertype=4的藥費醫令)==>存在E1006C且[drug_dot]=0且沒有其他order_type=4的ordercode
3.有E1007C沒有藥的醫令(order_type=4)==>存在E1007C且沒有其他ordertype=4的醫令，扣E1006C+E1007C
*/
--dsvc_dot
update Dedu set
                Dedu.deducode6='1',
                Dedu.c_dsvc_dot = f.dsvc_dot
    from
	(select distinct
		 a.fee_ym,
		 a.data_id,
		 a.dsvc_dot
  from Dedu a
  join DeduOrder b
    on a.fee_ym = b.fee_ym
   and a.data_id = b.data_id
 where a.dsvc_dot > 0
   and a.drug_dot = 0) f
where Dedu.fee_ym = f.fee_ym and Dedu.data_id = f.data_id

update DeduOrder set
    DeduOrder.c_order_dot = f.order_dot
    from
    (select distinct
		 a.fee_ym,
		 a.data_id,
		 b.order_seq_no,
		 b.order_dot
  from Dedu a
  join DeduOrder b
    on a.fee_ym = b.fee_ym
   and a.data_id = b.data_id
 where a.dsvc_dot > 0
   and a.drug_dot = 0
   and b.order_code in ('E1006C','E1007C')) f
where DeduOrder.fee_ym = f.fee_ym and DeduOrder.data_id = f.data_id and DeduOrder.order_seq_no = f.order_seq_no

--order_dot E1006C
update Dedu set
                Dedu.deducode6='1',
                Dedu.c_dsvc_dot = f.dsvc_dot
    from
	(select distinct a.fee_ym, a.data_id, a.dsvc_dot
		from Dedu a
		join DeduOrder b
		on a.fee_ym = b.fee_ym
		and a.data_id = b.data_id
		left join (select * from DeduOrder where order_type = '4' or order_type = '1') c
		on a.fee_ym = c.fee_ym
		and a.data_id = c.data_id
		where b.order_code = 'E1006C'
		and a.drug_dot = 0
		and c.order_seq_no is null) f
where Dedu.fee_ym = f.fee_ym and Dedu.data_id = f.data_id

update DeduOrder set
    DeduOrder.c_order_dot = f.order_dot
    from
	(select distinct a.fee_ym, a.data_id, b.order_seq_no, b.order_dot
		from Dedu a
		join DeduOrder b
		on a.fee_ym = b.fee_ym
		and a.data_id = b.data_id
		left join (select * from DeduOrder where order_type = '4' or order_type = '1') c
		on a.fee_ym = c.fee_ym
		and a.data_id = c.data_id
		where b.order_code = 'E1006C'
		and a.drug_dot = 0
		and c.order_seq_no is null) f
where DeduOrder.fee_ym = f.fee_ym and DeduOrder.data_id = f.data_id and DeduOrder.order_seq_no = f.order_seq_no

--order_dot E1007C
update Dedu set
                Dedu.deducode6='1',
                Dedu.c_dsvc_dot = f.dsvc_dot
    from
	(select distinct a.fee_ym, a.data_id, a.dsvc_dot
		from Dedu a
		join DeduOrder b
		on a.fee_ym = b.fee_ym
		and a.data_id = b.data_id
		left join (select * from DeduOrder where order_type = '4' or order_type = '1') c
		on a.fee_ym = c.fee_ym
		and a.data_id = c.data_id
		where b.order_code = 'E1007C'
		and c.order_seq_no is null) f
where Dedu.fee_ym = f.fee_ym and Dedu.data_id = f.data_id

update DeduOrder set
    DeduOrder.c_order_dot = f.order_dot
    from
	(select distinct a.fee_ym, a.data_id, b.order_seq_no, b.order_dot
		from Dedu a
		join DeduOrder b
		on a.fee_ym = b.fee_ym
		and a.data_id = b.data_id
		left join (select * from DeduOrder where order_type = '4' or order_type = '1') c
		on a.fee_ym = c.fee_ym
		and a.data_id = c.data_id
		where b.order_code = 'E1007C'
		and c.order_seq_no is null) f
where DeduOrder.fee_ym = f.fee_ym and DeduOrder.data_id = f.data_id and DeduOrder.order_seq_no = f.order_seq_no


--審查八：非戒菸用藥 --order_dot
update Dedu set
    Dedu.deducode8='1'
    from
	(select a1.fee_ym, a1.data_id
	  from (select distinct b.fee_ym, b.data_id
			  from Dedu a
			  join DeduOrder b
				on a.fee_ym = b.fee_ym
			   and a.data_id = b.data_id
			  left join (select drugno,
							   orderstartdate,
							   isnull((case
										when rtrim(orderenddate) = '' then
										 null
										else
										 rtrim(orderenddate)
									  end),
									  '20991231') as orderenddate
						  from GenDrugBasic) c
				on rtrim(b.order_code) = c.drugno
			 where b.order_type = '1'
			   and (rtrim(b.order_code) not in (select drugno from GenDrugBasic) or
				   a.func_date not between c.orderstartdate and c.orderenddate)) a1) f
where Dedu.fee_ym = f.fee_ym and Dedu.data_id = f.data_id

update DeduOrder set
    DeduOrder.c_order_dot=f.order_dot
    from
	(select a1.fee_ym, a1.data_id, a1.order_seq_no, a1.order_dot
	  from (select distinct b.fee_ym, b.data_id, b.order_seq_no, b.order_dot
			  from Dedu a
			  join DeduOrder b
				on a.fee_ym = b.fee_ym
			   and a.data_id = b.data_id
			  left join (select drugno,
							   orderstartdate,
							   isnull((case
										when rtrim(orderenddate) = '' then
										 null
										else
										 rtrim(orderenddate)
									  end),
									  '20991231') as orderenddate
						  from GenDrugBasic) c
				on rtrim(b.order_code) = c.drugno
			 where b.order_type = '1'
			   and (rtrim(b.order_code) not in (select drugno from GenDrugBasic) or
				   a.func_date not between c.orderstartdate and c.orderenddate)) a1) f
where DeduOrder.fee_ym = f.fee_ym and DeduOrder.data_id = f.data_id and DeduOrder.order_seq_no = f.order_seq_no


--審查九：非戒菸補助醫令類別 --order_dot
update Dedu set
    Dedu.deducode9='1'
    from
	(select a.fee_ym,a.data_id
	   from DeduOrder a 
	 where a.order_type not in ('1','2','4','9') and a.order_dot <> 0) f
where Dedu.fee_ym = f.fee_ym and Dedu.data_id = f.data_id


update DeduOrder set
    DeduOrder.c_order_dot=f.order_dot
    from
	(select a.fee_ym,a.data_id,a.order_seq_no,a.order_dot
	   from DeduOrder a 
	 where a.order_type not in ('1','2','4','9') and a.order_dot <> 0) f
where DeduOrder.fee_ym = f.fee_ym and DeduOrder.data_id = f.data_id and DeduOrder.order_seq_no = f.order_seq_no

--審查十：非補助醫令 --order_dot
update Dedu set
    Dedu.deducode10='1'
    from
	(select a.fee_ym,a.data_id
	   from DeduOrder a
	   join (select fee_ym,data_id from Dedu where data_type = '1') b on a.fee_ym = b.fee_ym and a.data_id = b.data_id
	  where ((a.order_type = '2' and a.order_code not in ('E1006C','E1007C','E1008C','E1022C','E1023C','E1024C','E1025C','E1026C'))
	 	or (a.order_type = '9' and a.order_code not in ('E1009D','E1010D','E1011C','E1012C','E1015B','E1016B','E1017A','E1018A','E1019A','E1020A')))
	 union all
	 select a.fee_ym,a.data_id
	   from DeduOrder a
	   join (select fee_ym,data_id from Dedu where data_type = '2') b on a.fee_ym = b.fee_ym and a.data_id = b.data_id
	  where ((a.order_type = '2' and a.order_code not in ('E1008C','E1022C','E1023C','E1024C','E1025C','E1026C'))
	 	or (a.order_type = '9' and a.order_code not in ('E1013B','E1014B')))) f
where Dedu.fee_ym = f.fee_ym and Dedu.data_id = f.data_id

update DeduOrder set
    DeduOrder.c_order_dot=f.order_dot
    from
	(select a.fee_ym,a.data_id,a.order_seq_no,a.order_dot
	   from DeduOrder a
	   join (select fee_ym,data_id from Dedu where data_type = '1') b on a.fee_ym = b.fee_ym and a.data_id = b.data_id
	  where ((a.order_type = '2' and a.order_code not in ('E1006C','E1007C','E1008C','E1022C','E1023C','E1024C','E1025C','E1026C'))
	 	or (a.order_type = '9' and a.order_code not in ('E1009D','E1010D','E1011C','E1012C','E1015B','E1016B','E1017A','E1018A','E1019A','E1020A')))
	 union all
	 select a.fee_ym,a.data_id,a.order_seq_no,a.order_dot
	   from DeduOrder a
	   join (select fee_ym,data_id from Dedu where data_type = '2') b on a.fee_ym = b.fee_ym and a.data_id = b.data_id
	  where ((a.order_type = '2' and a.order_code not in ('E1008C','E1022C','E1023C','E1024C','E1025C','E1026C'))
	 	or (a.order_type = '9' and a.order_code not in ('E1013B','E1014B')))) f
where DeduOrder.fee_ym = f.fee_ym and DeduOrder.data_id = f.data_id and DeduOrder.order_seq_no = f.order_seq_no


--審查十一：申報費用異常 (appl_dot > 8000) --appl_dot + part_amt
update Dedu set
                Dedu.deducode11='1',
                Dedu.c_appl_dot=f.appl_dot,
                Dedu.c_part_amt=f.part_amt
    from
	(select distinct a.fee_ym, a.data_id, a.appl_dot, a.part_amt
		from Dedu a
		join DeduOrder b
		on a.fee_ym = b.fee_ym
		and a.data_id = b.data_id
		where a.appl_dot > 8000) f
where Dedu.fee_ym = f.fee_ym and Dedu.data_id = f.data_id

--申報金額(E1022C, order_dot > 100, order_qty > 1) -- order_dot
update Dedu set
    Dedu.deducode11='1'
    from
	(select distinct a.fee_ym,
					     a.data_id,
					     b.order_code,
					     b.order_seq_no
		from Dedu a
		join DeduOrder b on a.fee_ym = b.fee_ym and a.data_id = b.data_id
	where b.order_code = 'E1022C' and b.order_qty > 1 and b.order_dot > 100) f
where Dedu.fee_ym = f.fee_ym and Dedu.data_id = f.data_id

update DeduOrder set
    DeduOrder.c_order_dot=f.order_dot-100
    from
	(select distinct a.fee_ym,
					     a.data_id,
					     b.order_code,
					     b.order_seq_no,
						 b.order_dot
		from Dedu a
		join DeduOrder b on a.fee_ym = b.fee_ym and a.data_id = b.data_id
	where b.order_code = 'E1022C' and b.order_qty > 1 and b.order_dot > 100) f
where DeduOrder.fee_ym = f.fee_ym and DeduOrder.data_id = f.data_id and DeduOrder.order_seq_no = f.order_seq_no


--審查十二：重複申報
/*
院所，藥局(不用篩選orig_hosp_id=N)
1.用藥申報MedApply=1 相同的hospid+id+birthday+func_date+order_code 筆數大於一筆，就要扣[drug_dot]+[dsvc_dot]及E1006C,E1007C [order_dot] (留下申報日期,流水號最大的那一筆)
2.衛教申報InstructApply=1，相同的hospid+id+birthday+func_date+order_code 筆數大於一筆，就要扣order_code=E1022C的order_dot (留下申報日期,流水號最大的那一筆)
3.追蹤申報TraceApply in (2,3,4,5)，相同的hospid+id+birthday+func_date+order_code 筆數大於一筆，就要扣
  TraceApply = 2，order_code = 'E1023C' order_dot
  TraceApply = 3，order_code = 'E1024C' order_dot
  TraceApply = 4，order_code = 'E1025C' order_dot
  TraceApply = 5，order_code = 'E1026C' order_dot
  追蹤申報另外判斷 MhbtQsData 如下 :
  order_code=E1023C 對到dbo_MhbtQsData!Cure_Type=1，HospID+ID+Birthday+TraceDate且筆數相同就不用扣
  order_code=E1024C 對到dbo_MhbtQsData!Cure_Type=1，HospID+ID+Birthday+Trace_Date2且筆數相同就不用扣
  order_code=E1025C 對到dbo_MhbtQsData!Cure_Type=2，HospID+ID+Birthday+Trace_Date且筆數相同就不用扣
  order_code=E1026C 對到dbo_MhbtQsData!Cure_Type=2，HospID+ID+Birthday+Trace_Date2且筆數相同就不用扣
4.釋出申報ReleaseApply=1，相同的hospid+id+birthday+func_date+releaseapply=1 筆數大於一筆，就要扣 [appl_dot]+[part_amt] (留下申報日期,流水號最大的那一筆)
*/
--用藥申報 (dsvc_dot + drug_dot)
update Dedu set
                Dedu.deducode12='1',
                Dedu.c_dsvc_dot=f.dsvc_dot,
                Dedu.c_drug_dot=f.drug_dot
    from
	(select distinct a.fee_ym, a.data_id, a.dsvc_dot, a.drug_dot
		from Dedu a
		join DeduOrder b
		on a.fee_ym = b.fee_ym
		and a.data_id = b.data_id
		where a.fee_ym + a.data_id in (select a.fee_ym + a.data_id
										from (select a.fee_ym,
													a.data_id,
													row_number() over(partition by a.hospid, a.id, a.birthday, a.func_date order by a.appl_date desc, a.seq_no desc) as rnk
												from Dedu a
												where a.medapply = '1') a
										where a.rnk > 1)
) f where Dedu.fee_ym = f.fee_ym and Dedu.data_id = f.data_id

--用藥申報 (order_dot)
update DeduOrder set
    DeduOrder.c_order_dot=f.order_dot
    from
	(select distinct a.fee_ym, a.data_id, b.order_seq_no, b.order_dot
		from Dedu a
		join DeduOrder b
		on a.fee_ym = b.fee_ym
		and a.data_id = b.data_id
		where a.fee_ym + a.data_id in (select a.fee_ym + a.data_id
										from (select a.fee_ym,
													a.data_id,
													row_number() over(partition by a.hospid, a.id, a.birthday, a.func_date order by a.appl_date desc, a.seq_no desc) as rnk
												from Dedu a
												where a.medapply = '1') a
										where a.rnk > 1)
		    and b.order_code in ('E1006C','E1007C')
) f where DeduOrder.fee_ym = f.fee_ym and DeduOrder.data_id = f.data_id and DeduOrder.order_seq_no = f.order_seq_no

--衛教申報
update Dedu set
    Dedu.deducode12='1'
    from
	(select distinct a.fee_ym, a.data_id, a.dsvc_dot, a.drug_dot
		from Dedu a
		join DeduOrder b
		on a.fee_ym = b.fee_ym
		and a.data_id = b.data_id
		where a.fee_ym + a.data_id + convert(varchar, b.order_seq_no) in (select a.fee_ym + a.data_id + convert(varchar, a.order_seq_no)
										from (select a.fee_ym,
													a.data_id,
													b.order_seq_no,
													row_number() over(partition by a.hospid, a.id, a.birthday, a.func_date, b.order_code order by a.appl_date desc, a.seq_no desc) as rnk
												from Dedu a
												join (select * from DeduOrder where order_code = 'E1022C') b on a.fee_ym = b.fee_ym and a.data_id = b.data_id
												--where a.InstructApply= '1'
												) a
										where a.rnk > 1)
		   and b.order_code = 'E1022C'
) f where Dedu.fee_ym = f.fee_ym and Dedu.data_id = f.data_id

--衛教申報 (order_dot)
update DeduOrder set
    DeduOrder.c_order_dot=f.order_dot
    from
	(select distinct a.fee_ym, a.data_id, b.order_seq_no, b.order_dot
		from Dedu a
		join DeduOrder b
		on a.fee_ym = b.fee_ym
		and a.data_id = b.data_id
		where a.fee_ym + a.data_id + convert(varchar, b.order_seq_no) in (select a.fee_ym + a.data_id + convert(varchar, a.order_seq_no)
										from (select a.fee_ym,
													a.data_id,
													b.order_seq_no,
													row_number() over(partition by a.hospid, a.id, a.birthday, a.func_date, b.order_code order by a.appl_date desc, a.seq_no desc) as rnk
												from Dedu a
												join (select * from DeduOrder where order_code = 'E1022C') b on a.fee_ym = b.fee_ym and a.data_id = b.data_id
												--where a.InstructApply= '1'
												) a
										where a.rnk > 1)
		   and b.order_code = 'E1022C'
) f where DeduOrder.fee_ym = f.fee_ym and DeduOrder.data_id = f.data_id and DeduOrder.order_seq_no = f.order_seq_no

----釋出申報 (appl_dot + part_amt)
--update Dedu set 
--       Dedu.deducode12='1',
--	   Dedu.c_appl_dot=f.appl_dot,
--	   Dedu.c_part_amt=f.part_amt
--from
--	(select distinct a.fee_ym, a.data_id, a.appl_dot, a.part_amt
--		from Dedu a
--		join DeduOrder b
--		on a.fee_ym = b.fee_ym
--		and a.data_id = b.data_id
--		where a.fee_ym + a.data_id in (select a.fee_ym + a.data_id
--										from (select a.fee_ym,
--													a.data_id,
--													row_number() over(partition by a.hospid, a.id, a.birthday, a.func_date order by a.appl_date desc, a.seq_no desc) as rnk
--												from Dedu a
--												where a.ReleaseApply = '1') a
--										where a.rnk > 1)
--) f where Dedu.fee_ym = f.fee_ym and Dedu.data_id = f.data_id

--追蹤申報
--E1023C
update Dedu set
    Dedu.deducode12='1'
    from (select distinct a.fee_ym,
							a.data_id,
							b.order_seq_no,
							b.order_dot
			  from (select * from Dedu --where TraceApply = '2'
			  ) a
			  join (select * from DeduOrder where order_code = 'E1023C') b
				on a.fee_ym = b.fee_ym
			   and a.data_id = b.data_id
			  join (select distinct a.fee_ym, a.data_id, a.order_seq_no
					  from (select a.fee_ym,
								   a.data_id,
								   a.hospid,
								   a.id,
								   a.birthday,
								   a.func_date,
								   a.order_seq_no,
								   a.rnk
							  from (select a.fee_ym,
										   a.data_id,
										   a.hospid,
										   a.id,
										   a.birthday,
										   a.func_date,
										   a.order_seq_no,
										   row_number() over(partition by a.hospid, a.id, a.birthday, a.func_date, a.order_code order by a.appl_date desc, a.seq_no desc) as rnk
									  from (select a.*,b.order_code,b.order_seq_no
											  from Dedu a
											  join (select * from DeduOrder where order_code = 'E1023C' and order_dot <> 0) b on a.fee_ym = b.fee_ym and a.data_id = b.data_id) a
									 --where a.TraceApply = '2'
									 ) a
							 where a.rnk > 1) a
					  left join (select hospid,
									   id,
									   birthday,
									   tracedate,
									   row_number() over(partition by hospid, id, birthday, tracedate order by hospid desc) as rnk
								  from MhbtQsData
								 where cure_type = '1') b
						on a.hospid = b.hospid
					   and a.id = b.id
					   and a.birthday = b.birthday
					   and a.func_date = b.tracedate
					   and a.rnk = b.rnk
					 where b.hospid is null) c
				on a.fee_ym = c.fee_ym
			   and a.data_id = c.data_id
			   and b.order_seq_no = c.order_seq_no
) f where Dedu.fee_ym = f.fee_ym and Dedu.data_id = f.data_id

--E1023C (order_dot)
update DeduOrder set
    DeduOrder.c_order_dot=f.order_dot
    from (select distinct a.fee_ym,
							a.data_id,
							b.order_seq_no,
							b.order_dot
			  from (select * from Dedu --where TraceApply = '2'
			  ) a
			  join (select * from DeduOrder where order_code = 'E1023C') b
				on a.fee_ym = b.fee_ym
			   and a.data_id = b.data_id
			  join (select distinct a.fee_ym, a.data_id, a.order_seq_no
					  from (select a.fee_ym,
								   a.data_id,
								   a.hospid,
								   a.id,
								   a.birthday,
								   a.func_date,
								   a.order_seq_no,
								   a.rnk
							  from (select a.fee_ym,
										   a.data_id,
										   a.hospid,
										   a.id,
										   a.birthday,
										   a.func_date,
										   a.order_seq_no,
										   row_number() over(partition by a.hospid, a.id, a.birthday, a.func_date, a.order_code order by a.appl_date desc, a.seq_no desc) as rnk
									  from (select a.*,b.order_code,b.order_seq_no
											  from Dedu a
											  join (select * from DeduOrder where order_code = 'E1023C' and order_dot <> 0) b on a.fee_ym = b.fee_ym and a.data_id = b.data_id) a
									 --where a.TraceApply = '2'
									 ) a
							 where a.rnk > 1) a
					  left join (select hospid,
									   id,
									   birthday,
									   tracedate,
									   row_number() over(partition by hospid, id, birthday, tracedate order by hospid desc) as rnk
								  from MhbtQsData
								 where cure_type = '1') b
						on a.hospid = b.hospid
					   and a.id = b.id
					   and a.birthday = b.birthday
					   and a.func_date = b.tracedate
					   and a.rnk = b.rnk
					 where b.hospid is null) c
				on a.fee_ym = c.fee_ym
			   and a.data_id = c.data_id
			   and b.order_seq_no = c.order_seq_no
) f where DeduOrder.fee_ym = f.fee_ym and DeduOrder.data_id = f.data_id and DeduOrder.order_seq_no = f.order_seq_no

--E1024C
update Dedu set
    Dedu.deducode12='1'
    from (select distinct a.fee_ym,
							a.data_id,
							b.order_seq_no,
							b.order_dot
			  from (select * from Dedu --where TraceApply = '3'
			  ) a
			  join (select * from DeduOrder where order_code = 'E1024C') b
				on a.fee_ym = b.fee_ym
			   and a.data_id = b.data_id
			  join (select distinct a.fee_ym, a.data_id, a.order_seq_no
					  from (select a.fee_ym,
								   a.data_id,
								   a.hospid,
								   a.id,
								   a.birthday,
								   a.func_date,
								   a.order_seq_no,
								   a.rnk
							  from (select a.fee_ym,
										   a.data_id,
										   a.hospid,
										   a.id,
										   a.birthday,
										   a.order_seq_no,
										   a.func_date,
										   row_number() over(partition by a.hospid, a.id, a.birthday, a.func_date, a.order_code order by a.appl_date desc, a.seq_no desc) as rnk
									  from (select a.*,b.order_code,b.order_seq_no
											  from Dedu a
											  join (select * from DeduOrder where order_code = 'E1024C' and order_dot <> 0) b on a.fee_ym = b.fee_ym and a.data_id = b.data_id) a
									 --where a.TraceApply = '3'
									 ) a
							 where a.rnk > 1) a
					  left join (select hospid,
									   id,
									   birthday,
									   trace_date2,
									   row_number() over(partition by hospid, id, birthday, trace_date2 order by hospid desc) as rnk
								  from MhbtQsData
								 where cure_type = '1') b
						on a.hospid = b.hospid
					   and a.id = b.id
					   and a.birthday = b.birthday
					   and a.func_date = b.trace_date2
					   and a.rnk = b.rnk
					 where b.hospid is null) c
				on a.fee_ym = c.fee_ym
			   and a.data_id = c.data_id
			   and b.order_seq_no = c.order_seq_no
) f where Dedu.fee_ym = f.fee_ym and Dedu.data_id = f.data_id

--E1024C (order_dot)
update DeduOrder set
    DeduOrder.c_order_dot=f.order_dot
    from (select distinct a.fee_ym,
							a.data_id,
							b.order_seq_no,
							b.order_dot
			  from (select * from Dedu --where TraceApply = '3'
			  ) a
			  join (select * from DeduOrder where order_code = 'E1024C') b
				on a.fee_ym = b.fee_ym
			   and a.data_id = b.data_id
			  join (select distinct a.fee_ym, a.data_id, a.order_seq_no
					  from (select a.fee_ym,
								   a.data_id,
								   a.hospid,
								   a.id,
								   a.birthday,
								   a.func_date,
								   a.order_seq_no,
								   a.rnk
							  from (select a.fee_ym,
										   a.data_id,
										   a.hospid,
										   a.id,
										   a.birthday,
										   a.func_date,
										   a.order_seq_no,
										   row_number() over(partition by a.hospid, a.id, a.birthday, a.func_date, a.order_code order by a.appl_date desc, a.seq_no desc) as rnk
									  from (select a.*,b.order_code,b.order_seq_no
											  from Dedu a
											  join (select * from DeduOrder where order_code = 'E1024C' and order_dot <> 0) b on a.fee_ym = b.fee_ym and a.data_id = b.data_id) a
									 --where a.TraceApply = '3'
									 ) a
							 where a.rnk > 1) a
					  left join (select hospid,
									   id,
									   birthday,
									   trace_date2,
									   row_number() over(partition by hospid, id, birthday, trace_date2 order by hospid desc) as rnk
								  from MhbtQsData
								 where cure_type = '1') b
						on a.hospid = b.hospid
					   and a.id = b.id
					   and a.birthday = b.birthday
					   and a.func_date = b.trace_date2
					   and a.rnk = b.rnk
					 where b.hospid is null) c
				on a.fee_ym = c.fee_ym
			   and a.data_id = c.data_id
			   and b.order_seq_no = c.order_seq_no
) f where DeduOrder.fee_ym = f.fee_ym and DeduOrder.data_id = f.data_id and DeduOrder.order_seq_no = f.order_seq_no

--E1025C
update Dedu set
    Dedu.deducode12='1'
    from (select distinct a.fee_ym,
							a.data_id,
							b.order_seq_no,
							b.order_dot
			  from (select * from Dedu --where TraceApply = '4'
			  ) a
			  join (select * from DeduOrder where order_code = 'E1025C') b
				on a.fee_ym = b.fee_ym
			   and a.data_id = b.data_id
			  join (select distinct a.fee_ym, a.data_id, a.order_seq_no
					  from (select a.fee_ym,
								   a.data_id,
								   a.hospid,
								   a.id,
								   a.birthday,
								   a.func_date,
								   a.order_seq_no,
								   a.rnk
							  from (select a.fee_ym,
										   a.data_id,
										   a.hospid,
										   a.id,
										   a.birthday,
										   a.func_date,
										   a.order_seq_no,
										   row_number() over(partition by a.hospid, a.id, a.birthday, a.func_date, a.order_code order by  a.appl_date desc, a.seq_no desc) as rnk
									  from (select a.*,b.order_code,b.order_seq_no
											  from Dedu a
											  join (select * from DeduOrder where order_code = 'E1025C' and order_dot <> 0) b on a.fee_ym = b.fee_ym and a.data_id = b.data_id) a
									 --where a.TraceApply = '4'
									 ) a
							 where a.rnk > 1) a
					  left join (select hospid,
									   id,
									   birthday,
									   tracedate,
									   row_number() over(partition by hospid, id, birthday, tracedate order by hospid desc) as rnk
								  from MhbtQsData
								 where cure_type = '2') b
						on a.hospid = b.hospid
					   and a.id = b.id
					   and a.birthday = b.birthday
					   and a.func_date = b.tracedate
					   and a.rnk = b.rnk
					 where b.hospid is null) c
				on a.fee_ym = c.fee_ym
			   and a.data_id = c.data_id
			   and b.order_seq_no = c.order_seq_no
) f where Dedu.fee_ym = f.fee_ym and Dedu.data_id = f.data_id

--E1025C (order_dot)
update DeduOrder set
    DeduOrder.c_order_dot=f.order_dot
    from (select distinct a.fee_ym,
							a.data_id,
							b.order_seq_no,
							b.order_dot
			  from (select * from Dedu --where TraceApply = '4'
			  ) a
			  join (select * from DeduOrder where order_code = 'E1025C') b
				on a.fee_ym = b.fee_ym
			   and a.data_id = b.data_id
			  join (select distinct a.fee_ym, a.data_id, a.order_seq_no
					  from (select a.fee_ym,
								   a.data_id,
								   a.hospid,
								   a.id,
								   a.birthday,
								   a.func_date,
								   a.order_seq_no,
								   a.rnk
							  from (select a.fee_ym,
										   a.data_id,
										   a.hospid,
										   a.id,
										   a.birthday,
										   a.func_date,
										   a.order_seq_no,
										   row_number() over(partition by a.hospid, a.id, a.birthday, a.func_date, a.order_code order by a.appl_date desc, a.seq_no desc) as rnk
									  from (select a.*,b.order_code,b.order_seq_no
											  from Dedu a
											  join (select * from DeduOrder where order_code = 'E1025C' and order_dot <> 0) b on a.fee_ym = b.fee_ym and a.data_id = b.data_id) a
									 --where a.TraceApply = '4'
									 ) a
							 where a.rnk > 1) a
					  left join (select hospid,
									   id,
									   birthday,
									   tracedate,
									   row_number() over(partition by hospid, id, birthday, tracedate order by hospid desc) as rnk
								  from MhbtQsData
								 where cure_type = '2') b
						on a.hospid = b.hospid
					   and a.id = b.id
					   and a.birthday = b.birthday
					   and a.func_date = b.tracedate
					   and a.rnk = b.rnk
					 where b.hospid is null) c
				on a.fee_ym = c.fee_ym
			   and a.data_id = c.data_id
			   and b.order_seq_no = c.order_seq_no
) f where DeduOrder.fee_ym = f.fee_ym and DeduOrder.data_id = f.data_id and DeduOrder.order_seq_no = f.order_seq_no

--E1026C
update Dedu set
    Dedu.deducode12='1'
    from (select distinct a.fee_ym,
							a.data_id,
							b.order_seq_no,
							b.order_dot
			  from (select * from Dedu --where TraceApply = '5'
			  ) a
			  join (select * from DeduOrder where order_code = 'E1026C') b
				on a.fee_ym = b.fee_ym
			   and a.data_id = b.data_id
			  join (select distinct a.fee_ym, a.data_id, a.order_seq_no
					  from (select a.fee_ym,
								   a.data_id,
								   a.hospid,
								   a.id,
								   a.birthday,
								   a.func_date,
								   a.order_seq_no,
								   a.rnk
							  from (select a.fee_ym,
										   a.data_id,
										   a.hospid,
										   a.id,
										   a.birthday,
										   a.func_date,
										   a.order_seq_no,
										   row_number() over(partition by a.hospid, a.id, a.birthday, a.func_date, a.order_code order by a.appl_date desc, a.seq_no desc) as rnk
									  from (select a.*,b.order_code,b.order_seq_no
											  from Dedu a
											  join (select * from DeduOrder where order_code = 'E1026C' and order_dot <> 0) b on a.fee_ym = b.fee_ym and a.data_id = b.data_id) a
									 --where a.TraceApply = '5'
									 ) a
							 where a.rnk > 1) a
					  left join (select hospid,
									   id,
									   birthday,
									   trace_date2,
									   row_number() over(partition by hospid, id, birthday, trace_date2 order by hospid desc) as rnk
								  from MhbtQsData
								 where cure_type = '2') b
						on a.hospid = b.hospid
					   and a.id = b.id
					   and a.birthday = b.birthday
					   and a.func_date = b.trace_date2
					   and a.rnk = b.rnk
					 where b.hospid is null) c
				on a.fee_ym = c.fee_ym
			   and a.data_id = c.data_id
			   and b.order_seq_no = c.order_seq_no
) f where Dedu.fee_ym = f.fee_ym and Dedu.data_id = f.data_id

--E1026C (order_dot)
update DeduOrder set
    DeduOrder.c_order_dot=f.order_dot
    from (select distinct a.fee_ym,
							a.data_id,
							b.order_seq_no,
							b.order_dot
			  from (select * from Dedu --where TraceApply = '5'
			  ) a
			  join (select * from DeduOrder where order_code = 'E1026C') b
				on a.fee_ym = b.fee_ym
			   and a.data_id = b.data_id
			  join (select distinct a.fee_ym, a.data_id, a.order_seq_no
					  from (select a.fee_ym,
								   a.data_id,
								   a.hospid,
								   a.id,
								   a.birthday,
								   a.func_date,
								   a.order_seq_no,
								   a.rnk
							  from (select a.fee_ym,
										   a.data_id,
										   a.hospid,
										   a.id,
										   a.birthday,
										   a.func_date,
										   a.order_seq_no,
										   row_number() over(partition by a.hospid, a.id, a.birthday, a.func_date, a.order_code order by a.appl_date desc, a.seq_no desc) as rnk
									  from (select a.*,b.order_code,b.order_seq_no
											  from Dedu a
											  join (select * from DeduOrder where order_code = 'E1026C' and order_dot <> 0) b on a.fee_ym = b.fee_ym and a.data_id = b.data_id) a
									 --where a.TraceApply = '5'
									 ) a
							 where a.rnk > 1) a
					  left join (select hospid,
									   id,
									   birthday,
									   trace_date2,
									   row_number() over(partition by hospid, id, birthday, trace_date2 order by hospid desc) as rnk
								  from MhbtQsData
								 where cure_type = '2') b
						on a.hospid = b.hospid
					   and a.id = b.id
					   and a.birthday = b.birthday
					   and a.func_date = b.trace_date2
					   and a.rnk = b.rnk
					 where b.hospid is null) c
				on a.fee_ym = c.fee_ym
			   and a.data_id = c.data_id
			   and b.order_seq_no = c.order_seq_no
) f where DeduOrder.fee_ym = f.fee_ym and DeduOrder.data_id = f.data_id and DeduOrder.order_seq_no = f.order_seq_no


--審查十三：給藥天數不符VPN週數 --[drug_dot]+[dsvc_dot]及E1006C,E1007C [order_dot]
Declare cursor13 cursor for
select a.fee_ym,
       a.data_id,
       a.drug_dot,
       a.dsvc_dot,
       b.order_seq_no,
       isnull(b.order_dot, 0) as order_dot
from (select * from Dedu where MedApply='1') a
         left join (select * from DeduOrder where order_code in ('E1006C','E1007C')) b
                   on a.fee_ym = b.fee_ym
                       and a.data_id = b.data_id
         join (select * from MhbtQsData where cure_type='1') c
              on a.hospid = c.hospid
                  and a.id = c.id
                  and a.birthday = c.birthday
                  and a.func_date = c.funcdate
         left join (select distinct a.fee_ym, a.data_id
                    from Dedu a
                             join DeduOrder b
                                  on a.fee_ym = b.fee_ym
                                      and a.data_id = b.data_id
                             join MhbtQsData c
                                  on a.hospid = c.hospid
                                      and a.id = c.id
                                      and a.birthday = c.birthday
                                      and a.func_date = c.funcdate
                    where c.cure_type = '1'
                      and (isnull(c.cureweek, 0) * 7 = isnull(a.drug_days, 0) or
                           isnull(c.cureweek, 0) * 7 = isnull(b.order_drug_day, 0))
) d
                   on a.fee_ym = d.fee_ym
                       and a.data_id = d.data_id
where d.data_id is null
    Open cursor13 Fetch Next From cursor13 Into @strfee_ym,@strdata_id,@intdrug_dot,@intdsvc_dot,@intorder_seq_no,@intorder_dot
    While @@Fetch_Status = 0
Begin
update Dedu set DeduCode13='1',c_drug_dot=@intdrug_dot,c_dsvc_dot=@intdsvc_dot where fee_ym=@strfee_ym and data_id=@strdata_id
update DeduOrder set c_order_dot=@intorder_dot where fee_ym=@strfee_ym and data_id=@strdata_id and order_seq_no=@intorder_seq_no
    Fetch Next From cursor13 Into @strfee_ym,@strdata_id,@intdrug_dot,@intdsvc_dot,@intorder_seq_no,@intorder_dot
End
Close cursor13
    Deallocate cursor13


--審查七：部分負擔金額不符 --drug_dot
update Dedu set
                Dedu.deducode7='1',
                Dedu.c_drug_dot=(case when isnull(Dedu.c_drug_dot,0) < f.drug_dot then f.drug_dot else isnull(Dedu.c_drug_dot,0) end)
    from
	(select distinct a1.fee_ym,a1.data_id,isnull(a1.drug_dot,0) - isnull(a1.part_amt,0) as drug_dot
	  from (select a.fee_ym,a.data_id,
				(case when a.drug_dot <= 100 then
						   0
					  when a.drug_dot >= 101 and a.drug_dot <= 200 then
						   20
					  when a.drug_dot >= 201 and a.drug_dot <= 300 then
						   40
					  when a.drug_dot >= 301 and a.drug_dot <= 400 then
						   60
					  when a.drug_dot >= 401 and a.drug_dot <= 500 then
						   80
					  when a.drug_dot >= 501 and a.drug_dot <= 600 then
						   100
					  when a.drug_dot >= 601 and a.drug_dot <= 700 then
						   120
					  when a.drug_dot >= 701 and a.drug_dot <= 800 then
						   140
					  when a.drug_dot >= 801 and a.drug_dot <= 900 then
						   160
					  when a.drug_dot >= 901 and a.drug_dot <= 1000 then
						   180
					  when a.drug_dot >= 1001 then
						   200
				   end) * (case when a.area_service in ('01','02') then 0.8 else 1 end) as drug_dot,
				   a.part_amt 
			  from Dedu a 
			where a.part_code not in ('003','007','907')
			   --and a.drug_dot > 0 and a.dsvc_dot > 0      
	  ) a1 where isnull(a1.drug_dot,0) > isnull(a1.part_amt,0)) f
where Dedu.fee_ym = f.fee_ym and Dedu.data_id = f.data_id


--更新 Dedu.DeduDot (不含 c_other_dot)
update Dedu set
    Dedu.DeduDot=f.DeduDot
    from (select a.fee_ym,
				a.data_id,
				case when a.c_dsvc_dot + a.c_drug_dot + sum(b.c_order_dot) > a.appl_dot + a.part_amt then
					a.appl_dot + a.part_amt
				else
				    a.c_dsvc_dot + a.c_drug_dot + sum(b.c_order_dot)
				end as dedudot
			from Dedu a
			join DeduOrder b
			on a.fee_ym = b.fee_ym
			and a.data_id = b.data_id
			group by a.fee_ym,
					a.data_id,
					a.c_dsvc_dot,
					a.c_drug_dot,
					a.appl_dot,
					a.part_amt
) f where Dedu.fee_ym = f.fee_ym and Dedu.data_id = f.data_id

--更新 Dedu.DeduDot (判斷是否更新 c_other_dot)
update Dedu set
    Dedu.DeduDot=Dedu.DeduDot + f.DeduDot
    from (select a.fee_ym,
				a.data_id,
				case when a.c_dsvc_dot > 0 or a.c_drug_dot > 0 or (b.order_code in ('E1006C','E1007C') and sum(b.c_order_dot) > 0) then
					0
				else
					sum(b.c_other_dot)
				end as dedudot
			from Dedu a
			join DeduOrder b
			on a.fee_ym = b.fee_ym
			and a.data_id = b.data_id
			group by a.fee_ym,
					a.data_id,
					a.c_dsvc_dot,
					a.c_drug_dot,
					a.appl_dot,
					a.part_amt,
					b.order_code
) f where Dedu.fee_ym = f.fee_ym and Dedu.data_id = f.data_id

--更新 Dedu.DeduDot (c_appl_dot + c_part_amt)
update Dedu set
    Dedu.DeduDot=f.DeduDot
    from (select a.fee_ym,
			   a.data_id,
			   a.c_appl_dot + a.c_part_amt as dedudot
		  from Dedu a
		  join DeduOrder b
			on a.fee_ym = b.fee_ym
		   and a.data_id = b.data_id
		 where a.c_appl_dot + a.c_part_amt > 0
		 group by a.fee_ym,
				  a.data_id,
				  a.c_appl_dot,
				  a.c_part_amt
) f where Dedu.fee_ym = f.fee_ym and Dedu.data_id = f.data_id

update Dedu set
                Dedu.DeduCode1 = '',
                Dedu.DeduCode2 = '',
                Dedu.DeduCode3 = '',
                Dedu.DeduCode4 = '',
                Dedu.DeduCode5 = '',
                Dedu.DeduCode6 = '',
                Dedu.DeduCode7 = '',
                Dedu.DeduCode8 = '',
                Dedu.DeduCode9 = '',
                Dedu.DeduCode10 = '',
                Dedu.DeduCode11 = '',
                Dedu.DeduCode12 = '',
                Dedu.DeduCode13 = ''
where Dedu.DeduDot = 0

END
GO
/****** Object:  StoredProcedure [dbo].[SurveySampling]    Script Date: 2021/7/16 下午 10:10:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SurveySampling]
	--設定訪問月份
	@VisitMonth CHAR(6) 

AS
BEGIN
	SET NOCOUNT ON;

	--建立母群表格
DROP TABLE #SamplingPop
CREATE TABLE #SamplingPop
(	CaseNo varchar(10),
     FirstMonth varchar(6),
     FirstDate varchar(8),
     TimeSpan int,
     LastHospID varchar(10),
     LastHospSeqNo varchar(2),
     ID varchar(10),
     Birthday varchar(8),
     Edition varchar(5),
     HospName nvarchar(80),
     DivisionName varchar(20),
     Name nvarchar(20),
     BirthYear varchar(4),
     Sex varchar(1),
     Tel varchar(30),
     Nameless varchar(1),
     IDDup varchar(1),
     TelInvalid varchar(1),
     TelDup varchar(1),
     Correction varchar(1),
     SurveyIn1Y varchar(1),
     Untouchable varchar(1),
     ProgIn3M varchar(1),
     NotSelect varchar(1)
)

    --插入VPN樣本
    INSERT #SamplingPop
SELECT
    Q.CaseNo,
    LEFT(A.FirstDate,6) AS FirstMonth,
    MIN(A.FirstDate) AS FirstDate,
    DATEDIFF(MONTH, LEFT(A.FirstDate,6)+'01', @VisitMonth+'01') AS TimeSpan,
    H1.LastHospID,
    H1.LastHospSeqNo,
    A.ID,
    A.Birthday,
    MIN(ISNULL(Q.Edition,'')) AS Edition,
    MAX(H1.HospName) AS HospName,
    '' AS DivisionName,
    MAX(P.Name) AS Name,
    LEFT(A.Birthday,4) AS BirthYear,
    MAX(A.ID_Sex) AS Sex,
    MAX(
    CASE
    WHEN ISNULL(RTRIM(P.TelM), '') LIKE '09________'
    THEN  RTRIM(P.TelM)
    WHEN P.TelD != ''
    THEN P.TelD
    ELSE P.TelN
    END) AS Tel,	--電話優先順序為手機、白天電話、晚上電話
    MAX(CASE WHEN P.Name LIKE '%[#*/?]%' THEN '1' ELSE '' END) AS Nameless,
    '' AS IDDup,
    '' AS TelInvalid,
    '' AS 	TelDup,
    MAX(CASE WHEN A.Correction = '1' THEN '1' ELSE '' END) AS Correction, --矯正機關
    CASE
    WHEN EXISTS(
    SELECT *
    FROM QuitDataAll
    WHERE
    DATEDIFF(MONTH, VisitDate, @VisitMonth+'01') < 12
    --1年內訪問過的樣本，不包括一年追蹤樣本
    AND NOT (TimeSpan = 6 AND  DATEDIFF(MONTH, VisitDate, @VisitMonth+'01') = 6)
    AND ID = A.ID
    )
    THEN '1' ELSE '' END	 AS SurveyIn1Y,
		MAX(
			CASE
				WHEN Result IN ('50', '60', '80', '81', '90', '91', '92', '120', '121', '122', '123', '160', '180', '181', '190')
				THEN '1' ELSE '' END
			) AS Untouchable,
		'' AS ProgIn3M,
		'' AS NotSelect
	FROM
		(	Visits AS A
			JOIN HospBasic AS H1
				ON A.HospID = H1.HospID
				AND A.HospSeqNo = H1.HospSeqNo
		)
		--健保檔串VPN，兩者都要先取得最後院所代碼
		JOIN
		(	MhbtQsData AS B
			JOIN HospBasic AS H2
				ON B.HospID = H2.HospID
				AND B.HospSeqNo = H2.HospSeqNo
		)  
			ON A.ID = B.ID
			AND A.Birthday = B.Birthday	
			AND A.FirstDate = B.FirstTreatDate
			AND A.CureType = B.Cure_Type
			AND H1.LastHospID = H2.LastHospID
			AND H1.LastHospSeqNo = H2.LastHospSeqNo	
		JOIN MhbtAgentPatient AS P
			ON B.HospID = P.HospID
			AND B.ID = P.ID
			AND B.Birthday = P.Birthday
		LEFT JOIN  --串戒菸檔時，同樣要先取得最後院所代碼
		(	QuitDataAll AS Q
			JOIN HospBasic AS H3
				ON Q.HospID = H3.HospID
				AND Q.HospSeqNo = H3.HospSeqNo	
		)
			ON A.ID = Q.ID
			AND LEFT(A.FirstDate,6) = Q.FirstMonth
			AND H1.LastHospID = H3.LastHospID
			AND H1.LastHospSeqNo = H3.LastHospSeqNo
	WHERE
			--6個月前初診個案
		(	DATEDIFF(MONTH, A.FirstDate, @VisitMonth+'01') = 6
			OR
			--5年內每年追蹤，5年後每5年追蹤1次
			(	SUBSTRING(A.FirstDate,5,2) = RIGHT(@VisitMonth,2) 
				AND
				(	DATEDIFF(YEAR, A.FirstDate, @VisitMonth+'01') / 5 < 1
					OR DATEDIFF(YEAR, A.FirstDate, @VisitMonth+'01') % 5 = 0
				)
				AND Q.CaseNo IS NOT NULL	--1年以上個案，需曾為6個月追蹤樣本
			)
		)
		AND	A.UnCount = ''
	GROUP BY
		Q.CaseNo,
		LEFT(A.FirstDate,6),
		A.ID,
		A.Birthday,
		H1.LastHospID,
		H1.LastHospSeqNo
	ORDER BY
		FirstMonth DESC,
		Edition,
		LastHospID,
		LastHospSeqNo,
		ID

	--插入紙本紀錄表樣本
	INSERT #SamplingPop
SELECT
    Q.CaseNo,
    LEFT(A.FirstDate,6) AS FirstMonth,
    MIN(A.FirstDate) AS FirstDate,
    DATEDIFF(MONTH, LEFT(A.FirstDate,6)+'01', @VisitMonth+'01') AS TimeSpan,
    H1.LastHospID,
    H1.LastHospSeqNo,
    A.ID,
    A.Birthday,
    MIN(ISNULL(Q.Edition, '')) AS Edition,
    MAX(H1.HospName) AS HospName,
    '' AS DivisionName,
    MAX(B.Name) AS Name,
    LEFT(A.Birthday,4) AS BirthYear,
    MAX(A.ID_Sex) AS Sex,
    MAX(
    CASE
    WHEN B.TelM LIKE '09________'
    THEN B.TelM
    WHEN B.TelD != '999' AND B.TelD != ''
    THEN B.TelD
    ELSE B.TelN
    END
    ) AS Tel,
    MAX(CASE WHEN B.Name LIKE '%[#*/?]%' THEN '1' ELSE '' END) AS Nameless,
    '' AS IDDup,
    '' AS TelInvalid,
    '' AS 	TelDup,
    MAX(CASE WHEN A.Correction = '1' THEN '1' ELSE '' END) AS Correction,
    CASE
    WHEN EXISTS(
    SELECT *
    FROM QuitDataAll
    WHERE
    DATEDIFF(MONTH, VisitDate, @VisitMonth+'01') < 12
    --1年內訪問過的樣本，不包括一年追蹤樣本
    AND NOT (TimeSpan = 6 AND  DATEDIFF(MONTH, VisitDate, @VisitMonth+'01') = 6)
    AND ID = A.ID
    )
    THEN '1' ELSE '' END	 AS SurveyIn1Y,
		MAX(
			CASE
				WHEN Result IN ('50', '60', '80', '81', '90', '91', '92', '120', '121', '122', '123', '160', '180', '181', '190')
				THEN '1' ELSE '' END
			) AS Untouchable,
		'' AS ProgIn3M,
		'' AS NotSelect
	FROM
		(	Visits AS A
			JOIN HospBasic AS H1
				ON A.HospID = H1.HospID
				AND A.HospSeqNo = H1.HospSeqNo
		)
		JOIN
		(	CaseAgentPatient AS B
			JOIN HospBasic AS H2
				ON B.HospID = H2.HospID
				AND H2.HospSeqNo = '00'
		)
			ON A.ID = B.ID
			AND A.Birthday = B.Birthday	
			AND A.FirstDate = B.FirstTreatDate 
			AND H1.LastHospID = H2.LastHospID
			AND H1.LastHospSeqNo = H2.LastHospSeqNo
		JOIN QuitDataAll AS Q
			ON B.CaseNo = Q.CaseNo
			AND LEFT(A.FirstDate,6) = Q.FirstMonth
	WHERE
		SUBSTRING(A.FirstDate,5,2) = RIGHT(@VisitMonth,2) 
		AND
		(	DATEDIFF(YEAR, A.FirstDate, @VisitMonth+'01') / 5 < 1
			OR DATEDIFF(YEAR, A.FirstDate, @VisitMonth+'01') % 5 = 0
		)
		AND A.UnCount = ''
	GROUP BY
		Q.CaseNo,
		LEFT(A.FirstDate,6),
		A.ID,
		A.Birthday,
		H1.LastHospID,
		H1.LastHospSeqNo
	ORDER BY
		FirstMonth DESC,
		Edition,
		LastHospID,
		LastHospSeqNo,
		ID

	--註記一年內稽查樣本
UPDATE #SamplingPop
SET SurveyIn1Y = '1', NotSelect = '1'
    FROM CheckCase AS C
WHERE
    #SamplingPop.ID LIKE C.ID
  AND C.CheckDate >= @VisitMonth+'01'

--註記無效電話
--更新紙本紀錄表電話為VPN電話
UPDATE #SamplingPop
SET Tel =	CASE
                 WHEN ISNULL(RTRIM(P.TelM), '') LIKE '09________'
                     THEN  RTRIM(P.TelM)
                 WHEN P.TelD != ''
					THEN P.TelD
                 WHEN P.TelN != ''
					THEN P.TelN
                 ELSE #SamplingPop.Tel
    END
    FROM
		MhbtAgentPatient AS P
		JOIN HospBasic AS H
ON P.HospID = H.HospID
    AND H.HospSeqNo = '00'
WHERE
    #SamplingPop.ID = P.ID
  AND #SamplingPop.Birthday = P.Birthday
  AND #SamplingPop.LastHospID = H.LastHospID
  AND #SamplingPop.LastHospSeqNo = H.LastHospSeqNo
  AND #SamplingPop.FirstMonth <= '200703'
;
--註記電話格式不符
WITH TelInvalid AS
         (
             SELECT
                 ID,
                 FirstDate,
                 Tel,
                 REPLACE(
                     LEFT(Tel,CHARINDEX('#',Tel)-1)  --去除分機碼
			,'-'
                     ,''
                     ) AS NetTel
             FROM  #SamplingPop
         )

UPDATE #SamplingPop
SET TelInvalid = '1', NotSelect = '1'
    FROM TelInvalid
WHERE
    #SamplingPop.Tel = TelInvalid.Tel
  AND NetTel NOT LIKE '09________'
  AND NetTel NOT LIKE '02[235678]_______'
  AND NetTel NOT LIKE '03[^017]______'
  AND NetTel NOT LIKE '037[^01]_____'
  AND NetTel NOT LIKE '04[23]_______'
  AND NetTel NOT LIKE '04[78]______'
  AND NetTel NOT LIKE '049[^01]______'
  AND NetTel NOT LIKE '05[234567]______'
  AND NetTel NOT LIKE '06[235679]______'
  AND NetTel NOT LIKE '07[^01]______'
  AND NetTel NOT LIKE '08[478]______'
  AND NetTel NOT LIKE '082[^016]_____'
  AND NetTel NOT LIKE '08266____'
  AND NetTel NOT LIKE '0836[^01]____'
  AND NetTel NOT LIKE '089[^01]_____'

--註記療養院、監所等電話
UPDATE #SamplingPop
SET TelInvalid = '1', NotSelect = '1'
    FROM InvalidTel AS T
WHERE
    #SamplingPop.Tel LIKE T.Tel+'%'

--註記拒訪樣本
UPDATE #SamplingPop
SET SurveyIn1Y = '1', NotSelect = '1'
    FROM RefuseCase AS R
WHERE
    #SamplingPop.ID LIKE R.ID

--註記距上療程三個月內（6個月追蹤）
UPDATE #SamplingPop
SET ProgIn3M = '1', NotSelect = '1'
    FROM Visits AS A
WHERE
    DATEDIFF(MONTH, A.FirstDate, #SamplingPop.FirstDate) BETWEEN 1 AND 3
  AND #SamplingPop.TimeSpan = 6
  AND #SamplingPop.ID = A.ID

--註記排除樣本
UPDATE #SamplingPop
SET NotSelect = '1'
WHERE
        Nameless = '1'
   OR IDDup = '1'
   OR TelInvalid = '1'
   OR TelDup = '1'
   OR Correction = '1'
   OR SurveyIn1Y = '1'
   OR Untouchable = '1'
   OR ProgIn3M = '1'
;

--刪除同人同初診月重複（同初診月有2家院所）
--依序保留未排除、初診日、院所代碼較前者
WITH DupCase AS
         (
             SELECT
                 ID,
                 FirstMonth,
                 LastHospID,
                 LastHospSeqNo,
                 ROW_NUMBER() OVER
                 (	PARTITION BY ID, FirstMonth
                     ORDER BY NotSelect, FirstDate, LastHospID, LastHospSeqNo
                 ) AS RowRank
             FROM #SamplingPop
         )

DELETE #SamplingPop
	FROM DupCase AS A
	WHERE
		#SamplingPop.ID = A.ID
		AND #SamplingPop.FirstMonth = A.FirstMonth
		AND #SamplingPop.LastHospID = A.LastHospID
		AND #SamplingPop.LastHospSeqNo = A.LastHospSeqNo
		AND A.RowRank >1
	;

	--註記重複ID（同人在不同初診月）
		--保留初診月較近者
		--注意，先排除所有不選樣本後，才檢查是否有重複ID
WITH DupID AS
         (
             SELECT
                 ID,
                 FirstMonth,
                 ROW_NUMBER() OVER
                 (	PARTITION BY ID
                     ORDER BY FirstMonth DESC
                 ) AS RowRank
             FROM #SamplingPop
             WHERE NotSelect != 1
    )

UPDATE #SamplingPop
SET IDDup = '1', NotSelect = '1'
    FROM DupID AS A
WHERE
    #SamplingPop.ID = A.ID
  AND #SamplingPop.FirstMonth = A.FirstMonth
  AND RowRank > 1
;

--註記重複電話
--保留初診月較久、長卷、ID尾碼較小者
--注意，先排除所有不選樣本及重複ID後，才檢查是否有重複電話
WITH DupTel AS
         (
             SELECT
                 Tel,
                 FirstMonth,
                 Edition,
                 ID,
                 ROW_NUMBER() OVER
                 (	PARTITION BY Tel
                     ORDER BY FirstMonth, LEFT(Edition,1), REVERSE(ID)
                 ) AS RowRank
             FROM #SamplingPop
             WHERE NotSelect != 1
    )

UPDATE #SamplingPop
SET TelDup = '1', NotSelect = '1'
    FROM DupTel AS A
WHERE
    #SamplingPop.Tel = A.Tel
  AND #SamplingPop.ID = A.ID
  AND #SamplingPop.FirstMonth = A.FirstMonth
  AND #SamplingPop.Edition = A.Edition
  AND RowRank > 1

--加入縣市名稱
UPDATE #SamplingPop
SET DivisionName = D.DivisionName
    FROM
		HospBasic AS H
		JOIN GenDivision AS D
ON H.DivisionNo = D.DivisionNo
WHERE
    #SamplingPop.LastHospID = H.HospID
  AND #SamplingPop.LastHospSeqNo = H.HospSeqNo

--院所名稱更新為最後院所代碼之名稱
UPDATE #SamplingPop
SET HospName = H.HospName
    FROM HospBasic AS H
WHERE
    #SamplingPop.LastHospID = H.HospID
  AND #SamplingPop.LastHospSeqNo = H.HospSeqNo

--轉出清單
SELECT
    CASE
        WHEN CaseNo IS NULL
            THEN
                FORMAT( FirstMonth-191100-10000, '0000') +
                FORMAT(
                        ROW_NUMBER()
                            OVER(ORDER BY FirstMonth DESC, Edition, LastHospID, LastHospSeqNo, ID)
                    , '000000')
        ELSE CaseNo END AS CaseNo,
    FirstMonth,
    FirstDate,
    TimeSpan,
    LastHospID AS HospID,
    LastHospSeqNo AS HospSeqNo,
    ID,
    Birthday,
    Edition,
    HospName AS 院所名稱,
    DivisionName AS 院所縣市,
    Name AS 姓名,
    BirthYear AS 出生年,
    Sex AS 性別,
    Tel AS 電話,
    Nameless,
    IDDup,
    TelInvalid,
    TelDup,
    Correction,
    SurveyIn1Y,
    Untouchable,
    ProgIn3M,
    NotSelect
FROM #SamplingPop
ORDER BY
    FirstMonth DESC,
    Edition,
    LastHospID,
    LastHospSeqNo,
    ID
END
GO
/****** Object:  StoredProcedure [dbo].[UpdateApplyWeekCount]    Script Date: 2021/7/16 下午 10:10:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO











CREATE PROCEDURE [dbo].[UpdateApplyWeekCount] 
@GetTranDateS char(8),
@GetTranDateE char(8)
AS
Begin
    --開藥申報(OP)
update iniOpDtl set iniOpDtl.MedApply='1'
    from iniOpOrd
where iniOpDtl.data_id=iniOpOrd.data_id
  and iniOpDtl.fee_ym=iniOpOrd.fee_ym
  and (len(iniOpOrd.order_code)=10
   or iniOpOrd.order_code in ('E1006C','E1007C'))
  and iniOpDtl.tran_date>=@GetTranDateS and iniOpDtl.tran_date<=@GetTranDateE

--衛教申報(OP)
update iniOpDtl set iniOpDtl.InstructApply='1'
    from iniOpOrd
where iniOpDtl.data_id=iniOpOrd.data_id
  and iniOpDtl.fee_ym=iniOpOrd.fee_ym
  and iniOpOrd.order_code in ('E1022C')
  and iniOpDtl.tran_date>=@GetTranDateS and iniOpDtl.tran_date<=@GetTranDateE

--追蹤申報(OP)
update iniOpDtl set iniOpDtl.TraceApply='1'
    from iniOpOrd
where iniOpDtl.data_id=iniOpOrd.data_id
  and iniOpDtl.fee_ym=iniOpOrd.fee_ym
  and iniOpOrd.order_code in ('E1021C')
  and iniOpDtl.tran_date>=@GetTranDateS and iniOpDtl.tran_date<=@GetTranDateE

update iniOpDtl set iniOpDtl.TraceApply='2'
    from iniOpOrd
where iniOpDtl.data_id=iniOpOrd.data_id
  and iniOpDtl.fee_ym=iniOpOrd.fee_ym
  and iniOpOrd.order_code in ('E1023C')
  and iniOpDtl.tran_date>=@GetTranDateS and iniOpDtl.tran_date<=@GetTranDateE

update iniOpDtl set iniOpDtl.TraceApply='3'
    from iniOpOrd
where iniOpDtl.data_id=iniOpOrd.data_id
  and iniOpDtl.fee_ym=iniOpOrd.fee_ym
  and iniOpOrd.order_code in ('E1024C')
  and iniOpDtl.tran_date>=@GetTranDateS and iniOpDtl.tran_date<=@GetTranDateE

update iniOpDtl set iniOpDtl.TraceApply='4'
    from iniOpOrd
where iniOpDtl.data_id=iniOpOrd.data_id
  and iniOpDtl.fee_ym=iniOpOrd.fee_ym
  and iniOpOrd.order_code in ('E1025C')
  and iniOpDtl.tran_date>=@GetTranDateS and iniOpDtl.tran_date<=@GetTranDateE

update iniOpDtl set iniOpDtl.TraceApply='5'
    from iniOpOrd
where iniOpDtl.data_id=iniOpOrd.data_id
  and iniOpDtl.fee_ym=iniOpOrd.fee_ym
  and iniOpOrd.order_code in ('E1026C')
  and iniOpDtl.tran_date>=@GetTranDateS and iniOpDtl.tran_date<=@GetTranDateE

--開藥申報(DR)
update iniDrDtl set iniDrDtl.MedApply='1'
    from iniDrOrd
where iniDrDtl.data_id=iniDrOrd.data_id
  and iniDrDtl.fee_ym=iniDrOrd.fee_ym
  and len(iniDrOrd.order_code)=10
  and (iniDrDtl.orig_hosp_id is null or upper(rtrim(iniDrDtl.orig_hosp_id)) = 'N')
  and iniDrDtl.tran_date>=@GetTranDateS and iniDrDtl.tran_date<=@GetTranDateE

--衛教申報(DR)
update iniDrDtl set iniDrDtl.InstructApply='1'
    from iniDrOrd
where iniDrDtl.data_id=iniDrOrd.data_id
  and iniDrDtl.fee_ym=iniDrOrd.fee_ym
  and iniDrOrd.order_code in ('E1022C')
  and iniDrDtl.tran_date>=@GetTranDateS and iniDrDtl.tran_date<=@GetTranDateE

--追蹤申報(DR)
update iniDrDtl set iniDrDtl.TraceApply='1'
    from iniDrOrd
where iniDrDtl.data_id=iniDrOrd.data_id
  and iniDrDtl.fee_ym=iniDrOrd.fee_ym
  and iniDrOrd.order_code in ('E1021C')
  and iniDrDtl.tran_date>=@GetTranDateS and iniDrDtl.tran_date<=@GetTranDateE

update iniDrDtl set iniDrDtl.TraceApply='2'
    from iniDrOrd
where iniDrDtl.data_id=iniDrOrd.data_id
  and iniDrDtl.fee_ym=iniDrOrd.fee_ym
  and iniDrOrd.order_code in ('E1023C')
  and iniDrDtl.tran_date>=@GetTranDateS and iniDrDtl.tran_date<=@GetTranDateE

update iniDrDtl set iniDrDtl.TraceApply='3'
    from iniDrOrd
where iniDrDtl.data_id=iniDrOrd.data_id
  and iniDrDtl.fee_ym=iniDrOrd.fee_ym
  and iniDrOrd.order_code in ('E1024C')
  and iniDrDtl.tran_date>=@GetTranDateS and iniDrDtl.tran_date<=@GetTranDateE

update iniDrDtl set iniDrDtl.TraceApply='4'
    from iniDrOrd
where iniDrDtl.data_id=iniDrOrd.data_id
  and iniDrDtl.fee_ym=iniDrOrd.fee_ym
  and iniDrOrd.order_code in ('E1025C')
  and iniDrDtl.tran_date>=@GetTranDateS and iniDrDtl.tran_date<=@GetTranDateE

update iniDrDtl set iniDrDtl.TraceApply='5'
    from iniDrOrd
where iniDrDtl.data_id=iniDrOrd.data_id
  and iniDrDtl.fee_ym=iniDrOrd.fee_ym
  and iniDrOrd.order_code in ('E1026C')
  and iniDrDtl.tran_date>=@GetTranDateS and iniDrDtl.tran_date<=@GetTranDateE

--釋出申報(DR)
update iniDrDtl set iniDrDtl.ReleaseApply='1'
where (iniDrDtl.orig_hosp_id is not null or upper(rtrim(iniDrDtl.orig_hosp_id)) <> 'N')
  and iniDrDtl.tran_date>=@GetTranDateS and iniDrDtl.tran_date<=@GetTranDateE
update iniDrDtl set iniDrDtl.ReleaseApply=null where upper(rtrim(orig_hosp_id)) = 'N'

--開藥週數(OP)
update iniOpDtl set iniOpDtl.WeekCount=MhbtQsData.CureWeek
    from MhbtQsData
where MhbtQsData.HospID=iniOpDtl.HospID
  and MhbtQsData.ID=iniOpDtl.id
  and MhbtQsData.Birthday=iniOpDtl.Birthday
  and MhbtQsData.FuncDate=iniOpDtl.func_date
  and MhbtQsData.Cure_Type = '1'
  and iniOpDtl.MedApply='1'
  and iniOpDtl.tran_date>=@GetTranDateS and iniOpDtl.tran_date<=@GetTranDateE

update iniOpDtl set iniOpDtl.WeekCount=1
where iniOpDtl.drug_days<=7
  and iniOpDtl.MedApply='1'
  and (iniOpDtl.WeekCount is null or rtrim(iniOpDtl.WeekCount) = '')
  and iniOpDtl.tran_date>=@GetTranDateS and iniOpDtl.tran_date<=@GetTranDateE

update iniOpDtl set iniOpDtl.WeekCount=2
where iniOpDtl.drug_days<=14
  and iniOpDtl.MedApply='1'
  and (iniOpDtl.WeekCount is null or rtrim(iniOpDtl.WeekCount) = '')
  and iniOpDtl.tran_date>=@GetTranDateS and iniOpDtl.tran_date<=@GetTranDateE

update iniOpDtl set iniOpDtl.WeekCount=3
where iniOpDtl.drug_days<=21
  and iniOpDtl.MedApply='1'
  and (iniOpDtl.WeekCount is null or rtrim(iniOpDtl.WeekCount) = '')
  and iniOpDtl.tran_date>=@GetTranDateS and iniOpDtl.tran_date<=@GetTranDateE

update iniOpDtl set iniOpDtl.WeekCount=4
where iniOpDtl.drug_days>=22
  and iniOpDtl.MedApply='1'
  and (iniOpDtl.WeekCount is null or rtrim(iniOpDtl.WeekCount) = '')
  and iniOpDtl.tran_date>=@GetTranDateS and iniOpDtl.tran_date<=@GetTranDateE

--開藥週數(DR)
update iniDrDtl set iniDrDtl.WeekCount=MhbtQsData.CureWeek
    from MhbtQsData
where MhbtQsData.HospID=iniDrDtl.HospID
  and MhbtQsData.ID=iniDrDtl.id
  and MhbtQsData.Birthday=iniDrDtl.Birthday
  and MhbtQsData.FuncDate=iniDrDtl.func_date
  and MhbtQsData.Cure_Type = '1'
  and iniDrDtl.MedApply='1'
  and iniDrDtl.tran_date>=@GetTranDateS and iniDrDtl.tran_date<=@GetTranDateE

update iniDrDtl set iniDrDtl.WeekCount=1
where iniDrDtl.drug_days<=7
  and iniDrDtl.MedApply='1'
  and (iniDrDtl.WeekCount is null or rtrim(iniDrDtl.WeekCount) = '')
  and iniDrDtl.tran_date>=@GetTranDateS and iniDrDtl.tran_date<=@GetTranDateE

update iniDrDtl set iniDrDtl.WeekCount=2
where iniDrDtl.drug_days<=14
  and iniDrDtl.MedApply='1'
  and (iniDrDtl.WeekCount is null or rtrim(iniDrDtl.WeekCount) = '')
  and iniDrDtl.tran_date>=@GetTranDateS and iniDrDtl.tran_date<=@GetTranDateE

update iniDrDtl set iniDrDtl.WeekCount=3
where iniDrDtl.drug_days<=21
  and iniDrDtl.MedApply='1'
  and (iniDrDtl.WeekCount is null or rtrim(iniDrDtl.WeekCount) = '')
  and iniDrDtl.tran_date>=@GetTranDateS and iniDrDtl.tran_date<=@GetTranDateE

update iniDrDtl set iniDrDtl.WeekCount=4
where iniDrDtl.drug_days>=22
  and iniDrDtl.MedApply='1'
  and (iniDrDtl.WeekCount is null or rtrim(iniDrDtl.WeekCount) = '')
  and iniDrDtl.tran_date>=@GetTranDateS and iniDrDtl.tran_date<=@GetTranDateE

End
GO
/****** Object:  StoredProcedure [dbo].[UpdateDrData]    Script Date: 2021/7/16 下午 10:10:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UpdateDrData]
@GetTranDateS char(8),
@GetTranDateE char(8)
AS
Begin
--OP
update iniDrDtl set iniDrDtl.FirstTreatDate=MedYearTB.FirstTreatDate,iniDrDtl.ExamYear=substring(MedYearTB.FirstTreatDate,1,4) from MedYearTB where iniDrDtl.data_id=MedYearTB.data_id and iniDrDtl.fee_ym=MedYearTB.fee_ym and iniDrDtl.tran_date=MedYearTB.tran_date and iniDrDtl.tran_date>= @GetTranDateS and iniDrDtl.tran_date<=@GetTranDateE and iniDrDtl.MedApply='1'
update iniDrDtl set iniDrDtl.FirstInstructDate=InstructYearTB.FirstInstructDate,iniDrDtl.InstructExamYear=substring(InstructYearTB.FirstInstructDate,1,4),iniDrDtl.InctructSerial=InstructYearTB.InctructSerial from InstructYearTB where iniDrDtl.data_id=InstructYearTB.data_id and iniDrDtl.fee_ym=InstructYearTB.fee_ym and iniDrDtl.tran_date=InstructYearTB.tran_date and iniDrDtl.tran_date>= @GetTranDateS and iniDrDtl.tran_date<=@GetTranDateE and iniDrDtl.InstructApply='1'

update iniDrDtl set iniDrDtl.ExamTime=MedTimeTB.ExamTime from MedTimeTB where iniDrDtl.data_id=MedTimeTB.data_id and iniDrDtl.fee_ym=MedTimeTB.fee_ym and iniDrDtl.tran_date=MedTimeTB.tran_date and iniDrDtl.tran_date>= @GetTranDateS and iniDrDtl.tran_date<=@GetTranDateE and iniDrDtl.MedApply='1'
update updDrDtl set updDrDtl.ExamTime=MedTimeTB.ExamTime from MedTimeTB where updDrDtl.data_id=MedTimeTB.data_id and updDrDtl.fee_ym=MedTimeTB.fee_ym and updDrDtl.tran_date=MedTimeTB.tran_date  and updDrDtl.tran_date>= @GetTranDateS and updDrDtl.tran_date<=@GetTranDateE and updDrDtl.MedApply='1'
update iniDrDtl set iniDrDtl.InstructExamTime=InstructTimeTB.ExamTime from InstructTimeTB where iniDrDtl.data_id=InstructTimeTB.data_id and iniDrDtl.fee_ym=InstructTimeTB.fee_ym and iniDrDtl.tran_date=InstructTimeTB.tran_date and iniDrDtl.tran_date>= @GetTranDateS and iniDrDtl.tran_date<=@GetTranDateE and iniDrDtl.InstructApply='1'
update updDrDtl set updDrDtl.InstructExamTime=InstructTimeTB.ExamTime from InstructTimeTB where updDrDtl.data_id=InstructTimeTB.data_id and updDrDtl.fee_ym=InstructTimeTB.fee_ym and updDrDtl.tran_date=InstructTimeTB.tran_date  and updDrDtl.tran_date>= @GetTranDateS and updDrDtl.tran_date<=@GetTranDateE and updDrDtl.InstructApply='1'
End
GO
/****** Object:  StoredProcedure [dbo].[UpdateDrProcess]    Script Date: 2021/7/16 下午 10:10:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UpdateDrProcess]
@GetID char(10),
@GetTranDateS char(8),
@GetTranDateE char(8)
AS
Declare @strData_id varchar(28)
Declare @strID char(10)
Declare @strHospID char(10)
Declare @strHospSeqNo char(2)
Declare @strFeeYM char(6)
Declare @strFunc_Date char(8)
Declare @intWeekCount int
Declare @strFirstTreatDate char(8)
Declare @strFirstInstructDate char(8)
Declare @intInctructSerial int
Declare @strMedApply char(1)
Declare @strInstructApply char(1)
Declare @strTran_Date char(8)

Declare @strGetData_id varchar(28)
Declare @strGetID char(10)
Declare @strGetHospID char(10)
Declare @strGetHospSeqNo char(2)
Declare @strGetFeeYM char(6)
Declare @strGetFunc_Date char(8)
Declare @intGetWeekCount int
Declare @strGetFirstTreatDate char(8)
Declare @strGetFirstInstructDate char(8)
Declare @intGetInctructSerial int
Declare @strGetMedApply char(1)
Declare @strGetInstructApply char(1)
Declare @strGetTran_Date char(8)

Declare @strFinFirstDate char(8)
Declare @intWkCount int
Declare @intDayCount int
Declare @intFinInctructSerial int
Declare @strChkFlg char(1)
Declare @strCntFlg char(1)

BEGIN


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[HealthTempTB]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[HealthTempTB]
CREATE TABLE [dbo].[HealthTempTB] (
    [data_id] [varchar] (28) ,
    [fee_ym] [char] (6) ,
    [id] [char] (10) ,
    [HospID] [char] (10) ,
    [HospSeqNo] [char] (2) ,
    [func_date] [char] (8) ,
    [WeekCount] [int] NULL,
    [FirstTreatDate] [char] (8) ,
    [FirstInstructDate] [char] (8) ,
    [InctructSerial] [int],
    [Tran_Date] [char] (8)
    )
CREATE NONCLUSTERED INDEX [INX_HealthTempTB] ON [dbo].[HealthTempTB]
(
	[Tran_Date] ASC,
	[id] ASC,
	[data_id] ASC,
	[fee_ym] ASC,
	[func_date] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[HealthTempTBExe]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[HealthTempTBExe]
CREATE TABLE [dbo].[HealthTempTBExe] (
    [data_id] [varchar] (28) ,
    [fee_ym] [char] (6) ,
    [id] [char] (10) ,
    [HospID] [char] (10) ,
    [HospSeqNo] [char] (2) ,
    [func_date] [char] (8) ,
    [WeekCount] [int] NULL,
    [FirstTreatDate] [char] (8) ,
    [FirstInstructDate] [char] (8) ,
    [InctructSerial] [int],
    [Tran_Date] [char] (8)
    )
CREATE NONCLUSTERED INDEX [INX_HealthTempTBExe] ON [dbo].[HealthTempTBExe]
(
	[Tran_Date] ASC,
	[id] ASC,
	[data_id] ASC,
	[fee_ym] ASC,
	[func_date] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

--iniDrDtl(療程初診日)
insert into HealthTempTB
select iniDrDtl.data_id,iniDrDtl.fee_ym,id,iniDrDtl.HospID,HospBasic.HospSeqNo,
       iniDrDtl.func_date,iniDrDtl.WeekCount,iniDrDtl.FirstTreatDate,iniDrDtl.FirstInstructDate,iniDrDtl.InctructSerial,iniDrDtl.tran_date
from iniDrDtl left join HospBasic on iniDrDtl.HospID=HospBasic.HospID and (case when HospBasic.HospID in ('0101090517') then replace(substring(iniDrDtl.data_id,21,2),'00','10') else substring(iniDrDtl.data_id,21,2) end)=HospBasic.HospSeqNo where iniDrDtl.id=@GetID and iniDrDtl.MedApply='1' and iniDrDtl.data_id+iniDrDtl.fee_ym not in (select data_id+fee_ym from updDrDtl)
    insert into HealthTempTB
select updDrDtl.data_id,updDrDtl.fee_ym,id,updDrDtl.HospID,HospBasic.HospSeqNo,
       updDrDtl.func_date,updDrDtl.WeekCount,updDrDtl.FirstTreatDate,updDrDtl.FirstInstructDate,updDrDtl.InctructSerial,updDrDtl.tran_date
from updDrDtl left join HospBasic on updDrDtl.HospID=HospBasic.HospID and (case when HospBasic.HospID in ('0101090517') then replace(substring(updDrDtl.data_id,21,2),'00','10') else substring(updDrDtl.data_id,21,2) end)=HospBasic.HospSeqNo where updDrDtl.id=@GetID and updDrDtl.MedApply='1'

    insert into HealthTempTBExe select * from HealthTempTB
delete from HealthTempTB

Declare HealthTempTBExe_Cursor Cursor For select data_id,[id],HospID,HospSeqNo,fee_ym,func_date,WeekCount,FirstTreatDate,FirstInstructDate,InctructSerial,tran_date from HealthTempTBExe order by func_date
    Open HealthTempTBExe_Cursor Fetch Next From HealthTempTBExe_Cursor Into @strData_id,@strID,@strHospID,@strHospSeqNo,@strFeeYM,@strFunc_Date,@intWeekCount,@strFirstTreatDate,@strFirstInstructDate,@intInctructSerial,@strTran_Date
                                              While @@Fetch_Status = 0
Begin
    set @strFinFirstDate = ''
    Declare HealthTempTB_Cursor Cursor For select top 1 data_id,[id],HospID,HospSeqNo,fee_ym,func_date,WeekCount,FirstTreatDate,FirstInstructDate,InctructSerial,tran_date from HealthTempTB order by FirstTreatDate desc
    Open HealthTempTB_Cursor Fetch Next From HealthTempTB_Cursor Into @strGetData_id,@strGetID,@strGetHospID,@strGetHospSeqNo,@strGetFeeYM,@strGetFunc_Date,@intGetWeekCount,@strGetFirstTreatDate,@strGetFirstInstructDate,@intGetInctructSerial,@strTran_Date
                                                       While @@Fetch_Status = 0
Begin 
        if @strGetFirstTreatDate is null or rtrim(@strGetFirstTreatDate) = ''
Begin
               set @strGetFirstTreatDate = @strFunc_Date
End
        set @intWkCount = (select sum(WeekCount) as WeekCount from HealthTempTB where FirstTreatDate=@strGetFirstTreatDate)        
        set @intDayCount = datediff("D", convert(datetime, substring(@strGetFirstTreatDate,1,4)+'/'+substring(@strGetFirstTreatDate,5,2)+'/'+ substring(@strGetFirstTreatDate,7,2),111), convert(datetime, substring(@strFunc_Date,1,4)+'/'+substring(@strFunc_Date,5,2)+'/'+ substring(@strFunc_Date,7,2),111))
        if (@strGetHospID <> @strHospID) or (@intWkCount >= 8 or (@intWkCount + @intWeekCount) > 8) or @intDayCount >= 90 or (substring(@strGetFeeYM,1,4) <> substring(@strFeeYM,1,4))
Begin
				set @strFinFirstDate = @strFunc_Date
End
else
Begin
				set @strFinFirstDate = @strGetFirstTreatDate
End
Fetch Next From HealthTempTB_Cursor Into @strGetData_id,@strGetID,@strGetHospID,@strGetHospSeqNo,@strGetFeeYM,@strGetFunc_Date,@intGetWeekCount,@strGetFirstTreatDate,@strGetFirstInstructDate,@intGetInctructSerial,@strGetTran_Date
End

Close HealthTempTB_Cursor
    Deallocate HealthTempTB_Cursor

    if @strFinFirstDate is null or rtrim(@strFinFirstDate)=''
Begin
			set @strFinFirstDate = @strFunc_Date
End
update HealthTempTBExe set FirstTreatDate=@strFinFirstDate where data_id=@strData_id and fee_ym=@strFeeYM
    insert into HealthTempTB select * from HealthTempTBExe where data_id=@strData_id and fee_ym=@strFeeYM
--update iniDrDtl set iniDrDtl.FirstTreatDate=HealthTempTBExe.FirstTreatDate,iniDrDtl.ExamYear=substring(HealthTempTBExe.FirstTreatDate,1,4) from HealthTempTBExe where iniDrDtl.data_id=HealthTempTBExe.data_id and iniDrDtl.fee_ym=HealthTempTBExe.fee_ym and iniDrDtl.tran_date>=@GetTranDateS and iniDrDtl.tran_date<=@GetTranDateE
delete from MedYearTB where data_id=@strData_id and fee_ym=@strFeeYM
    insert into MedYearTB select data_id,fee_ym,FirstTreatDate,tran_date from HealthTempTBExe where tran_date>=@GetTranDateS and tran_date<=@GetTranDateE

    Fetch Next From HealthTempTBExe_Cursor Into @strData_id,@strID,@strHospID,@strHospSeqNo,@strFeeYM,@strFunc_Date,@intWeekCount,@strFirstTreatDate,@strFirstInstructDate,@intInctructSerial,@strTran_Date
End
Close HealthTempTBExe_Cursor
    Deallocate HealthTempTBExe_Cursor

truncate table HealthTempTB
truncate table HealthTempTBExe



--iniDrDtl(衛教初診日、衛教序次)
insert into HealthTempTB
select iniDrDtl.data_id,iniDrDtl.fee_ym,id,iniDrDtl.HospID,HospBasic.HospSeqNo,
       iniDrDtl.func_date,iniDrDtl.WeekCount,iniDrDtl.FirstTreatDate,iniDrDtl.FirstInstructDate,iniDrDtl.InctructSerial,iniDrDtl.tran_date
from iniDrDtl left join HospBasic on iniDrDtl.HospID=HospBasic.HospID and (case when HospBasic.HospID in ('0101090517') then replace(substring(iniDrDtl.data_id,21,2),'00','10') else substring(iniDrDtl.data_id,21,2) end)=HospBasic.HospSeqNo where iniDrDtl.id=@GetID and iniDrDtl.InstructApply='1'  and iniDrDtl.data_id+iniDrDtl.fee_ym not in (select data_id+fee_ym from updOpDtl)
    insert into HealthTempTB
select updDrDtl.data_id,updDrDtl.fee_ym,id,updDrDtl.HospID,HospBasic.HospSeqNo,
       updDrDtl.func_date,updDrDtl.WeekCount,updDrDtl.FirstTreatDate,updDrDtl.FirstInstructDate,updDrDtl.InctructSerial,updDrDtl.tran_date
from updDrDtl left join HospBasic on updDrDtl.HospID=HospBasic.HospID and (case when HospBasic.HospID in ('0101090517') then replace(substring(updDrDtl.data_id,21,2),'00','10') else substring(updDrDtl.data_id,21,2) end)=HospBasic.HospSeqNo where updDrDtl.id=@GetID and updDrDtl.InstructApply='1'

    insert into HealthTempTBExe select * from HealthTempTB
delete from HealthTempTB

    set @strCntFlg = 0
Declare HealthTempTBExe_Cursor Cursor For select data_id,[id],HospID,HospSeqNo,fee_ym,func_date,WeekCount,FirstTreatDate,FirstInstructDate,InctructSerial,tran_date from HealthTempTBExe order by func_date
    Open HealthTempTBExe_Cursor Fetch Next From HealthTempTBExe_Cursor Into @strData_id,@strID,@strHospID,@strHospSeqNo,@strFeeYM,@strFunc_Date,@intWeekCount,@strFirstTreatDate,@strFirstInstructDate,@intInctructSerial,@strTran_Date
                                              While @@Fetch_Status = 0
Begin
    set @strFinFirstDate = ''
    Declare HealthTempTB_Cursor Cursor For select top 1 data_id,[id],HospID,HospSeqNo,fee_ym,func_date,WeekCount,FirstTreatDate,FirstInstructDate,InctructSerial,tran_date from HealthTempTB where func_date<@strFunc_Date order by func_date desc
    Open HealthTempTB_Cursor Fetch Next From HealthTempTB_Cursor Into @strGetData_id,@strGetID,@strGetHospID,@strGetHospSeqNo,@strGetFeeYM,@strGetFunc_Date,@intGetWeekCount,@strGetFirstTreatDate,@strGetFirstInstructDate,@intGetInctructSerial,@strGettran_date
                                                       While @@Fetch_Status = 0
Begin 
        if @strGetFirstInstructDate is null or rtrim(@strGetFirstInstructDate) = ''
Begin
               set @strGetFirstInstructDate = @strFunc_Date
			   set @intFinInctructSerial = 1
End
        if @intGetInctructSerial is null or rtrim(@intGetInctructSerial) = ''
Begin
              set @intFinInctructSerial = 1
End
else
Begin
              if @strCntFlg = 0
Begin
					set @intFinInctructSerial = @intGetInctructSerial
                    set @strCntFlg = 1
End
              set @intFinInctructSerial = @intFinInctructSerial + 1
End
        set @strChkFlg = 0
        set @intWkCount = (select sum(WeekCount) as WeekCount from HealthTempTB where FirstInstructDate=@strGetFirstInstructDate)
        set @intDayCount = datediff("D", convert(datetime, substring(@strGetFirstInstructDate,1,4)+'/'+substring(@strGetFirstInstructDate,5,2)+'/'+ substring(@strGetFirstInstructDate,7,2),111), convert(datetime, substring(@strFunc_Date,1,4)+'/'+substring(@strFunc_Date,5,2)+'/'+ substring(@strFunc_Date,7,2),111))
        if (@strGetHospID <> @strHospID) or (@strGetHospSeqNo <> @strHospSeqNo) or (@intDayCount >= 90) or (substring(@strGetFeeYM,1,4) <> substring(@strFeeYM,1,4))
Begin
				set @strFinFirstDate = @strFunc_Date
				set @intFinInctructSerial = 1
				set @strChkFlg = 1
End
else
Begin
				set @strFinFirstDate = @strGetFirstInstructDate
End

		--衛教序次
        if @strChkFlg = 0
Begin
				if @intFinInctructSerial < 6 and @intDayCount >= 30
Begin
						set @intFinInctructSerial = 6
End

				if @intFinInctructSerial > 8
Begin
						set @strFinFirstDate = @strFunc_Date
						set @intFinInctructSerial = 1
End
End

Fetch Next From HealthTempTB_Cursor Into @strGetData_id,@strGetID,@strGetHospID,@strGetHospSeqNo,@strGetFeeYM,@strGetFunc_Date,@intGetWeekCount,@strGetFirstTreatDate,@strGetFirstInstructDate,@intGetInctructSerial,@strGettran_date
End

Close HealthTempTB_Cursor
    Deallocate HealthTempTB_Cursor

    if @strFinFirstDate is null or rtrim(@strFinFirstDate)=''
Begin
			set @strFinFirstDate = @strFunc_Date
			set @intFinInctructSerial = 1
End
update HealthTempTBExe set FirstInstructDate=@strFinFirstDate where data_id=@strData_id and fee_ym=@strFeeYM
update HealthTempTBExe set InctructSerial=@intFinInctructSerial where data_id=@strData_id and fee_ym=@strFeeYM
    insert into HealthTempTB select * from HealthTempTBExe where data_id=@strData_id and fee_ym=@strFeeYM
--update iniDrDtl set iniDrDtl.FirstInstructDate=HealthTempTBExe.FirstInstructDate,iniDrDtl.InstructExamYear=substring(HealthTempTBExe.FirstInstructDate,1,4),iniDrDtl.InctructSerial=HealthTempTBExe.InctructSerial from HealthTempTBExe where iniDrDtl.data_id=HealthTempTBExe.data_id and iniDrDtl.fee_ym=HealthTempTBExe.fee_ym and iniDrDtl.tran_date>=@GetTranDateS and iniDrDtl.tran_date<=@GetTranDateE
delete from InstructYearTB where data_id=@strData_id and fee_ym=@strFeeYM
    insert into InstructYearTB select data_id,fee_ym,FirstInstructDate,InctructSerial,tran_date from HealthTempTBExe where tran_date>=@GetTranDateS and tran_date<=@GetTranDateE

    Fetch Next From HealthTempTBExe_Cursor Into @strData_id,@strID,@strHospID,@strHospSeqNo,@strFeeYM,@strFunc_Date,@intWeekCount,@strFirstTreatDate,@strFirstInstructDate,@intInctructSerial,@strTran_Date
End
Close HealthTempTBExe_Cursor
    Deallocate HealthTempTBExe_Cursor

END
GO
/****** Object:  StoredProcedure [dbo].[UpdateExamTimeDr]    Script Date: 2021/7/16 下午 10:10:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[UpdateExamTimeDr]
@strGetID char(10),
@strGetTranDateS char(8),
@strGetTranDateE char(8)
AS
Declare @strID char(10) 
Declare @strOldID char(10) 
Declare @strFirstDate char(8)
Declare @strExamYear char(4)
Declare @strOldExamYear char(4)
Declare @intExamTime int 

--療程次數
set @intExamTime=1
set @strOldID=''

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[HealthDataBaseCD]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[HealthDataBaseCD]
CREATE TABLE [dbo].[HealthDataBaseCD] (
    [data_id] [varchar] (28) COLLATE Chinese_Taiwan_Stroke_CI_AS NULL ,
    [ExamYear] [char] (4) COLLATE Chinese_Taiwan_Stroke_CI_AS NULL ,
    [fee_ym] [char] (6) COLLATE Chinese_Taiwan_Stroke_CI_AS NULL ,
    [ExamTime] [int] NULL ,
    [FirstTreatDate] [char] (8) COLLATE Chinese_Taiwan_Stroke_CI_AS NULL ,
    [id] [char] (10) COLLATE Chinese_Taiwan_Stroke_CI_AS NULL ,
    [tran_date] [char] (8) COLLATE Chinese_Taiwan_Stroke_CI_AS NULL )
CREATE NONCLUSTERED INDEX [INX_HealthDataBaseCD] ON [dbo].[HealthDataBaseCD]
(
    [data_id] ASC,
	[id] ASC,
	[fee_ym] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

insert into HealthDataBaseCD select data_id,ExamYear,fee_ym,0,FirstTreatDate,[id],tran_date from iniDrDtl where id=@strGetID and MedApply='1' and data_id not in (select data_id from updDrDtl where id=@strGetID)
                             insert into HealthDataBaseCD select data_id,ExamYear,fee_ym,0,FirstTreatDate,[id],tran_date from updDrDtl where ID=@strGetID and MedApply='1'
Declare HealthDataBaseCD_Cursor Cursor For select [id],FirstTreatDate,ExamYear from HealthDataBaseCD group by [id],FirstTreatDate,ExamYear order by [id],FirstTreatDate,ExamYear
    Open HealthDataBaseCD_Cursor Fetch Next From HealthDataBaseCD_Cursor Into @strID,@strFirstDate,@strExamYear
                                               While @@Fetch_Status = 0
Begin     
	if (@strOldID='' or @strOldID<>@strID) or (@strOldExamYear='' or @strOldExamYear<>@strExamYear)
Begin         
		set @strOldID=@strID
        set @strOldExamYear=@strExamYear  
		set @intExamTime=1
End
Else
Begin         
		set @intExamTime=@intExamTime+1
End
update HealthDataBaseCD set ExamTime=@intExamTime where [id]=@strID and FirstTreatDate=@strFirstDate
    Fetch Next From HealthDataBaseCD_Cursor Into @strID,@strFirstDate,@strExamYear
End
Close HealthDataBaseCD_Cursor
    Deallocate HealthDataBaseCD_Cursor 

--update iniDrDtl set iniDrDtl.ExamTime=HealthDataBaseCD.ExamTime from HealthDataBaseCD where iniDrDtl.data_id=HealthDataBaseCD.data_id and iniDrDtl.fee_ym=HealthDataBaseCD.fee_ym and iniDrDtl.tran_date>=@strGetTranDateS and iniDrDtl.tran_date<=@strGetTranDateE
--update updDrDtl set updDrDtl.ExamTime=HealthDataBaseCD.ExamTime from HealthDataBaseCD where updDrDtl.data_id=HealthDataBaseCD.data_id and updDrDtl.fee_ym=HealthDataBaseCD.fee_ym and updDrDtl.tran_date>=@strGetTranDateS and updDrDtl.tran_date<=@strGetTranDateE
insert into MedTimeTB select data_id,fee_ym,ExamTime,tran_date from HealthDataBaseCD where tran_date>=@strGetTranDateS and tran_date<=@strGetTranDateE


--衛教療程次數
    set @intExamTime=1
                      set @strOldID=''

                              if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[HealthDataBaseCD]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[HealthDataBaseCD]
CREATE TABLE [dbo].[HealthDataBaseCD] (
    [data_id] [varchar] (28) COLLATE Chinese_Taiwan_Stroke_CI_AS NULL ,
    [ExamYear] [char] (4) COLLATE Chinese_Taiwan_Stroke_CI_AS NULL ,
    [fee_ym] [char] (6) COLLATE Chinese_Taiwan_Stroke_CI_AS NULL ,
    [ExamTime] [int] NULL ,
    [FirstTreatDate] [char] (8) COLLATE Chinese_Taiwan_Stroke_CI_AS NULL ,
    [id] [char] (10) COLLATE Chinese_Taiwan_Stroke_CI_AS NULL ,
    [tran_date] [char] (8) COLLATE Chinese_Taiwan_Stroke_CI_AS NULL )
CREATE NONCLUSTERED INDEX [INX_HealthDataBaseCD] ON [dbo].[HealthDataBaseCD]
(
    [data_id] ASC,
	[id] ASC,
	[fee_ym] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

insert into HealthDataBaseCD select data_id,InstructExamYear,fee_ym,0,FirstInstructDate,[id],tran_date from iniDrDtl where id=@strGetID and InstructApply='1' and data_id not in (select data_id from updDrDtl where id=@strGetID)
                             insert into HealthDataBaseCD select data_id,InstructExamYear,fee_ym,0,FirstInstructDate,[id],tran_date from updDrDtl where ID=@strGetID and InstructApply='1'
Declare HealthDataBaseCD_Cursor Cursor For select [id],FirstTreatDate,ExamYear from HealthDataBaseCD group by [id],FirstTreatDate,ExamYear order by [id],FirstTreatDate,ExamYear
    Open HealthDataBaseCD_Cursor Fetch Next From HealthDataBaseCD_Cursor Into @strID,@strFirstDate,@strExamYear
                                               While @@Fetch_Status = 0
Begin     
	if (@strOldID='' or @strOldID<>@strID) or (@strOldExamYear='' or @strOldExamYear<>@strExamYear)
Begin         
		set @strOldID=@strID
        set @strOldExamYear=@strExamYear  
		set @intExamTime=1
End
Else
Begin         
		set @intExamTime=@intExamTime+1
End
update HealthDataBaseCD set ExamTime=@intExamTime where [id]=@strID and FirstTreatDate=@strFirstDate
    Fetch Next From HealthDataBaseCD_Cursor Into @strID,@strFirstDate,@strExamYear
End
Close HealthDataBaseCD_Cursor
    Deallocate HealthDataBaseCD_Cursor 

--update iniDrDtl set iniDrDtl.InstructExamTime=HealthDataBaseCD.ExamTime from HealthDataBaseCD where iniDrDtl.data_id=HealthDataBaseCD.data_id and iniDrDtl.fee_ym=HealthDataBaseCD.fee_ym and iniDrDtl.tran_date>=@strGetTranDateS and iniDrDtl.tran_date<=@strGetTranDateE
--update updDrDtl set updDrDtl.InstructExamTime=HealthDataBaseCD.ExamTime from HealthDataBaseCD where updDrDtl.data_id=HealthDataBaseCD.data_id and updDrDtl.fee_ym=HealthDataBaseCD.fee_ym and updDrDtl.tran_date>=@strGetTranDateS and updDrDtl.tran_date<=@strGetTranDateE
insert into InstructTimeTB select data_id,fee_ym,ExamTime,tran_date from HealthDataBaseCD where tran_date>=@strGetTranDateS and tran_date<=@strGetTranDateE
    GO
/****** Object:  StoredProcedure [dbo].[UpdateExamTimeOp]    Script Date: 2021/7/16 下午 10:10:23 ******/
                           SET ANSI_NULLS ON
                                   GO
                                   SET QUOTED_IDENTIFIER ON
                                   GO

CREATE   PROCEDURE [dbo].[UpdateExamTimeOp]
@strGetID char(10),
@strGetTranDateS char(8),
@strGetTranDateE char(8)
AS
Declare @strID char(10) 
Declare @strOldID char(10) 
Declare @strFirstDate char(8)
Declare @strExamYear char(4)
Declare @strOldExamYear char(4)
Declare @intExamTime int 

--療程次數
set @intExamTime=1
set @strOldID=''

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[HealthDataBaseCD]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[HealthDataBaseCD]
CREATE TABLE [dbo].[HealthDataBaseCD] (
    [data_id] [varchar] (28) COLLATE Chinese_Taiwan_Stroke_CI_AS NULL ,
    [ExamYear] [char] (4) COLLATE Chinese_Taiwan_Stroke_CI_AS NULL ,
    [fee_ym] [char] (6) COLLATE Chinese_Taiwan_Stroke_CI_AS NULL ,
    [ExamTime] [int] NULL ,
    [FirstTreatDate] [char] (8) COLLATE Chinese_Taiwan_Stroke_CI_AS NULL ,
    [id] [char] (10) COLLATE Chinese_Taiwan_Stroke_CI_AS NULL ,
    [tran_date] [char] (8) COLLATE Chinese_Taiwan_Stroke_CI_AS NULL )
CREATE NONCLUSTERED INDEX [INX_HealthDataBaseCD] ON [dbo].[HealthDataBaseCD]
(
    [data_id] ASC,
	[id] ASC,
	[fee_ym] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

insert into HealthDataBaseCD select data_id,ExamYear,fee_ym,0,FirstTreatDate,[id],tran_date from iniOpDtl where id=@strGetID and MedApply='1' and data_id not in (select data_id from updOpDtl where id=@strGetID)
                             insert into HealthDataBaseCD select data_id,ExamYear,fee_ym,0,FirstTreatDate,[id],tran_date from updOpDtl where ID=@strGetID and MedApply='1'
Declare HealthDataBaseCD_Cursor Cursor For select [id],FirstTreatDate,ExamYear from HealthDataBaseCD group by [id],FirstTreatDate,ExamYear order by [id],FirstTreatDate,ExamYear
    Open HealthDataBaseCD_Cursor Fetch Next From HealthDataBaseCD_Cursor Into @strID,@strFirstDate,@strExamYear
                                               While @@Fetch_Status = 0
Begin     
	if (@strOldID='' or @strOldID<>@strID) or (@strOldExamYear='' or @strOldExamYear<>@strExamYear)
Begin         
		set @strOldID=@strID
        set @strOldExamYear=@strExamYear  
		set @intExamTime=1
End
Else
Begin         
		set @intExamTime=@intExamTime+1
End
update HealthDataBaseCD set ExamTime=@intExamTime where [id]=@strID and FirstTreatDate=@strFirstDate
    Fetch Next From HealthDataBaseCD_Cursor Into @strID,@strFirstDate,@strExamYear
End
Close HealthDataBaseCD_Cursor
    Deallocate HealthDataBaseCD_Cursor 

--update iniOpDtl set iniOpDtl.ExamTime=HealthDataBaseCD.ExamTime from HealthDataBaseCD where iniOpDtl.data_id=HealthDataBaseCD.data_id and iniOpDtl.fee_ym=HealthDataBaseCD.fee_ym and iniOpDtl.tran_date>=@strGetTranDateS and iniOpDtl.tran_date<=@strGetTranDateE
--update updOpDtl set updOpDtl.ExamTime=HealthDataBaseCD.ExamTime from HealthDataBaseCD where updOpDtl.data_id=HealthDataBaseCD.data_id and updOpDtl.fee_ym=HealthDataBaseCD.fee_ym and updOpDtl.tran_date>=@strGetTranDateS and updOpDtl.tran_date<=@strGetTranDateE
insert into MedTimeTB select data_id,fee_ym,ExamTime,tran_date from HealthDataBaseCD where tran_date>=@strGetTranDateS and tran_date<=@strGetTranDateE


--衛教療程次數
    set @intExamTime=1
                      set @strOldID=''

                              if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[HealthDataBaseCD]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[HealthDataBaseCD]
CREATE TABLE [dbo].[HealthDataBaseCD] (
    [data_id] [varchar] (28) COLLATE Chinese_Taiwan_Stroke_CI_AS NULL ,
    [ExamYear] [char] (4) COLLATE Chinese_Taiwan_Stroke_CI_AS NULL ,
    [fee_ym] [char] (6) COLLATE Chinese_Taiwan_Stroke_CI_AS NULL ,
    [ExamTime] [int] NULL ,
    [FirstTreatDate] [char] (8) COLLATE Chinese_Taiwan_Stroke_CI_AS NULL ,
    [id] [char] (10) COLLATE Chinese_Taiwan_Stroke_CI_AS NULL ,
    [tran_date] [char] (8) COLLATE Chinese_Taiwan_Stroke_CI_AS NULL )
CREATE NONCLUSTERED INDEX [INX_HealthDataBaseCD] ON [dbo].[HealthDataBaseCD]
(
    [data_id] ASC,
	[id] ASC,
	[fee_ym] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

insert into HealthDataBaseCD select data_id,InstructExamYear,fee_ym,0,FirstInstructDate,[id],tran_date from iniOpDtl where id=@strGetID and InstructApply='1' and data_id not in (select data_id from updOpDtl where id=@strGetID)
                             insert into HealthDataBaseCD select data_id,InstructExamYear,fee_ym,0,FirstInstructDate,[id],tran_date from updOpDtl where ID=@strGetID and InstructApply='1'
Declare HealthDataBaseCD_Cursor Cursor For select [id],FirstTreatDate,ExamYear from HealthDataBaseCD group by [id],FirstTreatDate,ExamYear order by [id],FirstTreatDate,ExamYear
    Open HealthDataBaseCD_Cursor Fetch Next From HealthDataBaseCD_Cursor Into @strID,@strFirstDate,@strExamYear
                                               While @@Fetch_Status = 0
Begin     
	if (@strOldID='' or @strOldID<>@strID) or (@strOldExamYear='' or @strOldExamYear<>@strExamYear)
Begin         
		set @strOldID=@strID
        set @strOldExamYear=@strExamYear  
		set @intExamTime=1
End
Else
Begin         
		set @intExamTime=@intExamTime+1
End
update HealthDataBaseCD set ExamTime=@intExamTime where [id]=@strID and FirstTreatDate=@strFirstDate
    Fetch Next From HealthDataBaseCD_Cursor Into @strID,@strFirstDate,@strExamYear
End
Close HealthDataBaseCD_Cursor
    Deallocate HealthDataBaseCD_Cursor 

--update iniOpDtl set iniOpDtl.InstructExamTime=HealthDataBaseCD.ExamTime from HealthDataBaseCD where iniOpDtl.data_id=HealthDataBaseCD.data_id and iniOpDtl.fee_ym=HealthDataBaseCD.fee_ym and iniOpDtl.tran_date>=@strGetTranDateS and iniOpDtl.tran_date<=@strGetTranDateE
--update updOpDtl set updOpDtl.InstructExamTime=HealthDataBaseCD.ExamTime from HealthDataBaseCD where updOpDtl.data_id=HealthDataBaseCD.data_id and updOpDtl.fee_ym=HealthDataBaseCD.fee_ym and updOpDtl.tran_date>=@strGetTranDateS and updOpDtl.tran_date<=@strGetTranDateE
insert into InstructTimeTB select data_id,fee_ym,ExamTime,tran_date from HealthDataBaseCD where tran_date>=@strGetTranDateS and tran_date<=@strGetTranDateE
    GO
/****** Object:  StoredProcedure [dbo].[UpdateMhbtQsData]    Script Date: 2021/7/16 下午 10:10:23 ******/
                           SET ANSI_NULLS ON
                                   GO
                                   SET QUOTED_IDENTIFIER ON
                                   GO
CREATE PROCEDURE [dbo].[UpdateMhbtQsData]

AS
BEGIN
Declare @strHospID char(10)
Declare @strID char(10)
Declare @strBirthday char(8)
Declare @strFirstTreatDate char(8)
Declare @strCureStage char(1)
Declare @strExamYear char(4)
Declare @strCureType char(1)

Declare MhbtQsData_Cursor Cursor For select HospID,ID,Birthday,min(FuncDate) as FirstTreatDate,CureStage,ExamYear,Cure_Type
                                     from MhbtQsData with (nolock)
                                     group by HospID,[ID],Birthday,CureStage,ExamYear,Cure_Type
                                                                                                 Open MhbtQsData_Cursor Fetch Next From MhbtQsData_Cursor Into @strHospID,@strID,@strBirthday,@strFirstTreatDate,@strCureStage,@strExamYear,@strCureType
                                                                                                 While @@Fetch_Status = 0
Begin
update MhbtQsData set FirstTreatDate=@strFirstTreatDate where HospID=@strHospID and ID=@strID and Birthday=@strBirthday and CureStage=@strCureStage and ExamYear=@strExamYear and Cure_Type=@strCureType

    Fetch Next From MhbtQsData_Cursor Into @strHospID,@strID,@strBirthday,@strFirstTreatDate,@strCureStage,@strExamYear,@strCureType
End
Close MhbtQsData_Cursor
    Deallocate MhbtQsData_Cursor

END
GO
/****** Object:  StoredProcedure [dbo].[UpdateOpData]    Script Date: 2021/7/16 下午 10:10:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UpdateOpData]
@GetTranDateS char(8),
@GetTranDateE char(8)
AS
Begin
--OP
update iniOpDtl set iniOpDtl.FirstTreatDate=MedYearTB.FirstTreatDate,iniOpDtl.ExamYear=substring(MedYearTB.FirstTreatDate,1,4) from MedYearTB where iniOpDtl.data_id=MedYearTB.data_id and iniOpDtl.fee_ym=MedYearTB.fee_ym and iniOpDtl.tran_date=MedYearTB.tran_date and iniOpDtl.tran_date>= @GetTranDateS and iniOpDtl.tran_date<=@GetTranDateE and iniOpDtl.MedApply='1'
update iniOpDtl set iniOpDtl.FirstInstructDate=InstructYearTB.FirstInstructDate,iniOpDtl.InstructExamYear=substring(InstructYearTB.FirstInstructDate,1,4),iniOpDtl.InctructSerial=InstructYearTB.InctructSerial from InstructYearTB where iniOpDtl.data_id=InstructYearTB.data_id and iniOpDtl.fee_ym=InstructYearTB.fee_ym and iniOpDtl.tran_date=InstructYearTB.tran_date and iniOpDtl.tran_date>= @GetTranDateS and iniOpDtl.tran_date<=@GetTranDateE and iniOpDtl.InstructApply='1'

update iniOpDtl set iniOpDtl.ExamTime=MedTimeTB.ExamTime from MedTimeTB where iniOpDtl.data_id=MedTimeTB.data_id and iniOpDtl.fee_ym=MedTimeTB.fee_ym and iniOpDtl.tran_date=MedTimeTB.tran_date and iniOpDtl.tran_date>= @GetTranDateS and iniOpDtl.tran_date<=@GetTranDateE and iniOpDtl.MedApply='1'
update updOpDtl set updOpDtl.ExamTime=MedTimeTB.ExamTime from MedTimeTB where updOpDtl.data_id=MedTimeTB.data_id and updOpDtl.fee_ym=MedTimeTB.fee_ym and updOpDtl.tran_date=MedTimeTB.tran_date  and updOpDtl.tran_date>= @GetTranDateS and updOpDtl.tran_date<=@GetTranDateE and updOpDtl.MedApply='1'
update iniOpDtl set iniOpDtl.InstructExamTime=InstructTimeTB.ExamTime from InstructTimeTB where iniOpDtl.data_id=InstructTimeTB.data_id and iniOpDtl.fee_ym=InstructTimeTB.fee_ym and iniOpDtl.tran_date=InstructTimeTB.tran_date and iniOpDtl.tran_date>= @GetTranDateS and iniOpDtl.tran_date<=@GetTranDateE and iniOpDtl.InstructApply='1'
update updOpDtl set updOpDtl.InstructExamTime=InstructTimeTB.ExamTime from InstructTimeTB where updOpDtl.data_id=InstructTimeTB.data_id and updOpDtl.fee_ym=InstructTimeTB.fee_ym and updOpDtl.tran_date=InstructTimeTB.tran_date  and updOpDtl.tran_date>= @GetTranDateS and updOpDtl.tran_date<=@GetTranDateE and updOpDtl.InstructApply='1'
End
GO
/****** Object:  StoredProcedure [dbo].[UpdateOpProcess]    Script Date: 2021/7/16 下午 10:10:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UpdateOpProcess]
@GetID char(10),
@GetTranDateS char(8),
@GetTranDateE char(8)
AS
Declare @strData_id varchar(28)
Declare @strID char(10)
Declare @strHospID char(10)
Declare @strHospSeqNo char(2)
Declare @strFeeYM char(6)
Declare @strFunc_Date char(8)
Declare @intWeekCount int
Declare @strFirstTreatDate char(8)
Declare @strFirstInstructDate char(8)
Declare @intInctructSerial int
Declare @strMedApply char(1)
Declare @strInstructApply char(1)
Declare @strTran_Date char(8)

Declare @strGetData_id varchar(28)
Declare @strGetID char(10)
Declare @strGetHospID char(10)
Declare @strGetHospSeqNo char(2)
Declare @strGetFeeYM char(6)
Declare @strGetFunc_Date char(8)
Declare @intGetWeekCount int
Declare @strGetFirstTreatDate char(8)
Declare @strGetFirstInstructDate char(8)
Declare @intGetInctructSerial int
Declare @strGetMedApply char(1)
Declare @strGetInstructApply char(1)
Declare @strGetTran_Date char(8)

Declare @strFinFirstDate char(8)
Declare @intWkCount int
Declare @intDayCount int
Declare @intFinInctructSerial int
Declare @strChkFlg char(1)
Declare @strCntFlg char(1)

BEGIN


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[HealthTempTB]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[HealthTempTB]
CREATE TABLE [dbo].[HealthTempTB] (
    [data_id] [varchar] (28) ,
    [fee_ym] [char] (6) ,
    [id] [char] (10) ,
    [HospID] [char] (10) ,
    [HospSeqNo] [char] (2) ,
    [func_date] [char] (8) ,
    [WeekCount] [int] NULL,
    [FirstTreatDate] [char] (8) ,
    [FirstInstructDate] [char] (8) ,
    [InctructSerial] [int],
    [Tran_Date] [char] (8)
    )
CREATE NONCLUSTERED INDEX [INX_HealthTempTB] ON [dbo].[HealthTempTB]
(
	[Tran_Date] ASC,
	[id] ASC,
	[data_id] ASC,
	[fee_ym] ASC,
	[func_date] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[HealthTempTBExe]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[HealthTempTBExe]
CREATE TABLE [dbo].[HealthTempTBExe] (
    [data_id] [varchar] (28) ,
    [fee_ym] [char] (6) ,
    [id] [char] (10) ,
    [HospID] [char] (10) ,
    [HospSeqNo] [char] (2) ,
    [func_date] [char] (8) ,
    [WeekCount] [int] NULL,
    [FirstTreatDate] [char] (8) ,
    [FirstInstructDate] [char] (8) ,
    [InctructSerial] [int],
    [Tran_Date] [char] (8)
    )
CREATE NONCLUSTERED INDEX [INX_HealthTempTBExe] ON [dbo].[HealthTempTBExe]
(
	[Tran_Date] ASC,
	[id] ASC,
	[data_id] ASC,
	[fee_ym] ASC,
	[func_date] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

--iniOpDtl(療程初診日)
insert into HealthTempTB
select iniOpDtl.data_id,iniOpDtl.fee_ym,id,iniOpDtl.HospID,HospBasic.HospSeqNo,
       iniOpDtl.func_date,iniOpDtl.WeekCount,iniOpDtl.FirstTreatDate,iniOpDtl.FirstInstructDate,iniOpDtl.InctructSerial,iniOpDtl.Tran_Date
from iniOpDtl left join HospBasic on iniOpDtl.HospID=HospBasic.HospID and (case when HospBasic.HospID in ('0101090517') then replace(substring(iniOpDtl.data_id,21,2),'00','10') else substring(iniOpDtl.data_id,21,2) end)=HospBasic.HospSeqNo where iniOpDtl.id=@GetID and iniOpDtl.MedApply='1' and iniOpDtl.data_id+iniOpDtl.fee_ym not in (select data_id+fee_ym from updOpDtl)
    insert into HealthTempTB
select updOpDtl.data_id,updOpDtl.fee_ym,id,updOpDtl.HospID,HospBasic.HospSeqNo,
       updOpDtl.func_date,updOpDtl.WeekCount,updOpDtl.FirstTreatDate,updOpDtl.FirstInstructDate,updOpDtl.InctructSerial,updOpDtl.Tran_Date
from updOpDtl left join HospBasic on updOpDtl.HospID=HospBasic.HospID and (case when HospBasic.HospID in ('0101090517') then replace(substring(UpdOpDtl.data_id,21,2),'00','10') else substring(UpdOpDtl.data_id,21,2) end)=HospBasic.HospSeqNo where updOpDtl.id=@GetID and updOpDtl.MedApply='1'
    
    insert into HealthTempTBExe select * from HealthTempTB
delete from HealthTempTB

Declare HealthTempTBExe_Cursor Cursor For select data_id,[id],HospID,HospSeqNo,fee_ym,func_date,WeekCount,FirstTreatDate,FirstInstructDate,InctructSerial,Tran_Date from HealthTempTBExe order by func_date
    Open HealthTempTBExe_Cursor Fetch Next From HealthTempTBExe_Cursor Into @strData_id,@strID,@strHospID,@strHospSeqNo,@strFeeYM,@strFunc_Date,@intWeekCount,@strFirstTreatDate,@strFirstInstructDate,@intInctructSerial,@strTran_Date
                                              While @@Fetch_Status = 0
Begin
    set @strFinFirstDate = ''
    Declare HealthTempTB_Cursor Cursor For select top 1 data_id,[id],HospID,HospSeqNo,fee_ym,func_date,WeekCount,FirstTreatDate,FirstInstructDate,InctructSerial,Tran_Date from HealthTempTB order by FirstTreatDate desc
    Open HealthTempTB_Cursor Fetch Next From HealthTempTB_Cursor Into @strGetData_id,@strGetID,@strGetHospID,@strGetHospSeqNo,@strGetFeeYM,@strGetFunc_Date,@intGetWeekCount,@strGetFirstTreatDate,@strGetFirstInstructDate,@intGetInctructSerial,@strGetTran_Date
                                                       While @@Fetch_Status = 0
Begin 
        if @strGetFirstTreatDate is null or rtrim(@strGetFirstTreatDate) = ''
Begin
               set @strGetFirstTreatDate = @strFunc_Date
End
        set @intWkCount = (select sum(WeekCount) as WeekCount from HealthTempTB where FirstTreatDate=@strGetFirstTreatDate)     
        set @intDayCount = datediff("D", convert(datetime, substring(@strGetFirstTreatDate,1,4)+'/'+substring(@strGetFirstTreatDate,5,2)+'/'+ substring(@strGetFirstTreatDate,7,2),111), convert(datetime, substring(@strFunc_Date,1,4)+'/'+substring(@strFunc_Date,5,2)+'/'+ substring(@strFunc_Date,7,2),111))
        if (@strGetHospID <> @strHospID) or (@strGetHospSeqNo <> @strHospSeqNo) or (@intWkCount >= 8 or (@intWkCount + @intWeekCount) > 8) or @intDayCount >= 90 or (substring(@strGetFeeYM,1,4) <> substring(@strFeeYM,1,4))
Begin
				set @strFinFirstDate = @strFunc_Date
End
else
Begin
				set @strFinFirstDate = @strGetFirstTreatDate
End
Fetch Next From HealthTempTB_Cursor Into @strGetData_id,@strGetID,@strGetHospID,@strGetHospSeqNo,@strGetFeeYM,@strGetFunc_Date,@intGetWeekCount,@strGetFirstTreatDate,@strGetFirstInstructDate,@intGetInctructSerial,@strGetTran_Date
End

Close HealthTempTB_Cursor
    Deallocate HealthTempTB_Cursor

    if @strFinFirstDate is null or rtrim(@strFinFirstDate)=''
Begin
			set @strFinFirstDate = @strFunc_Date
End
update HealthTempTBExe set FirstTreatDate=@strFinFirstDate where data_id=@strData_id and fee_ym=@strFeeYM
    insert into HealthTempTB select * from HealthTempTBExe where data_id=@strData_id and fee_ym=@strFeeYM
--update iniOpDtl set iniOpDtl.FirstTreatDate=HealthTempTBExe.FirstTreatDate,iniOpDtl.ExamYear=substring(HealthTempTBExe.FirstTreatDate,1,4) from HealthTempTBExe where iniOpDtl.data_id=HealthTempTBExe.data_id and iniOpDtl.fee_ym=HealthTempTBExe.fee_ym and iniOpDtl.tran_date>=@GetTranDateS and iniOpDtl.tran_date<=@GetTranDateE
delete from MedYearTB where data_id=@strData_id and fee_ym=@strFeeYM
    insert into MedYearTB select data_id,fee_ym,FirstTreatDate,tran_date from HealthTempTBExe where tran_date>=@GetTranDateS and tran_date<=@GetTranDateE

    Fetch Next From HealthTempTBExe_Cursor Into @strData_id,@strID,@strHospID,@strHospSeqNo,@strFeeYM,@strFunc_Date,@intWeekCount,@strFirstTreatDate,@strFirstInstructDate,@intInctructSerial,@strTran_Date
End
Close HealthTempTBExe_Cursor
    Deallocate HealthTempTBExe_Cursor

truncate table HealthTempTB
truncate table HealthTempTBExe


--iniOpDtl(衛教初診日、衛教序次)
insert into HealthTempTB
select iniOpDtl.data_id,iniOpDtl.fee_ym,id,iniOpDtl.HospID,HospBasic.HospSeqNo,
       iniOpDtl.func_date,iniOpDtl.WeekCount,iniOpDtl.FirstTreatDate,iniOpDtl.FirstInstructDate,iniOpDtl.InctructSerial,iniOpDtl.Tran_Date
from iniOpDtl left join HospBasic on iniOpDtl.HospID=HospBasic.HospID and (case when HospBasic.HospID in ('0101090517') then replace(substring(iniOpDtl.data_id,21,2),'00','10') else substring(iniOpDtl.data_id,21,2) end)=HospBasic.HospSeqNo where iniOpDtl.id=@GetID and iniOpDtl.InstructApply='1'  and iniOpDtl.data_id+iniOpDtl.fee_ym not in (select data_id+fee_ym from updOpDtl)
    insert into HealthTempTB
select updOpDtl.data_id,updOpDtl.fee_ym,id,updOpDtl.HospID,HospBasic.HospSeqNo,
       updOpDtl.func_date,updOpDtl.WeekCount,updOpDtl.FirstTreatDate,updOpDtl.FirstInstructDate,updOpDtl.InctructSerial,updOpDtl.Tran_Date
from updOpDtl left join HospBasic on updOpDtl.HospID=HospBasic.HospID and (case when HospBasic.HospID in ('0101090517') then replace(substring(updOpDtl.data_id,21,2),'00','10') else substring(updOpDtl.data_id,21,2) end)=HospBasic.HospSeqNo where updOpDtl.id=@GetID and updOpDtl.InstructApply='1'

    insert into HealthTempTBExe select * from HealthTempTB
delete from HealthTempTB

    set @strCntFlg = 0
Declare HealthTempTBExe_Cursor Cursor For select data_id,[id],HospID,HospSeqNo,fee_ym,func_date,WeekCount,FirstTreatDate,FirstInstructDate,InctructSerial,Tran_Date from HealthTempTBExe order by func_date
    Open HealthTempTBExe_Cursor Fetch Next From HealthTempTBExe_Cursor Into @strData_id,@strID,@strHospID,@strHospSeqNo,@strFeeYM,@strFunc_Date,@intWeekCount,@strFirstTreatDate,@strFirstInstructDate,@intInctructSerial,@strTran_Date
                                              While @@Fetch_Status = 0
Begin
    set @strFinFirstDate = ''
    Declare HealthTempTB_Cursor Cursor For select top 1 data_id,[id],HospID,HospSeqNo,fee_ym,func_date,WeekCount,FirstTreatDate,FirstInstructDate,InctructSerial,Tran_Date from HealthTempTB where func_date<@strFunc_Date  order by func_date desc
    Open HealthTempTB_Cursor Fetch Next From HealthTempTB_Cursor Into @strGetData_id,@strGetID,@strGetHospID,@strGetHospSeqNo,@strGetFeeYM,@strGetFunc_Date,@intGetWeekCount,@strGetFirstTreatDate,@strGetFirstInstructDate,@intGetInctructSerial,@strGetTran_Date
                                                       While @@Fetch_Status = 0
Begin 
        if @strGetFirstInstructDate is null or rtrim(@strGetFirstInstructDate) = ''
Begin
               set @strGetFirstInstructDate = @strFunc_Date
			   set @intFinInctructSerial = 1
End
        if @intGetInctructSerial is null or rtrim(@intGetInctructSerial) = ''
Begin
              set @intFinInctructSerial = 1
End
else
Begin
              if @strCntFlg = 0
Begin
					set @intFinInctructSerial = @intGetInctructSerial
                    set @strCntFlg = 1
End
              set @intFinInctructSerial = @intFinInctructSerial + 1
End
        set @strChkFlg = 0
        set @intWkCount = (select sum(WeekCount) as WeekCount from HealthTempTB where FirstInstructDate=@strGetFirstInstructDate)
        set @intDayCount = datediff("D", convert(datetime, substring(@strGetFirstInstructDate,1,4)+'/'+substring(@strGetFirstInstructDate,5,2)+'/'+ substring(@strGetFirstInstructDate,7,2),111), convert(datetime, substring(@strFunc_Date,1,4)+'/'+substring(@strFunc_Date,5,2)+'/'+ substring(@strFunc_Date,7,2),111))
        if (@strGetHospID <> @strHospID) or (@strGetHospSeqNo <> @strHospSeqNo) or (@intDayCount >= 90) or (substring(@strGetFeeYM,1,4) <> substring(@strFeeYM,1,4))
Begin
				set @strFinFirstDate = @strFunc_Date
				set @intFinInctructSerial = 1
				set @strChkFlg = 1
End
else
Begin
				set @strFinFirstDate = @strGetFirstInstructDate
End

		--衛教序次
        if @strChkFlg = 0
Begin
				if @intFinInctructSerial < 6 and @intDayCount >= 30
Begin
						set @intFinInctructSerial = 6
End

				if @intFinInctructSerial > 8
Begin
						set @strFinFirstDate = @strFunc_Date
						set @intFinInctructSerial = 1
End
End

Fetch Next From HealthTempTB_Cursor Into @strGetData_id,@strGetID,@strGetHospID,@strGetHospSeqNo,@strGetFeeYM,@strGetFunc_Date,@intGetWeekCount,@strGetFirstTreatDate,@strGetFirstInstructDate,@intGetInctructSerial,@strGetTran_Date
End

Close HealthTempTB_Cursor
    Deallocate HealthTempTB_Cursor

    if @strFinFirstDate is null or rtrim(@strFinFirstDate)=''
Begin
			set @strFinFirstDate = @strFunc_Date
			set @intFinInctructSerial = 1
End
update HealthTempTBExe set FirstInstructDate=@strFinFirstDate where data_id=@strData_id and fee_ym=@strFeeYM
update HealthTempTBExe set InctructSerial=@intFinInctructSerial where data_id=@strData_id and fee_ym=@strFeeYM
    insert into HealthTempTB select * from HealthTempTBExe where data_id=@strData_id and fee_ym=@strFeeYM
--update iniOpDtl set iniOpDtl.FirstInstructDate=HealthTempTBExe.FirstInstructDate,iniOpDtl.InstructExamYear=substring(HealthTempTBExe.FirstInstructDate,1,4),iniOpDtl.InctructSerial=HealthTempTBExe.InctructSerial from HealthTempTBExe where iniOpDtl.data_id=HealthTempTBExe.data_id and iniOpDtl.fee_ym=HealthTempTBExe.fee_ym and iniOpDtl.tran_date>=@GetTranDateS and iniOpDtl.tran_date<=@GetTranDateE
delete from InstructYearTB where data_id=@strData_id and fee_ym=@strFeeYM
    insert into InstructYearTB select data_id,fee_ym,FirstInstructDate,InctructSerial,tran_date from HealthTempTBExe where tran_date>=@GetTranDateS and tran_date<=@GetTranDateE

    Fetch Next From HealthTempTBExe_Cursor Into @strData_id,@strID,@strHospID,@strHospSeqNo,@strFeeYM,@strFunc_Date,@intWeekCount,@strFirstTreatDate,@strFirstInstructDate,@intInctructSerial,@strTran_Date
End
Close HealthTempTBExe_Cursor
    Deallocate HealthTempTBExe_Cursor

END
GO
/****** Object:  StoredProcedure [dbo].[UpdateSqlQuery]    Script Date: 2021/7/16 下午 10:10:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UpdateSqlQuery] 
@strFlag char(1)
AS
DECLARE @obj sysname , @sch sysname  
DECLARE @owner varchar(100)  
DECLARE @sql varchar(1000)   
  
DECLARE tblcur INSENSITIVE CURSOR FOR
SELECT schema_name(uid) as schema_name, object_name(id) as obj_name
FROM sysobjects o WHERE OBJECTPROPERTY(o.id, N'IsUserTable') = 1
                    AND object_name(id) in ('iniOpDtl','iniOpOrd','iniDrDtl','iniDrOrd','updOpDtl','updOpOrd','updDrDtl','updDrOrd')

    OPEN tblcur   
  
WHILE 1 = 1
BEGIN
FETCH tblcur INTO @sch, @obj
    IF @@fetch_status <0 BREAK   
  
    SET @sql = 'SELECT * FROM [' + @sch + '].[' + @obj + ']'  
    EXEC(@sql)
END   
  
DEALLOCATE tblcur
GO
/****** Object:  StoredProcedure [dbo].[UpdateTempTable]    Script Date: 2021/7/16 下午 10:10:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UpdateTempTable] 
AS
Begin
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[MedYearTB]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[MedYearTB]
CREATE TABLE [dbo].[MedYearTB] (
    [data_id] [varchar] (28) ,
    [fee_ym] [char] (6) ,
    [FirstTreatDate] [char] (8),
    [tran_date] [char] (8)
    )

    if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[InstructYearTB]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[InstructYearTB]
CREATE TABLE [dbo].[InstructYearTB] (
    [data_id] [varchar] (28) ,
    [fee_ym] [char] (6) ,
    [FirstInstructDate] [char] (8),
    [InctructSerial] int,
    [tran_date] [char] (8)
    )

    if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[MedTimeTB]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[MedTimeTB]
CREATE TABLE [dbo].[MedTimeTB] (
    [data_id] [varchar] (28) ,
    [fee_ym] [char] (6) ,
    [ExamTime] [int],
    [tran_date] [char] (8)
    )

    if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[InstructTimeTB]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[InstructTimeTB]
CREATE TABLE [dbo].[InstructTimeTB] (
    [data_id] [varchar] (28) ,
    [fee_ym] [char] (6) ,
    [ExamTime] [int],
    [tran_date] [char] (8)
    )
End
GO
