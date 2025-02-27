using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Yozian.DependencyInjectionPlus.Attributes;
using SMK.Data.Dto;
using SMK.Data;
using Yozian.Extension;
using SMK.Data.Entity;
using SMK.Data.Utility;

namespace SMK.Web.Services
{
    [ScopedService]
    public class RoleService : GenericService
    {
        PrivilegeService privilegeService;
        public RoleService(
                SMKWEBContext context,
                PrivilegeService privilegeService,
                SessionManager smgr
            )
            : base(context, smgr)
        {
            this.privilegeService = privilegeService;
        }

        public async Task<LogicRtnModel<List<RoleEmpModel>>> QueryEmpRoles(RoleQueryModel model)
        {
            try
            {
                var rtnModel = new LogicRtnModel<List<RoleEmpModel>>();
                var role = await context.Role.FirstOrDefaultAsync(x => x.Id.Equals(model.Id));

                var query = context.RoleEmpMapping
                        .Where(x => x.RoleId.Equals(role.Id))
                        .Join(
                                context.GenEmpData,
                                empRole => empRole.EmpId,
                                emp => emp.Id,
                                (adminRole, emp) => new RoleEmpModel()
                                {
                                    Id = adminRole.Id,
                                    RoleId = role.Id,
                                    RoleName = role.Name,
                                    EmpId = emp.Id,
                                    Account = emp.Account,
                                    EmpName = emp.Name,
                                    CreatedAt = adminRole.CreatedAt,
                                    UpdatedAt = adminRole.UpdatedAt,
                                    UpdatedBy = adminRole.UpdatedBy
                                }
                            )
                            .WhereWhen(
                                !string.IsNullOrEmpty(model.Name),
                                x => x.EmpName.Contains(model.Name)
                            )
                            .WhereWhen(
                                !string.IsNullOrEmpty(model.EmpAccount),
                                x => x.Account.Contains(model.EmpAccount)
                            );

                rtnModel.Data = await query.ToListAsync();

                rtnModel.ExtendData = role;

                return rtnModel;
            }
            catch (Exception ex)
            {
                return new LogicRtnModel<List<RoleEmpModel>>(MsgType.QueryFail, ex.Message)
                {
                    StackTrace = ex.DumpDetail()
                };
            }
        }

            public async Task<LogicRtnModel<bool>> Remove(string id)
        {
            try
            {
                var rtnModel = new LogicRtnModel<bool>(MsgType.RemoceSuccess);
                context.Remove(new Role() { Id = id });
                context.RoleEmpMapping.Where(x => x.RoleId.Equals(id));
                context.RolePrivilegeMapping.Where(x => x.RoleId.Equals(id));
                var result = await context.SaveChangesWithAuditAsync(identity.Account);

                return rtnModel;

            }
            catch (Exception ex)
            {

                return new LogicRtnModel<bool>(MsgType.RemoveFail, ex.Message)
                {
                    StackTrace = ex.DumpDetail()
                };
            }

        }
        public async Task<LogicRtnModel<GenEmpData>> EmpJoinRole(string roleId, GenEmpData model)
        {
            try
            {
                var rtnModel = new LogicRtnModel<GenEmpData>(MsgType.CreateSuccess)
                {
                    Data = model
                };

                var role = await context
                    .Role
                    .Where(x => x.Id.Equals(roleId))
                    .FirstOrDefaultAsync();

                rtnModel.ExtendData = role;

                var empData = await context
                    .GenEmpData
                    .Where(x => x.Account.Equals(model.Account))
                    .FirstOrDefaultAsync();
                if (empData == null || role == null)
                {
                    rtnModel.ErrMsg = "帳號或角色不存在";
                    return rtnModel;
                }

                var roleEmpMapping = await context.
                    RoleEmpMapping
                    .AnyAsync(p => p.EmpId == empData.Id && p.RoleId == roleId);
                if (roleEmpMapping)
                {
                    rtnModel.ErrMsg = "該帳號已經加入角色";
                    return rtnModel;
                }


                context
                    .RoleEmpMapping
                    .Add(new RoleEmpMapping()
                    {
                        Id = MyGuid.NewGuid(),
                        RoleId = roleId,
                        EmpId = empData.Id,
                    });

                var result = await context.SaveChangesWithAuditAsync(identity.Account);

                return rtnModel;
            }
            catch (Exception ex)
            {
                return new LogicRtnModel<GenEmpData>(MsgType.CreateFail, ex.Message)
                {
                    Data = model,
                    StackTrace = ex.DumpDetail()

                };
            }
        }

        public async Task<LogicRtnModel<GenEmpData>> EmpLeaveRole(string roleId, string empId)
        {
            try
            {
                var rtnModel = new LogicRtnModel<GenEmpData>(MsgType.RemoceSuccess);

                if (string.IsNullOrEmpty(roleId) || string.IsNullOrEmpty(empId))
                {
                    return new LogicRtnModel<GenEmpData>("帳號或角色Id不存在");
                }

                var rm = await context
                    .RoleEmpMapping
                    .Where(x => x.RoleId.Equals(roleId) && x.EmpId.Equals(empId))
                    .FirstOrDefaultAsync();

                context
                    .RoleEmpMapping
                    .Remove(rm);

                var result = await context.SaveChangesWithAuditAsync(identity.Account);

                return rtnModel;
            }
            catch (Exception ex)
            {
                return new LogicRtnModel<GenEmpData>(MsgType.RemoveFail, ex.Message)
                {
                    StackTrace = ex.DumpDetail()
                };
            }
        }

        public async Task<LogicRtnModel<PrivilegeNodeModel>> GetAllPrivilegesByRole(string roleId)
        {
            try
            {
                var rtnModel = new LogicRtnModel<PrivilegeNodeModel>();

                var role = await context.Role.Where(x => x.Id.Equals(roleId)).FirstOrDefaultAsync();

                if (role == null)
                {
                    return new LogicRtnModel<PrivilegeNodeModel>("角色不存在");
                }

                var root = await privilegeService.GetPrivilegesByRole(roleId, false);

                rtnModel.Data = root;
                rtnModel.ExtendData = role;

                return rtnModel;
            }
            catch (Exception ex)
            {
                return new LogicRtnModel<PrivilegeNodeModel>(MsgType.QueryFail, ex.Message)
                {
                    StackTrace = ex.DumpDetail()
                };
            }
        }

        public async Task<LogicRtnModel<bool>> SwitchRolePrivilege(
            string roleId,
            string privilegeId,
            bool enable)
        {
            try
            {
                var rtnModel = new LogicRtnModel<bool>(MsgType.SaveSuccess);

                if (string.IsNullOrEmpty(roleId) || string.IsNullOrEmpty(privilegeId))
                {
                    return new LogicRtnModel<bool>("RoleId & PrivilegeId are REQUIRED!");
                }
                var privilege = await context.Privilege
                                                 .Where(p => p.Id == privilegeId)
                                                 .FirstOrDefaultAsync();
                if (privilege != null && !string.IsNullOrEmpty(privilege.ParentId))
                {
                    if (enable)
                        await SwitchRolePrivilege(roleId, privilege.ParentId, enable);
                    else {
                        var shouldParent =context.RolePrivilegeMapping
                                                 .Where(p => p.EnableEntry
                                                    && p.RoleId == roleId
                                                    && context.Privilege
                                                              .Where(q=>q.ParentId== privilege.ParentId 
                                                                    && q.Id==p.PrivilegeId 
                                                                    && !q.Id.Contains(privilegeId))
                                                              .Any())
                                                 .Any();
                        if (!shouldParent)
                            await SwitchRolePrivilege(roleId, privilege.ParentId, enable);
                    }
                }

                
                rtnModel.Data = await privilegeService
                    .UpdateRolePrivilege(roleId, privilegeId, enable);

                return rtnModel;
            }
            catch (Exception ex)
            {
                return new LogicRtnModel<bool>(MsgType.SaveFail, ex.Message)
                {
                    StackTrace = ex.DumpDetail()
                };
            }
        }



    }
}