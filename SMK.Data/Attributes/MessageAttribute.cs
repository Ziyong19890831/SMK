using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMK.Data.Attributes
{
    public class MessageAttribute : Attribute
    {

        /// <summary>
        /// message use
        /// </summary>
        public string Message { get; set; }

        public MessageAttribute(string message)
        {
            this.Message = message;
        }
    }
}
