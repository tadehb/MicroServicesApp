using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ordering.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Data
{
    public class OrderContextSeed
    {
        public static async Task SeedAsync(OrderContext orderContext, ILoggerFactory loggerFactory, int? retry = null)
        {
            int retryForAvailability = retry.Value;


            try
            {
                orderContext.Database.Migrate();

                if (!orderContext.Orders.Any())
                {
                    orderContext.Orders.AddRange(GetPreConfiguredPrders());
                    await orderContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {

                if (retryForAvailability < 3)
                {
                    retryForAvailability++;
                    var log = loggerFactory.CreateLogger<OrderContextSeed>();
                    log.LogError(ex.Message);
                    await SeedAsync(orderContext,loggerFactory,retryForAvailability);
                }
            }

            
        }

        private static IEnumerable<Order> GetPreConfiguredPrders()
        {
            return new List<Order>
            {
                new Order
                {
                  UserName = "Jim2001",
                  FirstName = "jim",
                  LastName = "Branson0",
                  Email = "jim200@gmail.com",
                  AddressLine = "California",
                  Country = "USA",


                }
            };
        }
    }
}
