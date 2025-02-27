using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SMK.Data;
using SMK.Data.Dto;
using SMK.Data.Entity;
using SMK.Data.Enums;
using SMK.Data.Utility;
using SMK.Shared.Extensions;
using SMK.Web.Models;
using SMK.Web.Extensions;
using Yozian.DependencyInjectionPlus.Attributes;
using Yozian.Extension;
using Microsoft.EntityFrameworkCore;

namespace SMK.Web.Services
{
    [ScopedService]
    public class EmpLogService : GenericService
    {

        public EmpLogService(SMKWEBContext context, SessionManager smgr)
            : base(context, smgr)
        {

        }

        public async Task<LogicRtnModel<PagedModel<EmpLoginLogViewModel>>> Query(EmpLoginLogViewModel model)
        {
            try
            {
                DateTime? LoginTime_Start = DateTime.Now.AddDays(-10);
                DateTime? LoginTime_End = DateTime.Now;
                if (!string.IsNullOrWhiteSpace(model.LoginTime_Start))
                {
                    LoginTime_Start = model.LoginTime_Start.TwDateToDateTime().ToDateTime();
                    LoginTime_End = model.LoginTime_End.TwDateToDateTime().ToDateTime()?.AddTicks(-1).AddDays(1);
                }

                var data = from loginLog in context.GenLoginLog
                           join empData in context.GenEmpData
                           on loginLog.User_Id equals empData.Id
                           where (loginLog.LoginTime >= LoginTime_Start && loginLog.LoginTime <= LoginTime_End)
                           orderby loginLog.LoginTime descending
                           select new EmpLoginLogViewModel()
                           {
                               User_Name = empData.Name,
                               User_Account = empData.Account,
                               Enable = loginLog.Enable == true ? "啟用" : "停用",
                               LoginTime = loginLog.LoginTime.ToString("g"),
                               LoginMsg = loginLog.LoginMsg
                           };

                string checkmode = model.Enable == "true" ? "啟用" : "停用";
                // var pagedDate = (await QueryPaging(model.get(), data)).Data;
                var result = await QueryPaging(model.get(), data.AsAsyncQueryable()
                    .WhereWhen(!string.IsNullOrEmpty(model.User_Name), p => p.User_Name == model.User_Name)
                    .WhereWhen(!string.IsNullOrEmpty(model.Enable), p => p.Enable == checkmode)
                    .WhereWhen(!string.IsNullOrEmpty(model.User_Account), p => p.User_Account == model.User_Account)
                    .WhereWhen(!string.IsNullOrEmpty(model.LoginMsg), p => p.LoginMsg == model.LoginMsg)
                    .Select(x => new EmpLoginLogViewModel()
                    {
                        User_Account = x.User_Account,
                        User_Name = x.User_Name,
                        Enable = x.Enable,
                        LoginTime = x.LoginTime,
                        LoginMsg = x.LoginMsg
                    }));
                return result;
            }
            catch (Exception e)
            {
                return new LogicRtnModel<PagedModel<EmpLoginLogViewModel>>()
                {
                    IsSuccess = false,
                    ErrMsg = e.Message,
                    StackTrace = e.StackTrace,
                };
            }
        }

        public async Task<LogicRtnModel<PagedModel<EmpLoginLogViewModel>>> GetEmpLoginLog(EmpLoginLogViewModel model)
        {
            DateTime? LoginTime_Start = DateTime.Now.AddDays(-10);
            DateTime? LoginTime_End = DateTime.Now;
            if (!string.IsNullOrWhiteSpace(model.LoginTime_Start))
            {
                LoginTime_Start = model.LoginTime_Start.TwDateToDateTime().ToDateTime();
                LoginTime_End = model.LoginTime_End.TwDateToDateTime().ToDateTime()?.AddTicks(-1).AddDays(1);
            }

            var data = from loginLog in context.GenLoginLog
                       join empData in context.GenEmpData
                       on loginLog.User_Id equals empData.Id
                       where (loginLog.LoginTime >= LoginTime_Start && loginLog.LoginTime <= LoginTime_End)
                       orderby loginLog.LoginTime descending
                       select new EmpLoginLogViewModel()
                       {
                           User_Name = empData.Name,
                           User_Account = empData.Account,
                           Enable = loginLog.Enable == true ? "啟用" : "停用",
                           LoginTime = loginLog.LoginTime.ToString("g"),
                           LoginMsg = loginLog.LoginMsg
                       };

            string checkmode = model.Enable == "true" ? "啟用" : "停用";
            var list = data
                    .WhereWhen(!string.IsNullOrEmpty(model.User_Name), p => p.User_Name == model.User_Name)
                    .WhereWhen(!string.IsNullOrEmpty(model.Enable), p => p.Enable == checkmode)
                    .WhereWhen(!string.IsNullOrEmpty(model.User_Account), p => p.User_Account == model.User_Account)
                    .WhereWhen(!string.IsNullOrEmpty(model.LoginMsg), p => p.LoginMsg == model.LoginMsg)
                    .Select(x => new EmpLoginLogViewModel()
                    {
                        User_Account = x.User_Account,
                        User_Name = x.User_Name,
                        Enable = x.Enable,
                        LoginTime = x.LoginTime,
                        LoginMsg = x.LoginMsg
                    });
            return await QueryPaging(model.get(), list);
        }


    }
}