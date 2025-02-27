using Microsoft.EntityFrameworkCore;
using SMK.Data;
using SMK.Data.Dto;
using SMK.Data.Entity;
using SMK.Data.Enums;
using SMK.Data.Utility;
using SMK.Web.Helpers;
using System;
using System.Linq;
using System.Threading.Tasks;
using Yozian.DependencyInjectionPlus.Attributes;

namespace SMK.Web.Services
{
    [ScopedService]
    public class AuthService : GenericService
    {
        private readonly EmpService empService;
        private readonly PrivilegeService privilegeService;

        public IdentityModel Identity
        {
            get
            {
                return this.identity;
            }
            private set { }
        }

        public AuthService(
                SMKWEBContext context,
                EmpService empService,
                PrivilegeService privilegeService,
                SessionManager sessionManager
            ) : base(context, sessionManager)
        {
            this.empService = empService;
            this.privilegeService = privilegeService;
        }



        public async Task<string> Login(string account, string pwd)
        {
            if (string.IsNullOrEmpty(account) || string.IsNullOrEmpty(pwd))
            {
                return "帳號或密碼錯誤！";
            }

            var encryptedPwd = Cryptos.EncryptPwd(pwd);
            var admin = await context.GenEmpData
                .FirstOrDefaultAsync(
                    x => x.Account.Equals(account)
                    && x.Pwd.Equals(encryptedPwd)
                );

            if (admin == null) // 帳號密碼錯誤
            {
                var acc = await context.GenEmpData
                    .FirstOrDefaultAsync(x => x.Account.Equals(account));
                if (acc != null)
                {
                    acc.LoginError += 1;
                    acc.LoginErrorAt = DateTime.Now;
                    var ret = await this.context.SaveChangesWithAuditAsync(account);
                    if (acc.LoginError > 2) { add_Login_Logs(acc.Id, LoginLog.error_Lock, acc.Enable); }
                    else { add_Login_Logs(acc.Id, LoginLog.error, acc.Enable); }
                }
                return "帳號或密碼錯誤！";
            }
            else // 檢查是否有被鎖定
            {
                if (admin.LoginError > 2 && admin.LoginErrorAt.HasValue
                    && admin.LoginErrorAt.Value.AddMinutes(30) > DateTime.Now)
                {
                    add_Login_Logs(admin.Id, LoginLog.error_Lock, admin.Enable);
                    return "帳號登入錯誤已達三次以上，請等待30分鐘後重新登入！";
                }
                else if (!admin.Enable)
                {
                    add_Login_Logs(admin.Id, LoginLog.error, admin.Enable);
                    return "帳號已被停用，請聯繫管理員！";
                }
                else
                {
                    admin.LoginError = 0;
                    admin.LoginErrorAt = null;
                }
            }

            // get all the avalible privileges
            var privileges = await this.privilegeService.GetMyPrivileges(admin.Id, true);

            // we should map them to menu
            var menuNode = this.privilegeService
                .EstablishTree(
                new PrivilegeNodeModel()
                {
                    LinkType = PrivilegeNodeType.Root
                },
                privileges
                    .Where(x => x.LinkType == PrivilegeNodeType.Node || x.LinkType == PrivilegeNodeType.Link)
                    .ToList()
                );

            admin.LastLoginDate = DateTime.Now;
            admin.UpdatedAt = DateTime.Now;


            var result = await this.context.SaveChangesWithAuditAsync(admin.Account);

            add_Login_Logs(admin.Id, LoginLog.success, admin.Enable);

            var identity = new IdentityModel()
            {
                Account = admin.Account,
                Name = admin.Name,
                Authorized = true,
                Privileges = privileges,
                Menus = menuNode,
                LastLoginDate = admin.LastLoginDate
            };

            // keep in session
            smgr.Set(identity);

            return string.Empty;
        }

        public void add_Login_Logs(string admin_ID, LoginLog LoginLog, bool Enable)
        {
            var Login_Log = new GenLoginLog()
            {
                User_Id = admin_ID,
                LoginMsg = LoginLog.GetEnumDescription(),
                Enable = Enable
            };
            context.Add(Login_Log);
            context.SaveChanges();
        }


        /// <summary>
        /// 檢查是否超過90天未修改密碼
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public async Task<bool> ForceChangePwd(string account)
        {
            bool ret = false;
            var admin = await context.GenEmpData
                .FirstOrDefaultAsync(
                    x => x.Account.Equals(account)
                );
            if (admin != null && admin.PasswordModifyAt.HasValue == true && admin.PasswordModifyAt.Value.AddDays(90) < DateTime.Now)
            {
                ret = true;
            }
            return ret;
        }
        public async Task<LogicRtnModel<bool>> UpdateMyPwd(string pwd)
        {
            var rtnModel = new LogicRtnModel<bool>(MsgType.SaveSuccess);
            try
            {
                var me = await context.GenEmpData
                    .FirstOrDefaultAsync(x => x.Account.Equals(this.identity.Account));

                me.Pwd = Cryptos.EncryptPwd(pwd);
                me.PasswordModifyAt = DateTime.Now;

                await this.context.SaveChangesWithAuditAsync(this.identity.Account, "ChangePwd");
            }
            catch (Exception ex)
            {
                rtnModel = new LogicRtnModel<bool>(MsgType.SaveFail, ex.Message);
            }

            return rtnModel;
        }


        public bool HasAuthorizedFor(string controllerName, string actionName)
        {
            if (string.IsNullOrEmpty(controllerName) || string.IsNullOrEmpty(actionName))
            {
                throw new Exception("AuthorizedFor controllerName & actionName must NOT be NULL");
            }
            var privilege = identity.Privileges
                .Where(x => controllerName.Equals(x.ControllerName)
                         && actionName.Equals(x.ActionName)
                )
                .FirstOrDefault();
            if (privilege == null) return false;

            return privilege.EnableEntry;
        }

        public PrivilegeNodeModel GetMyMenu()
        {
            return this.identity.Menus;
        }

        public PrivilegeNodeModel GetPrivilegeNodeModel(string controllerName, string actionName)
        {
            var privilegNodeModel = identity.Privileges.Where(p =>
                                            p.ControllerName == controllerName
                                            && p.ActionName == actionName)
                                    .FirstOrDefault();
            if (privilegNodeModel != null)
            {
                privilegNodeModel.ParentNode = identity.Privileges
                    .Where(p => p.PrivilegeId == privilegNodeModel.ParentId)
                    .FirstOrDefault();
            }
            return privilegNodeModel;
        }
    }
}