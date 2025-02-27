using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMK.Web.Exceptions
{
    public class APIException : Exception
    {
        public APIException(string message) : base(message)
        {
        }

        public APIException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
