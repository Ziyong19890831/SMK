using SMK.Data;
using SMK.Web.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Yozian.DependencyInjectionPlus.Attributes;
using Dapper;
using Microsoft.EntityFrameworkCore;
using SMK.Web.Extensions;
using SMK.Data.Dto;
using System.Linq;

namespace SMK.Web.Services.Foundation
{
    [ScopedService]
    public class ProReviewService : GenericService
    {
        private readonly IDbConnection _conn = null;
        public ProReviewService(SMKWEBContext context, SessionManager smgr)
            : base(context, smgr)
        {
            _conn = context.Database.GetDbConnection();
        }

        /// <summary>
        /// 查詢專審名冊
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<LogicRtnModel<PagedModel<ProReviewDto>>> QueryProReviewData(ProReviewQueryModel model)
        {
            LogicRtnModel<PagedModel<ProReviewDto>> rtnModel = new LogicRtnModel<PagedModel<ProReviewDto>>
            {
                IsSuccess = true,
                Data = null
            };
            var SFeeYM = model.SFeeYM?.ToYYYYMMFromTaiwan();
            var EFeeYM = model.EFeeYM?.ToYYYYMMFromTaiwan();

            if (string.IsNullOrEmpty(SFeeYM) || string.IsNullOrEmpty(EFeeYM))
            {
                rtnModel.IsSuccess = false;
                rtnModel.ErrMsg = "請填寫填報年月";
                return rtnModel;
            }

            #region SQL 
            string sqlstr = $@";with Sourct as (
	select f.fee_ym as FeeYN,f.data_id,f.samplingno as SamplingNo
,f.hospid as HospId
,f.hospname as HospName
,f.name as UserName,f.id as UserId,f.func_date as FuncDate
,f.firsttreatdate,f.seq_no,f.drug_days,f.cure_dot,
			f.dsvc_dot,f.other_part_amt,f.appl_dot,f.part_amt,f.finish_amt,                                                             
 			case when len(f.reviewremark) > 0 then                                                                                      
 				substring(f.reviewremark,1,len(f.reviewremark)-1)                                                                       
 			end as reviewremark,f.reviewamt as ReviewAmt,f.review as Review
,f.reviewdate,                                                                      
 			case when len(f.appealsremark) > 0 then                                                                                     
 				substring(f.appealsremark,1,len(f.appealsremark)-1)                                                                     
 			end as appealsremark,f.appealsamt as AppealsAmt,f.appeals as Appeals,f.appealsdate,f.chkflg                                                          
	  from (
			select a.fee_ym,a.data_id,a.samplingno,c.hospid,d.hospname,case when rtrim(isnull(e.name,''))='' then f.name else e.name end as name,c.id,c.func_date,c.firsttreatdate,c.seq_no,c.drug_days,c.cure_dot,                                               
 					c.dsvc_dot,c.other_part_amt,c.appl_dot,c.part_amt,c.finish_amt,                                                     
 					(select cast(reviewremark as varchar) + '、' from SamplingData where fee_ym = a.fee_ym and data_id = a.data_id for xml path('')) as reviewremark,sum(b.reviewamt) as reviewamt,max(b.review) as review,max(b.reviewdate) as reviewdate,        
 					(select cast(appealsremark as varchar) + '、' from SamplingData where fee_ym = a.fee_ym and data_id = a.data_id for xml path(''))  as appealsremark,sum(b.appealsamt) as appealsamt,max(b.appeals) as appeals,max(b.appealsdate) as appealsdate, 
 					case when a.chkflg='1' then 'V' else ' ' end as chkflg                                                     
 				from (select * from SamplingList where fee_ym >= @Sfee_ym and fee_ym <= @Efee_ym) a             
 				join (select * from SamplingData where fee_ym >= @Sfee_ym and fee_ym <= @Efee_ym) b on a.fee_ym = b.fee_ym and a.data_id = b.data_id                                                                                      
 				join (
					select a.fee_ym,a.data_id,a.hospid,a.id,a.birthday,a.func_date,a.firsttreatdate,a.seq_no,a.drug_days,a.cure_dot,a.dsvc_dot,null as other_part_amt,a.appl_dot,a.part_amt,a.appl_dot-a.part_amt as finish_amt                                 
 					from iniOpDtl a 
					where a.fee_ym >=@Sfee_ym and a.fee_ym <= @Efee_ym and a.fee_ym+a.data_id not in (select fee_ym+data_id from updOpDtl)
 					union all                                                                                                           
 					select a.fee_ym,a.data_id,a.hospid,a.id,a.birthday,a.func_date,a.firsttreatdate,a.seq_no,a.drug_days,a.cure_dot,a.dsvc_dot,other_part_amt,a.appl_dot,a.part_amt,a.appl_dot-a.part_amt as finish_amt                                           
 					from iniDrDtl a 
					where a.fee_ym >=@Sfee_ym and a.fee_ym <= @Efee_ym and a.fee_ym+a.data_id not in (select fee_ym+data_id from updDrDtl)
				) c on a.fee_ym = c.fee_ym and a.data_id = c.data_id                                                               
 				left join HospBasic d on c.hospid = d.hospid and d.hospseqno = '00'                                                     
 				left join MhbtAgentPatient e on c.hospid=e.hospid and c.id=e.id and c.birthday=e.birthday                               
				left join (select distinct a.hospid,a.id,a.birthday,max(a.name) as name                                                 
							 from
							  (
								  select hospid,id,birthday,name 
								  from iniOpDtl I
								  where name is not null
								   AND exists( select 'x' 
									   from SamplingList a 
									   JOIN DtlWithSet b ON a.data_id = b.data_id  AND a.fee_ym = b.fee_ym 
									   where a.fee_ym >=@Sfee_ym and a.fee_ym <= @Efee_ym AND b.ID=I.id)                             
								  union                                                                                                
								  select hospid,id,birthday,name 
								  from iniDrDtl I
								  where name is not null
								   AND exists( select 'x' 
									   from SamplingList a 
									   JOIN DtlWithSet b ON a.data_id = b.data_id  AND a.fee_ym = b.fee_ym 
									   where a.fee_ym >=@Sfee_ym and a.fee_ym <= @Efee_ym AND b.ID=I.id) 
							  ) a group by a.hospid,a.id,a.birthday
		) f on c.hospid=f.hospid and c.id=f.id and c.birthday=f.birthday
		{(string.IsNullOrEmpty(model.HospID) ? "" : "where c.hospid = @wkHospID")}
		group by a.fee_ym,a.data_id,a.samplingno,c.hospid,d.hospname,e.name,f.name,c.id,c.func_date,c.firsttreatdate,c.seq_no,c.drug_days,c.cure_dot,                                                                                                 
		c.dsvc_dot,c.other_part_amt,c.appl_dot,c.part_amt,c.finish_amt,a.chkflg
	) f      
)
 select S.*
  from SamplingData a                                                                                                                                                                
join (select a.fee_ym,a.data_id,a.order_seq_no,a.order_type,a.order_drug_day,a.order_code,a.drug_num,a.drug_fre,a.drug_path,a.order_qty,a.order_uprice,a.order_dot from iniOpOrd a   
      union                                                                                                                                                                          
      select a.fee_ym,a.data_id,a.order_seq_no,a.order_type,a.order_drug_day,a.order_code,a.drug_num,a.drug_fre,a.drug_path,a.order_qty,a.order_uprice,a.order_dot from iniDrOrd a   
      ) b on a.fee_ym = b.fee_ym and a.data_id = b.data_id and a.order_seq_no = b.order_seq_no                                                                                       
left join GenOrderCode c on b.order_code = c.ordercode 
join Sourct S on( a.fee_ym=S.FeeYN AND a.data_id=S.data_id)
order by b.order_seq_no";
            #endregion
            var data = await _conn.QueryAsync<ProReviewDto>(sqlstr, new
            {
                Sfee_ym = SFeeYM,
                Efee_ym = EFeeYM,
                wkHospID = model.HospID
            });

            return await QueryPaging(model.get(), data.AsAsyncQueryable());
        }
    }
}
