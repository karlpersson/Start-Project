using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure;
using Infrastructure.DataProtection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Logging;
using PaymentAPI.Middleware;
using Serilog;

namespace PaymentAPI
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _environment = environment;
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            if (_environment.EnvironmentName != "Offline")
                services.AddDataProtectionWithSqlServerForPaymentApi(_configuration);

            services.AddControllersWithViews();
            services.AddHsts(opts => { opts.IncludeSubDomains = true; opts.MaxAge = TimeSpan.FromSeconds(15768000); });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).
                 AddJwtBearer(opt => { opt.Audience = "paymentapi"; opt.Authority = _configuration["openid:authority"]; opt.MapInboundClaims = false; opt.TokenValidationParameters.RoleClaimType = "roles"; opt.TokenValidationParameters.NameClaimType = "name"; opt.IncludeErrorDetails = true; opt.BackchannelHttpHandler = new BackChannelListener(); opt.BackchannelTimeout = TimeSpan.FromSeconds(5); opt.TokenValidationParameters.ClockSkew = TimeSpan.FromSeconds(0); });

            
            
            //Add the listener to the ETW system

            //var listener = new IdentityModelEventListener();
            //IdentityModelEventSource.Logger.LogLevel =
            //    System.Diagnostics.Tracing.EventLevel.Verbose;
            //IdentityModelEventSource.ShowPII = true;

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (!env.IsProduction())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseSerilogRequestLogging();

            //Add this string to the PaymentAPI ConfigureServices method:
            //Make sure its placed before app.UseAuthentication();
            //Wait for IdentityServer to startup
            app.UseWaitForIdentityServer(new WaitForIdentityServerOptions()
            { Authority = _configuration["openid:authority"] });

            app.UseHttpsRedirection();

            app.UseSecurityHeaders();

            app.UseRouting();

            app.UseRequestLocalization(new RequestLocalizationOptions().SetDefaultCulture("sv-SE"));

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

            });
          

        }
    }
}
