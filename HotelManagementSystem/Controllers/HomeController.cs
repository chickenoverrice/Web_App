using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Helpers;
using Microsoft.AspNet.Identity;
using HotelManagementSystem.Models;
using BizLogic;
using DataModel;

namespace HotelManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //NOT robust, will fix in spare time.
            var today = DateTime.Now;
            if (today.Month == 12 && today.Day == 31)
            {
                using (var context = new DataModel.HotelDatabaseContainer())
                {
                    var customer = from customers in context.Customers
                                   select customers;
                    foreach(var c in customer)
                    {
                        c.stays = 0;
                    }
                    context.SaveChanges();
                }
            }
            return View();
        }
        public ActionResult Search()
        {
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult Search(DateTime? Arrival, DateTime? Departure)
        {
            if (!Arrival.HasValue)
            {
                Arrival = DateTime.Now;
            }
            if (!Departure.HasValue)
            {
                Departure = DateTime.Now.AddDays(1);
            }
            SearchRoomViewModel srm = new SearchRoomViewModel();
            srm.checkIn = Arrival.Value;
            srm.checkOut = Departure.Value;
            srm.nights = BizLogic.Utilities.calculateNight(Arrival.Value, Departure.Value);
            srm.roomTypes = new List<RoomType>();
            srm.listPrices = new List<List<double>>();
            // Find User Preference Room
            if (getPersonByEmail() != null)
            {
                using (var context = new DataModel.HotelDatabaseContainer())
                {
                    Customer c = context.Customers.Find(getPersonByEmail().Id);
                    srm.prefRoom = c.RoomPref.Id;
                }
            }
            // Find available rooms
            List<RoomType> roomTypes = new List<RoomType>();
            using (var roomtypecontext = new DataModel.HotelDatabaseContainer())
            {
                roomTypes = roomtypecontext.RoomTypes.ToList();
            }
            for (int i = 0; i < roomTypes.Count; i++)
            {
                Boolean available = true;
                List<double> listPrice = new List<double>();

                int roomId = roomTypes[i].Id;
                int roomNum = roomTypes[i].numberOfRooms;
                double baseprice = roomTypes[i].basePrice;

                for (int j = 0; j < srm.nights.Count; j++)
                {
                    DateTime date = srm.nights[j];
                    double price = 0;
                    using (var availableContext = new DataModel.HotelDatabaseContainer())
                    {
                        var sqlstring =
                           "SELECT COUNT(*) FROM dbo.Reservations WHERE dbo.Reservations.RoomTypeId = "
                           + roomId + " AND dbo.Reservations.checkIn <= '" + date + "' AND dbo.Reservations.checkOut > '" + date + "'";
                        int reserved = availableContext.Database.SqlQuery<int>(sqlstring).First();
                        //System.Diagnostics.Debug.WriteLine(roomId +"@"+ date +": "+reserved);
                        if (reserved == roomNum)
                        {
                            // if not available, exit check loop
                            available = false;
                            break;
                        }
                        else
                        {
                            // if available, calculate price for each day
                            double percentage = (double) reserved / roomNum;
                            if (percentage >= 0.75) {
                                price = baseprice * 2;
                            } else if (0.75 > percentage && percentage >= 0.5) {
                                price = baseprice * 1.5;
                            } else {
                                price = baseprice * 1;
                            }
                            listPrice.Add(price);
                        }
                    }
                }
                if (available)
                {
                    srm.roomTypes.Add(roomTypes[i]);
                    srm.listPrices.Add(listPrice);
                }
            }
            return View(srm);
        }
        public ActionResult Book()
        {
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult Book(SearchRoomViewModel srm)
        {
            Session["start"] = DateTime.Now.ToString();

        
            ReservationDetailViewModel rvm = new ReservationDetailViewModel();
            rvm.checkIn = srm.checkIn;
            rvm.checkOut = srm.checkOut;
            rvm.nights = BizLogic.Utilities.calculateNight(srm.checkIn, srm.checkOut);
            rvm.roomId = srm.roomId;
            rvm.listPrice = srm.listPrice;
            using (var roomcontext = new DataModel.HotelDatabaseContainer())
            {
                var room = roomcontext.RoomTypes.Find(rvm.roomId);
                rvm.roomType = room.type;
                rvm.roomGuest = room.maxGuests;
            }
            // Check if user is login
            Person p = getPersonByEmail();
            if (p != null)
            {
                rvm.firstName = p.firstName;
                rvm.lastName = p.lastName;
                rvm.email = p.email;
                rvm.phone = p.phone;
                rvm.address = p.address;
                rvm.city = p.city;
                rvm.state = p.state;
                rvm.zip = p.zip;
            }
            return View(rvm);
        }
        [HttpGet]
        public ActionResult Reserve()
        {
            Reservation r = getReservationById(Int32.Parse(Request.Cookies["Reservation"]["Id"]));
            ReservationDetailViewModel rvm = new ReservationDetailViewModel();
            rvm.reservationId = r.Id.ToString();
            RoomType room = new RoomType();
            using (var roomcontext = new DataModel.HotelDatabaseContainer())
            {
                room = roomcontext.RoomTypes.Find(r.RoomTypeId);
            }
            rvm.roomType = room.type;
            rvm.roomGuest = room.maxGuests;
            rvm.guestInfoList = r.guestsInfo.Split(';').ToList();
            rvm.checkIn = r.checkIn;
            rvm.checkOut = r.checkOut;
            rvm.nights = BizLogic.Utilities.calculateNight(r.checkIn, r.checkOut);
            rvm.firstName = r.firstName;
            rvm.lastName = r.lastName;
            rvm.email = r.email;
            rvm.phone = r.phone;
            rvm.address = r.address;
            rvm.city = r.city;
            rvm.state = r.state;
            rvm.zip = r.zip;
            rvm.bill = r.bill;
            return View(rvm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Reserve(ReservationDetailViewModel rvm)
        {
            var current = DateTime.Now;
            string start = (string)Session["start"];
            var startTime = DateTime.Parse(start);
            if (current.Subtract(startTime) >= TimeSpan.FromMinutes(10))
            {
                ViewBag.message = "Your reservation time has expired. You will be redirected to the home page. Click 'OK' to continue.";
                return View();
            }
            else
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        using (var reservationcontext = new DataModel.HotelDatabaseContainer())
                        {
                            DataModel.Reservation r = new DataModel.Reservation();
                            r.checkIn = rvm.checkIn;
                            r.checkOut = rvm.checkOut;
                            r.firstName = rvm.firstName;
                            r.lastName = rvm.lastName;
                            r.email = rvm.email;
                            r.phone = rvm.phone;
                            r.address = rvm.address;
                            r.city = rvm.city;
                            r.state = rvm.state;
                            r.zip = rvm.zip;
                            r.bill = rvm.bill;
                            r.guestsInfo = String.Join(";", rvm.guestInfoList.ToArray());
                            r.RoomTypeId = rvm.roomId;
                            r.PersonId = getPersonByEmail().Id;
                            reservationcontext.Reservations.Add(r);
                            reservationcontext.SaveChanges();
                            Response.Cookies["Reservation"]["Id"] = r.Id.ToString();
                            return RedirectToAction("Reserve");
                        }
                    }
                    else
                    {
                        rvm.nights = BizLogic.Utilities.calculateNight(rvm.checkIn, rvm.checkOut);
                        RoomType room = new RoomType();
                        using (var roomcontext = new DataModel.HotelDatabaseContainer())
                        {
                            room = roomcontext.RoomTypes.Find(rvm.roomId);
                        }
                        rvm.roomType = room.type;
                        rvm.roomGuest = room.maxGuests;
                        return View("Book", rvm);
                    }
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine("Catch: " + e);
                    return RedirectToAction("Index");
                }
            }
        }

        [HttpGet]
        public ActionResult Check()
        {
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult Check(String reservationId, String email)
        {
            Reservation r = getReservationByIdEmail(Int32.Parse(reservationId), email);
            if (r == null) {
                // show not found for users
                System.Windows.Forms.MessageBox.Show(
                    "Cannot Find This Reservation",
                    "Information",
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Exclamation,
                    System.Windows.Forms.MessageBoxDefaultButton.Button1);
                return RedirectToAction("Index");
            }
            ReservationDetailViewModel rvm = new ReservationDetailViewModel();
            rvm.reservationId = r.Id.ToString();
            RoomType room = new RoomType();
            using (var roomcontext = new DataModel.HotelDatabaseContainer())
            {
                room = roomcontext.RoomTypes.Find(r.RoomTypeId);
            }
            rvm.roomType = room.type;
            rvm.roomGuest = room.maxGuests;
            rvm.guestInfoList = r.guestsInfo.Split(';').ToList();
            rvm.checkIn = r.checkIn;
            rvm.checkOut = r.checkOut;
            rvm.nights = BizLogic.Utilities.calculateNight(r.checkIn, r.checkOut);
            rvm.firstName = r.firstName;
            rvm.lastName = r.lastName;
            rvm.email = r.email;
            rvm.phone = r.phone;
            rvm.address = r.address;
            rvm.city = r.city;
            rvm.state = r.state;
            rvm.zip = r.zip;
            rvm.bill = r.bill;
            return View(rvm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cancel(int reservationId)
        {
            using (var context = new DataModel.HotelDatabaseContainer())
            {
                Reservation r = context.Reservations.Find(reservationId);
                context.Reservations.Remove(r);
                context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        public ActionResult Room()
        {
            ViewBag.Message = "Rooms & Suites";
            List<RoomType> roomTypes = new List<RoomType>();
            using (var roomtypecontext = new DataModel.HotelDatabaseContainer())
            {
                roomTypes = roomtypecontext.RoomTypes.ToList();
            }
            return View(roomTypes);
        }
        public ActionResult About()
        {
            return View();
        }
        public ActionResult Location()
        {
            return View();
        }
        public static Reservation getReservationByIdEmail(int id, String email)
        {
            Reservation r = new Reservation();
            using (var context = new DataModel.HotelDatabaseContainer())
            {
                var query = (from p in context.Reservations
                             where p.Id == id && p.email == email
                             select p).ToList();
                if (query.Count == 0) {
                    return null;
                } else {
                    r = query.First();
                    return r;
                }
            }
        }

        public static Reservation getReservationById(int id)
        {
            using (var context = new DataModel.HotelDatabaseContainer())
            {
                Reservation r = context.Reservations.Find(id);
                return r;
            }
        }

        public Person getPersonByEmail()
        {
            Person person = new Person();
            using (var context = new DataModel.HotelDatabaseContainer())
            {
                var query = (from p in context.People
                             where p.email == User.Identity.Name
                             select p).ToList();
                if (query.Count == 0)
                {
                    return null;
                }
                else
                {
                    person = query.First();
                    return person;
                }
            }
        }
    }
}