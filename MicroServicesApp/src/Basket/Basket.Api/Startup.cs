using Basket.Api.data;
using Basket.Api.data.Interfaces;
using Basket.Api.Repositories;
using Basket.Api.Repositories.Interfaces;
using EventBusRabbitMQ;
using EventBusRabbitMQ.Interfaces;
using EventBusRabbitMQ.Producer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using RabbitMQ.Client;
using StackExchange.Redis;

namespace Basket.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IBasketContext, BasketContext>();
            var test = Configuration.GetConnectionString("Redis");
            services.AddSingleton<IConnectionMultiplexer>(sp => ConnectionMultiplexer.Connect(Configuration.GetConnectionString("Redis")));


            services.AddScoped<IBasketRepository, BasketRepository>();
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Basket.Api", Version = "v1" });
            });


            services.AddSingleton<IRabbitMQConnection>(c =>
            {
                var factory = new ConnectionFactory();
                
                                 

                if (!string.IsNullOrEmpty(Configuration["EventBus:Host"]))
                {
                    factory.HostName = Configuration["EventBus:Host"];

                }

                if (!string.IsNullOrEmpty(Configuration["EventBus:UserName"]))
                {
                    factory.UserName = Configuration["EventBus:UserName"];

                }
              

                if (!string.IsNullOrEmpty(Configuration["EventBus:Password"]))
                {
                    factory.Password = Configuration["EventBus:Password"];

                }

                factory.VirtualHost = "/";

                return new RabbitMQConnection(factory);
            });

            services.AddSingleton<EventBusRabbitMQProducer>();

            services.AddAutoMapper(typeof(Startup));

            services.AddApiVersioning(config =>
            {
                config.DefaultApiVersion = new ApiVersion(1, 0);
                config.AssumeDefaultVersionWhenUnspecified = true;
                config.ReportApiVersions = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Basket.Api v1"));
            }
    
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
