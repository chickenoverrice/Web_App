﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DataModel;
using BizLogic;
using System.Text.RegularExpressions;

namespace HotelManagementSystem.Controllers
{
    [Authorize]
    public class CustomersController : Controller
    {
        private HotelDatabaseContainer db = new HotelDatabaseContainer();

        // GET: Customers
        public ActionResult Index()
        {
            Customer customer = (from users in db.Customers
                            where users.email == User.Identity.Name
                            select users).FirstOrDefault();
            if (customer == null)
                return HttpNotFound();
            return View(customer);
        }

        // GET: Customers/Details/5
        public ActionResult Details()
        {
            Customer customer = (from users in db.Customers
                                 where users.email == User.Identity.Name
                                 select users).FirstOrDefault();
            if (customer == null)
            {
                return HttpNotFound();
            }
            string s = CustomerOperations.ViewLoyalty(customer);
            string start = customer.lastStay.HasValue ? customer.lastStay.Value.ToShortDateString() : string.Empty;
            ViewBag.message = s;
            ViewBag.start = start;

            return View(customer);
        }

        public ActionResult ReservationDetails()
        {
            return RedirectToAction("Index", "CustomerReservations");

        }

        public ActionResult MakeReservation()
        {
            //Session["customer"] = id;
            return RedirectToAction("Create", "CustomerReservations");

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
        public ActionResult Edit()
        {
            Customer customer = (from users in db.Customers
                                 where users.email == User.Identity.Name
                                 select users).FirstOrDefault();
            if (customer == null)
            {
                return HttpNotFound();
            }
            ViewBag.RoomPref = new SelectList(db.RoomTypes, "Id", "type");
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id, firstName, lastName, city, address, zip, state,phone")]Customer customer, FormCollection fc)
        {
            ViewBag.RoomPref = new SelectList(db.RoomTypes, "Id", "type");
            int rtype = Convert.ToInt32(fc["roomPref"]);
            var roomtype = (from types in db.RoomTypes
                            where types.Id == rtype
                            select types).FirstOrDefault();
            if (ModelState.IsValid)
            {
                Customer dbcustomer = (from users in db.Customers
                                       where users.email == User.Identity.Name
                                       select users).FirstOrDefault();
                if (dbcustomer == null)
                {
                    return HttpNotFound();
                }
                customer.RoomPref = roomtype;
                CustomerOperations.ChangeAccount(ref dbcustomer, customer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
           return View(customer);
        }


        // GET: Customers/Delete/5
        public ActionResult Delete()
        {
            Customer customer = (from users in db.Customers
                                 where users.email == User.Identity.Name
                                 select users).FirstOrDefault();
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Customer customer)
        {
            var dbcustomer = db.Customers.FirstOrDefault(c => c.Id == customer.Id);
            db.People.Remove(dbcustomer);
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
