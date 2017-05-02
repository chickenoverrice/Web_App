using System;
using System.Collections.Generic;
using System.Data;
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
        public static List<DateTime> calculateNight(DateTime StartingDate, DateTime EndingDate)
        {
            if (StartingDate > EndingDate)
            {
                return null;
            }
            List<DateTime> dateList = new List<DateTime>();
            if (StartingDate == EndingDate)
            {
                dateList.Add(StartingDate);
                return dateList;
            }
            DateTime tmpDate = StartingDate;
            while (tmpDate != EndingDate)
            {
                dateList.Add(tmpDate);
                tmpDate = tmpDate.AddDays(1);
            }
            return dateList;
        }
    }
}
