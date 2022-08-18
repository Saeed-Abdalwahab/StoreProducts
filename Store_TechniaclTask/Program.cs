using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Store_TechniaclTask.DAL.Context;
using Store_TechniaclTask.Services.ModelServices.Abstraction.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Store_TechniaclTask.Web
{
    public class Program
    {
        public static IConfiguration Configuration { get; } = new ConfigurationBuilder()

.AddEnvironmentVariables()
.Build();

        public static void Main(string[] args)
        {
       
            try
            {
                var host = CreateHostBuilder(args).ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                }).Build();
         

                host.Run();
            }
            catch (Exception ex)
            {
            }
            finally
            {
            }
            //CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
