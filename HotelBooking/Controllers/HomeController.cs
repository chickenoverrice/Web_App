using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HotelBooking.Controllers
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
            ViewBag.Message = "You select";
            ViewBag.From = from.Value.ToString("dd MMMM, yyyy");
            ViewBag.To = to.Value.ToString("dd MMMM, yyyy");
            // get reservation data and roomtype data
            return View();
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
    }
}