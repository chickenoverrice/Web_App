using DataModel;
using System;
using BizLogic;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace TestConsole
{
    public static class StaffTests
    {
        //=============Unit Tests==================

        //Assert Bill is Set to Correct Value
        public static string generateBill_Test()
        {
            using (var context = new HotelDatabaseContainer())
            {
                DateTime date = DateTime.Now.Date;
                DateTime dateEnd = date.AddDays(1).AddHours(14); //2PM the next day

                //Add 1 new RoomType
                RoomType rType = new RoomType { type="ply209_Single", basePrice=100.0};
                context.RoomTypes.Add(rType);

                //Add 2 Rooms
                Room rm1 = new Room { RoomType = rType };
                Room rm2 = new Room { RoomType = rType };
                context.Rooms.Add(rm1);
                context.Rooms.Add(rm2);

                //Create a Person
                Person dummy = new Person { firstName = "ply209_FN",
                                            lastName = "ply209_LN",
                                            email = "ply209_email@dummy.com"
                                          };
                context.People.Add(dummy);

                //Add 2 Reservations
                Reservation origRes = new Reservation { People = dummy, checkIn = DateTime.Now, checkOut = DateTime.Now.Date.AddDays(1), Room = rm1 };
                context.Reservations.Add(origRes);

                try
                {
                    context.SaveChanges();
                } catch (Exception e)
                {
                    return "generateBill: Failed: " + e.Message;
                }

                //Calculate Occupied
                double occupied = (from res in context.Reservations
                                   where res.checkIn >= date && res.checkOut <= dateEnd
                                   select res).Count();

                double total = (from rm in context.Rooms select rm).Count();

                double occupiedPer = occupied / total;

                double bill = StaffUtilities.generateBill(origRes, occupiedPer);

                double calcBill = rType.basePrice * Math.Pow(3, occupiedPer); //Formula: Around 173.21 for clean Slate

                //CleanUp
                context.Reservations.Remove(origRes);
                context.Rooms.Remove(rm1);
                context.Rooms.Remove(rm2);
                context.RoomTypes.Remove(rType);
                context.People.Remove(dummy);

                try
                {
                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    return "generateBill: Failed: " + e.Message;
                }

                if (bill != calcBill)
                    return "generateBill: Failed: " + bill;

                return "generateBill: Passed " + bill;
            }
        }

        //Assert CheckIn
        public static string checkIn_Test()
        {
            using (var context = new HotelDatabaseContainer())
            {
                DateTime date = DateTime.Now.Date;

                //Add 1 new RoomType
                RoomType rType = new RoomType { type = "ply209_Single", basePrice = 100.0 };
                context.RoomTypes.Add(rType);

                //Add 1 Room
                Room rm1 = new Room { RoomType = rType };
                context.Rooms.Add(rm1);

                //Create a Person
                Person dummy = new Person
                {
                    firstName = "ply209_FN",
                    lastName = "ply209_LN",
                    email = "ply209_email@dummy.com"
                };

                //Add a Reservation
                Reservation origRes = new Reservation { People = dummy, checkIn = DateTime.Now, checkOut = DateTime.Now, Room = rm1 };
                context.Reservations.Add(origRes);

                //CheckIn
                StaffUtilities.checkIn(origRes);

                if (!rm1.occupied)
                    return "checkIn: Failed: ";

                return "checkIn: Passed ";
            }
        }

        //Assert CheckOut
        public static string checkOut_Test()
        {
            using (var context = new HotelDatabaseContainer())
            {
                DateTime date = DateTime.Now.Date;

                //Add 1 new RoomType
                RoomType rType = new RoomType { type = "ply209_Single", basePrice = 100.0 };
                context.RoomTypes.Add(rType);

                //Add 1 Occupied Room
                Room rm1 = new Room { RoomType = rType, occupied = true };
                context.Rooms.Add(rm1);

                //Create a Person
                Person dummy = new Person
                {
                    firstName = "ply209_FN",
                    lastName = "ply209_LN",
                    email = "ply209_email@dummy.com"
                };

                //Add 1 Reservation
                Reservation origRes = new Reservation { People = dummy, checkIn = DateTime.Now, checkOut = DateTime.Now, Room = rm1 };
                context.Reservations.Add(origRes);

                //Checkout
                StaffUtilities.checkOut(origRes);

                if (rm1.occupied)
                    return "checkIn: Failed: ";

                return "checkIn: Passed ";
            }
        }

        //Assert can Decrease Rooms Works
        public static string canDecreaseRooms_Test()
        {
            using (var context = new HotelDatabaseContainer())
            {
                DateTime date = DateTime.Now.Date;

                //Add 1 new RoomType
                RoomType rType = new RoomType { type = "ply209_Single", basePrice = 100.0 };
                context.RoomTypes.Add(rType);

                //Add 2 Rooms
                Room rm1 = new Room { RoomType = rType };
                Room rm2 = new Room { RoomType = rType };
                context.Rooms.Add(rm1);
                context.Rooms.Add(rm2);

                //Create a Person
                Person dummy = new Person
                {
                    firstName = "ply209_FN",
                    lastName = "ply209_LN",
                    email = "ply209_email@dummy.com"
                };

                //Add 2 Reservations
                Reservation origRes = new Reservation { People = dummy, checkIn = DateTime.Now, checkOut = DateTime.Now.AddDays(1), Room = rm1 };
                Reservation conflictingRes = new Reservation { People = dummy, checkIn = DateTime.Now, checkOut = DateTime.Now.AddDays(1), Room = rm2 };
                context.Reservations.Add(origRes);
                context.Reservations.Add(conflictingRes);

                try
                {
                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    return "canDecreaseRooms: Failed: " + e.Message;
                }

                List<Reservation> futRes = 
                    (from res1 in context.Reservations
                     where res1.checkIn >= date && res1.Room.RoomType.Id == rType.Id
                    select res1).ToList();

                string fail = "canDecreaseRooms: Success";

                //Cannot Take out all Room Today
                if (StaffUtilities.canDecreaseRooms(0, futRes))
                    fail = "canDecreaseRooms: Failed on False";

                int total = (from rm in context.Rooms select rm).Count();

                //Can Decrease Rooms to Greate than max
                if (!StaffUtilities.canDecreaseRooms(total + 1, futRes))
                    fail = "canDecreaseRooms: Failed on True";

                //CleanUp
                context.Reservations.Remove(origRes);
                context.Reservations.Remove(conflictingRes);
                context.Rooms.Remove(rm1);
                context.Rooms.Remove(rm2);
                context.RoomTypes.Remove(rType);
                context.People.Remove(dummy);

                try
                {
                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    return "canDecreaseRooms: Failed: " + e.Message;
                }

                return fail;
            }
        }

        //=============End: Unit Tests====================

        //==============Unit Test Driver==================

        //Run all Tests
        public static void runTests()
        {
            //Run Tests
            StringBuilder results = new StringBuilder();

            results.AppendLine(generateBill_Test());
            results.AppendLine(checkIn_Test());
            results.AppendLine(checkOut_Test());
            results.AppendLine(canDecreaseRooms_Test());

            Debug.Write(results.ToString());
        }

        //==============End: Unit Test Driver=============
    }
}
