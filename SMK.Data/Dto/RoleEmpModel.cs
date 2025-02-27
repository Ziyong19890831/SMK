using SMK.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace SMK.Data.Dto
{
    public class RoleEmpModel : RoleEmpMapping
    {
        [Display(Name = "帳號")]
        [MaxLength(128)]
        public string Account { get; set; }

        [Display(Name = "姓名")]
        public string EmpName { get; set; }

        [Display(Name = "角色")]
        public string RoleName { get; set; }

    }
}
