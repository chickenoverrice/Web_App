using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HotelManagementSystem.CodeFirstModel;
using HotelManagementSystem.Models;
using BizLogic;

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
            System.Diagnostics.Debug.WriteLine(Arrival.GetType());
            
            ViewBag.From = Arrival.Value.ToString("MM/dd/yyyy");
            ViewBag.To = Departure.Value.ToString("MM/dd/yyyy");

            ViewBag.Nights = BizLogic.Utilities.CalculateNight(Arrival.Value, Departure.Value).Count;
            ViewBag.Rooms = rooms;
            // get reservation data and roomtype data
            // NEED TO PUT IMPORTANT SQL HERE!!!
            using (var roomtypecontext = new RoomTypeContext())
            {
                return View(roomtypecontext.RoomTypes.ToList());
            }
        }
        public ActionResult Book()
        {
            return View("Index");
        }
        [HttpPost]
        public ActionResult Book(DateTime? Arrival, DateTime? Departure, int? Nights, String RoomType, int? RoomId, int? Rooms)
        {
            ViewBag.RoomId = RoomId;
            ViewBag.RoomType = RoomType;
            ViewBag.Rooms = Rooms;
            ViewBag.From = Arrival.Value.ToString("MM/dd/yyyy");
            ViewBag.To = Departure.Value.ToString("MM/dd/yyyy");
            ViewBag.Nights = Nights;
            return View();
        }
        public ActionResult Reserve()
        {
            return View("Index");
        }
        [HttpPost]
        public ActionResult Reserve(Reservation rm)
        {
            using (var reservationcontext = new ReservationContext())
            {
                try
                {
                    if (ModelState.IsValid) {
                        Reservation r = new Reservation {

                            checkIn = rm.checkIn,
                            checkOut = rm.checkOut,
                            roomId = rm.roomId,
                            roomNumber = rm.roomNumber,

                            firstName = rm.firstName,
                            lastName = rm.lastName,
                            email = rm.email,
                            phone = rm.phone,

                            personId = rm.personId,
                        };
                        reservationcontext.Reservations.Add(r);
                        reservationcontext.SaveChanges();
                        return View();
                    }
                    //else {
                        foreach (ModelState ms in ViewData.ModelState.Values)
                        {
                            foreach (ModelError e in ms.Errors)
                            {
                                System.Diagnostics.Debug.WriteLine("Error: " + e.ErrorMessage);
                            }
                        }
                        ViewBag.RoomId = rm.roomId;
                        
                        ViewBag.From = rm.checkIn;
                        ViewBag.To = rm.checkOut;
                        ViewBag.Nights = ViewBag.Nights = BizLogic.Utilities.CalculateNight(rm.checkIn.Value, rm.checkOut.Value).Count; ;
                        return View("Book", rm);
                    //}
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine("Error: '{0}'", e);
                    return View();
                }
            }
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