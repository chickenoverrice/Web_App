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
            current.time = future.ToString();
        }
    }
}
