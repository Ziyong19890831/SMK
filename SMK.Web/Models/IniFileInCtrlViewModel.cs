using SMK.Data.Entity;

namespace SMK.Web.Models
{
    public class IniFileInCtrlViewModel : IniFileInCtrl
    {
        public string StatusStr => Status.ToString();
        public bool FileIsExisted { get; set; }
    }
}
