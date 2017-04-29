using System;
using DataModel;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace HotelManagementSystem.Models
{
    public partial class HotelDatabaseContainer : DbContext
    {
        public HotelDatabaseContainer() : base("DefaultConnection")
        {

        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Person> People { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Staff> Staffs { get; set; }
        public virtual DbSet<Room> Rooms { get; set; }
        public virtual DbSet<RoomType> RoomTypes { get; set; }
        public virtual DbSet<Reservation> Reservations { get; set; }
        public virtual DbSet<CurrentDateTime> CurrentDateTimes { get; set; }
    }
}
