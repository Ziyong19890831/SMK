using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using SMK.Data;
using SMK.Data.Utility;
using SMK.Web.Models;
using Yozian.DependencyInjectionPlus.Attributes;

namespace SMK.Web.Repositories
{
    [ScopedService]
    public class HospBasicRepository
    {
        private readonly string sql = @"
select distinct a.HospID,a.HospSeqNo,a.HospName,a.HospTel,a.HospFax,
       a.HospOwnName,a.HospOwnID,a.ZIP,a.HospAddress,a.Contact1,a.ContactTel1,
       isnull(b.cnt,0) as bcnt,isnull(c.cnt,0) as ccnt,isnull(i.cnt,0) as icnt,
       isnull(j.cnt,0) as jcnt,e.HospContName,f.BranchName,g.minHospStartDate, 
       h.maxHospEndDate
  from HospBasic a 
  left join (select z.HospID,z.HospSeqNo,count(1) as cnt from PrsnContract z join PrsnBasic y on z.PrsnID=y.PrsnID where z.CouldTreat='1' and (z.PrsnStartDate is not null and rtrim(z.PrsnStartDate) <> '') and (z.PrsnEndDate is null or rtrim(z.PrsnEndDate)='') group by z.HospID,z.HospSeqNo) b on a.HospID=b.HospID and a.HospSeqNo=b.HospSeqNo 
  left join (select z.HospID,z.HospSeqNo,count(1) as cnt from PrsnContract z join PrsnBasic y on z.PrsnID=y.PrsnID where z.CouldInstruct='1' and (z.PrsnStartDate is not null and rtrim(z.PrsnStartDate) <> '') and (z.PrsnEndDate is null or rtrim(z.PrsnEndDate)='') group by z.HospID,z.HospSeqNo) c on a.HospID=c.HospID and a.HospSeqNo=c.HospSeqNo 
  left join (select HospID,HospSeqNo,count(1) as cnt from HospContract where (HospEndDate is null or rtrim(HospEndDate)='') and SMKContractType='02' group by HospID,HospSeqNo) i on a.HospID=i.HospID and a.HospSeqNo=i.HospSeqNo 
  left join (select HospID,HospSeqNo,count(1) as cnt from HospContract where (HospEndDate is null or rtrim(HospEndDate)='') and SMKContractType='03' group by HospID,HospSeqno) j on a.HospID=j.HospID and a.HospSeqNo=j.HospSeqNo 
  left join GenHospCont e on a.LastContType=e.HospContType 
  left join GenBranch f on a.BranchNo=f.BranchNo
  left join (select a.HospID,a.HospSeqNo,min(b.HospStartDate) as minHospStartDate 
              from HospBasic a join HospContract b on a.HospID = b.HospID and a.HospSeqNo = b.HospSeqNo 
             where ((a.HospStatus = '1' and (b.HospStartDate is null or rtrim(b.HospStartDate)=''))
                 or (a.HospStatus = '2' and (b.HospStartDate is not null and rtrim(b.HospStartDate)<>'') and (b.HospEndDate is null or rtrim(b.HospEndDate)=''))
                 or (a.HospStatus = '3' and (b.HospEndDate is not null and rtrim(b.HospEndDate)<>'')))
             group by a.HospID,a.HospSeqNo) g on a.HospID=g.HospID and a.HospSeqNo=g.HospSeqNo
  left join (select a.HospID,a.HospSeqNo,max(b.HospEndDate) as maxHospEndDate 
               from HospBasic a join HospContract b on a.HospID = b.HospID and a.HospSeqNo = b.HospSeqNo 
              where ((a.HospStatus = '1' and (b.HospStartDate is null or rtrim(b.HospStartDate)=''))
                 or (a.HospStatus = '2' and (b.HospStartDate is not null and rtrim(b.HospStartDate)<>'') and (b.HospEndDate is null or rtrim(b.HospEndDate)=''))
                 or (a.HospStatus = '3' and (b.HospEndDate is not null and rtrim(b.HospEndDate)<>'')))
              group by a.HospID,a.HospSeqNo) h on a.HospID=h.HospID and a.HospSeqNo=h.HospSeqNo
 where 1 = 1 
    {0}
 order by a.HospID";

        private readonly RepositoryHelper repositoryHelper;
        private readonly ILogger<HospBasicRepository> logger;

        public HospBasicRepository(RepositoryHelper repositoryHelper,
            ILogger<HospBasicRepository> logger)
        {
            this.repositoryHelper = repositoryHelper;
            this.logger = logger;
        }
        /// <summary>
        /// 資料/報表輸出>匯出醫事機構清單
        /// </summary>
        /// <returns></returns>
        public async Task<List<HospBasicExportModel>> QueryHospBasicList(string hospCont, 
            string contractStatus,
            bool couldTreat,
            bool couldInstruct,
            bool contractType2,
            bool contractType3)
        {
            // sql = "select distinct a.HospID,a.HospSeqNo,a.HospName,a.HospTel,a.HospFax,a.HospOwnName,a.HospOwnID,"
            // sql += "a.ZIP,a.HospAddress,a.Contact1,a.ContactTel1,isnull(b.cnt,0) as bcnt,isnull(c.cnt,0) as ccnt,isnull(i.cnt,0) as icnt,isnull(j.cnt,0) as jcnt,"
            // sql += "e.HospContName,f.BranchName,g.minHospStartDate, h.maxHospEndDate"
            // sql += " from HospBasic a "
            // sql += " left join (select z.HospID,z.HospSeqNo,count(1) as cnt from PrsnContract z join PrsnBasic y on z.PrsnID=y.PrsnID where z.CouldTreat='1' and (z.PrsnStartDate is not null and rtrim(z.PrsnStartDate) <> '') and (z.PrsnEndDate is null or rtrim(z.PrsnEndDate)='') group by z.HospID,z.HospSeqNo) b on a.HospID=b.HospID and a.HospSeqNo=b.HospSeqNo "
            // sql += " left join (select z.HospID,z.HospSeqNo,count(1) as cnt from PrsnContract z join PrsnBasic y on z.PrsnID=y.PrsnID where z.CouldInstruct='1' and (z.PrsnStartDate is not null and rtrim(z.PrsnStartDate) <> '') and (z.PrsnEndDate is null or rtrim(z.PrsnEndDate)='') group by z.HospID,z.HospSeqNo) c on a.HospID=c.HospID and a.HospSeqNo=c.HospSeqNo "
            // sql += " left join (select HospID,HospSeqNo,count(1) as cnt from HospContract where (HospEndDate is null or rtrim(HospEndDate)='') and SMKContractType='02' group by HospID,HospSeqNo) i on a.HospID=i.HospID and a.HospSeqNo=i.HospSeqNo "
            // sql += " left join (select HospID,HospSeqNo,count(1) as cnt from HospContract where (HospEndDate is null or rtrim(HospEndDate)='') and SMKContractType='03' group by HospID,HospSeqno) j on a.HospID=j.HospID and a.HospSeqNo=j.HospSeqNo "
            // sql += " left join GenHospCont e on a.LastContType=e.HospContType "
            // sql += " left join GenBranch f on a.BranchNo=f.BranchNo "
            //
            // sql += " left join (select a.HospID,a.HospSeqNo,min(b.HospStartDate) as minHospStartDate "
            // sql += "              from HospBasic a join HospContract b on a.HospID = b.HospID and a.HospSeqNo = b.HospSeqNo "
            // sql += "             where ((a.HospStatus = '1' and (b.HospStartDate is null or rtrim(b.HospStartDate)=''))"
            // sql += "                or (a.HospStatus = '2' and (b.HospStartDate is not null and rtrim(b.HospStartDate)<>'') and (b.HospEndDate is null or rtrim(b.HospEndDate)=''))"
            // sql += "                or (a.HospStatus = '3' and (b.HospEndDate is not null and rtrim(b.HospEndDate)<>'')))"
            // sql += "             group by a.HospID,a.HospSeqNo) g on a.HospID=g.HospID and a.HospSeqNo=g.HospSeqNo "
            //
            // sql += " left join (select a.HospID,a.HospSeqNo,max(b.HospEndDate) as maxHospEndDate "
            // sql += "              from HospBasic a join HospContract b on a.HospID = b.HospID and a.HospSeqNo = b.HospSeqNo "
            // sql += "             where ((a.HospStatus = '1' and (b.HospStartDate is null or rtrim(b.HospStartDate)=''))"
            // sql += "                or (a.HospStatus = '2' and (b.HospStartDate is not null and rtrim(b.HospStartDate)<>'') and (b.HospEndDate is null or rtrim(b.HospEndDate)=''))"
            // sql += "                or (a.HospStatus = '3' and (b.HospEndDate is not null and rtrim(b.HospEndDate)<>'')))"
            // sql += "             group by a.HospID,a.HospSeqNo) h on a.HospID=h.HospID and a.HospSeqNo=h.HospSeqNo "
            //
            // sql += " where 1=1 "
            // Dim itemHospCont As item = CType(objHospCont.SelectedItem, item)
            // If itemHospCont.value.ToString.Trim() <> "" Then
            //     wkHospCont = itemHospCont.value.ToString.Trim()
            //     sql += " and a.LastContType='" & wkHospCont & "'"
            // End If
            // Dim itemContractStatus As item = CType(objContractStatus.SelectedItem, item)
            // If itemContractStatus.value.ToString.Trim() <> "" Then
            //     wkContractStatus = itemContractStatus.value.ToString.Trim()
            //     sql += " and a.HospStatus='" & wkContractStatus & "'"
            // End If
            // If objCouldTreat.Checked = True And objCouldInstruct.Checked = False Then
            //     sql += " and isnull(b.cnt,0)>0"
            // End If
            // If objCouldInstruct.Checked = True And objCouldTreat.Checked = False Then
            //     sql += " and isnull(c.cnt,0)>0"
            // End If
            // If objCouldInstruct.Checked = True And objCouldTreat.Checked = True Then
            //     sql += " and (isnull(b.cnt,0)>0 or isnull(c.cnt,0)>0)"
            // End If
            // 'If objCouldTreat.Checked = False And objCouldInstruct.Checked = False Then
            // '    sql += " and (isnull(b.cnt,0)=0 and isnull(c.cnt,0)=0)"
            // 'End If
            // If objContractType2.Checked = True And objContractType3.Checked = False Then
            //     sql += " and isnull(i.cnt,0)>0"
            // End If
            // If objContractType3.Checked = True And objContractType2.Checked = False Then
            //     sql += " and isnull(j.cnt,0)>0'"
            // End If
            // If objContractType2.Checked = True And objContractType3.Checked = True Then
            //     sql += " and (isnull(i.cnt,0)>0 or isnull(j.cnt,0)>0)"
            // End If
            // 'If objContractType2.Checked = False And objContractType3.Checked = False Then
            // '    sql += " and (isnull(i.cnt,0)=0 and isnull(j.cnt,0)=0)"
            // 'End If
            // sql += " order by a.HospID "

            // Dim itemHospCont As item = CType(objHospCont.SelectedItem, item)
            // If itemHospCont.value.ToString.Trim() <> "" Then
            //     wkHospCont = itemHospCont.value.ToString.Trim()
            //     sql += " and a.LastContType='" & wkHospCont & "'"
            // End If

            var parameters = new List<SqlParameter>();
            var where = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(hospCont))
            {
                where.AppendLine(" and a.LastContType = @HospCont");
                parameters.Add(new SqlParameter("@HospCont", SqlDbType.VarChar){ Value = hospCont });
            }
            
            // Dim itemContractStatus As item = CType(objContractStatus.SelectedItem, item)
            // If itemContractStatus.value.ToString.Trim() <> "" Then
            //     wkContractStatus = itemContractStatus.value.ToString.Trim()
            //     sql += " and a.HospStatus='" & wkContractStatus & "'"
            // End If
            if (!string.IsNullOrWhiteSpace(contractStatus))
            {
                where.AppendLine(" and a.HospStatus = @HospStatus");
                parameters.Add(new SqlParameter("@HospStatus", SqlDbType.VarChar){ Value = contractStatus });
            }

            // If objCouldTreat.Checked = True And objCouldInstruct.Checked = False Then
            //     sql += " and isnull(b.cnt,0)>0"
            // End If
            if (couldTreat && !couldInstruct)
            {
                where.AppendLine(" and isnull(b.cnt,0)>0");
            }

            // If objCouldInstruct.Checked = True And objCouldTreat.Checked = False Then
            //     sql += " and isnull(c.cnt,0)>0"
            // End If
            if (couldInstruct && !couldTreat)
            {
                where.AppendLine(" and isnull(c.cnt,0)>0");
            }

            // If objCouldInstruct.Checked = True And objCouldTreat.Checked = True Then
            //     sql += " and (isnull(b.cnt,0)>0 or isnull(c.cnt,0)>0)"
            // End If
            if (couldInstruct && couldTreat)
            {
                where.AppendLine(" and (isnull(b.cnt,0)>0 or isnull(c.cnt,0)>0)");
            }

            // 'If objCouldTreat.Checked = False And objCouldInstruct.Checked = False Then
            // '    sql += " and (isnull(b.cnt,0)=0 and isnull(c.cnt,0)=0)"
            // 'End If
            if (!couldInstruct && !couldTreat)
            {
                where.AppendLine(" and (isnull(b.cnt,0)=0 and isnull(c.cnt,0)=0)");
            }

            // If objContractType2.Checked = True And objContractType3.Checked = False Then
            //     sql += " and isnull(i.cnt,0)>0"
            // End If
            if (contractType2 && !contractType3)
            {
                where.AppendLine(" and isnull(i.cnt,0)>0");
            }

            // If objContractType3.Checked = True And objContractType2.Checked = False Then
            //     sql += " and isnull(j.cnt,0)>0'"
            // End If
            if (!contractType2 && contractType3)
            {
                where.AppendLine(" and isnull(j.cnt,0)>0'");
            }

            // If objContractType2.Checked = True And objContractType3.Checked = True Then
            //     sql += " and (isnull(i.cnt,0)>0 or isnull(j.cnt,0)>0)"
            // End If
            if (contractType2 && contractType3)
            {
                where.AppendLine(" and (isnull(i.cnt,0)>0 or isnull(j.cnt,0)>0)");
            }
            // 'If objContractType2.Checked = False And objContractType3.Checked = False Then
            // '    sql += " and (isnull(i.cnt,0)=0 and isnull(j.cnt,0)=0)"
            // 'End If
            if (!contractType2 && !contractType3)
            {
                where.AppendLine(" and (isnull(i.cnt,0)=0 and isnull(j.cnt,0)=0)");
            }
            var commandText = new StringBuilder().AppendFormat(sql, where).ToString();
            logger.LogInformation("{CommandText}", commandText);

            return (await repositoryHelper.RawSqlQueryAsync(commandText,
                parameters,
                x => new HospBasicExportModel()
                {
                    HospId = x["HospId"] as string,
                    HospSeqNo = x[1] as string,
                    HospName = x[2] as string,
                    HospTel = x[3] as string,
                    HospFax = x[4] as string,
                    HospOwnName = x[5] as string,
                    HospOwnID = x[6] as string,
                    ZIP = x[7] as string,
                    HospAddress = x[8] as string,
                    Contact1 = x[9] as string,
                    ContactTel1 = x[10] as string,
                    Bcnt = x[11] is int ? (int) x[11] : 0,
                    Ccnt = x[12] is int ? (int) x[12] : 0,
                    Icnt = x[13] is int ? (int) x[13] : 0,
                    Jcnt = x[14] is int ? (int) x[14] : 0,
                    HospContName = x[15] as string,
                    BranchName = x[16] as string,
                    MinHospStartDate = (x[17] as string).WestToTwDate(),
                    MaxHospEndDate = (x[18] as string).WestToTwDate(),
                })).ToList();
        }
    }
}