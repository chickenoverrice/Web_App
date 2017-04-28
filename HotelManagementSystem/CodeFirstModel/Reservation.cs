using System.Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelManagementSystem.CodeFirstModel
{
    [Table("Reservation")]
    public class Reservation
    {
        public Reservation()
        {

        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        //[Display(Name = "Arrival")]
        //[Required(ErrorMessage = "Check-in date is required")]
        public DateTime checkIn { get; set; }
        //[Display(Name = "Departure")]
        //[Required(ErrorMessage = "Check-out date is required")]
        public DateTime checkOut { get; set; }
        //[Display(Name = "Guest Information")]
        public string guestsInfo { get; set; }
        public double bill { get; set; }        
        [Required(ErrorMessage = "First name is required")]
        public string firstName { get; set; }
        [Required(ErrorMessage = "Last name is required")]
        public string lastName { get; set; }
        //[Required(ErrorMessage = "Email address is required")]
        //[EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string email { get; set; }
        //[Required(ErrorMessage = "Phone number is required")]
        //[DataType(DataType.PhoneNumber)]
        //[RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Invalid phone number")]
        public string phone { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        //[DataType(DataType.PostalCode, ErrorMessage = "Invalid postal code")]
        public string zip { get; set; }
        public int roomId { get; set; }
        public int personId { get; set; }
        [ForeignKey("personId")]
        public Person Person { get; set; }
        [ForeignKey("roomId")]
        public RoomType RoomType { get; set; }
    }

    public class ReservationContext: DbContext{
        public ReservationContext() : base("DefaultConnection") { }
        public DbSet<Reservation> Reservations { get; set; }
    }
}
