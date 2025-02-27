using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using SMK.Data;
using System.Data;
using Yozian.DependencyInjectionPlus.Attributes;
using Dapper;
using System.IO;
using System.Threading.Tasks;
using SMK.Web.Models;
using SMK.Web.Models.HighReportingAgencyReportExportModel;
using System.Linq;
using Yozian.WebCore.Library.Utility.Excel;
using System;

namespace SMK.Web.Services.Foundation
{
    [ScopedService]
    public class HighReportingAgencyReportService : GenericService
    {
        private readonly IDbConnection _conn = null;
        private readonly IWebHostEnvironment _env;
        public HighReportingAgencyReportService(SMKWEBContext context, SessionManager smgr, IWebHostEnvironment env)
        : base(context, smgr)
        {
            _conn = context.Database.GetDbConnection();
            _env = env;
        }
        public async Task<byte[]> Export(HighReportingAgencyReportQueryModel model)
        {
            string sql = string.Empty;
            switch (model.Type)
            {
                case "levelsummary":
                    sql = "exec sp_HighReportingAgencyByLevelSummary @p0, @p1";
                    var levelsummary = await _conn.QueryAsync<HighReportingAgencyByLevelSummaryResult>(sql, new
                    {
                        p0 = model.STARTDATE,
                        p1 = model.ENDDATE
                    }, commandTimeout: 300);
                    return await Task.Run(() =>
                    {
                        return new MyExcelExporter<HighReportingAgencyByLevelSummaryResult>(levelsummary.ToList())
                            .DefineColumns((bindder) =>
                            {
                                bindder.ColumnFor(p => p.層級, "層級");
                                bindder.ColumnFor(p => p.用藥人數, "用藥人數");
                                bindder.ColumnFor(p => p.給藥週數, "給藥週數");
                                bindder.ColumnFor(p => p.平均每人給藥次數, "平均每人給藥次數");
                                bindder.ColumnFor(p => p.平均每人給藥週數, "平均每人給藥週數");
                                bindder.ColumnFor(p => p.衛教人次, "衛教人次");
                                bindder.ColumnFor(p => p.衛教人數, "衛教人數");
                                bindder.ColumnFor(p => p.平均每人衛教次數, "平均每人衛教次數");
                                bindder.ColumnFor(p => p.用藥執行機構數, "用藥執行機構數");
                                bindder.ColumnFor(p => p.衛教執行機構數, "衛教執行機構數");
                                bindder.ColumnFor(p => p.平均每機構用藥人次, "平均每機構用藥人次");
                                bindder.ColumnFor(p => p.平均每機構衛教人次, "平均每機構衛教人次");
                            })
                            .GetResult();
                    });
                case "season":
                    sql = "exec sp_HighReportingAgencyBySeason @p0, @p1";
                    var season = await _conn.QueryAsync<HighReportingAgencyBySeasonResult>(sql, new
                    {
                        p0 = model.STARTDATE,
                        p1 = model.ENDDATE
                    }, commandTimeout: 300);
                    return await Task.Run(() =>
                    {
                        return new MyExcelExporter<HighReportingAgencyBySeasonResult>(season.ToList())
                            .DefineColumns((bindder) =>
                            {
                                bindder.ColumnFor(p => p.用藥人次, "用藥人次");
                                bindder.ColumnFor(p => p.用藥人數, "用藥人數");
                                bindder.ColumnFor(p => p.給藥週數, "給藥週數");
                                bindder.ColumnFor(p => p.平均每人給藥次數, "平均每人給藥次數");
                                bindder.ColumnFor(p => p.平均每人給藥週數, "平均每人給藥週數");
                                bindder.ColumnFor(p => p.衛教人次, "衛教人次");
                                bindder.ColumnFor(p => p.衛教人數, "衛教人數");
                                bindder.ColumnFor(p => p.平均每人衛教次數, "平均每人衛教次數");
                                bindder.ColumnFor(p => p.用藥執行機構數, "用藥執行機構數");
                                bindder.ColumnFor(p => p.衛教執行機構數, "衛教執行機構數");
                                bindder.ColumnFor(p => p.平均每機構用藥人次, "平均每機構用藥人次");
                                bindder.ColumnFor(p => p.平均每機構衛教人次, "平均每機構衛教人次");
                            })
                            .GetResult();
                    });
                case "level":
                    sql = "exec sp_HighReportingAgencyByLevel @p0, @p1";
                    var level = await _conn.QueryAsync<HighReportingAgencyByLevelResult>(sql, new
                    {
                        p0 = model.STARTDATE,
                        p1 = model.ENDDATE
                    }, commandTimeout: 300);
                    return await Task.Run(() =>
                    {
                        return new MyExcelExporter<HighReportingAgencyByLevelResult>(level.ToList())
                            .DefineColumns((bindder) =>
                            {
                                bindder.ColumnFor(p => p.層級, "層級");
                                bindder.ColumnFor(p => p.給藥人次, "給藥人次");
                                bindder.ColumnFor(p => p.平均每人給藥次數, "平均每人給藥次數");
                                bindder.ColumnFor(p => p.平均每人給藥週數, "平均每人給藥週數");
                                bindder.ColumnFor(p => p.衛教人次, "衛教人次");
                                bindder.ColumnFor(p => p.平均每人衛教次數, "平均每人衛教次數");
                            })
                            .GetResult();
                    });
                case "agency":
                    sql = "exec sp_HighReportingAgencyByAgency @p0, @p1";
                    var agency = await _conn.QueryAsync<HighReportingAgencyByAgencyResult>(sql, new
                    {
                        p0 = model.STARTDATE,
                        p1 = model.ENDDATE
                    }, commandTimeout: 300);
                    return await Task.Run(() =>
                    {
                        return new MyExcelExporter<HighReportingAgencyByAgencyResult>(agency.ToList())
                            .DefineColumns((bindder) =>
                            {
                                bindder.ColumnFor(p => p.層級, "層級");
                                bindder.ColumnFor(p => p.分區別, "分區別");
                                bindder.ColumnFor(p => p.院所代碼, "院所代碼");
                                bindder.ColumnFor(p => p.院區別, "院區別");
                                bindder.ColumnFor(p => p.院所名稱, "院所名稱");
                                bindder.ColumnFor(p => p.給藥人次, "給藥人次");
                                bindder.ColumnFor(p => p.平均每人給藥次數, "平均每人給藥次數");
                                bindder.ColumnFor(p => p.平均每人給藥週數, "平均每人給藥週數");
                                bindder.ColumnFor(p => p.衛教人次, "衛教人次");
                                bindder.ColumnFor(p => p.平均每人衛教次數, "平均每人衛教次數");
        
                            })
                            .GetResult();
                    });
                default:
                    throw new Exception("不存在的類別");
            }
        }
    }

}
