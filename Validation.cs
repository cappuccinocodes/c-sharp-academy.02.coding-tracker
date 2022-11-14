using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coding_Tracker
{
    internal class Validation
    {
        internal bool IsValidNumber(string? input) => Int32.TryParse(input, out _);
        internal bool IsValidTimeSpan(DateTime startDate, DateTime endDate) =>
            (endDate - startDate) >= TimeSpan.Zero;
    }
}
