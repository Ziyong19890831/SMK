using SMK.Data.Dto;
using System.ComponentModel;

namespace SMK.Web.Models
{
    public class EmpLoginLogViewModel : PagedRequest
    {
        [DisplayName("登入時間(起)")]
        public string LoginTime_Start { get; set; }
        [DisplayName("登入時間(迄)")]
        public string LoginTime_End { get; set; }    
        [DisplayName("使用者姓名")]
        public string User_Name { get; set; }   
        [DisplayName("使用者帳號")]
        public string User_Account { get; set; }     
        [DisplayName("登入狀態")]
        public string LoginMsg { get; set; }
        [DisplayName("登入時間")]
        public string LoginTime { get; set; }
        [DisplayName("帳號狀態")]
        public string Enable { get; set; }
    }
}
