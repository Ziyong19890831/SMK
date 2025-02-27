using Dapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using SMK.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Yozian.DependencyInjectionPlus.Attributes;
using SMK.Data.Dto;
using SMK.Web.Extensions;
using SMK.Web.Models;
using Microsoft.Data.SqlClient;

namespace SMK.Web.Services.Foundation
{
    [ScopedService]
    public class RegularMonthlyReportService : GenericService
    {
        private readonly IDbConnection _conn = null;
        private readonly IWebHostEnvironment _env;
        private readonly SMKWEBContext _context;
        public RegularMonthlyReportService(SMKWEBContext context, SessionManager smgr,IWebHostEnvironment env)
            : base(context, smgr)
        {
            _context = context;
            _conn = context.Database.GetDbConnection();
            _env = env;
        }
        public async Task<LogicRtnModel<PagedModel<ExportTotalTableResult>>> ExportTotalTable(RegularMonthlyReportQueryModel model)
        {
            string sqlstr = "exec sp_ExportTotalTable @p0, @p1";
            var data = await _conn.QueryAsync<ExportTotalTableResult>(sqlstr, new
            {
               p0= model.ED1,
               p1= model.EX
            }, commandTimeout: 300);
            return await QueryPaging(model.get(), data.AsAsyncQueryable());
            
        }
        public async Task<LogicRtnModel<List<ExportCategoryList>>> ExportCategoryList(RegularMonthlyReportQueryModel model)
        {
            var ExportCategoryListHealthInsuranceFile = await this.ExportCategoryListHealthInsuranceFile(model);
            var ExportCategoryListContractFile = await this.ExportCategoryListContractFile(model);
            
            List<ExportCategoryList> ret = new List<ExportCategoryList>();
            //var ret2 = ExportCategoryListHealthInsuranceFile.Join(ExportCategoryListHealthInsuranceFile, c => c.類別, s => s.類別, new ExportCategoryList
            //{

            //});
            foreach (var item in ExportCategoryListHealthInsuranceFile)
            {
                ExportCategoryList exportCategoryList = new ExportCategoryList();
                exportCategoryList.N = item.N;
                exportCategoryList.用藥_衛教人數 = item.用藥_衛教人數;
                exportCategoryList.用藥人數 = item.用藥人數;
                exportCategoryList.用藥人次 = item.用藥人次;
                exportCategoryList.用藥週數 = item.用藥週數;
                exportCategoryList.申報人數 = item.申報人數;
                exportCategoryList.申報人次 = item.申報人次;
                exportCategoryList.申報金額 = item.申報金額;
                exportCategoryList.衛教人數 = item.衛教人數;
                exportCategoryList.衛教人次 = item.衛教人次;
                exportCategoryList.類別 = item.類別;
                exportCategoryList.平均每人用藥週數 = Math.Round((double) item.用藥人數 / (double)item.用藥週數,1,MidpointRounding.AwayFromZero);
                exportCategoryList.平均每人給藥次數 = Math.Round((double)item.用藥人次 / (double)item.用藥人數, 1, MidpointRounding.AwayFromZero);
                exportCategoryList.平均每人衛教次數 = Math.Round((double)item.衛教人次 / (double)item.衛教人數, 1, MidpointRounding.AwayFromZero);
                var contract = ExportCategoryListContractFile.FirstOrDefault(x => x.類別 == item.類別);
                if (contract != null)
                {
                    exportCategoryList.合約機構數_年底 = contract.合約機構數_年底;
                    exportCategoryList.合約機構數_年度 = contract.合約機構數_年度;                    
                    exportCategoryList.執行機構數 = contract.執行機構數;
                    exportCategoryList.合約人員數_年底 = contract.合約人員數_年底;
                    exportCategoryList.合約人員數_年度 = contract.合約人員數_年度;
                    exportCategoryList.執行人員數 = contract.執行人員數;
                }
                ret.Add(exportCategoryList);
            }
            return new LogicRtnModel<List<ExportCategoryList>>()
            {
                IsSuccess = true,
                Data = ret
            };
        }
        public async Task<IEnumerable<ExportCategoryListHealthInsuranceFileResult>> ExportCategoryListHealthInsuranceFile(RegularMonthlyReportQueryModel model)
        {
            //string sqlstr = "exec sp_ExportCategoryListHealthInsuranceFile @YSTART, @YEND";
            var data = await _conn.QueryAsync<ExportCategoryListHealthInsuranceFileResult>("sp_ExportCategoryListHealthInsuranceFile", new
            {
                YSTART = model.YSTART1,
                YEND = model.YEND1
            } ,commandTimeout: 300, commandType: CommandType.StoredProcedure);
            return data;

        }
        public async Task<IEnumerable <ExportCategoryListContractFileResult>> ExportCategoryListContractFile(RegularMonthlyReportQueryModel model)
        {

            //string sqlstr = "exec sp_ExportCategoryListContractFile @p0, @p1, @p2, @p3";
            //var data = await _conn.QueryAsync<ExportCategoryListContractFileResult>(sqlstr, new
            //{
            //    p0 = model.YY,
            //    p1 = model.YYE,
            //    p2 = model.YSTART,
            //    p3 = model.YEND
            //}, commandTimeout: 3000);
            var data = await _conn.QueryAsync<ExportCategoryListContractFileResult>("sp_ExportCategoryListContractFile", new
            {
                YY = model.YY,
                YYE = model.YYE,
                YSTART = model.YSTART,
                YEND = model.YEND
            }, commandTimeout: 3000, commandType: CommandType.StoredProcedure);
            return data;
        }
    }
}
