using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Web.Helpers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelManagementSystem.Models
{
    [Table("Customer")]
    public class Customer
    {
        public Customer()
        {
            this.member = false;
            this.stays = 0;
        }

        [Key]
        public int id { get; set; }
        public bool member { get; set; }
        public string password { get; set; }
        public int loyaltyNum { get; set; }
        public int stays { get; set; }
        public DateTime expirationDate { get; set; }
        public DateTime lastStay { get; set; }
        public int roomPref { get; set; }
        
        [ForeignKey("roomPref")]
        public RoomType RoomType { get; set; }

        [ForeignKey("id")]
        public Person Person { get; set; }
    }
    public class CustomerContext : DbContext
    {
        public CustomerContext() : base("DefaultConnection") { }
        public DbSet<Customer> Customers { get; set; }

    }
}
