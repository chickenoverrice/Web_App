using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModel;
using BizLogic;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new HotelDatabaseContainer())
            {
                Customer customer = new Customer
                {
                    Id = 1,
                    firstName = "Jon",
                    lastName = "Doe",
                    email = "jd@gmail.com",
                    member = false,
                    expirationDate = "",
                    lastExpirationDate = ""
                };
                context.People.Add((Person)customer);

                RoomType type = new RoomType { Id = 1, type = "single", basePrice = 100 };
                Room room = new Room { Id = 1, RoomType = type };
                context.RoomTypes.Add(type);
                context.Rooms.Add(room);

                CurrentDateTime now = new CurrentDateTime { time = System.DateTime.Now.ToString() };
                context.CurrentDateTimes.Add(now);

                CustomerOperations.setLoyalty(customer);
                System.Console.WriteLine(customer.member);
                for (int i = 1; i < 6; i++)
                {
                    Reservation reservation = new Reservation
                    {
                        Id = i,
                        Customers = customer,
                        Room = room,
                        checkIn = now.time,
                        checkOut = Convert.ToDateTime(now.time).AddDays(2).ToString()
                    };
                    context.Reservations.Add(reservation);
                    Utilities.fastForward(now, Convert.ToDateTime(now.time).AddDays(3));
                }

                CustomerOperations.setLoyalty(customer);
                try
                {
                    context.SaveChanges();
                }
                catch (Exception e)
                {

                }

                System.Console.WriteLine(customer.member);
            }
        }
    }
}
