using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Yozian.DependencyInjectionPlus.Attributes;
using SMK.Data;
using SMK.Data.Dto;
using Yozian.Extension;
using SMK.Data.Entity;
using SMK.Data.Enums;
using SMK.Data.Utility;
using Yozian.EFCorePlus.Extensions;

namespace SMK.Web.Services
{
    [ScopedService]
    public class PrivilegeService : GenericService
    {

        public PrivilegeService(SMKWEBContext context, SessionManager smgr)
            : base(context, smgr)
        {

        }

        /// <summary>
        /// privileges including type node, link, func
        /// </summary>
        /// <param name="empId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<PrivilegeNodeModel>> GetMyPrivileges(string empId, bool? filterEnableEntry)
        {
            var myRoles = await context.RoleEmpMapping
                .Where(x => x.EmpId.Equals(empId))
                .Select(x => x.RoleId)
                .ToListAsync();

            var privileges = (await context
                    .Privilege
                    .GroupJoin(
                        context.RolePrivilegeMapping,
                        privilege => privilege.Id,
                        rolePrivilege => rolePrivilege.PrivilegeId,
                        (privilege, rolePrivilege) => new
                        {
                            privilege,
                            rolePrivilege
                        }
                    )
                    .SelectMany(
                        x => x.rolePrivilege.DefaultIfEmpty(),
                        (privilege, rolePrivilege) => new { privilege.privilege, rolePrivilege }
                    )
                    .Where(x => myRoles.Contains(x.rolePrivilege.RoleId))
                    .WhereWhen(filterEnableEntry.HasValue,x=>x.rolePrivilege.EnableEntry== filterEnableEntry.Value)
                    .Select(composite =>
                        new PrivilegeNodeModel()
                        {
                            Name = composite.privilege.Name,
                            PrivilegeId = composite.privilege.Id,
                            ParentId = composite.privilege.ParentId,
                            ControllerName = composite.privilege.ControllerName,
                            ActionName = composite.privilege.ActionName,
                            EnableEntry = composite.rolePrivilege.EnableEntry,
                            LinkType = composite.privilege.LinkType,
                            Sort = composite.privilege.Sort
                        }
                        )
                    .ToListAsync()
                    )
                    .DistinctBy(x => x.PrivilegeId);

            return privileges;
        }


        public async Task<IEnumerable<Privilege>> GetAllFuntionalities()
        {
            return await this.context.Privilege.Where(x => x.Id.Length > 0).ToListAsync();
        }


        public async Task<PrivilegeNodeModel> GetPrivilegesByRole(string roleId, bool filterOutDisabled)
        {
            var privilegeState = await context.RolePrivilegeMapping
                    .Where(x => x.RoleId.Equals(roleId))
                    .ToListAsync();


            var privileges = context.Privilege
                    .Where(x => !string.IsNullOrEmpty(x.Id))
                    .OrderBy(x => x.ParentId)
                    .ToList()
                    .Select((t) => transformPrivilegeToNode(t))
                    .ToList();

            // 
            privileges.ForEach((p) =>
            {
                p.EnableEntry = privilegeState.Any(x => x.PrivilegeId.Equals(p.PrivilegeId) && x.EnableEntry);
            });

            if (filterOutDisabled)
            {
                privileges = privileges.Where(x => x.EnableEntry).ToList();
            }

            var root = new PrivilegeNodeModel() { LinkType = PrivilegeNodeType.Root };

            root = EstablishTree(root, privileges.ToList());

            return root;
        }

        public async Task<bool> UpdateRolePrivilege(
            string roleId,
            string privilegeId,
            bool enable
            )
        {
            var rpm = await context.RolePrivilegeMapping
                .Where(x => x.RoleId.Equals(roleId))
                .Where(x => x.PrivilegeId.Equals(privilegeId))
                .FirstOrDefaultAsync();

            if (rpm == null)
            {
                // should add
                rpm = new RolePrivilegeMapping()
                {
                    Id = MyGuid.NewGuid(),
                    RoleId = roleId,
                    PrivilegeId = privilegeId,
                    EnableEntry = enable
                };
                context.RolePrivilegeMapping.Add(rpm);
            }
            else
            {
                // should update
                rpm.EnableEntry = enable;
                rpm.UpdatedAt = DateTime.Now;
                context.Update(
                        rpm,
                        x => x.EnableEntry,
                        x => x.UpdatedAt
                    );
            }
            var result = await context.SaveChangesWithAuditAsync(identity.Account) == 1;
            return result;
        }


        public PrivilegeNodeModel EstablishTree(
                PrivilegeNodeModel root,
                List<PrivilegeNodeModel> source
                )
        {
            // for root
            if (string.IsNullOrEmpty(root.ParentId))
            {
                var childs = source
                    .Where(x => string.IsNullOrEmpty(x.ParentId))
                    .OrderBy(x => x.Sort);

                root.Nodes.AddRange(childs);
            }
            var myChilds = source
                .Where(x => !string.IsNullOrEmpty(x.ParentId))
                .Where(x => x.ParentId.Equals(root.PrivilegeId))
                .OrderBy(x => x.Sort);

            if (myChilds != null && myChilds.Count() > 0)
            {
                root.Nodes = myChilds.ToList();
            }

            if (root.Nodes.Count > 0)
            {
                root.Nodes.ForEach((node) =>
                {
                    node = EstablishTree(node, source);
                });
            }

            root.Nodes = root.Nodes.OrderBy(x => x.Sort).ToList();

            return root;
        }


        private PrivilegeNodeModel transformPrivilegeToNode(Privilege model)
        {
            return new PrivilegeNodeModel()
            {
                PrivilegeId = model.Id,
                ParentId = model.ParentId,
                Name = model.Name,
                Sort = model.Sort,
                LinkType = model.LinkType,
                ControllerName = model.ControllerName,
                ActionName = model.ActionName,
                QueryParams = model.QueryParams
            };
        }
    }
}