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

        public ActionResult Book(DateTime? from, DateTime? to)
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

        public ActionResult Room()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}