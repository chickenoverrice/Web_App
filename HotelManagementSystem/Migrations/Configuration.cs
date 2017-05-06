namespace HotelManagementSystem.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<HotelManagementSystem.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "HotelManagementSystem.Models.ApplicationDbContext";
        }

        protected override void Seed(HotelManagementSystem.Models.ApplicationDbContext context)
        {
            if (!(context.Users.Any(u => u.UserName == "staff@lxyz.com")))
            {
                var userStore = new UserStore<ApplicationUser>(context);
                var userManager = new UserManager<ApplicationUser>(userStore);
                var userToInsert = new ApplicationUser { UserName = "staff@lxyz.com", Email = "staff@lxyz.com" };
                userManager.Create(userToInsert, "Staff@123");

                DataModel.Staff staff = new DataModel.Staff
                {
                    firstName = "staff",
                    lastName = "member",
                    email = "staff@lxyz.com",
                    address = "address",
                    city = "city",
                    state = "state",
                    zip = "zip",
                    phone = "1115556600",
                    sessionExpiration = System.DateTime.Now.AddMinutes(10)
                };

                using (var contextHotel = new DataModel.HotelDatabaseContainer())
                {
                    contextHotel.Staff.Add(staff);
                    contextHotel.SaveChanges();
                }


                // Adding a new role
                var RoleManager = new RoleManager<IdentityRole>(
                                    new RoleStore<IdentityRole>(context));

                var roleresult = RoleManager.Create(new IdentityRole("Staff"));

                // Adding a user to a role
                userManager.AddToRole(userToInsert.Id, "Staff");
                context.SaveChanges();
            }
        }
    }
}
