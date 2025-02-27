using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SMK.Web.Helpers
{
    public static class EnumExtension
    {
        public static string GetEnumDescription(this Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            if (fi == null) return string.Empty;

            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            //若取不到屬性，則取名稱
            if ((attributes != null) && (attributes.Length > 0))
            {
                return attributes[0].Description;
            }
            else
            {
                return value.ToString();
            }
        }

        public static List<SelectListItem> getEnumList<T>() where T : Enum {
            return Enum.GetValues(typeof(T))
                .Cast<T>()
                .Select(p=>
                new SelectListItem(GetEnumDescription(p),Convert.ToInt32(p).ToString()))
                .ToList();
        }

        public static T ParseNoError<T>(this string value) where T : Enum
        {
            object result;
            if (Enum.TryParse(typeof(T),value, out result)) {
                return (T)result;
            }
            return default(T);
        }
    }
}
