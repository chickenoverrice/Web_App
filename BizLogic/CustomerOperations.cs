using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;
using DataModel;

namespace BizLogic
{
    public static class CustomerOperations
    {
        public static Room isRoomAvailable(RoomType rt)
        {
            bool emptyroom = false;
            var room = new Room();
            foreach (Room r in rt.Rooms)
            {
                if (!r.occupied)
                {
                    emptyroom = true;
                    room = r;
                    break;
                }
            }
            if (!emptyroom)
                return null;
            else
                return room;
        }

        public static Customer CreateAccount(string firstname, string lastname, string email, 
            string psw, string phone = null, string address = null, string state = null, 
            string city = null, string zip = null)
        {
            var user = new Customer();
            user.firstName = firstname;
            user.lastName = lastname;
            user.email = email;
            user.password = Crypto.HashPassword(psw);
            user.member = false;
            user.phone = phone;
            user.sessionId = Guid.NewGuid().ToString();
            user.state = state;
            user.zip = zip;
            user.city = city;
            user.address = address;
            return user;
        }

        public static void ChangeAccount(ref Customer customer, string firstname = null, string lastname = null, 
            string email = null, string psw = null, string phone = null, string address = null, string state = null,
            string city = null, string zip = null)
        {
            if (firstname != null)
                customer.firstName = firstname;
            if (lastname != null)
                customer.lastName = lastname;
            if (email != null)
                customer.email = email;
            if (psw != null)
                customer.password = Crypto.HashPassword(psw);
            if(phone != null)
                customer.phone = phone;
            if(state != null)
                customer.state = state;
            if(zip != null)
                customer.zip = zip;
            if(city != null)
                customer.city = city;
            if(address != null)
                customer.address = address;
        }

        public static void CheckIn(ref Customer customer, ref Reservation r)
        {            
            r.Room.occupied = true;
            customer.stays++;
            //Check elite member status after each stay.
            if (!customer.member)
            {
                setLoyalty(customer);
                //If promoted to elite status, set the expiration date.
                if (customer.member)
                {
                    var ex = new DateTime(Convert.ToDateTime(r.checkIn).Year + 1, 12, 31);
                    customer.expirationDate = ex.ToString();
                }
            }                         
        }

        public static void CheckOut(ref Reservation r)
        {
            r.Room.occupied = false;
            //Delete reservation after checkout.
            r.Room.Reservations.Remove(r);
        }

        public static Reservation MakeReservation(ref Customer customer, DateTime start, DateTime end, RoomType rt, CurrentDateTime current)
        {
            //Should start and end time be validated here?
            if (DateTime.Compare(start, end) > 0 || DateTime.Compare(current.time, start) > 0)
            {
                return null;
            }

            var room = isRoomAvailable(rt);
            if (room != null)
            {
                var r = new Reservation();
                r.Customer = customer;
                r.checkIn = start;
                r.checkOut = end;
                r.Room = room;
                customer.Reservations.Add(r); //Change customer status; pass by ref.
                room.Reservations.Add(r);    //Change room status; pass by ref.       
                return r;
            }
            else
            {
                return null;
            }
        }

        public static bool CancelReservation(Reservation r, CurrentDateTime current)
        {
            bool err = true;
            if (DateTime.Compare(Convert.ToDateTime(current.time), r.checkIn) <= 0)
            {
                r.Room.Reservations.Remove(r);
                r.Customers.Reservations.Remove(r);
            }
            else
                err = false;
            return err;
        }

        public static int ViewRoom(RoomType rt) 
	    {
            int count = 0;
            foreach (Room r in rt.Rooms)
            {
                if (!r.occupied)
                {
                    count++;
                }
            }
            return count;
        }

        public static ICollection<Reservation> ViewReservation(Customer customer, CurrentDateTime current)
        {
            ICollection<Reservation> view = new List<Reservation>();
            foreach(Reservation r in customer.Reservations)
            {
                if(DateTime.Compare(r.checkIn, current.time) > 0) 
                    view.Add(r);
            }
            return view;
        }

        public static string ViewLoyalty(Customer customer)
        {
            if (!customer.member)
            {
                return (5-customer.stays).ToString() + " more stays to become a memeber";
            }
            else
            {
                return "membership expires on: " + customer.expirationDate;
            }
        }

        //this method is wrong. should not count the reservation!
        public static void setLoyalty(Customer customer)
        {
            if (!customer.member && customer.lastStay != null)
            {
                if (customer.lastStay.Value.Year == now.time.Year)
                {
                    if (customer.stays >= 5)
                    {
                        customer.member = true;
                        customer.expirationDate = new DateTime(now.time.Year, 12, 31);
                    }
                }
            }
        }
    }
}
