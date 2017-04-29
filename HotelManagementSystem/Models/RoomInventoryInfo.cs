using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataModel;

namespace HotelManagementSystem.Models
{
    public class RoomInventoryInfo
    {
        public RoomType rmType { get; set; }
        public int quantity { get; set; }
        public int occupiedRooms { get; set; }
    }
}