//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class Reservation
    {
        public System.DateTime checkIn { get; set; }
        public System.DateTime checkOut { get; set; }
        public int Id { get; set; }
        public double bill { get; set; }
        public string guestsInfo { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string address { get; set; }
        public string phone { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
        public Nullable<int> RoomTypeId { get; set; }
    
        public virtual Room Room { get; set; }
        public virtual Person People { get; set; }
        public virtual RoomType RoomType { get; set; }
    }
}
