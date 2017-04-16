using DataModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLogic
{
    public static class StaffUtilities
    {
        //Generate Bill for Date
        public static double generateBill(Reservation res, double capacityFilled)
        {
            if(res.checkOut == null || res.checkIn == null)
                return 0.0;

            double basePrice = res.Room.RoomType.basePrice;
            double totalDays = ((DateTime) res.checkOut - (DateTime) res.checkIn).TotalDays;

            return basePrice * Math.Pow(3, capacityFilled);
        }

        //Check in
        public static void checkIn(Reservation res)
        {
            res.Room.occupied = true;
        }

        //Check Out 
        public static void checkOut(Reservation res)
        {
            res.Room.occupied = false;
        }

        //Can Decrease Rooms
        //futRes = (from res in context.Reservations where res.checkIn > DateTime.Now and res.Room.RoomType.Id == rType select res)
        public static Boolean canDecreaseRooms(int newQuant, List<Reservation> futRes)
        {
            Dictionary<DateTime, int> occupied = new Dictionary<DateTime, int>();
            foreach(Reservation res in futRes)
            {
                for(DateTime date = res.checkIn.Date; date.Date <= res.checkOut.AddDays(-1).Date; date = date.AddDays(1))
                {
                    if(!occupied.ContainsKey(date))
                    {
                        occupied.Add(date, 0);
                    }

                    occupied[date] += 1;

                    if(occupied[date] > newQuant)
                    {
                        
                        return false;
                    }
                }
            }
            return true;
        }

        //List of Customers expected to check in/Check out: Not Biz Logic

        //Auto-Checkout? Scheduler? 

        //Add Rooms: Not Biz Logic

        //Hotel Occupancy Percentages for Given Date: Not Biz Logic
        //Doesn't quite belong in Biz Logic
        public static double capacityFilled(DateTime date, int rType, HotelDatabaseContainer context)
        {
            /*
            if (rType == -1) //All Types
            {
                double occupied = (from res in context.Reservations
                                   where res.checkIn < date && res.checkOut > date
                                   select res).Count();
                int totalRooms = (from rm in context.Rooms
                                  select rm).Count();
                return occupied / totalRooms;
            }
            else
            {
                double occupied = (from res in context.Reservations
                                   where res.checkIn < date && res.checkOut > date && res.Room.RoomType.Id == rType
                                   select res).Count();
                int totalRooms = (from rm in context.Rooms
                                  where rm.RoomType.Id == rType
                                  select rm).Count();
                return occupied / totalRooms;
            }*/

            return 0;
        }
    }
}
