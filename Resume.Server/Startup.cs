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
using Resume.Server.Data;
using Resume.Server.Services.EmailServices;
using Resume.Server.Services.ExceptionNotifierServices;
using Resume.Server.Data.Repositories;
using Resume.Domain;
using Resume.Server.Hubs;
using Resume.Application;
using MediatR;
using System.Reflection;
using Resume.Server.ProtoServer.FootballMatchProtoServices;

namespace Resume.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(Configuration).CreateLogger();
        }
        string corsAllowAll = "AllowAll";
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();

            services.AddDbContext<ResumeDbContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("ResumeDbContext")));

            services.AddDbContext<ResumeBackgroundServiceDbContext>(options =>
                   options.UseSqlServer(Configuration.GetConnectionString("ResumeDbContext")), ServiceLifetime.Singleton);

            services.AddMediatR(Assembly.GetExecutingAssembly());

            services.AddScoped<IEmailServiceScoped, SendGridEmailServiceScoped>();
            services.AddSingleton<IEmailServiceSingleton, SendGridEmailServiceSingleton>();

            services.AddScoped<IExceptionNotifierScoped, ExceptionNotifierScoped>();
            services.AddSingleton<IExceptionNotifierSingleton, ExeptionNotifierSingleton>();

            services.AddSingleton<IRootAggregateRepositorySingleton<FootballTeam>, RootAggregateRepositorySingletonFootballTeam>();
            services.AddScoped<IRootAggregateRepositoryScoped<FootballTeam>, RootAggregateRepositoryScopedFootballTeam>();

            services.AddHttpClient<SportDataApiFootballWorkerService>();

            //services.AddSingleton<IFootBallWorker, SportDataApiFootballWorkerService>();
            services.AddSingleton<IFootBallWorker, MockFootballWorker>();

            services.AddHostedService<BackgroundFootballService>();

            services.AddSignalR();
            services.AddGrpc();

            services.AddCors((config) =>
            {
                config.AddPolicy(corsAllowAll, (builder) =>
                {
                    builder.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .Build();
                });
            });
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

            app.UseGrpcWeb();
            app.UseCors(corsAllowAll);

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapHub<MatchHub>($"/{URLS.HubLiveMatchEnpointNameOnly_NotUrl}");
                endpoints.MapGrpcService<FootballMatchProtoServicesProtoImplementation>().EnableGrpcWeb().RequireCors(corsAllowAll);
            });
        }
    }
}
