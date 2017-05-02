using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HotelManagementSystem.Models;
using BizLogic;
using DataModel;

namespace HotelManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
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
            srm.nights = BizLogic.Utilities.CalculateNight(Arrival.Value, Departure.Value);
            srm.roomTypes = new List<RoomType>();
            //srm.avgPrices = new List<double>();
            srm.listPrices = new List<List<double>>();
            // Find available rooms
            List<RoomType> roomTypes = new List<RoomType>();
            using (var roomtypecontext = new DataModel.HotelDatabaseContainer())
            {
                roomTypes = roomtypecontext.RoomTypes.ToList();
            }
            //srm.rooms = roomTypes;
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
                        System.Diagnostics.Debug.WriteLine(roomId +"@"+ date +": "+reserved);
                        if (reserved == roomNum)
                        {
                            // not available
                            System.Diagnostics.Debug.WriteLine(roomId + "@" + date + " is not available ");
                            available = false;
                            // exit night check loop
                            break;
                        }
                        else
                        {
                            // available
                            // calculate every night price
                            double percentage = (double) reserved / roomNum;
                            System.Diagnostics.Debug.WriteLine("Reserved: "+reserved+", Number of rooms: "+roomNum+", Percentage: " + percentage);
                            if (percentage >= 0.75)
                            {
                                price = baseprice * 2;
                            }
                            else if (0.75 > percentage && percentage >= 0.5)
                            {
                                price = baseprice * 1.5;
                            }
                            else
                            {
                                price = baseprice * 1;
                            }
                            System.Diagnostics.Debug.WriteLine("Price: " + price);
                            listPrice.Add(price);
                        }
                    }
                }
                if (available)
                {
                    srm.roomTypes.Add(roomTypes[i]);
                    // calculate average price for all night
                    //srm.avgPrices.Add(listPrice.Average());
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
            ReservationDetailViewModel rvm = new ReservationDetailViewModel();
            rvm.checkIn = srm.checkIn;
            rvm.checkOut = srm.checkOut;
            rvm.nights = BizLogic.Utilities.CalculateNight(srm.checkIn, srm.checkOut);
            rvm.roomId = srm.roomId;
            rvm.listPrice = srm.listPrice;
            System.Diagnostics.Debug.WriteLine("srm.listPrice="+ srm.listPrice);
            System.Diagnostics.Debug.WriteLine("srm.listPrice.count=" + srm.listPrice.Count);
            // get roomType and maxGuest by using roomId
            RoomType room = null;
            using (var roomcontext = new DataModel.HotelDatabaseContainer())
            {
                //roomcontext.Reservations.First().Room.RoomType.Id;
                room = roomcontext.RoomTypes.Find(rvm.roomId);
            }
            rvm.roomType = room.type;
            rvm.roomGuest = room.maxGuests;
            return View(rvm);
        }
        public ActionResult Reserve()
        {
            return View("Index");
        }
        [HttpPost]
        public ActionResult Reserve(ReservationDetailViewModel rvm)
        {
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
                        // what should be in bill?
                        r.bill = rvm.bill;
                        r.RoomTypeId = rvm.roomId;
                        //r.People.Id = 1;
                        // Chck if user has login id
                        // Yes, r.personid = user.id
                        // No, create a person for guest
                        reservationcontext.Reservations.Add(r);
                        reservationcontext.SaveChanges();
                        System.Diagnostics.Debug.WriteLine("Reservation Made");
                        return View("Confirm", rvm);
                    }
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine("Catch" + e);
                    return View("Index");
                }
            }
            rvm.nights = BizLogic.Utilities.CalculateNight(rvm.checkIn, rvm.checkOut);
            RoomType room = null;
            using (var roomcontext = new DataModel.HotelDatabaseContainer())
            {
                room = roomcontext.RoomTypes.Find(rvm.roomId);
            }
            rvm.roomType = room.type;
            rvm.roomGuest = room.maxGuests;
            return View("Book", rvm);
        }
        public ActionResult Room()
        {
            ViewBag.Message = "Room page.";
            return View();
        }
        public ActionResult About()
        {
            ViewBag.Message = "About page.";
            return View();
        }
        public ActionResult Contact()
        {
            ViewBag.Message = "Contact page.";
            return View();
        }
        public ActionResult Confirm(ReservationDetailViewModel rvm)
        {
            ViewBag.Message = "Confirm page.";
            return View();
        }

    }
}