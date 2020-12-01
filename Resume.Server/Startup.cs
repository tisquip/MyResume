using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Data;
using Serilog;
using Resume.Domain.Interfaces;
using Resume.Server.Services;
using Resume.Server.Services.FootballWorkerService;
using Resume.Server.Services.FootballWorkerService.InterfaceImplementations;

namespace Resume.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(Configuration).CreateLogger();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();

            services.AddDbContext<ResumeDbContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("ResumeDbContext")));

            services.AddScoped<IEmailService, SendGridEmailService>();
            services.AddScoped<IExceptionNotifier, ExceptionNotifier>();
            services.AddHttpClient<SportDataApiFootballWorkerService>();
            services.AddSingleton<IFootBallWorker, SportDataApiFootballWorkerService>();
            services.AddHostedService<BackgroundFootballService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
