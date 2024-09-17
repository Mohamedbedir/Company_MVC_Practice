using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Company.DAL.Contexts;
using Company.DAL.Models;
using Company.PL.MapperProfile;
using Company.PLL.Interfaces;
using Company.PLL.Repositios;
using Microsoft.AspNetCore.Authentication.Cookies;

using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;


namespace Company.PL
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);
            #region Config Service that allaw DI
            //DI

            builder.Services.AddControllersWithViews(); //mvc
            builder.Services.AddDbContext<CompanyDbContext>(Options =>
            {
                Options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            // services.AddScoped<IDepatmentRepostrios, DepartmentReposatory>(); //object lifetime per Request 

            //services.AddTransient<IDepatmentRepostrios, DepartmentReposatory>(); //object lifetime per operation 
            //services.AddSingleton<IDepatmentRepostrios, DepartmentReposatory>(); //object lifetime per Application --> caching ,logging , signalR 

            // services.AddScoped<IEmployeeRepostrios, EmployeeReposatory>();

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            //DI(scoped , singleton, transient)
            //services.AddAutoMapper(typeof(Startup));
            //add services automapper
            builder.Services.AddAutoMapper(m => m.AddProfile(new EmployeeProfile()));
            builder.Services.AddAutoMapper(u => u.AddProfile(new UserProfile()));
            builder.Services.AddAutoMapper(r => r.AddProfile(new RoleProfile()));

            //DI Security
            //services.AddScoped<UserManager<ApplicationUser>>();
            //services.AddScoped<SignInManager<ApplicationUser>>();
            //services.AddScoped<RoleManager<IdentityRole>>();

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(Options =>
                {
                    Options.LoginPath = "Account/Login";
                    Options.AccessDeniedPath = "Home/Error";
                }); //لما حد يطلب obj من ال usermanager/signin/role هتبعتهله

            //بتكمل الامبليمنتيشن بتاع الفانكشمز الناقصه CreateAsync()
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;

            }).AddEntityFrameworkStores<CompanyDbContext>()
            .AddDefaultTokenProviders();//default Token  

            #endregion

            var app=builder.Build();
            var env = builder.Environment;

            #region Configure http request pipline
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
            #endregion
            app.Run();
        }

        //Confg .net 5
        //public static IHostBuilder CreateHostBuilder(string[] args) =>
        //    Host.CreateDefaultBuilder(args)
        //        .ConfigureWebHostDefaults(webBuilder =>
        //        {
        //            webBuilder.UseStartup<Startup>();
        //        });
    }
}
