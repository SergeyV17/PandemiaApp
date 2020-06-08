using Newtonsoft.Json;
using System;

namespace ImportData
{
    /// <summary>
    /// Кастомный конвертер DateTime
    /// </summary>
    public class CustomDateTimeConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(DateTime));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            string date = (string)reader.Value;
            
            DateTime parseResult;

            if (DateTime.TryParse(date, out parseResult))
                return Convert.ToDateTime(date);
            else
                return reader.Value;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
