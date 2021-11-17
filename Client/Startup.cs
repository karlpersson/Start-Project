using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityModel;
using Infrastructure;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Serilog;

namespace Client
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
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            }).AddCookie(opt =>
            {
                opt.LogoutPath = "/user/Logout";
                opt.AccessDeniedPath = "/user/AccessDenied";
            }).AddOpenIdConnect(options =>
            {
                options.Authority = _configuration["openid:authority"];
                options.ClientId = _configuration["openid:clientid"];

                options.ClientSecret = "mysecret";
                options.ResponseType = "code";


                options.Scope.Clear();
                options.Scope.Add("openid");
                options.Scope.Add("profile");
                options.Scope.Add("email");

                options.GetClaimsFromUserInfoEndpoint = true;
                options.SaveTokens = true;
                options.Prompt = "consent";
                options.AccessDeniedPath = "/User/AccessDenied";

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = JwtClaimTypes.Name,
                    RoleClaimType = JwtClaimTypes.Role
                };
            });


            services.AddControllersWithViews();

            services.AddHsts(opts => { opts.IncludeSubDomains = true; opts.MaxAge = TimeSpan.FromSeconds(15768000); });
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
            app.UseHttpsRedirection();



            app.UseStaticFiles();

            app.UseRouting();

            app.UseRequestLocalization(new RequestLocalizationOptions().SetDefaultCulture("sv-SE"));

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSecurityHeaders();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
