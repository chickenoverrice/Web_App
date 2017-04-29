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
        public int roomNumber { get; set; }
        public String personName { get; set; }
        public Boolean roomOccupied { get; set; }
    }
}