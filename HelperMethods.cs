using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeTracker1
{
    class HelperMethods
    {
        public static long CalculateMonth(int month, int year)
        {
            DateTime yearMonth = new DateTime(year, month, 01);

            long monthTicks = yearMonth.Ticks;

            return monthTicks;
        }
    }
}
