using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Data.Entity;
using HotelManagementSystem.Models;
namespace HotelManagementSystem
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //Database.SetInitializer<ReservationContext>(new DropCreateDatabaseAlways<ReservationContext>());
            //Database.SetInitializer<ReservationDetailContext>(new DropCreateDatabaseAlways<ReservationDetailContext>());
            //Database.SetInitializer<PersonContext>(new DropCreateDatabaseAlways<PersonContext>());
            //Database.SetInitializer<CustomerContext>(new DropCreateDatabaseAlways<CustomerContext>());
            //Database.SetInitializer<RoomTypeContext>(new DropCreateDatabaseAlways<RoomTypeContext>());
            //Database.SetInitializer<StaffContext>(new DropCreateDatabaseAlways<StaffContext>());

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            ResetStayTimer.Start();
        }
    }
}
