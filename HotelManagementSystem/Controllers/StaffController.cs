using DataModel;
using BizLogic;
using HotelManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace HotelManagementSystem.Controllers
{
    [Authorize]
    public class StaffController : Controller
    {
        public ActionResult Index()
        {
            var context = new HotelDatabaseContainer();
            Staff staff = (from s in context.Staff
                           where s.email == User.Identity.Name
                           select s).FirstOrDefault();
            return View(staff);
        }

        // GET: Staff/Inventory
        public ActionResult Inventory()
        {
            var now = DateTime.Now;
            List<RoomInventoryInfo> rmInfo = new List<RoomInventoryInfo>();
            using (var context = new HotelDatabaseContainer())
            {
                foreach (var rmType in context.RoomTypes)
                {
                    rmInfo.Add(new RoomInventoryInfo
                    {
                        rmType = rmType,
                        quantity = rmType.numberOfRooms,
                        occupiedRooms = occupiedRooms(rmType, context)
                    });
                }
            }

            //Model Needs List<Custom RoomType/Quantity/Rooms Occupied Obj>
            return View(rmInfo);
        }

        private int occupiedRooms(RoomType rmType, HotelDatabaseContainer context)
        {
            var dates = from res in context.Reservations
                        where res.RoomType.Id == rmType.Id
                        select res.checkOut;

            if (!dates.Any())
                return 0;

            var farthestDate = dates.Max();
            var occupied = 0;
            for (DateTime date = DateTime.Today.AddDays(1); date.Date <= farthestDate; date = date.AddDays(1))
            {
                var occupiedToday = (from res in context.Reservations
                                     where res.RoomType.Id == rmType.Id &&
                                     res.checkIn < date && res.checkOut >= date
                                     select res).Count();
                if (occupiedToday > occupied)
                    occupied = occupiedToday;
            }
            return occupied;
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
                var currRooms = roomType.numberOfRooms;

                //Get Used Rooms
                var usedRoomsCount = occupiedRooms(roomType, context);

                if (currRooms > newValue)
                { //Subtract Rooms
                    //Get Unused rooms with Left Outer Join
                    var unusedRoomsRes = currRooms - usedRoomsCount;

                    var unusedRooms = (from res in context.Rooms where !res.occupied select res).ToList();

                    if (unusedRoomsRes < 0 || unusedRooms.Count() < unusedRoomsRes)
                        return Json(new { errorMessage = "Not enough unused rooms to get rid of.", usedRooms = usedRoomsCount });

                    //Delete Rooms
                    for (var i = 0; i < unusedRoomsRes; i++)
                    {
                        context.Rooms.Remove(unusedRooms[i]);
                    }

                }
                else if (currRooms < newValue)
                { //Add Rooms
                    var newRooms = newValue - currRooms;

                    //Add Rooms
                    for (var i = 0; i < newValue - currRooms; i++)
                    {
                        context.Rooms.Add(new Room { RoomType = roomType });
                    }

                }

                roomType.numberOfRooms = newValue;

                //Update DB
                context.SaveChanges();

                return Json(new { usedRooms = usedRoomsCount });
            }

        }

        // GET: Staff/HotelStatus
        public ActionResult HotelStatus()
        {
            return View();
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
                        checkIn = res.Room == null ? "*" : res.checkIn.ToString("g"),
                        checkOut = res.Room == null ? "*" : res.Room.occupied? "*": res.checkOut.ToString("g"),
                        bill = res.bill,
                        guestInfo = res.guestsInfo,
                        roomType = res.RoomType.type,
                        roomNumber = res.Room == null ? "*" : res.Room.Id.ToString(),
                        personName = res.People.lastName + "," + res.People.firstName,
                        checkInToday = res.checkIn.Date == DateTime.Today,
                        checkOutToday = res.checkOut.Date == DateTime.Today,
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
            date = date.Date.AddDays(1);
            List<RoomInventoryInfo> rmInventory = new List<RoomInventoryInfo>();
            using (var context = new HotelDatabaseContainer())
            {
                if (!context.RoomTypes.Any())
                    return new List<RoomInventoryInfo>();

                var rmTypes =  from rm in context.RoomTypes
                               select rm;

                foreach(var rmType in rmTypes)
                {
                    rmInventory.Add(new RoomInventoryInfo
                    {
                        rmType = rmType,
                        quantity = rmType.Rooms.Count(),
                        occupiedRooms = (from res in context.Reservations
                                         where res.RoomType.Id == rmType.Id &&
                                         res.checkIn < date && res.checkOut >= date
                                         select res).Count()
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

        public JsonResult CheckIn(int resId)
        {
            using (var context = new HotelDatabaseContainer())
            {
                var reservation = (from res in context.Reservations where res.Id == resId select res).First();

                var customer = (from cus in context.Customers where cus.Id == reservation.PersonId select cus).FirstOrDefault();

                if(customer != null) {
                    customer.stays++;
                    customer.lastStay = reservation.checkIn;
                    CustomerOperations.setLoyalty(customer, DateTime.Now);
                }

                //Assign Room
                var availibleRooms = from room in context.Rooms
                                    where !room.occupied && room.RoomType.Id == reservation.RoomType.Id
                                    select room;

                //Only hit if everyone still hasn't checkedout
                if(!availibleRooms.Any()) {
                    return Json(new { errorMessage = "Please wait for someone to check out later today." });
                }

                reservation.Room = availibleRooms.First();

                //Occupy Room
                reservation.Room.occupied = true;
                reservation.checkIn = DateTime.Now;

                context.SaveChanges();

                return Json(new { roomNum = reservation.Room.Id, checkInTime = reservation.checkIn.ToString("g") });
            }
        }

        public JsonResult CheckOut(int resId)
        {
            using (var context = new HotelDatabaseContainer())
            {
                var reservation = (from res in context.Reservations where res.Id == resId select res).First();

                //Occupy Room
                reservation.Room.occupied = false;
                reservation.checkOut = DateTime.Now;

                context.SaveChanges();

                return Json(new
                {
                    checkOutTime = reservation.checkOut.ToString("g"),
                });
           }
        }
    }
}