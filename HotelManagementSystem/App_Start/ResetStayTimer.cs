using System;
using System.Threading;
using HotelManagementSystem;
using System.Linq;


[assembly: WebActivatorEx.PreApplicationStartMethod(
  typeof(ResetStayTimer), "Start")]
public class ResetStayTimer
    {
        private static readonly Timer _timer = new Timer(OnTimerElapsed);
        private static readonly ResetStay _job = new ResetStay();

        public static void Start()
        {
            _timer.Change(TimeSpan.Zero, TimeSpan.FromMilliseconds(1000));
        }

        private static void OnTimerElapsed(object sender)
        {
            _job.DoWork(() => {
                var today = DateTime.Now;
                if (today.Month == 12 && today.Day == 31)
                {
                    using (var context = new DataModel.HotelDatabaseContainer())
                    {
                        var customer = from customers in context.Customers
                                       select customers;
                        foreach (var c in customer)
                        {
                            c.stays = 0;
                        }
                        context.SaveChanges();
                    }
                }
            });
        }
    }
