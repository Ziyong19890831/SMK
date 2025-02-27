using Newtonsoft.Json;
using SMK.Data.Utility;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMK.Data.Attributes
{
    /// <summary>
    /// 轉換json自動轉成民國年/反之則轉回資料庫使用欄位樣態
    /// </summary>
    public class WestTwDateConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (string.IsNullOrEmpty((string)value) || string.IsNullOrWhiteSpace((string)value))
            {
                writer.WriteValue(value);
                return;
            }
            writer.WriteValue(((string)value).WestToTwDate());
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (string.IsNullOrEmpty((string)reader.Value) || string.IsNullOrWhiteSpace((string)reader.Value))
                return ((string)reader.Value).TwDateToDateTime();
            return reader.Value;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(string);
        }
    }
}
