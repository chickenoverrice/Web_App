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

                CustomerOperations.ChangeAccount(ref customer, email:"jg@yahoo.com");
                //Console.WriteLine(customer.firstName + " " + customer.lastName + " " + customer.email);
                
                RoomType type = new RoomType { Id = 1, type = "single", basePrice = 100 };
                Room room = new Room { Id = 1, RoomType = type };
                context.RoomTypes.Add(type);
                context.Rooms.Add(room);
                //Console.WriteLine(CustomerOperations.ViewRoom(type));
                
                CurrentDateTime now = new CurrentDateTime { time = System.DateTime.Now.ToString() };
                context.CurrentDateTimes.Add(now);
                DateTime start = new DateTime(2017, 5, 10);
                DateTime end = new DateTime(2017, 5, 15);
                var rs = CustomerOperations.MakeReservation(ref customer, start, end, type);
                foreach (Reservation r in customer.Reservation)
                    Console.WriteLine(r.checkIn+" "+r.checkOut+" "+r.Room.Id);
                var canceled  = CustomerOperations.CancelReservation(rs, now);
                if (canceled) { }
                //Console.WriteLine(room.occupied);
                //CustomerOperations.CheckIn(ref customer, ref rs);
                //Console.WriteLine(room.occupied);
                Console.ReadKey();
                //CustomerOperations.setLoyalty(customer);
                //System.Console.WriteLine(customer.member);
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
