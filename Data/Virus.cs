using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    class Virus
    {
        public bool IsInfected { get; set; }

        public readonly TimeSpan _infectedTime;

        public readonly TimeSpan _firstStageOfTheDisease;

        public readonly TimeSpan _secondStageOfTheDisease;

        public readonly TimeSpan _totalDiseaseTime;

        public readonly TimeSpan _immunityTime;

        public Virus()
        {
            _infectedTime = TimeSpan.FromMinutes(5);

            _firstStageOfTheDisease = TimeSpan.FromDays(4);

            _secondStageOfTheDisease = TimeSpan.FromDays(7);

            _totalDiseaseTime = _firstStageOfTheDisease + _secondStageOfTheDisease;

            _immunityTime = TimeSpan.FromDays(14);
        }
    }
}
