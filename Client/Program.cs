using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Client
{
    /// <summary>
    /// This is the starter-kit that we use in the training course IdentityServer in Production  
    /// by https://www.tn-data.se
    /// </summary>
    public class Program
    {
        private static readonly string _applicationName = "Client";

        public static void Main(string[] args)
        {
            Settings.StartupTime = DateTime.Now;

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
