using System;
using System.Diagnostics;

namespace Data
{
    /// <summary>
    /// Класс вируса
    /// </summary>
    public static class Virus
    {
        /// <summary>
        /// Безопасное время при котором вирус не передаётся
        /// </summary>
        public static  TimeSpan SafeTime {get; set;}

        public static string SafeTimeStr // Добавил свойство т.к. не смог реализовать привязку в 0:mm в xaml
        {
            get { return string.Format("{0:mm}", SafeTime); }
        }

        /// <summary>
        /// Первая стадия болезни (вирус не передаётся)
        /// </summary>
        public static TimeSpan FirstStageOfTheDisease { get; private set; }

        /// <summary>
        /// Вторая стадия болезни (вирус передаётся)
        /// </summary>
        public static TimeSpan SecondStageOfTheDisease { get; private set; }

        /// <summary>
        /// Общее время болезни
        /// </summary>
        public static TimeSpan TotalDiseaseTime { get; private set; }

        /// <summary>
        /// Время действия иммунитета
        /// </summary>
        public static TimeSpan ImmunityTime { get; private set; }

        static Virus()
        {
            SafeTime = TimeSpan.FromMinutes(5);

            FirstStageOfTheDisease = TimeSpan.FromDays(4);

            SecondStageOfTheDisease = TimeSpan.FromDays(12);

            TotalDiseaseTime = FirstStageOfTheDisease + SecondStageOfTheDisease;

            ImmunityTime = TimeSpan.FromDays(14);
        }

        public static void AcceptVirusSettings(string safeTime, string firstStageOfTheDisease, string secondStageOfTheDisease, string immunityTime)
        {

            if (ErrorProcessing(safeTime, firstStageOfTheDisease, secondStageOfTheDisease, immunityTime))
            {
                throw new Exception("Parsing error");
            }
            else
            {
                Virus.SafeTime = TimeSpan.FromMinutes(int.Parse(safeTime));

                Virus.FirstStageOfTheDisease = TimeSpan.FromDays(int.Parse(firstStageOfTheDisease));

                Virus.SecondStageOfTheDisease = TimeSpan.FromDays(int.Parse(secondStageOfTheDisease));

                Virus.ImmunityTime = TimeSpan.FromDays(int.Parse(immunityTime));

                TotalDiseaseTime = FirstStageOfTheDisease + SecondStageOfTheDisease;
            }
        }


        private static bool ErrorProcessing(string safeTime, string firstStageOfTheDisease, string secondStageOfTheDisease, string immunityTime)
        {
            bool error = false;

            int safeTimeRes;
            int firstStageOfTheDiseaseRes;
            int secondStageOfTheDiseaseRes;
            int immunityTimeRes;

            if (!int.TryParse(safeTime, out safeTimeRes))
                error = true;

            if (!int.TryParse(firstStageOfTheDisease, out firstStageOfTheDiseaseRes))
                error = true;

            if (!int.TryParse(secondStageOfTheDisease, out secondStageOfTheDiseaseRes))
                error = true;

            if (!int.TryParse(immunityTime, out immunityTimeRes))
                error = true;

            return error;
        }
    }
}
