using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using DataModel;

namespace HotelManagementSystem.Models
{
    public class ReservationViewModel
    {
        public DateTime checkIn { get; set; }
        public DateTime checkOut { get; set; }
        public int RoomId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string address { get; set; }
        public string phone { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
    }
    public class ReservationDetailContext: DbContext
    {
        public ReservationDetailContext() : base("DefaultConnection") { }
        public DbSet<Person> People { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
    }
}