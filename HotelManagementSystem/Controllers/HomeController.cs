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
            // Find available rooms
            // NEED TO PUT IMPORTANT SQL HERE!!!
            List<RoomType> rooms = null;
            using (var roomtypecontext = new DataModel.HotelDatabaseContainer())
            {
                rooms = roomtypecontext.RoomTypes.ToList();
            }
            srm.rooms = rooms;
            for (int i = 0; i < rooms.Count; i++)
            {
                for (int j = 0; j < srm.nights.Count; j++)
                {
                    int roomId = rooms[i].Id;
                    DateTime date = srm.nights[j];
                    int roomNum = rooms[i].numberOfRooms;
                    using (var availableContext = new DataModel.HotelDatabaseContainer())
                    {
                        var sqlstring = 
                           "SELECT COUNT(*) FROM dbo.Reservations WHERE Room_Id = "
                           + roomId + " AND " + date + " BETWEEN checkIn AND checkOut";
                        System.Diagnostics.Debug.WriteLine(sqlstring);
                        var sql = availableContext.Reservations.SqlQuery(sqlstring).First();
                        System.Diagnostics.Debug.WriteLine(sql);
                    }
                }        
            }
           //srm.avgPrices = ......;
            
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
            // get roomType and maxGuest by using roomId
            RoomType room = null;
            using (var roomcontext = new DataModel.HotelDatabaseContainer())
            {
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
                        r.bill = rvm.bill;
                        r.Room.Id = 5004;
                        r.People.Id = 1;
                        // Chck if user has login id
                        // Yes, r.personid = user.id
                        // No, create a person for guest
                        reservationcontext.Reservations.Add(r);
                        reservationcontext.SaveChanges();
                        System.Diagnostics.Debug.WriteLine("Reservation Made");
                        return View("Confirm");
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

    }
}