using SMK.Data.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using Yozian.Extension;

namespace SMK.Data.Dto
{
    public class PrivilegeNodeModel
    {
        public PrivilegeNodeModel ParentNode { get; set; } = null;

        public List<PrivilegeNodeModel> Nodes { get; set; } = new List<PrivilegeNodeModel>();

        public string PrivilegeId { get; set; }

        public string ParentId { get; set; }

        public string Name { get; set; }

        public int Sort { get; set; }

        public string LinkTypeStr
        {
            get
            {
                return this.LinkType.ToString();
            }
            set
            {
                this.LinkType = value.ToEnum<PrivilegeNodeType>();
            }
        }

        public PrivilegeNodeType LinkType { get; set; }

        public string ControllerName { get; set; }

        public string ActionName { get; set; }

        public string QueryParams { get; set; }

        public bool EnableEntry { get; set; }

    }
}
