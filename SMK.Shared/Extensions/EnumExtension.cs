using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace SMK.Shared.Extensions
{
    public static class EnumExtension
    {
        /// <summary>
        /// Gets an attribute on an enum field value
        /// </summary>
        /// <typeparam name="T">The type of the attribute you want to retrieve</typeparam>
        /// <param name="enumVal">The enum value</param>
        /// <returns>The attribute of type T that exists on the enum value</returns>
        /// <example><![CDATA[string desc = myEnumVariable.GetAttributeOfType<DescriptionAttribute>().Description;]]></example>
        public static T GetAttributeOfType<T>(this Enum enumVal) where T : Attribute
        {
            var type = enumVal.GetType();
            var memInfo = type.GetMember(enumVal.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(T), false);
            return (attributes.Length > 0) ? (T)attributes[0] : null;
        }

        public static Expected GetAttributeValue<T, Expected>(this Enum enumeration, Func<T, Expected> expression)
            where T : Attribute
        {
            T attribute =
                enumeration
                    .GetType()
                    .GetMember(enumeration.ToString())
                    .Where(member => member.MemberType == MemberTypes.Field)
                    .FirstOrDefault()
                    ?.GetCustomAttributes(typeof(T), false)
                    .Cast<T>()
                    .SingleOrDefault();

            if (attribute == null)
                return default(Expected);

            return expression(attribute);
        }

        public static T GetAttribute<T>(this Enum value) where T : Attribute {
            var type = value.GetType();
            var memberInfo = type.GetMember(value.ToString());
            var attributes = memberInfo[0].GetCustomAttributes(typeof(T), false);
            return attributes.Length > 0
                ? (T)attributes[0]
                : null;
        }

        public static string ToName(this Enum value) {
            var attribute = value.GetAttribute<DescriptionAttribute>();
            return attribute == null ? value.ToString() : attribute.Description;
        }
    }
}
