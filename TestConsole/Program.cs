using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModel;
using BizLogic;
using System.Diagnostics;
using System.Data.Entity.Validation;

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
                    password = "badpass"
                    //member = false //Defaults to False
                };
                context.People.Add((Person)customer);

                RoomType type = new RoomType { type = "hmz224_single", basePrice = 100.0 };
                Room room = new Room { RoomType = type };
                context.RoomTypes.Add(type);
                context.Rooms.Add(room);

                CurrentDateTime now = new CurrentDateTime { time = System.DateTime.Now};
                context.CurrentDateTimes.Add(now);

                CustomerOperations.setLoyalty(customer, now);
                for (int i = 1; i < 6; i++)
                {
                    Reservation reservation = new Reservation
                    {
                        Id = i,
                        People = customer,
                        Room = room,
                        checkIn = now.time,
                        checkOut = Convert.ToDateTime(now.time).AddDays(3)
                    };

                    if (customer.lastStay != null && customer.lastStay.Value.Year == now.time.Year)
                        customer.stays++;
                    else
                        customer.stays = 0;

                    context.Reservations.Add(reservation);
                    customer.stays++; 
                    customer.lastStay = reservation.checkOut;
                    Utilities.fastForward(now, Convert.ToDateTime(now.time).AddDays(3));
                }

                CustomerOperations.setLoyalty(customer, now);

                try
                {
                    context.SaveChanges();
                    Debug.WriteLine("Success");
                    Debug.WriteLine("Loyalty: " + customer.member);
                } 
                catch (DbEntityValidationException e)
                {
                    foreach (DbEntityValidationResult err in e.EntityValidationErrors)
                    {
                        foreach (DbValidationError valerr in err.ValidationErrors)
                        {
                            Debug.WriteLine(valerr.ErrorMessage);
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                }

                //Cleanup
                context.RoomTypes.Remove(type);
                context.Rooms.Remove(room);
                context.People.Remove((Person)customer);
                context.CurrentDateTimes.Remove(now);
                foreach (var entity in context.Reservations)
                    context.Reservations.Remove(entity);

                try
                {
                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                }
            }

            //Run Staff Tests
            StaffTests.runTests();

            //Run customer tests.
            testCustomerFunction.test();
        }
    }
}
