using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModel;
using BizLogic;
using System.Data.Entity.Validation;

namespace TestConsole
{
    class TestValidation
    {
        public static void test()
        {
            try
            {
                using (var context = new HotelDatabaseContainer())
                {
                    
                    string firstname = "Derek";
                    string lastname = "Jeter";
                    string email = "XXXX@gmail.com";
                    string password = "12345";
                    string sessionId = Guid.NewGuid().ToString();
                    DateTime sessionExp = DateTime.Now.AddMinutes(10);
                    string phone = "123-456-7890";
                    string address = "251 Mercer St";
                    string city = "New York";
                    string state = "NY";
                    string zip = "10012";

                    if (context.People.Any(o => o.email == email))
                    {
                        Console.WriteLine("Duplicated Account");
                        return;
                    }

                    Customer c1 = new Customer(firstname, lastname, email, sessionId, sessionExp, password, phone, address, state, city, zip);
                    context.People.Add((Person)c1);
                    context.SaveChanges();
                    Console.WriteLine("Add C1");

                    // firstname, email, phone invalid
                    firstname = null;
                    email = "12345";
                    phone = "12345";
                    zip = "Hello";
                    Customer c2 = new Customer(firstname, lastname, email, sessionId, sessionExp, password, phone, address, state, city, zip);
                    context.People.Add((Person)c2);
                    context.SaveChanges();
                    Console.WriteLine("Add C2");                    
                    
                }
            }
            catch (DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                // Throw a new DbEntityValidationException with the improved exception message.
                Console.WriteLine(exceptionMessage);
                Console.ReadLine();
                //throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
        }
    }
}
