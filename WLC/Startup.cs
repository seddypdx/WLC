using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WLC.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WLC.Models;
using WLC.Services;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;

namespace WLC
{
    public class Startup
    {
        ILogger<Startup> _logger;
        public Startup(IConfiguration configuration, ILogger<Startup> logger)
        {
            Configuration = configuration;
            _logger = logger;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            _logger.LogDebug($"con:{connectionString}");

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    connectionString));


            services.AddDefaultIdentity<IdentityUser>()
                .AddEntityFrameworkStores<ApplicationDbContext>(
                );


            services.AddDbContext<WLCRacesContext>(options =>
                  options.UseSqlServer(
              Configuration.GetConnectionString("WLCConnection")));

            services.AddSession(options => {
                options.IdleTimeout = TimeSpan.FromMinutes(480);//You can set Time
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_0);

            services.AddTransient<IEmailSender, EmailSender>(i =>
           new EmailSender(
               Configuration["EmailSender:Host"],
               Configuration.GetValue<int>("EmailSender:Port"),
               Configuration.GetValue<bool>("EmailSender:EnableSSL"),
               Configuration["EmailSender:UserName"],
               Configuration["EmailSender:Password"]
           )
       );

            services.AddMvc().AddRazorPagesOptions(options =>
            {
                options.AllowAreas = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();

              //  app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
              //  app.UseHsts();
            }

         //   app.UseHttpsRedirection();
            app.UseStaticFiles();
            //app.UseCookiePolicy();
            app.UseSession();

            app.UseAuthentication();


            app.UseMvc(routes =>
            {
                routes.MapAreaRoute("Races", "Races", "Races/{controller=Home}/{action=Index}/{id?}");
                routes.MapAreaRoute("Checkin", "Checkin", "Checkin/{controller=Home}/{action=Index}/{id?}");
                routes.MapAreaRoute("Notices", "Notices", "Checkin/{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
