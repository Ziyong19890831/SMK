using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMK.Data.Dto
{
    public class IdentityModel
    {

        public string Account { get; set; }
        public string Name { get; set; }
        public bool Authorized { get; set; }


        public IEnumerable<PrivilegeNodeModel> Privileges { get; set; }

        public PrivilegeNodeModel Menus { get; set; }

        public DateTime LastLoginDate { get; set; }

    }

}
