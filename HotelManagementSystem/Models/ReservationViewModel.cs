using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using DataModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelManagementSystem.Models
{
    public class SearchRoomViewModel
    {
        public SearchRoomViewModel() { }
        // for parameters from Index
        public System.DateTime checkIn { get; set; }
        public System.DateTime checkOut { get; set; }
        public List<DateTime> nights { get; set; }
        public List<DataModel.RoomType> roomTypes { get; set; }
        public List<double> listPrice { get; set; }
        public List<List<double>> listPrices { get; set; }
        public int prefRoom { get; set; }
        // for parameters to Book
        public int roomId { get; set; }
    }
    public class ReservationDetailViewModel
    {
        public ReservationDetailViewModel() { }
        public String reservationId { get; set; }
        public System.DateTime checkIn { get; set; }
        public System.DateTime checkOut { get; set; }
        public List<DateTime> nights { get ; set; }
        public List<double> listPrice { get; set; }
        public int numberOfNights { get; set; }
        public List<String> guestInfoList { get; set; }
        public string guestInfo { get; set; }
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
        [Display(Name = "Address")]
        [Required(ErrorMessage = "Address is required")]
        public string address { get; set; }
        [Display(Name = "Phone Number")]
        [Required(ErrorMessage = "Phone number is required")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Invalid phone number")]
        public string phone { get; set; }
        [Display(Name = "City")]
        [Required(ErrorMessage = "City is required")]
        public string city { get; set; }
        [Display(Name = "State")]
        [Required(ErrorMessage = "State is required")]
        public string state { get; set; }
        [Display(Name = "Zip Code")]
        [Required(ErrorMessage = "Zip Code is required")]
        [DataType(DataType.PostalCode, ErrorMessage = "Invalid postal code")]
        public string zip { get; set; }
        public string roomType { get; set; }
        public int roomId { get; set; }
        public int roomGuest { get; set; }
        public int personId { get; set; }

    }
    public class ReservationDetailContext: DbContext
    {
        public ReservationDetailContext() : base("DefaultConnection") { }
        public DbSet<Person> People { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
    }
}