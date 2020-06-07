using System;

namespace Data
{
    static class Virus
    {
        public static readonly TimeSpan _safeTime;

        public static readonly TimeSpan _firstStageOfTheDisease;

        public static readonly TimeSpan _secondStageOfTheDisease;

        public static readonly TimeSpan _totalDiseaseTime;

        public static readonly TimeSpan _immunityTime;

        static Virus()
        {
            _safeTime = TimeSpan.FromMinutes(5);

            _firstStageOfTheDisease = TimeSpan.FromDays(4);

            _secondStageOfTheDisease = TimeSpan.FromDays(12);

            _totalDiseaseTime = _firstStageOfTheDisease + _secondStageOfTheDisease;

            _immunityTime = TimeSpan.FromDays(14);
        }
    }
}
