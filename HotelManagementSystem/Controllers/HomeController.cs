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
            using (var roomtypecontext = new HotelDatabaseContainer())
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
        [ValidateAntiForgeryToken]
        public ActionResult Reserve(Reservation rvm)
        {
            using (var reservationcontext = new HotelDatabaseContainer())
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        Reservation r = new Reservation();
                        r.checkIn = rvm.checkIn;
                        r.checkOut = rvm.checkOut;
                        r.Id = rvm.Id;
                        

                        Person p = new Person();
                        // Set Person value
                        p.firstName = rvm.firstName;
                        p.lastName = rvm.lastName;


                        Customer c = new Customer();
                        // Set Customer value
                        
                        reservationcontext.People.Add(p);
                        c.Id = p.Id;
                        reservationcontext.Customers.Add(c);
                        r.Id = p.Id;
                        reservationcontext.Reservations.Add(r);
                        reservationcontext.SaveChanges();
                        System.Diagnostics.Debug.WriteLine("Reservation Made");
                        return RedirectToAction("Index");
                    }
                }
                catch
                {
                    System.Diagnostics.Debug.WriteLine("Catch");
                    return View();
                }
            }
            return View();
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