using DataModel;
using HotelManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace HotelManagementSystem.Controllers
{
    public class StaffController : Controller
    {
        // GET: Staff/Member/Id
        public ActionResult Member()
        {
            Session["user"] = "1"; //Testing Purposes

            //If User was set
            if (Session["user"] != null)
            {

                int userId = Int32.Parse((string)Session["user"]);

                using (var context = new HotelDatabaseContainer())
                {
                    var member = from x in context.Staff where x.Id == userId select x;

                    if (member.Any())
                    {
                        return View(member.First());
                    }
                }
            }

            return RedirectToAction("Login", "Account"); //Unauthenticated
        }

        // GET: Staff/Inventory
        public ActionResult Inventory()
        {
            //If Authenticated
            if (Session["user"] != null)
            {

                var now = DateTime.Now;
                List<RoomInventoryInfo> rmInfo = new List<RoomInventoryInfo>();
                using (var context = new HotelDatabaseContainer())
                {
                    var roomTypes = from room in context.Rooms
                                    group room by room.RoomType into roomGroups
                                    select new
                                    {
                                        rmType = roomGroups.Key,
                                        quantity = roomGroups.Count(),
                                        occupied = (from res in context.Reservations
                                                    where res.checkOut >= now &&
                                                            res.Room.RoomType == roomGroups.Key
                                                    group res by res.Room into uniqRooms
                                                    select uniqRooms.Key).Count()
                                    };

                    foreach (var roomInfo in roomTypes)
                    {
                        rmInfo.Add(new RoomInventoryInfo
                        {
                            rmType = roomInfo.rmType,
                            quantity = roomInfo.quantity,
                            occupiedRooms = roomInfo.occupied
                        });
                    }
                }

                //Model Needs List<Custom RoomType/Quantity/Rooms Occupied Obj>
                return View(rmInfo);
            }

            return RedirectToAction("Login", "Account"); //Unauthenticated
        }

        [HttpPost]
        public JsonResult updateRecord(int roomTypeId, int newValue)
        {
            var now = DateTime.Now;

            using (var context = new HotelDatabaseContainer())
            {
                //RoomType
                var rTypeById = from rType in context.RoomTypes where rType.Id == roomTypeId select rType;

                if (!rTypeById.Any())
                { //Should Not Be Reachable
                    return Json(new { errorMessage = "Could not Find Room Type" });
                }

                var roomType = rTypeById.First();

                //All Availible Rooms
                var currRooms = from room in context.Rooms where room.RoomType.Id == roomTypeId select room;

                //Get Used Rooms
                var usedRooms = from res in context.Reservations
                                where res.checkOut >= now &&
                                        res.Room.RoomType.Id == roomTypeId
                                group res by res.Room into uniqRooms
                                select uniqRooms.Key;

                if (currRooms.Count() > newValue)
                { //Subtract Rooms
                    //Get Unused rooms with Left Outer Join
                    var unusedRoomsRes = from curr in currRooms
                                         join used in usedRooms on curr.Id equals used.Id into unuRms
                                         from unused in unuRms.DefaultIfEmpty()
                                         where unused == null
                                         orderby curr.Id descending
                                         select curr;

                    if (!unusedRoomsRes.Any() || currRooms.Count() - newValue > unusedRoomsRes.Count())
                        return Json(new { errorMessage = "Not enough unused rooms to get rid of.", usedRooms = usedRooms.Count() });

                    var unusedRooms = unusedRoomsRes.ToList();

                    //Delete Rooms
                    for (var i = 0; i < currRooms.Count() - newValue; i++)
                    {
                        context.Rooms.Remove(unusedRooms[i]);
                    }

                }
                else if (currRooms.Count() < newValue)
                { //Add Rooms
                    var newRooms = newValue - currRooms.Count();

                    //Add Rooms
                    for (var i = 0; i < newValue - currRooms.Count(); i++)
                    {
                        context.Rooms.Add(new Room { RoomType = roomType });
                    }

                }

                //Update DB
                context.SaveChanges();

                return Json(new { usedRooms = usedRooms.Count() });
            }

        }

        // GET: Staff/HotelStatus
        public ActionResult HotelStatus()
        {
            //If Authenticated
            if (Session["user"] != null)
            {
                return View();
            }

            return RedirectToAction("Login", "Account"); //Unauthenticated
        }

        public List<ReservationInfo> ResInfoList(DateTime date, Boolean checkingIn)
        {
            List<ReservationInfo> resInfos = new List<ReservationInfo>();

            using (var context = new HotelDatabaseContainer())
            {
                IEnumerable<Reservation> resToday = null;
                if (checkingIn)
                {
                    resToday = from res in context.Reservations
                               where DbFunctions.TruncateTime(res.checkIn) == date
                               select res;
                }
                else
                {
                    resToday = from res in context.Reservations
                               where DbFunctions.TruncateTime(res.checkOut) == date
                               select res;
                }

                foreach (var res in resToday)
                {
                    resInfos.Add(new ReservationInfo
                    {
                        Id = res.Id,
                        checkIn = res.checkIn,
                        checkOut = res.checkOut,
                        bill = res.bill,
                        guestInfo = res.guestsInfo,
                        roomType = res.Room.RoomType.type,
                        roomNumber = res.Room.Id,
                        personName = res.People.lastName + "," + res.People.firstName,
                        roomOccupied = res.Room.occupied
                    });
                }
            }

            return resInfos;
        }

        public ActionResult CheckInGrid()
        {
            return PartialView("_CheckIn", ResInfoList(DateTime.Today, true));
        }

        public ActionResult CheckInGridDate(DateTime newDate)
        {
            return PartialView("_CheckIn", ResInfoList(newDate, true));
        }

        public ActionResult CheckOutGrid()
        {
            return PartialView("_CheckOut", ResInfoList(DateTime.Today, false));
        }

        public ActionResult CheckOutGridDate(DateTime newDate)
        {
            return PartialView("_CheckOut", ResInfoList(newDate, false));
        }

        public List<RoomInventoryInfo> roomInventory(DateTime date)
        {
            List<RoomInventoryInfo> rmInventory = new List<RoomInventoryInfo>();
            using (var context = new HotelDatabaseContainer())
            {

                var rmDate = from rm in context.RoomTypes
                             select new
                             {
                                 rmType = rm,
                                 quantity = rm.Rooms.Count(),
                                 occupiedRooms = (from res in context.Reservations
                                                  where res.Room.RoomType == rm &&
                                                         DbFunctions.TruncateTime(res.checkIn) <= date &&
                                                         DbFunctions.TruncateTime(res.checkOut) >= date
                                                  group res by res.Room into uniqRooms
                                                  select uniqRooms.Key).Count()
                             };

                foreach (var res in rmDate)
                {
                    rmInventory.Add(new RoomInventoryInfo
                    {
                        rmType = res.rmType,
                        quantity = res.quantity,
                        occupiedRooms = res.occupiedRooms
                    });
                }
            }
            return rmInventory;
        }

        public ActionResult OccupancyGrid()
        {
            return PartialView("_Occupancy", roomInventory(DateTime.Today));
        }

        public ActionResult OccupancyGridDate(DateTime newDate)
        {
            return PartialView("_Occupancy", roomInventory(newDate));
        }

        public void CheckIn(int resId)
        {
            using (var context = new HotelDatabaseContainer())
            {
                var reservation = (from res in context.Reservations where res.Id == resId select res).First();

                reservation.Room.occupied = true;

                context.SaveChanges();
            }
        }

        public void CheckOut(int resId)
        {
            using (var context = new HotelDatabaseContainer())
            {
                var reservation = (from res in context.Reservations where res.Id == resId select res).First();

                reservation.Room.occupied = false;

                context.SaveChanges();
            }
        }
    }
}