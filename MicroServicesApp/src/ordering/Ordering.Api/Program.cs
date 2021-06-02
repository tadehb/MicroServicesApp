using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Ordering.Infrastructure.Data;
using System;

namespace Ordering.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
             var host = CreateHostBuilder(args).Build();
            CreateAndSeeddatabase(host);
            host.Run();

        }

        public static void CreateAndSeeddatabase(IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var loggerFactory = services.GetRequiredService<ILoggerFactory>();


                try
                {
                    var orderContext = services.GetRequiredService<OrderContext>();
                    _ = OrderContextSeed.SeedAsync(orderContext, loggerFactory,1);

                }
                catch (Exception ex)
                {

                    var logger = loggerFactory.CreateLogger<Program>();
                    logger.LogError(ex.Message);
                }

            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
