using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMK.Web.Models
{
    public class MsgComponentModel
    {
        public AlertMsgType Type { get; set; }
        public string Title { get; set; }
        public string Detail { get; set; }

        public bool DisplaySuccess
        {
            get
            {
                return Type == AlertMsgType.Success && !string.IsNullOrEmpty(Title);
            }
            private set { }
        }
        public bool DisplayInfo
        {
            get
            {
                return Type == AlertMsgType.Info && !string.IsNullOrEmpty(Title);
            }
            private set { }
        }

        public bool DisplayWarning
        {
            get
            {
                return Type == AlertMsgType.Warning && !string.IsNullOrEmpty(Title);
            }
            private set { }
        }
        public bool DisplayError
        {
            get
            {
                return Type == AlertMsgType.Error && !string.IsNullOrEmpty(Title);
            }
            private set { }
        }


    }


    public enum AlertMsgType
    {
        Success,
        Info,
        Warning,
        Error
    }
}
