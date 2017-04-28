using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace HotelManagementSystem.CodeFirstModel
{
    [Table("CurrentDateTime")]
    public class CurrentDateTime
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key, Required]
        public int Id { get; set; }
        public DateTime time { get; set; }
    }
    public class CurrentDateTimeContext : DbContext
    {
        public CurrentDateTimeContext() : base("DefaultConnection")
        {
            Database.SetInitializer<CurrentDateTimeContext>(null);
        }

        public DbSet<CurrentDateTime> CurrentDateTimeRecords { get; set; }
    }
}
