using SMK.Data.Dto;
using System;
using System.ComponentModel;

namespace SMK.Web.Models
{
    public class AuditLogQueryModel : PagedRequest
    {
        [DisplayName("帳號")]
        public string Account { get; set; }
        [DisplayName("異動方式")]
        public string ActionType { get; set; }
        [DisplayName("異動類型")]
        public string ActionRemark { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
    }
}
