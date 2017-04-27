using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace HotelManagementSystem.Models
{
    [Table("Person")]
    public partial class Person
    {   
        public Person()
        {
            this.Reservations = new HashSet<Reservation>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [Required(ErrorMessage ="First name is required")]
        public string firstName { get; set; }
        [Required(ErrorMessage = "Last name is required")]
        public string lastName { get; set; }
        [Required(ErrorMessage = "Email address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string email { get; set; }
        public string sessionId { get; set; }
        public string address { get; set; }
        [Required(ErrorMessage = "Phone number is required")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Invalid phone number")]
        public string phone { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        [DataType(DataType.PostalCode, ErrorMessage = "Invalid postal code")]
        public string zip { get; set; }
        public DateTime sessionExpiration { get; set; }
    
        public ICollection<Reservation> Reservations { get; set; }
        public ICollection<Customer> Customers { get; set; }
        public ICollection<Staff> Staffs { get; set; }
    }
    public class PersonContext : DbContext
    {
        public PersonContext() : base("DefaultConnection") { }
        public DbSet<Person> Persons { get; set; }

    }

}
