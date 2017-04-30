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
        public ActionResult Search(DateTime? Arrival, DateTime? Departure, int? rooms)
        {
            if (!Arrival.HasValue)
            {
                Arrival = DateTime.Now;
            }
            if (!Departure.HasValue)
            {
                Departure = DateTime.Now.AddDays(1);
            }
            if (!rooms.HasValue)
            {
                rooms = 1;
            }
            ViewBag.From = Arrival.Value.ToString("dd MMMM, yyyy");
            ViewBag.To = Departure.Value.ToString("dd MMMM, yyyy");
            ViewBag.Nights = BizLogic.Utilities.CalculateNight(Arrival.Value, Departure.Value).Count;
            ViewBag.Rooms = rooms;
            // get reservation data and roomtype data
            // NEED TO PUT IMPORTANT SQL HERE!!!
            using (var roomtypecontext = new DataModel.HotelDatabaseContainer())
            {
                return View(roomtypecontext.RoomTypes.ToList());
            }
        }
        public ActionResult Book()
        {
            return View("Index");
        }
        [HttpPost]
        public ActionResult Book(DateTime? Arrival, DateTime? Departure, int? Nights, String RoomType, int? RoomId, int? Rooms, int? RoomGuest)
        {
            ViewBag.RoomId = RoomId;
            ViewBag.RoomType = RoomType;
            ViewBag.Rooms = Rooms;
            ViewBag.RoomGuest = RoomGuest;
            ViewBag.From = Arrival.Value.ToString("dd MMMM, yyyy");
            ViewBag.To = Departure.Value.ToString("dd MMMM, yyyy");
            ViewBag.Nights = Nights;
            return View();
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
                        r.checkIn = rvm.checkIn.Value;
                        r.checkOut = rvm.checkOut.Value;
                        r.firstName = rvm.firstName;
                        r.lastName = rvm.lastName;
                        r.email = rvm.email;
                        r.phone = rvm.phone;
                        r.address = rvm.address;
                        r.city = rvm.city;
                        r.state = rvm.state;
                        r.zip = rvm.zip;
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
                    return View();
                }
            }
            ViewBag.RoomId = rvm.roomId;
            ViewBag.RoomType = rvm.roomType;
            ViewBag.RoomGuest = rvm.roomGuest;
            ViewBag.From = rvm.checkIn.Value.ToString("dd MMMM, yyyy");
            ViewBag.To = rvm.checkOut.Value.ToString("dd MMMM, yyyy");
            ViewBag.Nights = BizLogic.Utilities.CalculateNight(rvm.checkIn.Value, rvm.checkOut.Value).Count;
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