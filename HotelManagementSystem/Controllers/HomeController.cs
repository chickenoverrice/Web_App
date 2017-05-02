using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Helpers;
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
            return View("Index");
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
                            //System.Diagnostics.Debug.WriteLine(roomId + "@" + date + " is not available ");
                            available = false;
                            break;
                        }
                        else
                        {
                            // if available, calculate price
                            double percentage = (double) reserved / roomNum;
                            //System.Diagnostics.Debug.WriteLine("Reserved: "+reserved+", Number of rooms: "+roomNum+", Percentage: " + percentage);
                            if (percentage >= 0.75) {
                                price = baseprice * 2;
                            } else if (0.75 > percentage && percentage >= 0.5) {
                                price = baseprice * 1.5;
                            } else {
                                price = baseprice * 1;
                            }
                            //System.Diagnostics.Debug.WriteLine("Price: " + price);
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
            return View("Index");
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
            //System.Diagnostics.Debug.WriteLine("srm.listPrice="+ srm.listPrice);
            //System.Diagnostics.Debug.WriteLine("srm.listPrice.count=" + srm.listPrice.Count);
            RoomType room = new RoomType();
            using (var roomcontext = new DataModel.HotelDatabaseContainer())
            {
                room = roomcontext.RoomTypes.Find(rvm.roomId);
            }
            rvm.roomType = room.type;
            rvm.roomGuest = room.maxGuests;
            return View(rvm);
        }
        [HttpGet]
        public ActionResult Reserve()
        {
            System.Diagnostics.Debug.WriteLine("Reserve HttpGet");
            Reservation r = getReservation(Int32.Parse(Request.Cookies["Reservation"]["Id"]), Request.Cookies["Reservation"]["Email"]);
            ReservationDetailViewModel rvm = new ReservationDetailViewModel();
            RoomType room = new RoomType();
            using (var roomcontext = new DataModel.HotelDatabaseContainer())
            {
                room = roomcontext.RoomTypes.Find(r.RoomTypeId);
            }
            rvm.roomType = room.type;
            rvm.roomGuest = room.maxGuests;
            rvm.checkIn = r.checkIn;
            rvm.checkOut = r.checkOut;
            rvm.nights = BizLogic.Utilities.calculateNight(r.checkIn, r.checkOut);
            return View(rvm);
        }
        [HttpPost]
        public ActionResult Reserve(ReservationDetailViewModel rvm)
        {
            var current = DateTime.Now;
            string start = (string)Session["start"];
            var startTime= DateTime.Parse(start);
            if (current.Subtract(startTime) >= TimeSpan.FromMinutes(10))
            {
                return RedirectToAction("Index");
            }
            using (var reservationcontext = new DataModel.HotelDatabaseContainer())
            {
                try
                {
                    if (ModelState.IsValid)
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
                        r.guestsInfo = String.Join(";",rvm.guestInfoList.ToArray());
                        r.RoomTypeId = rvm.roomId;
                        //r.People.Id = 1;
                        // Chck if user has login id
                        // Yes, r.personid = user.id
                        // No, create a person for guest
                        reservationcontext.Reservations.Add(r);
                        reservationcontext.SaveChanges();
                        System.Diagnostics.Debug.WriteLine("Reservation Made");
                        Response.Cookies["Reservation"]["Id"] = r.Id.ToString();
                        Response.Cookies["Reservation"]["Email"] = r.email;
                        return RedirectToAction("Reserve");
                    }
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine("Catch: " + e);
                    return View("Index");
                }
            }
            rvm.nights = BizLogic.Utilities.calculateNight(rvm.checkIn, rvm.checkOut);
            RoomType room = null;
            using (var roomcontext = new DataModel.HotelDatabaseContainer())
            {
                room = roomcontext.RoomTypes.Find(rvm.roomId);
            }
            rvm.roomType = room.type;
            rvm.roomGuest = room.maxGuests;
            return View("Book", rvm);
        }
        [HttpGet]
        public ActionResult Check()
        {
            return View("Index");
        }
        [HttpPost]
        public ActionResult Check(String reservationId, String email)
        {
            Reservation r = getReservation(Int32.Parse(reservationId), email);
            if (r == null) {
                return View("Index");
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
            rvm.checkIn = r.checkIn;
            rvm.checkOut = r.checkOut;
            rvm.nights = BizLogic.Utilities.calculateNight(r.checkIn, r.checkOut);
            return View(rvm);
        }
        [HttpGet]
        public ActionResult Confirm()
        {
            return View();
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
            ViewBag.Message = "About";
            return View();
        }
        public ActionResult Location()
        {
            ViewBag.Message = "Location";
            return View();
        }
        public static Reservation getReservation(int id, String email)
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

    }
}