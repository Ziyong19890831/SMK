using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using SMK.Data;
using SMK.Data.Dto;
using SMK.Data.Utility;
using SMK.Web.Models;
using Yozian.DependencyInjectionPlus.Attributes;

namespace SMK.Web.Repositories
{
    [ScopedService]
    public class PrsnContractRepository
    {
        private readonly string sql = @"
select 
 a.HospID,a.HospSeqNo,a.HospName,a.ZIP,a.HospAddress,
 a.PrsnName,a.PrsnID,a.PrsnBirthday,a.PrsnTypeName,
 a.CouldInstruct, a.CouldTreat, a.minPrsnStartDate, a.maxPrsnEndDate,
 a.LastContType, a.HospStatus, PEmail=stuff(b.PEmail.value('/R[1]','nvarchar(max)'),1,1,''),
 a.HospEmail, a.ContactEmail1, a.ContactEmail2
 from
 (
   select distinct c.HospID,c.HospSeqNo,c.HospName,c.ZIP,
          c.HospAddress,a.PrsnName,a.PrsnID,a.PrsnBirthday,d.PrsnTypeName,
		  b.CouldInstruct, b.CouldTreat,b.minPrsnStartDate, b.maxPrsnEndDate,
		  c.LastContType, c.HospStatus, c.HospEmail, c.ContactEmail1, c.ContactEmail2
     from PrsnBasic a with (nolock)
     left join (select HospID,HospSeqNo,PrsnID,CouldInstruct,CouldTreat,
	                   PrsnStartDate as minPrsnStartDate,PrsnEndDate as maxPrsnEndDate 
				  from PrsnContract with (nolock)
 ) b on a.PrsnID=b.PrsnID
 join HospBasic c with (nolock) on b.HospID=c.HospID and b.HospSeqNo=c.HospSeqNo
 join GenPrsnType d with (nolock) on a.PrsnType=d.PrsnType
 left join (select HospID, HospSeqNo, SMKContractType from HospContract with (nolock) where (HospEndDate is null or rtrim(HospEndDate)='')) f on f.HospID=c.HospID and f.HospSeqNo=c.HospSeqNo
 left join (select HospID, HospSeqNo, PrsnID, count(1) as cnt from PrsnContract with (nolock) where (PrsnStartDate is null or rtrim(PrsnStartDate)='') and (PrsnEndDate is null or rtrim(PrsnEndDate)='') group by HospID, HospSeqNo, PrsnID) g on a.PrsnID=g.PrsnID and c.HospID=g.HospID and c.HospSeqNo=g.HospSeqNo
 left join (select HospID, HospSeqNo, PrsnID, count(1) as cnt from PrsnContract with (nolock) where (PrsnStartDate is not null and rtrim(PrsnStartDate)<>'') and (PrsnEndDate is null or rtrim(PrsnEndDate)='') group by HospID, HospSeqNo, PrsnID) h on a.PrsnID=h.PrsnID and c.HospID=h.HospID and c.HospSeqNo=h.HospSeqNo
 left join (select HospID, HospSeqNo, PrsnID, count(1) as cnt from PrsnContract with (nolock) where PrsnEndDate is not null and rtrim(PrsnEndDate)<>'' group by HospID, HospSeqNo, PrsnID) i on a.PrsnID=i.PrsnID and c.HospID=i.HospID and c.HospSeqNo=i.HospSeqNo
 where 1 = 1 
 {0}
 ) a
 Cross apply
   (select PEmail=(select N','+PEmail from PrsnEmail with (nolock) where PrsnID=a.PrsnID For XML PATH(''), ROOT('R'), TYPE)) b
order by a.HospID,a.PrsnID";

        private readonly SMKWEBContext context;
        private readonly ILogger<PrsnContractRepository> logger;
        private readonly RepositoryHelper repositoryHelper;

        public PrsnContractRepository(SMKWEBContext context,
            ILogger<PrsnContractRepository> logger,
            RepositoryHelper repositoryHelper)
        {
            this.context = context;
            this.logger = logger;
            this.repositoryHelper = repositoryHelper;
        }
        public async Task<IEnumerable<PrsnContractExportModel>> QueryPrsnContracts(string hospCont, 
            string contractStatus,
            bool couldTreat,
            bool couldInstruct,
            bool contractType2,
            bool contractType3)
        { 
            // sql = "select "
            // sql += " a.HospID,a.HospSeqNo,a.HospName,a.ZIP,a.HospAddress,a.PrsnName,a.PrsnID,a.PrsnBirthday,a.PrsnTypeName, "
            // sql += " a.CouldInstruct, a.CouldTreat, a.minPrsnStartDate, a.maxPrsnEndDate, a.LastContType, a.HospStatus, PEmail=stuff(b.PEmail.value('/R[1]','nvarchar(max)'),1,1,''), a.HospEmail, a.ContactEmail1, a.ContactEmail2 "
            // sql += " from "
            // sql += " ( "
            // sql += "select distinct c.HospID,c.HospSeqNo,c.HospName,c.ZIP,c.HospAddress,a.PrsnName,a.PrsnID,a.PrsnBirthday,d.PrsnTypeName,"
            // sql += "b.CouldInstruct, b.CouldTreat,b.minPrsnStartDate, b.maxPrsnEndDate, c.LastContType, c.HospStatus, c.HospEmail, c.ContactEmail1, c.ContactEmail2 "
            // sql += " from PrsnBasic a"
            // sql += " left join (select HospID,HospSeqNo,PrsnID,CouldInstruct,CouldTreat,PrsnStartDate as minPrsnStartDate,PrsnEndDate as maxPrsnEndDate from PrsnContract " 'where (PrsnStartDate is not null or rtrim(PrsnStartDate)<>'') and (PrsnEndDate is not null or rtrim(PrsnEndDate)<>'')"
            // sql += " ) b on a.PrsnID=b.PrsnID"
            // sql += " join HospBasic c on b.HospID=c.HospID and b.HospSeqNo=c.HospSeqNo "
            // sql += " join GenPrsnType d on a.PrsnType=d.PrsnType "
            // sql += " left join (select HospID, HospSeqNo, SMKContractType from HospContract where (HospEndDate is null or rtrim(HospEndDate)='')) f on f.HospID=c.HospID and f.HospSeqNo=c.HospSeqNo "
            // sql += " left join (select HospID, HospSeqNo, PrsnID, count(1) as cnt from PrsnContract where (PrsnStartDate is null or rtrim(PrsnStartDate)='') and (PrsnEndDate is null or rtrim(PrsnEndDate)='') group by HospID, HospSeqNo, PrsnID) g on a.PrsnID=g.PrsnID and c.HospID=g.HospID and c.HospSeqNo=g.HospSeqNo "
            // sql += " left join (select HospID, HospSeqNo, PrsnID, count(1) as cnt from PrsnContract where (PrsnStartDate is not null and rtrim(PrsnStartDate)<>'') and (PrsnEndDate is null or rtrim(PrsnEndDate)='') group by HospID, HospSeqNo, PrsnID) h on a.PrsnID=h.PrsnID and c.HospID=h.HospID and c.HospSeqNo=h.HospSeqNo "
            // sql += " left join (select HospID, HospSeqNo, PrsnID, count(1) as cnt from PrsnContract where PrsnEndDate is not null and rtrim(PrsnEndDate)<>'' group by HospID, HospSeqNo, PrsnID) i on a.PrsnID=i.PrsnID and c.HospID=i.HospID and c.HospSeqNo=i.HospSeqNo "
            // sql += " where 1=1 "
            // Dim itemHospCont As item = CType(objHospCont.SelectedItem, item)
            // If itemHospCont.value.ToString.Trim() <> "" Then
            //     wkHospCont = itemHospCont.value.ToString.Trim()
            //     sql += " and c.LastContType='" & wkHospCont & "'"
            // End If
            // Dim itemContractStatus As item = CType(objContractStatus.SelectedItem, item)
            // If itemContractStatus.value.ToString.Trim() <> "" Then
            //     wkContractStatus = itemContractStatus.value.ToString.Trim()
            //     If wkContractStatus = "1" Then
            //         sql += " and isnull(g.cnt,0)>0 "
            //     ElseIf wkContractStatus = "2" Then
            //         sql += " and isnull(h.cnt,0)>0 "
            //     ElseIf wkContractStatus = "3" Then
            //         sql += " and isnull(i.cnt,0)>0 "
            //     End If
            // End If
            // If objCouldTreat.Checked = True And objCouldInstruct.Checked = False Then
            //     sql += " and b.CouldTreat='1'"
            // End If
            // If objCouldInstruct.Checked = True And objCouldTreat.Checked = False Then
            //     sql += " and b.CouldInstruct='1'"
            // End If
            // If objCouldInstruct.Checked = True And objCouldTreat.Checked = True Then
            //     sql += " and (b.CouldTreat='1' or b.CouldInstruct='1')"
            // End If
            // 'If objCouldInstruct.Checked = False And objCouldTreat.Checked = False Then
            // '    sql += " and (a.CouldTreat<>'1' and a.CouldInstruct<>'1')"
            // 'End If
            // If objContractType2.Checked = True And objContractType3.Checked = False Then
            //     sql += " and f.SMKContractType='02'"
            // End If
            // If objContractType3.Checked = True And objContractType2.Checked = False Then
            //     sql += " and f.SMKContractType='03'"
            // End If
            // If objContractType2.Checked = True And objContractType3.Checked = True Then
            //     sql += " and f.SMKContractType in ('02','03')"
            // End If
            // 'If objContractType2.Checked = False And objContractType3.Checked = False Then
            // '    sql += " and f.SMKContractType not in ('02','03')"
            // 'End If
            // sql += " ) a "
            // sql += " Cross apply "
            // sql += "     (select PEmail=(select N','+PEmail from PrsnEmail where PrsnID=a.PrsnID For XML PATH(''), ROOT('R'), TYPE)) b "
            // sql += " order by a.HospID,a.PrsnID "

            var parameters = new List<SqlParameter>();
            var where = new StringBuilder();

            // Dim itemHospCont As item = CType(objHospCont.SelectedItem, item)
            // If itemHospCont.value.ToString.Trim() <> "" Then
            //     wkHospCont = itemHospCont.value.ToString.Trim()
            //     sql += " and c.LastContType='" & wkHospCont & "'"
            // End If
            if (!string.IsNullOrWhiteSpace(hospCont))
            {
                where.AppendLine(" and c.LastContType = @HospCont");
                parameters.Add(new SqlParameter("@HospCont", SqlDbType.VarChar){ Value = hospCont.Trim() });
            }

            // Dim itemContractStatus As item = CType(objContractStatus.SelectedItem, item)
            // If itemContractStatus.value.ToString.Trim() <> "" Then
            //     wkContractStatus = itemContractStatus.value.ToString.Trim()
            //     If wkContractStatus = "1" Then
            //         sql += " and isnull(g.cnt,0)>0 "
            //     ElseIf wkContractStatus = "2" Then
            //         sql += " and isnull(h.cnt,0)>0 "
            //     ElseIf wkContractStatus = "3" Then
            //         sql += " and isnull(i.cnt,0)>0 "
            //     End If
            // End If
            if (!string.IsNullOrWhiteSpace(contractStatus))
            {
                if (contractStatus == "1")
                {
                    where.AppendLine(" and isnull(g.cnt,0)>0 "); 
                } 
                else if (contractStatus == "2")
                {
                    where.AppendLine(" and isnull(h.cnt,0)>0 ");
                }
                else if (contractStatus == "3")
                {
                    where.AppendLine(" and isnull(i.cnt,0)>0 ");
                }
            }
            
            // If objCouldTreat.Checked = True And objCouldInstruct.Checked = False Then
            //     sql += " and b.CouldTreat='1'"
            // End If
            if (couldTreat && couldInstruct)
            {
                where.AppendLine(" and b.CouldTreat='1' ");
            }
            
            // If objCouldInstruct.Checked = True And objCouldTreat.Checked = False Then
            //     sql += " and b.CouldInstruct='1'"
            // End If
            if (!couldTreat && couldInstruct)
            {
                where.AppendLine(" and b.CouldInstruct='1' ");
            }
            
            // If objCouldInstruct.Checked = True And objCouldTreat.Checked = True Then
            //     sql += " and (b.CouldTreat='1' or b.CouldInstruct='1')"
            // End If
            if (couldTreat && couldInstruct)
            {
                where.AppendLine(" and (b.CouldTreat='1' or b.CouldInstruct='1')");
            }
            
            // 'If objCouldInstruct.Checked = False And objCouldTreat.Checked = False Then
            // '    sql += " and (a.CouldTreat<>'1' and a.CouldInstruct<>'1')"
            // 'End If
            
            // If objContractType2.Checked = True And objContractType3.Checked = False Then
            //     sql += " and f.SMKContractType='02'"
            // End If
            if (contractType2 && !contractType3)
            {
                where.AppendLine(" and f.SMKContractType='02'");
            }
            
            // If objContractType3.Checked = True And objContractType2.Checked = False Then
            //     sql += " and f.SMKContractType='03'"
            // End If
            if (!contractType2 && contractType3)
            {
                where.AppendLine(" and f.SMKContractType='03'");
            }

            // If objContractType2.Checked = True And objContractType3.Checked = True Then
            //     sql += " and f.SMKContractType in ('02','03')"
            // End If
            if (contractType2 && contractType3)
            {
                where.AppendLine(" and f.SMKContractType in ('02','03')");
            }

            // 'If objContractType2.Checked = False And objContractType3.Checked = False Then
            // '    sql += " and f.SMKContractType not in ('02','03')"
            // 'End If

            var commandText = new StringBuilder().AppendFormat(sql, where).ToString();
            logger.LogInformation("{CommandText}", commandText);

            return (await repositoryHelper.RawSqlQueryAsync(commandText,
                parameters,
                x => new PrsnContractExportModel()
                {
                    HospID = x["HospID"] as string,
                    HospSeqNo = x["HospSeqNo"] as string,
                    HospName = x["HospName"] as string,
                    ZIP = x["ZIP"] as string,
                    HospAddress = x["HospAddress"] as string,
                    PrsnName = x["PrsnName"] as string,
                    PrsnID = x["PrsnID"] as string,
                    PrsnBirthday = x["PrsnBirthday"] as string,
                    PrsnTypeName = x["PrsnTypeName"] as string,
                    CouldInstruct = (x["CouldInstruct"] as string) == "1" ? "Y" : "N",
                    CouldTreat = (x["CouldTreat"] as string) == "1" ? "Y" : "N",
                    MinPrsnStartDate = x["MinPrsnStartDate"] as string,
                    MaxPrsnEndDate = x["MaxPrsnEndDate"] as string,
                    LastContType = x["LastContType"] as string,
                    HospStatus = x["HospStatus"] as string,
                    PEmail = x["PEmail"] as string,
                    HospEmail = x["HospEmail"] as string,
                    ContactEmail1 = x["ContactEmail1"] as string,
                    ContactEmail2 = x["ContactEmail2"] as string,
                })).ToList();
        }
    }
}