using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SMK.Data;
using SMK.Data.Dto;
using SMK.Data.Entity;
using SMK.Data.Enums;
using SMK.Data.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Yozian.Extension;

namespace GenPrivilege
{
    public class PrivilegeFixutre
    {
        SMKWEBContext context;
        public PrivilegeFixutre(SMKWEBContext context)
        {
            this.context = context;
        }

        public void Process()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Privileges.json");
            var json = File.ReadAllText(path);
            var root = JsonConvert.DeserializeObject<PrivilegeNodeModel>(json);

            var privileges = toPlainList(root)
                .Select((node) =>
                {
                    return new Privilege()
                    {
                        Id = node.PrivilegeId,
                        ParentId = node.ParentId,
                        Name = node.Name,
                        ActionName = node.ActionName,
                        ControllerName = node.ControllerName,
                        LinkType = node.LinkType,
                        Sort = node.Sort
                    };
                });

            context.Database.ExecuteSqlRaw("Truncate Table [Privilege]");

            context.Privilege.AddRange(privileges);


            // add all privilege to SuperAdmin
            privileges
                .Select(x =>
                {
                    return new RolePrivilegeMapping()
                    {
                        Id = MyGuid.NewGuid(),
                        RoleId = Guid.Empty.ToString(),
                        PrivilegeId = x.Id,
                        EnableEntry = true
                    };
                })
                .ForEach(e => context.RolePrivilegeMapping.Add(e));

            context.SaveChanges();
        }


        private List<PrivilegeNodeModel> toPlainList(PrivilegeNodeModel root)
        {

            var collector = new List<PrivilegeNodeModel>();
            root.PrivilegeId = MyGuid.NewGuid();
            if (root.LinkType != PrivilegeNodeType.Root)
            {
                collector.Add(root);
            }
            if (root.Nodes == null) return collector;

            root.Nodes.ForEach((node) =>
            {
                // auto assign
                node.PrivilegeId = MyGuid.NewGuid();
                if (root.LinkType != PrivilegeNodeType.Root)
                {
                    node.ParentId = root.PrivilegeId;
                }
                collector.AddRange(toPlainList(node));
            });

            return collector;
        }
    }
}
