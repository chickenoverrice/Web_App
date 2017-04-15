using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModel;

namespace BizLogic
{
    public static class CustomerOperations
    {
        public static void setLoyalty(Customer customer)
        {
            if (!customer.member)
            {
                ICollection<Reservation> reservations = customer.Reservation;
                int stays = 0;
                int currentYear = 0;

                foreach (Reservation r in reservations)
                {
                    int year = Convert.ToDateTime(r.checkIn).Year;
                    if (year != currentYear)
                    {
                        stays = 0;
                        currentYear = year;
                    }
                    stays++;

                    if (stays >= 5)
                    {
                        customer.member = true;
                        return;
                    }
                }
            }
        }
    }
}
