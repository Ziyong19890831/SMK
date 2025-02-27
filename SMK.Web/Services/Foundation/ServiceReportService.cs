using Dapper;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using SMK.Data;
using SMK.Data.Dto;
using SMK.Data.Entity;
using SMK.Web.Extensions;
using SMK.Web.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Yozian.DependencyInjectionPlus.Attributes;
using Yozian.Extension;

namespace SMK.Web.Services.Foundation
{
    /// <summary>
    /// 服務人次
    /// </summary>
    [ScopedService]
    public class ServiceReportService : GenericService
    {
        private readonly IDbConnection _conn = null;
        public ServiceReportService(SMKWEBContext context, SessionManager smgr)
            : base(context, smgr)
        {
            _conn = context.Database.GetDbConnection();
        }
        /// <summary>
        /// 查詢服務人次
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<LogicRtnModel<PagedModel<ServiceReportViewModel>>> QueryServiceReportData(ServiceReportQueryModel model)
        {
            LogicRtnModel<PagedModel<ServiceReportViewModel>> rtnModel = new LogicRtnModel<PagedModel<ServiceReportViewModel>>
            {
                IsSuccess = true,
                Data = null
            };
            //if (string.IsNullOrEmpty(model.HospId))
            //{
            //    rtnModel.IsSuccess = false;
            //    rtnModel.ErrMsg = "請填寫機構代碼";
            //    return rtnModel;
            //}
            if (string.IsNullOrEmpty(model.Year))
            {
                rtnModel.IsSuccess = false;
                rtnModel.ErrMsg = "請填寫年度";
                return rtnModel;
            }
            #region 舊版商業邏輯
            //string sqlstr = $@";with ini as (
            //                     --藥局
            //                     SELECT HOSPID,SUM (CASE  WHEN  MedApply  =  '1'  THEN  1  ELSE  0  END)  AS  TreatCount
            //                         ,SUM (CASE  WHEN  InstructApply =  '1'  THEN  1  ELSE  0  END)  AS  InstructCount
            //                      FROM iniDrDtl
            //                      WHERE {(string.IsNullOrEmpty(model.HospId) ? " 1=1 " : " HOSPID = @HospId  ")} and HospSeqNo=@HospSeqNo and func_date between @SFuncDate and @EFuncDate
            //                      group by HOSPID
            //                     union all
            //                     --非藥局
            //                     SELECT HOSPID, SUM (CASE  WHEN  MedApply='1' THEN 1 ELSE 0 END) AS TreatCount
            //                         ,SUM (CASE  WHEN  InstructApply ='1' THEN 1 ELSE 0 END) AS InstructCount
            //                      FROM iniOpDtl
            //                      WHERE  {(string.IsNullOrEmpty(model.HospId) ? " 1=1 " : " HOSPID = @HospId  ")} and HospSeqNo=@HospSeqNo and func_date between @SFuncDate and @EFuncDate
            //                      group by HOSPID
            //                    ),
            //                    HospInfo as (
            //                     select distinct B.HospID,B.HospName,C.HospContName,C.HospContType,B.HospSeqNo,
            //                     C.QualityDefaultCount,C.QualityImproveCount,
            //                     (CASE  WHEN  HC.SMKContractType  =  '01'  THEN  1  ELSE  0  END) AS IsDefaultCount,
            //                     (CASE  WHEN  HC1.SMKContractType ='02'  THEN  1  ELSE  0  END) AS IsTopTreatCount,
            //                     (CASE  WHEN  HC2.SMKContractType ='03'  THEN  1  ELSE  0  END) AS IsTopInstructCount
            //                     from hospbasic B 
            //                     join GenHospCont C on B.LastContType=C.HospContType
            //                     left join HospContract HC on HC.HospID=B.HospID AND HC.SMKContractType='01'
            //                     left join HospContract HC1 on HC1.HospID=B.HospID AND HC1.SMKContractType='02'
            //                     left join HospContract HC2 on HC2.HospID=B.HospID AND HC2.SMKContractType='03'
            //                        where 1=1 
            //                     {(string.IsNullOrEmpty(model.HospId) ? "" : " AND B.HospID=@HospId ")}
            //                        {(string.IsNullOrEmpty(model.HospSeqNo) ? "" : " AND B.HospSeqNo=@HospSeqNo ")}
            //                    )
            //                    select H.*,ISNULL(I.InstructCount,0) as InstructCount,ISNULL(I.TreatCount,0) as TreatCount
            //                    from HospInfo H 
            //                    join ini I on(H.HospID =I.HospID)";
            //#endregion
            //var data = await _conn.QueryAsync<ServiceReportViewModel>(sqlstr, new
            //{
            //    HospId = model.HospId,
            //    HospSeqNo = string.IsNullOrEmpty(model.HospSeqNo) ? "00" : model.HospSeqNo,
            //    SFuncDate = (model.Year + "0101").ToYYYYMMFromTaiwan(),
            //    EFuncDate = (model.Year + "1231").ToYYYYMMFromTaiwan(),
            //});
            #endregion
            //--Step 1: 每年人次上限 (變更代碼或終止合約機構仍保留)
            var Step01 = context.QsQuota
                        .WhereWhen(!string.IsNullOrWhiteSpace(model.HospId), x => x.HOSP_ID == model.HospId)
                        .WhereWhen(!string.IsNullOrWhiteSpace(model.HospSeqNo), x => x.HOSP_SEQ_NO == model.HospSeqNo)
                        .WhereWhen(!string.IsNullOrWhiteSpace(model.Year), x => x.YEARS == model.Year)
                        .Where(x => x.HOSP_ID != "3501200000")
                        .Join(context.HospBasic, QSQ => new { Key1 = QSQ.HOSP_ID, Key2 = QSQ.HOSP_SEQ_NO }
                               , HB => new { Key1 = HB.HospId, Key2 = HB.HospSeqNo }, (QSQ, HB) => new { QSQ, HB })
                        .Join(context.GenHospCont, Data => Data.HB.LastContType
                               , GH => GH.HospContType, (Data, GH) => new { Data, GH })
                        .Where(x => x.Data.HB.HospStatus != "3")
                        .GroupBy(x => new
                        {
                            x.Data.QSQ.HOSP_ID,
                            x.Data.QSQ.HOSP_SEQ_NO,
                            x.Data.HB.HospName,
                            x.GH.HospContName,
                            x.Data.QSQ.YEARS,
                            x.Data.HB.HospStatus,
                        })
                        .Select(group => new ServiceReportViewModel()
                        {
                            HospID = group.Key.HOSP_ID,
                            HospSeqNo = group.Key.HOSP_SEQ_NO,
                            HospName = group.Key.HospName,
                            HospContName = group.Key.HospContName,
                            Year = group.Key.YEARS,
                            HospStatus = group.Key.HospStatus,
                            TopTreatCount = (int)group.Sum(x => x.Data.QSQ.CURE_TYPE == 1 ? x.Data.QSQ.QUOTA : 0),
                            TopInstructCount = (int)group.Sum(x => x.Data.QSQ.CURE_TYPE == 2 ? x.Data.QSQ.QUOTA : 0),
                        }).ToList();

            //--Step 2: 實際戒菸服務人次統計 (變更代碼或終止合約之機構代碼仍保留，其服務量累積到新代碼)
            var sqlStep02 = $@"
                            SELECT LastHospID AS HospID, LastHospSeqNo AS HospSeqNo,
                            SUM (CASE WHEN MedApply LIKE '1' THEN 1 ELSE 0 END) AS TreatReal,
                            SUM (CASE WHEN InstructApply LIKE '1' THEN 1 ELSE 0 END) AS InstructReal
                            FROM DtlOfB7 DTL LEFT JOIN HospBasic B ON DTL.HOSPID = B.HospID AND DTL.HospSeqNo = B.HospSeqNo
                            WHERE (MedApply = '1' OR InstructApply = '1') 
                            and {(string.IsNullOrEmpty(model.Year) ? "1 = 1" : "(substring(ltrim(rtrim(Fee_YM)), 1, 4) -1911 = @strCurYear)")}
                            and {(string.IsNullOrEmpty(model.HospId) ? "1 = 1" : "DTL.HospID = @HospId")}
                            and {(string.IsNullOrEmpty(model.HospSeqNo) ? "1 = 1" : "DTL.HospSeqNo = @HospSeqNo")}
                            GROUP BY LastHospID, LastHospSeqNo
                            ";
            var sqlStep02Data = await _conn.QueryAsync<ServiceReportViewModel>(sqlStep02, new
            {
                HospId = model.HospId,
                HospSeqNo = string.IsNullOrEmpty(model.HospSeqNo) ? "00" : model.HospSeqNo,
                strCurYear = model.Year,
            });

            //最後step01 left join sqlStep02Data
            var Alldata = (from Step01Data in Step01
                           join sqlStep02Datas in sqlStep02Data
                           on new { Key1 = Step01Data.HospID, Key2 = Step01Data.HospSeqNo }
                           equals new { Key1 = sqlStep02Datas.HospID, Key2 = sqlStep02Datas.HospSeqNo }
                           into joinedData
                           from sqlStep02Datas in joinedData.DefaultIfEmpty()
                           select new ServiceReportViewModel()
                           {
                               HospID = Step01Data.HospID,
                               HospSeqNo = Step01Data.HospSeqNo,
                               HospName = Step01Data.HospName,
                               HospContName = Step01Data.HospContName,
                               Year = Step01Data.Year,
                               TopTreatCount = Step01Data.TopTreatCount,
                               TopInstructCount = Step01Data.TopInstructCount,
                               TreatReal = sqlStep02Datas?.TreatReal ?? 0,
                               InstructReal = sqlStep02Datas?.InstructReal ?? 0,
                               TreatSussueRate = Math.Round((double)(sqlStep02Datas?.TreatReal ?? 0) / Math.Max(Step01Data.TopTreatCount, 1) * 100, 2),
                               InstructSussueRate = Math.Round((double)(sqlStep02Datas?.InstructReal ?? 0) / Math.Max(Step01Data.TopInstructCount, 1) * 100, 2),
                           }).ToList();
            //資料整理
            Alldata = Alldata.AsAsyncQueryable()
                     .WhereWhen(model.Rate != null, x => x.TreatSussueRate > model.Rate || x.InstructSussueRate > model.Rate)
                     .OrderBy(x=>x.HospID).ThenBy(e => e.HospSeqNo)
                     .ToList();

            return await QueryPaging(model.get(), Alldata.AsAsyncQueryable());
        }

    }
}
