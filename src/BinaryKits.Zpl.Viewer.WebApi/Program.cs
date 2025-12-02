using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;

namespace BinaryKits.Zpl.Viewer.WebApi
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
                    string env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "prod";
                    string webRoot = env.Equals("prod", StringComparison.OrdinalIgnoreCase)
                        ? "wwwroot-prod"
                        : "wwwroot";
                    webBuilder.UseWebRoot(webRoot);

                    webBuilder.UseStartup<Startup>();
                });
    }
}
