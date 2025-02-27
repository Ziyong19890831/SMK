using SMK.Data.Entity;

namespace SMK.Web.Models
{
    public class IniExportInCtrlViewModel : IniExportInCtrl
    {
        public string StatusStr => Status.ToString();
        public bool FileIsExisted { get; set; }
    }
}
