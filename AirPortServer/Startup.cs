using AirPortDal.Data;
using AirPortDal.Services;
using AirPortServer.Hubs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AirPortServer
{
    public class Startup
    {
        string corsPolicy = "corsPolicy";
        string clientUrl = "http://localhost:4200";
        string connectionString = "DefaultConnection";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AirPortContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString(connectionString)));

            services.AddCors(options =>
            {
                options.AddPolicy(corsPolicy, builder => builder
                .WithOrigins(clientUrl)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());
            });

            services.AddSignalR((options) =>
            {
                options.EnableDetailedErrors = true;
            });

            services.AddScoped<IDataService, DataService>();
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, AirPortContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseCors(corsPolicy);

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<BlToUiHub>("/BlToUI");
                endpoints.MapHub<BlToSimHub>("/BlToSim");
                endpoints.MapControllers();
            });
        }
    }
}
