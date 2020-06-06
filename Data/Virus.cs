using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            _secondStageOfTheDisease = TimeSpan.FromDays(16);

            _totalDiseaseTime = _firstStageOfTheDisease + _secondStageOfTheDisease;

            _immunityTime = TimeSpan.FromDays(14);
        }
    }
}
