using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMe.Application.Helpers
{
    public class WeekDaysProvider
    {
        public static List<string> GetPolishDaysOfWeek()
        {
            return new List<string>
        {
            "Poniedziałek", "Wtorek", "Środa", "Czwartek", "Piątek", "Sobota", "Niedziela"
        };
        }
    }
}
