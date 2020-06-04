using Newtonsoft.Json;
using System;

namespace ImportData
{
    public class CustomDateTimeConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(DateTime) || objectType == typeof(string));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            string date = (string)reader.Value;

            try
            {
                return Convert.ToDateTime(date);
            }
            catch 
            {
                return reader.Value;
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
