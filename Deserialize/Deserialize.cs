using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace ImportData
{
    /// <summary>
    /// Класс десериализации из Json
    /// </summary>
    public static class Deserialize
    {
        /// <summary>
        /// Метод десериализации из Json
        /// </summary>
        /// <typeparam name="T">объект десериализации</typeparam>
        /// <param name="path">путь к файлу</param>
        /// <returns>список объектов</returns>
        public static List<T> LoadJson<T>(string path)
        {
            string json = File.ReadAllText(path);

            return JsonConvert.DeserializeObject<List<T>>(json, new CustomDateTimeConverter());
        }
    }
}
