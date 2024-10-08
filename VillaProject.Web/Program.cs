using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Stripe;
using Syncfusion.EJ2;
using Syncfusion.Licensing;
using System.Globalization;
using System.Net.NetworkInformation;
using VillaNumberProject.Application.Services.Implementation;
using VillaProject.Application.Common.Interfaces;
using VillaProject.Application.Common.Utility;
using VillaProject.Application.Services.Implementation;
using VillaProject.Application.Services.Interface;
using VillaProject.Domain.Entities;
using VillaProject.Infrastructure.Data;
using VillaProject.Infrastructure.Repository;

namespace VillaProject.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var configuration = builder.Configuration;

            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<ApplicationDbContext>(option =>
            option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>() 
                .AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
            builder.Services.ConfigureApplicationCookie(options => {
                options.LoginPath = $"/Identity/Account/Login";
                options.LogoutPath = $"/Identity/Account/Logout";
                options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
            });
            builder.Services.AddAuthentication().AddFacebook(option =>
            {
                option.AppId = configuration["Authentication:Facebook:AppId"];
                option.AppSecret = configuration["Authentication:Facebook:AppSecret"];
            });
            builder.Services.AddAuthentication().AddMicrosoftAccount(option =>
            {
                option.ClientId = configuration["Authentication:Microsoft:ClientId"];
                option.ClientSecret = configuration["Authentication:Microsoft:ClientSecret"];
            });
            builder.Services.AddAuthentication().AddGoogle(option =>
            {
                option.ClientId = configuration["Authentication:Google:ClientId"];
                option.ClientSecret = configuration["Authentication:Google:ClientSecret"];
            });
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options => {
                options.IdleTimeout = TimeSpan.FromMinutes(100);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            builder.Services.ConfigureApplicationCookie(option =>
            {
                option.AccessDeniedPath = "/Account/AccessDenied";
                option.LoginPath = "/Identity/Account/Login";
            });
            builder.Services.Configure<IdentityOptions>(option =>
            {
                option.Password.RequiredLength = 6;
            });


            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IEmailSender, EmailSender>();
            builder.Services.AddScoped<IDashboardService, DashboardService>();
            builder.Services.AddScoped<IDbInitializer, DbInitializer>();

            builder.Services.AddScoped<IVillaService, VillaService>();
            builder.Services.AddScoped<IVillaNubmberService, VillaNumberService>();
            builder.Services.AddScoped<IAmenityService, AmenityService>();
            builder.Services.AddScoped<IBookingService, BookingService>();
            builder.Services.AddScoped<IFacilityService, FacilityService>();
            builder.Services.AddScoped<IFacilityRepository, FacilityRepository>();

            builder.Services.AddRazorPages();
            var app = builder.Build();
            StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Get<string>();

            SyncfusionLicenseProvider.RegisterLicense(builder.Configuration.GetSection("Syncfusion:LicenseKey").Get<string>());

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            var cultureInfo = new CultureInfo("en-US");
            cultureInfo.NumberFormat.CurrencySymbol = "$";

            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();
            SeedDatabase();
            app.MapRazorPages();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();

            //we need to get an implementation of initializer
            void SeedDatabase()
            {
                using (var scope = app.Services.CreateScope())
                {
                    //We want the implementation of DbInitializer and on that variable we can invoke the initialize method
                    var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
                    dbInitializer.Initialize();
                }
            }
        }
    }
}
