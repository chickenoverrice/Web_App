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
            this.checkIn = DateTime.Now;
            this.checkOut = DateTime.Now.AddDays(1);
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [Required(ErrorMessage = "Check-in date is required")]
        public System.DateTime? checkIn { get; set; }
        [Required(ErrorMessage = "Check-out date is required")]
        public System.DateTime? checkOut { get; set; }
        [Display(Name = "Guest Information")]
        [Required(ErrorMessage = "Guest Information is required")]
        public string guestsInfo { get; set; }
        public double bill { get; set; }
        [Display(Name = "First Name")]
        [Required(ErrorMessage = "First name is required")]
        public string firstName { get; set; }
        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Last name is required")]
        public string lastName { get; set; }
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string email { get; set; }
        [Display(Name = "Phone Number")]
        [Required(ErrorMessage = "Phone number is required")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Invalid phone number")]
        public string phone { get; set; }
        [Display(Name = "Address")]
        public string address { get; set; }
        [Display(Name = "City")]
        public string city { get; set; }
        [Display(Name = "State")]
        public string state { get; set; }
        [Display(Name = "Zip Code")]
        //[DataType(DataType.PostalCode, ErrorMessage = "Invalid postal code")]
        public string zip { get; set; }
        public int roomNumber { get; set; }
        public int roomId { get; set; }
        public int personId { get; set; }
        [ForeignKey("personId")]
        public Person Person { get; set; }
        [ForeignKey("roomId")]
        public RoomType RoomType { get; set; }
    }

    public class ReservationContext: DbContext{
        public ReservationContext() : base("DefaultConnection") {
            Database.SetInitializer<ReservationContext>(null);
        }
        public DbSet<Reservation> Reservations { get; set; }
    }
}
