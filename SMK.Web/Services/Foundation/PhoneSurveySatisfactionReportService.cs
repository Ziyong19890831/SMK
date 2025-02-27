using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using SMK.Data;
using System.Data;
using Yozian.DependencyInjectionPlus.Attributes;
using Dapper;
using System.Threading.Tasks;
using SMK.Web.Models;
using Yozian.WebCore.Library.Utility.Excel;
using System.Linq;

namespace SMK.Web.Services.Foundation
{
    [ScopedService]
    public class PhoneSurveySatisfactionReportService : GenericService
    {
        private readonly IDbConnection _conn = null;
        private readonly IWebHostEnvironment _env;
        public PhoneSurveySatisfactionReportService(SMKWEBContext context, SessionManager smgr, IWebHostEnvironment env)
        : base(context, smgr)
        {
            _conn = context.Database.GetDbConnection();
            _env = env;
        }
        public async Task<byte[]> Export(string ym)
        {
            string sql = @"SELECT * FROM Population_QuitSmkMan WHERE FIRSTMONTH = @ym and NotSelect != '1'";
            var data = await _conn.QueryAsync<PhoneSurveySatisfactionExportModel>(sql, new { ym  =  @ym});
            return await Task.Run(() =>
            {
                return new MyExcelExporter<PhoneSurveySatisfactionExportModel>(data.ToList())
                    .DefineColumns((bindder) =>
                    {
                        bindder.ColumnFor(p => p.CaseNo, "CaseNo");
                        bindder.ColumnFor(p => p.FirstMonth, "FirstMonth");
                        bindder.ColumnFor(p => p.FirstDate, "FirstDate");
                        bindder.ColumnFor(p => p.TimeSpan, "TimeSpan");
                        bindder.ColumnFor(p => p.HospID, "HospID");
                        bindder.ColumnFor(p => p.HospSeqNo, "HospSeqNo");
                        bindder.ColumnFor(p => p.ID, "ID");
                        bindder.ColumnFor(p => p.Birthday, "Birthday");
                        bindder.ColumnFor(p => p.Edition, "Edition");
                        bindder.ColumnFor(p => p.院所名稱, "院所名稱");
                        bindder.ColumnFor(p => p.院所縣市, "院所縣市");
                        bindder.ColumnFor(p => p.姓名, "姓名");
                        bindder.ColumnFor(p => p.出生年, "出生年");
                        bindder.ColumnFor(p => p.性別, "性別");
                        bindder.ColumnFor(p => p.電話, "電話");

                    })
                    .GetResult();
            });
        }
    }

}
