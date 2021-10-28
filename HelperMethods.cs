using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeTracker1
{
    class HelperMethods
    {
        public static (long start, long end) CalculateMonth(int year, int month)
        {
            int monthEndInt = month + 1;
            DateTime monthStart = new DateTime(year, month, 1);
            DateTime monthEnd = new DateTime(year, monthEndInt, 1);

            long monthStartTicks = monthStart.Ticks;
            long monthEndTicks = monthEnd.Ticks;

            return (monthStartTicks, monthEndTicks);
        }
    }
}
