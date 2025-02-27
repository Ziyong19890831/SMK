using SMK.Data;
using SMK.Data.Dto;
using SMK.Data.Entity;
using SMK.Data.Utility;
using SMK.Web.Validator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Yozian.DependencyInjectionPlus.Attributes;

namespace SMK.Web.Services
{
    [ScopedService]
    public class EmpService : GenericService
    {

        public EmpService(SMKWEBContext context, SessionManager smgr)
            : base(context, smgr)
        {

        }

        /// <summary>
        /// 新增帳號
        /// </summary>
        /// <param name="genEmpData"></param>
        /// <returns></returns>
        public async Task<LogicRtnModel<GenEmpData>> CreateEmp(GenEmpData genEmpData, string roleId)
        {
            genEmpData.Id = MyGuid.NewGuid();
            if (!string.IsNullOrEmpty(genEmpData.Pwd))
            {
                genEmpData.Pwd = Cryptos.EncryptPwd(genEmpData.Pwd);
            }
            genEmpData.Enable = true;
            genEmpData.CreatedAt = DateTime.Now;
            genEmpData.PasswordModifyAt = DateTime.Now;
            LogicRtnModel<GenEmpData> rtnModel = new LogicRtnModel<GenEmpData>();
            using (var txn = context.GetTransactionScope())
            {
                try
                {
                    rtnModel = await this.Create(genEmpData, new GenEmpDataValidator(true).Validate);

                    rtnModel.ExtendData = await this.Create(new RoleEmpMapping()
                    {
                        RoleId = roleId,
                        EmpId = genEmpData.Id,
                        Id = MyGuid.NewGuid(),
                        CreatedAt = DateTime.Now
                    }, null);
                    txn.Commit();
                }
                catch (Exception ex)
                {
                    txn.Rollback();
                    rtnModel = new LogicRtnModel<GenEmpData>(MsgType.CreateFail, ex.Message);
                }
            }
            return rtnModel;
        }

        /// <summary>
        /// 更新帳號資訊
        /// </summary>
        /// <param name="genEmpData"></param>
        /// <returns></returns>
        public async Task<LogicRtnModel<GenEmpData>> UpdateEmp(GenEmpData genEmpData)
        {
            //必要更新
            List<Expression<Func<GenEmpData, object>>> list =
                new List<Expression<Func<GenEmpData, object>>>() {
                x=>x.Account,
                x=>x.Enable,
                x=>x.Name,
                x=>x.UpdatedAt,
                x=>x.UpdatedBy
            };
            var isPasswordUpdate = !string.IsNullOrEmpty(genEmpData.Pwd);
            if (isPasswordUpdate)
            {
                genEmpData.Pwd = Cryptos.EncryptPwd(genEmpData.Pwd);
                list.Add(x => x.Pwd);
            }
            genEmpData.UpdatedAt = DateTime.Now;
            genEmpData.UpdatedBy = identity.Account;
            return await this.Update(genEmpData, new GenEmpDataValidator(isPasswordUpdate).Validate, true, list.ToArray());
        }
        /// <summary>
        /// 解鎖功能
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<LogicRtnModel<GenEmpData>> Release(string Id)
        {
            GenEmpData genEmpData = context.GenEmpData.FirstOrDefault(x => x.Id == Id);//他傳入值是id
            genEmpData.UpdatedAt = DateTime.Now;
            genEmpData.UpdatedBy = identity.Account;
            genEmpData.LoginError = 0;
            return await this.Update(genEmpData);
        }
    }
}