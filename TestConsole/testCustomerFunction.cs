using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModel;
using BizLogic;


namespace TestConsole
{
    class testCustomerFunction
    {
        public static void test()
        {
            //Create an account.
            var customer = CustomerOperations.CreateAccount(firstname: "Jon", lastname: "Doe",
                email: "jd@gmail.com", psw: "123456");

            Console.WriteLine(customer.firstName + " " + customer.lastName + " " + customer.email);

            //Change email of the account.
            CustomerOperations.ChangeAccount(ref customer, email: "jg@yahoo.com");
            Console.WriteLine(customer.firstName + " " + customer.lastName + " " + customer.email);

            //Set rooms and view the number of available rooms.
            RoomType type = new RoomType { Id = 1, type = "single", basePrice = 100 };
            Room room = new Room { Id = 1, RoomType = type, occupied = false };
            type.Rooms.Add(room);
            Console.WriteLine(CustomerOperations.ViewRoom(type));

            DateTime now = System.DateTime.Now;
            DateTime start1 = new DateTime(2017, 5, 10);
            DateTime end1 = new DateTime(2017, 5, 15);
            DateTime start2 = new DateTime(2017, 6, 10);
            DateTime end2 = new DateTime(2017, 6, 15);
            CurrentDateTime current = new CurrentDateTime();
            current.time = now;

            //Make a reservation.
            var rs1 = CustomerOperations.MakeReservation(ref customer, start1, end1, type, current);
            if (rs1 != null)
            {
                Console.WriteLine("reservation is made.");
            }

            //View reservations.
            var viewRes = CustomerOperations.ViewReservation(customer, current);
            foreach (Reservation r in viewRes)
                Console.WriteLine(r.checkIn + " " + r.checkOut + " " + r.Room.Id);

            //Cancel a reservation.
            var canceled = CustomerOperations.CancelReservation(rs1, current);
            if (canceled)
            {
                Console.WriteLine("reservation is cancelled.");
            }

            //Make another reservation.
            var rs2 = CustomerOperations.MakeReservation(ref customer, start2, end2, type, current);
            Console.WriteLine(room.occupied);

            //Check in.
            Utilities.fastForward(current, new DateTime(2017, 6, 10));
            var checkin = CustomerOperations.CheckIn(ref customer, ref rs2, current);
            Console.WriteLine(room.occupied);

            //Check out.
            Utilities.fastForward(current, new DateTime(2017, 6, 15));
            CustomerOperations.CheckOut(ref rs2, current);
            Console.WriteLine(room.occupied);

            //View loyalty status
            var viewloyal = CustomerOperations.ViewLoyalty(customer);
            Console.WriteLine(viewloyal);
            Console.ReadKey();
        }
    }
}

