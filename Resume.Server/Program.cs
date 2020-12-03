using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Resume.Application;
using Serilog;

namespace Resume.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    //TODO: Remove use url, in place so that I can debug server and blazor app together, the defualt kestrl behavior launches both on localhost:5000
                    webBuilder.UseUrls(URLS.Server);
                    webBuilder.UseSerilog();
                    webBuilder.UseStartup<Startup>();
                });
    }
}
