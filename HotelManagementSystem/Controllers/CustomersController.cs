using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DataModel;
using BizLogic;

namespace HotelManagementSystem.Controllers
{
    public class CustomersController : Controller
    {
        private HotelDatabaseContainer db = new HotelDatabaseContainer();

        // GET: Customers
        public ActionResult Index(int id)
        {
            ViewBag.id = id;
            Session["customer"] = id;
            Customer customer = db.Customers.Find(id);
            return View(customer);
        }

        // GET: Customers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        public ActionResult ReservationDetails(int id)
        {
            Session["customer"] = id;
            return RedirectToAction("Index", "CustomerReservations");

        }

        public ActionResult Loyalty(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            
            string s = CustomerOperations.ViewLoyalty(customer);
            ViewBag.message = s;
            return View();
        }

        // GET: Customers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,firstName,lastName,email,sessionId,address,phone,city,state,zip,sessionExpiration,expirationDate,member,password,loyaltyNum,stays,lastStay")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.People.Add(customer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(customer);
        }

        // GET: Customers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,firstName,lastName,email,address,phone,city,state,zip")] Customer customer)
               {
            if (ModelState.IsValid)
            {
                var dbcustomer = db.Customers.FirstOrDefault(c => c.Id == customer.Id);
                if (customer == null)
                {
                    return HttpNotFound();
                }
                CustomerOperations.ChangeAccount(ref dbcustomer, customer);
                db.SaveChanges();
                //int Id = (int)Session["customer"];
            }
           
                return RedirectToAction("Index", new { id = customer.Id});
        }

        // GET: Customers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Customer customer = db.Customers.Find(id);
            db.People.Remove(customer);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
