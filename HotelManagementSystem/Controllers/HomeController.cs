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

        [HttpPost]
        public ActionResult Search(DateTime? from, DateTime? to, int? rooms)
        {
            if (!from.HasValue)
            {
                from = DateTime.Now;
            }
            if (!to.HasValue)
            {
                to = DateTime.Now.AddDays(1);
            }
            ViewBag.From = from.Value.ToString("dd MMMM, yyyy");
            ViewBag.To = to.Value.ToString("dd MMMM, yyyy");
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
            ViewBag.Message = "Your book page.";

            return View();
        }
    }
}