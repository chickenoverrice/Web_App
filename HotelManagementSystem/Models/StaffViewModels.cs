using DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelManagementSystem.Models
{
    public class ReservationInfo
    {
        public DateTime checkIn { get; set; }
        public DateTime checkOut { get; set; }
        public int Id { get; set; }
        public Double bill { get; set; }
        public String guestInfo { get; set; }
        public String roomType { get; set; }
        public String roomNumber { get; set; }
        public String personName { get; set; }
        public String roomOccupied { get; set; }
    }

    public class RoomInventoryInfo
    {
        public RoomType rmType { get; set; }
        public int quantity { get; set; }
        public int occupiedRooms { get; set; }
    }
}