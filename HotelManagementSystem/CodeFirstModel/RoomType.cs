using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelManagementSystem.Models
{
    [Table("RoomType")]
    public class RoomType
    {
        public RoomType()
        {
 
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string type { get; set; }
        public double basePrice { get; set; }
        public int maxGuests { get; set; }
        public int numberOfRooms { get; set; }
        public string description { get; set; }
        public string amenities { get; set; }
        public string pic { get; set; }
        public ICollection<Customer> Customers { get; set; }
        public ICollection<Reservation> Reservations { get; set; }
    }
    public class RoomTypeContext : DbContext
    {
        public RoomTypeContext() : base("DefaultConnection") { }
        public DbSet<RoomType> RoomTypes { get; set; }

    }
}
