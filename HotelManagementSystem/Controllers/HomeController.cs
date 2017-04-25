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
            ViewBag.Message = "Your search page.";

            return View();
        }
        [HttpPost]
        public ActionResult Search(DateTime? Checkin, DateTime? Checkout, int? rooms)
        {
            if (!Checkin.HasValue)
            {
                Checkin = DateTime.Now;
            }
            if (!Checkout.HasValue)
            {
                Checkout = DateTime.Now.AddDays(1);
            }
            if (!rooms.HasValue)
            {
                rooms = 1;
            }
            ViewBag.From = Checkin.Value.ToString("dd MMMM, yyyy");
            ViewBag.To = Checkout.Value.ToString("dd MMMM, yyyy");
            ViewBag.Rooms = rooms;
            // get reservation data and roomtype data
            using (var roomtypecontext = new RoomTypeContext())
            {
                return View(roomtypecontext.RoomTypes.ToList());
            }
            
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Book()
        {
            ViewBag.Message = "Your Book page.";

            return View();
        }
        [HttpPost]
        public ActionResult Book(DateTime? Checkin, DateTime? Checkout,int? RoomId, int? Rooms)
        {
            ViewBag.Rooms = Rooms;
            ViewBag.From = Checkin.Value.ToString("dd MMMM, yyyy"); ;
            ViewBag.To = Checkout.Value.ToString("dd MMMM, yyyy"); ;
            using (var roomtypecontext = new RoomTypeContext())
            {
                var room = roomtypecontext.RoomTypes.Find(RoomId);
                ViewBag.RoomType = room.type;
            }
            return View();
        }
        public ActionResult Room()
        {
            ViewBag.Message = "Your room page.";

            return View();
        }
    }
}