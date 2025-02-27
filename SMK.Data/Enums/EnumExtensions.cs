using System;
using System.ComponentModel;
using System.Reflection;

namespace SMK.Data.Enums
{
    public static class EnumExtensions
    {
        public static string GetDescription<TEnum>(this TEnum value) where TEnum : Enum
        {
            // 取得該 enum 成員對應的 field
            FieldInfo field = value.GetType().GetField(value.ToString());

            // 檢查是否有 Description 屬性
            DescriptionAttribute attribute = field.GetCustomAttribute<DescriptionAttribute>();

            // 如果有 Description 屬性，則回傳對應的描述文字，否則回傳 enum 的名稱
            return attribute == null ? value.ToString() : attribute.Description;
        }
    }
}
