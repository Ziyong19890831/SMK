using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMK.Data.Utility
{
    public static class MyGuid
    {
        public static string NewGuid(bool lowerCase = true)
        {
            var guid = Guid.NewGuid().ToString();
            return lowerCase ? guid.ToLower() : guid.ToUpper();
        }
    }
}
