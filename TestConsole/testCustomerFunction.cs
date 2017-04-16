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
            var customer = CustomerOperations.CreateAccount(firstname: "Jon",lastname: "Doe",
                email: "jd@gmail.com", psw: "123456");

            Console.WriteLine(customer.firstName + " " + customer.lastName + " " + customer.email);

            //Change email of the account.
            CustomerOperations.ChangeAccount(ref customer, email: "jg@yahoo.com");
            Console.WriteLine(customer.firstName + " " + customer.lastName + " " + customer.email);

            //Set rooms and view the number of available rooms.
            RoomType type = new RoomType { Id = 1, type = "single", basePrice = 100 };
            Room room = new Room { Id = 1, RoomType = type };
            Console.WriteLine(CustomerOperations.ViewRoom(type));

            DateTime now = System.DateTime.Now;
            DateTime start = new DateTime(2017, 5, 10);
            DateTime end = new DateTime(2017, 5, 15);
            CurrentDateTime current = new CurrentDateTime;
            current.time = now.ToString();

            //Make a reservation.
            var rs = CustomerOperations.MakeReservation(ref customer, start, end, type, current);
            foreach (Reservation r in customer.Reservation)
                Console.WriteLine(r.checkIn + " " + r.checkOut + " " + r.Room.Id);
            var canceled = CustomerOperations.CancelReservation(rs, current);
            if (canceled) { }
            //Console.WriteLine(room.occupied);
            //CustomerOperations.CheckIn(ref customer, ref rs);
            //Console.WriteLine(room.occupied);
            Console.ReadKey();

            }
        }
}
}
