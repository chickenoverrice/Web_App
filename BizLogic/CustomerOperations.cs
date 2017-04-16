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
        public static void setLoyalty(Customer customer, CurrentDateTime now)
        {
            if (!customer.member && customer.lastStay != null)
            {
                if (customer.lastStay.Value.Year == now.time.Year)
                {
                    if (customer.stays >= 5)
                    {
                        customer.member = true;
                        customer.expirationDate = new DateTime(now.time.Year, 12, 31);
                    }
                }
            }
        }
    }
}
