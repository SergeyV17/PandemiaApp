using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    class WorkSchedule
    {
        public static int WorkHoursPerDay { get; private set; }

        static WorkSchedule()
        {
            WorkHoursPerDay = 8;
        }
    }
}
