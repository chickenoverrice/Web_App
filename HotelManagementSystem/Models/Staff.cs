using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace HotelManagementSystem.Models
{
    [Table("Staff")]
    public class Staff
    {
        public int id { get; set; }
        public string password { get; set; }
        [ForeignKey("id")]
        public Person Person { get; set; }
    }
    public class StaffContext : DbContext
    {
        public StaffContext() : base("DefaultConnection") { }
        public DbSet<Staff> Staffs { get; set; }

    }
}
