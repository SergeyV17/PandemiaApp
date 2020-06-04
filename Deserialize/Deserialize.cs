using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace ImportData
{
    /// <summary>
    /// Deserialize from json class
    /// </summary>
    public static class Deserialize
    {
        /// <summary>
        /// Deserealize from json method
        /// </summary>
        /// <typeparam name="T">deserialize object</typeparam>
        /// <param name="path">path to file</param>
        /// <returns>list of objects</returns>
        public static List<T> LoadJson<T>(string path)
        {
            string json = File.ReadAllText(path);

            return JsonConvert.DeserializeObject<List<T>>(json, new CustomDateTimeConverter());
        }
    }
}
