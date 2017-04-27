using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModel;

namespace BizLogic
{
    public static class Utilities
    {
        public static void changePrice(RoomType type, int newPrice)
        {
            type.basePrice = newPrice;
        }

        public static void fastForward(CurrentDateTime current, DateTime future)
        {
            current.time = future;
        }
        public static List<DateTime> CalculateNight(DateTime StartingDate, DateTime EndingDate)
        {
            if (StartingDate > EndingDate)
            {
                return null;
            }
            List<DateTime> rv = new List<DateTime>();
            if (StartingDate == EndingDate)
            {
                rv.Add(StartingDate);
                return rv;
            }
            DateTime tmpDate = StartingDate;
            while (tmpDate != EndingDate)
            {
                rv.Add(tmpDate);
                tmpDate = tmpDate.AddDays(1);
            }
            return rv;
        }
    }
}
