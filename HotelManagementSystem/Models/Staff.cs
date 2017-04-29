using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelManagementSystem.Models
{
    [NotMapped]
    public partial class Staff : Person
    {
        public string password { get; set; }
    }
}
